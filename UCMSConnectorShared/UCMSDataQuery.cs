/****************************************************************************
'*   (c) Copyright IMIP Technology And Solution Consultancy JSC., 2010. All rights reserved.
'*   Unauthorized use, duplication or distribution is strictly prohibited.
'*****************************************************************************
'*
'*   File:       UCMSDataQuery.cs
'*
'*   Purpose:    Query batch from UCMS server
*********************************************************************************/
#define _DEBUG
using System;
using System.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;
using System.DirectoryServices;
using IMIP.UniversalScan;
using IMIP.UniversalScan.Data;
using IMIP.UniversalScan.Def;
using IMIP.UniversalScan.Common;
using NLog;
using IMIP.UniversalScan.Connector.USMSConnectorShared.Properties;
using UCMS.Model;
using UCMS.RestClient;
using UCMS.Model.Enum;

namespace IMIP.UniversalScan.Connector.UCMSConnectorShared
{
    static class Extensions
    {
        /// <summary>
        /// Convert ArrayList to List.
        /// </summary>
        public static List<T> ToList<T>(this ArrayList arrayList)
        {
            List<T> list = new List<T>(arrayList.Count);
            foreach (T instance in arrayList)
            {
                list.Add(instance);
            }
            return list;
        }
    }

    public class UCMSDataQuery
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static object Properties { get; private set; }

        public static BPProcess GetProcessMetadata(UCMSApiClient restClient, Workflow workflow, List<UniFormType> lstOriginalTypes)
        {
            BPProcess oProcess = new BPProcess();
            oProcess.Name = workflow.Name;
            oProcess.Tag = workflow;

            oProcess.bpProcessSteps = GetUSCProcessSteps(restClient, workflow, lstOriginalTypes);
            oProcess.uniFormTypes = lstOriginalTypes;
            return oProcess;
        }

        public static List<BPClient> GetUCMSMetadata(UCMSApiClient restClient, bool scanMode)
        {
            return GetUCMSMetadata(restClient, Global.Activity_UniversalScan, scanMode);
        }

        public static List<BPClient> GetUCMSMetadata(UCMSApiClient restClient, string activityTypeId, bool scanMode)
        {
            ArrayList lstClient = new ArrayList();
            DateTime start, end, start1, end1;
            start = DateTime.Now;

            List<Folder> folders = null;
            
            if (scanMode)
                folders = restClient.Folder.GetFolders("", permission : Permission.Create).Items;
            else
                folders = restClient.Folder.GetFolders("", permission: Permission.Edit).Items;

            foreach (var rootfolder in folders)
            {
                logger.Info(string.Format("Get metadata for library {0}", rootfolder.Name));
                start1 = DateTime.Now;
#if _DEBUG
                if (!rootfolder.Name.Equals("Son_Test", StringComparison.OrdinalIgnoreCase) && !rootfolder.Name.Equals("ImportProcess", StringComparison.OrdinalIgnoreCase)
                    && !rootfolder.Name.Equals("BGProcess", StringComparison.OrdinalIgnoreCase))
                    continue;
#endif
                
                Library library = null;
                
                library = restClient.Folder.GetLibrary(rootfolder.Id);

                bool bValidClient = false;
                BPClient bpClient = new Data.BPClient() { Name = rootfolder.Name, ID = rootfolder.Id, Tag = library };


                foreach (Workflow workflow in library.Workflows)
                {
                    List<WorkflowStep> tempList = new List<WorkflowStep>();
                    var lstActivities = workflow.Steps;
                    bool bFoundStep = false;

                    for (int i = 0; i < workflow.Steps.Count; i++)
                    {
                        WorkflowStep oStep = workflow.Steps[i];

                        if (oStep.Activity.UniqueId == activityTypeId && oStep.IsCurrentUserAccessible)
                        {
                            bFoundStep = true;
                        }
                        else
                        {
                            logger.Info(string.Format("Step {0} of workflow {1} is not UniversalScan or inaccessible", oStep.Name, workflow.Name));
                            tempList.Add(oStep);
                        }
                    }

                    if (!bFoundStep)
                        continue;

                    foreach (WorkflowStep oStep in tempList)
                        workflow.Steps.Remove(oStep);

                    //get formtype from UCMS
                    List<ContentType> lstTypes = library.ContentTypes;
                    //UCMS library has library field, which can be considered as normal field to web scan
                    List<LibraryField> lstLibFields = library.Fields;

                    List<UniFormType> lstOriginalTypes = DataHelper.GetUniFormTypes(lstTypes, lstLibFields);

                    //Get Pre-Define fields
                    var lstPredefine = DataHelper.GetPreDefineField(lstTypes);

                    //Get metadata
                    var bpProcess = GetProcessMetadata(restClient, workflow, lstOriginalTypes);
                    bpClient.bpProcesses.Add(bpProcess);

                    List<BPProcessStep> stepTempList = new List<BPProcessStep>();
                    //Add predefine list to step settig
                    for (int i = 0; i < bpProcess.bpProcessSteps.Count; i++)
                    {
                        bool bValidStep = false;
                        BPProcessStep step = bpProcess.bpProcessSteps[i];
                        step.uniParameterSet.DocumentTypeProfileSetting.PreDefineFieldList = lstPredefine;

                        //for universal scan, the form type must be root
                        foreach (UniFormType oFormType in step.uniParameterSet.DocumentTypeProfileSetting.UniFormtypeList)
                        {
                            if (oFormType.Root)
                            {
                                bValidStep = true;
                                break;
                            }
                        }
                        if (!bValidStep)
                            logger.Info(string.Format("Step {0} does not have any root form type", step.Name));

                        //this step is for verifier
                        if (!bValidStep)
                        {
                            if (!step.uniParameterSet.AllowCreateNewBatch)
                                bValidStep = true;
                            else
                                logger.Info(string.Format("Step {0} is not a verifier step", step.Name));
                        }

                        if (!bValidStep)
                        {
                            stepTempList.Add(step);
                        }
                        else
                            bValidClient = true;
                    }

                    foreach (BPProcessStep step in stepTempList)
                    {
                        bpProcess.bpProcessSteps.Remove(step);
                        logger.Info(string.Format("Step {0} is invalid for web scan!!!", step.Name));
                    }
                }

                
                if (bValidClient)
                    lstClient.Add(bpClient);
                else
                    logger.Info(string.Format("Client {0} is invalid for web scan", bpClient.Name));

                end1 = DateTime.Now;
                logger.Info(string.Format("=== Get metadata for library {0} in {1} sec. ==========", rootfolder.Name, end1.Subtract(start1).TotalSeconds));

            }

            end = DateTime.Now;
            logger.Debug(string.Format("Total get metadata in {0} sec.", end.Subtract(start).TotalSeconds));

            lstClient.Sort(new SortByName());

            logger.Debug(string.Format("Total {0} valid clients", lstClient.Count));
            return lstClient.ToList<Data.BPClient>();
        }

        internal static List<BPProcessStep> GetUSCProcessSteps(UCMSApiClient restClient, Workflow workflow, List<UniFormType> lstOriginalTypes)
        {
            ArrayList arlProcessStep = new ArrayList();

            foreach (var lProcessStep in workflow.Steps)
            {
                if (Global.Activity_UniversalScan.Equals(lProcessStep.Activity.UniqueId))
                {
                    lProcessStep.Setting = restClient.Workflow.GetStepSetting(lProcessStep.Id).Setting;
                    UniParameterSet oUniParameter = GetUniScanParameters(lProcessStep);

                    if (oUniParameter.SettingReference == Global.Own_Setting)
                    {
                        //Synchronize document type (if have hidden field, lookup field, readonly field...)
                        DataHelper.SynchronizeDocumentType(oUniParameter, lstOriginalTypes);
                    }

                    if (oUniParameter.DocumentTypeProfileSetting.UniFormtypeList.Count > 0 || oUniParameter.SettingReference != Global.Own_Setting)
                        arlProcessStep.Add(new BPProcessStep()
                        {
                            Name = lProcessStep.Name,
                            ID = lProcessStep.Id,
                            FeatureName = lProcessStep.Activity.Name,
                            FeatureID = lProcessStep.Activity.UniqueId,
                            uniParameterSet = oUniParameter,
                            Tag = lProcessStep
                        });             

                }
            }

            foreach (BPProcessStep oProcessStep in arlProcessStep)
            {
                    string sSettingRef = oProcessStep.uniParameterSet.SettingReference;
                    if (sSettingRef != Global.Own_Setting)
                    {
                        BPProcessStep oRefStep = FindReferenceProcessStep(sSettingRef, arlProcessStep);
                        if (oRefStep != null)
                        {
                            oProcessStep.uniParameterSet.BatchNamingProfileSetting = oRefStep.uniParameterSet.BatchNamingProfileSetting;
                            oProcessStep.uniParameterSet.DocumentTypeProfileSetting = oRefStep.uniParameterSet.DocumentTypeProfileSetting;
                            oProcessStep.uniParameterSet.ScanProfileSetting = oRefStep.uniParameterSet.ScanProfileSetting;
                            oProcessStep.uniParameterSet.SeparationProfileSetting = oRefStep.uniParameterSet.SeparationProfileSetting;
                            oProcessStep.uniParameterSet.ValidationProfileSetting = oRefStep.uniParameterSet.ValidationProfileSetting;
                        }
                    }
                
            }

            //Sort process step
            arlProcessStep.Sort(new SortByName());

            return arlProcessStep.ToList<BPProcessStep>();
        }

        private static BPProcessStep FindReferenceProcessStep(string sSettingRef, ArrayList arlProcessStep)
        {
            foreach (BPProcessStep oProcessStep in arlProcessStep)
            {
                if (oProcessStep.Name == sSettingRef)
                    return oProcessStep;
            }

            return null;
        }

        private static List<string> GetMemberGroups(string strADUser, string strADAdmin, string strADPassword, string strADDomain, ref string strLastError)
        {
            List<string> lstMemberGroups = new List<string>();

            if (System.Environment.MachineName.ToLower().Equals(strADDomain))
            {
                //local user
                lstMemberGroups = GetGroupsOfMemberUsingWinNT(strADUser, strADDomain);
            }
            else
            {
                //is Domain user
                try
                {
                    lstMemberGroups = GetGroupsOfMemberUsingLDAP(strADUser, strADDomain, strADAdmin, strADPassword);
                }
                catch (Exception ex)
                {
                    string strMessageFormat = Resources.Msg_CannotAccessAD;
                    strLastError = string.Format(strMessageFormat, strADDomain, ex.Message);
                    lstMemberGroups = new List<string>();
                }
            }

            return lstMemberGroups;
        }

        private static List<string> GetMemberGroups(string strADUser, string strADPassword, string strADDomain, ref string strLastError)
        {
            List<string> lstMemberGroups = new List<string>();

            if (System.Environment.MachineName.ToLower().Equals(strADDomain))
            {
                //local user
                lstMemberGroups = GetGroupsOfMemberUsingWinNT(strADUser, strADDomain);
            }
            else
            {
                //is Domain user
                try
                {
                    lstMemberGroups = GetGroupsOfMemberUsingLDAP(strADUser, strADDomain, strADUser, strADPassword);
                }
                catch (Exception ex)
                {
                    string strMessageFormat = Resources.Msg_CannotAccessAD;
                    strLastError = string.Format(strMessageFormat, strADDomain, ex.Message);
                    lstMemberGroups = new List<string>();
                }
            }

            return lstMemberGroups;
        }

        public static GlobalParameterSet GetGlobalParameterSet(UCMSApiClient restClient)
        {
            GlobalParameterSet parameterset = new UCMSConnectorShared.GlobalParameterSet();
            var branchSetting = restClient.Setting.GetCustomSetting(Global.USC_BRANCHES);

            if (branchSetting == null || string.IsNullOrWhiteSpace(branchSetting.Value))
            {
                Branch oBranch = new Branch();
                oBranch.Name = GlobalParameterSet.AllBranch;
                oBranch.Users.Add(new Account() { Name = Account.Everyone });
                parameterset.Branches.Add(oBranch);
            }
            else
            {
                parameterset = (GlobalParameterSet)IMIP.UniversalScan.Common.Helper.DeSerializeObjectFromString(branchSetting.Value, typeof(GlobalParameterSet));
            }

            return parameterset;
        }

        public static List<Branch> GetListBranchId(UCMSApiClient restClient, string strADDomain, string strADUser, string strADPassword, string strGlobalParametersetVersion, ref string LastError)
        {
            ArrayList lstBranch = new ArrayList();
            LastError = "";

            if (strADDomain == null)
                strADDomain = "";
            if (strADPassword == null)
                strADPassword = "";

            //to lower characters
            strADDomain = strADDomain.ToLower().Trim();
            strADUser = strADUser.ToLower().Trim();
            bool bNotGetGroupOfUser = true;

            //Get Group of Member
            List<string> lstMemberGroups = new List<string>();

            GlobalParameterSet oParameter = GetGlobalParameterSet(restClient);
           
            if (oParameter == null)
            {
                oParameter = new GlobalParameterSet();
            }

            if (strGlobalParametersetVersion != null && !strGlobalParametersetVersion.Equals(""))
            {
                if (!oParameter.mVersion.Equals(strGlobalParametersetVersion))
                {
                    throw new ApplicationException(string.Format(Resources.Msg_GlobalParameterVersionNotSupported, oParameter.mVersion));
                }
            }

            foreach (var branch in oParameter.Branches)
            {
                foreach (var acount in branch.Users)
                {
                    if (acount.Name.ToLower() == IMIP.UniversalScan.Data.Account.Everyone.ToLower())
                    {
                        //lstBranch.Add(branch.Name + ";" + branch.CacheBatches + ";" + branch.SharedFolder);
                        lstBranch.Add(branch);
                        break;
                    }
                    else if (acount.IsUser())
                    {
                        string accountDomain = acount.GetDomain().Trim().ToLower();
                        string accountName = acount.GetAccountWithoutDomain().Trim().ToLower();
                        if (strADDomain.Equals(accountDomain)
                                && strADUser.Equals(accountName))
                        {
                            //lstBranch.Add(branch.Name + ";" + branch.CacheBatches + ";" + branch.SharedFolder);
                            lstBranch.Add(branch);
                            break;
                        }
                    }
                    else if (acount.IsGroup())
                    {
                        if (bNotGetGroupOfUser)
                        {
                            if (oParameter.UseExplicitAccountForAD)
                            {
                                if (LastError == "")
                                    lstMemberGroups = GetMemberGroups(strADUser, oParameter.ADAdmin, oParameter.ADPassword, strADDomain, ref LastError);
                            }
                            else
                            {
                                if (LastError == "")
                                    lstMemberGroups = GetMemberGroups(strADUser, strADPassword, strADDomain, ref LastError);
                            }

                            bNotGetGroupOfUser = false;
                        }

                        if (lstMemberGroups.Contains(acount.Name.Trim().ToLower()))
                        {
                            //lstBranch.Add(branch.Name + ";" + branch.CacheBatches + ";" + branch.SharedFolder);
                            lstBranch.Add(branch);
                            break;
                        }
                    }
                }
            }

            lstBranch.Sort(new SortByName());

            return lstBranch.ToList<Branch>();
        }

        private static List<string> GetGroupsOfMemberUsingWinNT(string username, string domain)
        {
            List<string> lstGroups = new List<string>();

            string locPath = "WinNT://" + domain;
            DirectoryEntry localGroup = new DirectoryEntry(locPath + "/" + username + ",user");
            object allGroups = localGroup.Invoke("Groups");
            foreach (object group in (IEnumerable)allGroups)
            {
                DirectoryEntry groupEntry = new DirectoryEntry(group);
                lstGroups.Add(groupEntry.Name.ToLower());
            }

            return lstGroups;
        }

        private static List<string> GetGroupsOfMemberUsingLDAP(string username, string domain, string ADlogin, string ADpassword)
        {
            //This is a function that receives a username to see if it's a
            //member of a specific group in AD.

            List<string> lstGroups = new List<string>();

            string EntryString = "LDAP://" + domain;
            //Above, we setup the LDAP basic entry string.
            DirectoryEntry myDE = default(DirectoryEntry);
            //Above, I dimension my DirectoryEntry object

            if ((!string.IsNullOrEmpty(ADlogin) && !string.IsNullOrEmpty(ADpassword)))
            {
                //If they provided a password, then add it
                //as an argument to the function
                //I recently learned about AndAlso, and it's pretty
                //cool. Basically it does not worry about checking
                //the next condition if the first one is not true.
                myDE = new DirectoryEntry(EntryString, ADlogin, ADpassword);
            }
            else
            {
                //Else, use the account credentials of the machine
                //making the request. You might not be able to get
                //away with this if your production server does not have
                //rights to query Active Directory.
                //Then again, there are workarounds for anything.
                myDE = new DirectoryEntry(EntryString);
            }

            DirectorySearcher myDirectorySearcher = new DirectorySearcher(myDE);
            myDirectorySearcher.Filter = "sAMAccountName=" + username;
            myDirectorySearcher.PropertiesToLoad.Add("MemberOf");
            //We only care about the MemberOf Properties, and we
            //specify that above.

            System.DirectoryServices.SearchResult myresult = myDirectorySearcher.FindOne();
            //SearchResult is a node in Active Directory that is returned
            //during a search through System.DirectoryServices.DirectorySearcher
            //Above, we dim a myresult object, and assign a node returned
            //from myDirectorySearcher.FindOne()
            //I've never heard of similar login Id's in Active Directory,
            //so I don't think we need to call FindAll(), so Instead
            //we call FindOne()
            if (myresult != null)
            {
                int NumberOfGroups = 0;
                NumberOfGroups = myresult.Properties["memberOf"].Count - 1;
                //Above we get the number of groups the user is a memberOf,
                //and store it in a variable. It is zero indexed, so we
                //remove 1 so we can loop through it.
                string tempString = null;
                //A temp string that we will use to get only what we
                //need from the MemberOf string property
                while ((NumberOfGroups >= 0))
                {
                    tempString = myresult.Properties["MemberOf"][NumberOfGroups].ToString();
                    tempString = tempString.Substring(0, tempString.IndexOf(",", 0));
                    //Above we set tempString to the first index of "," starting
                    //from the zeroth element of itself.
                    tempString = tempString.Replace("CN=", "");
                    //Above, we remove the "CN=" from the beginning of the string

                    lstGroups.Add(tempString.Trim().ToLower());
                    //If we have a match, the return is true
                    //username is a member of grouptoCheck
                    NumberOfGroups = NumberOfGroups - 1;
                }
            }

            return lstGroups;
        }


        public static List<string> GetMembersOfGroupUsingWinNT(string groupname, string domain)
        {
            List<string> arrUsers = new List<string>();

            string locPath = "WinNT://" + domain;
            DirectoryEntry localGroup = new DirectoryEntry(locPath + "/" + groupname + ",group");
            object allMembers = localGroup.Invoke("Members");
            foreach (object member in (IEnumerable)allMembers)
            {
                DirectoryEntry groupEntry = new DirectoryEntry(member);
                arrUsers.Add(groupEntry.Name.ToLower());
            }

            return arrUsers;
        }

        public static List<string> GetMembersOfGroupUsingLDAP(string groupname, string domain, string ADlogin, string ADpassword)
        {
            //This is a function that receives a username to see if it's a
            //member of a specific group in AD.

            List<string> lstMembers = new List<string>();

            string EntryString = "LDAP://" + domain;
            //Above, we setup the LDAP basic entry string.
            DirectoryEntry myDE = default(DirectoryEntry);
            //Above, I dimension my DirectoryEntry object

            if ((!string.IsNullOrEmpty(ADlogin) && !string.IsNullOrEmpty(ADpassword)))
            {
                //If they provided a password, then add it
                //as an argument to the function
                //I recently learned about AndAlso, and it's pretty
                //cool. Basically it does not worry about checking
                //the next condition if the first one is not true.
                myDE = new DirectoryEntry(EntryString, ADlogin, ADpassword);
            }
            else
            {
                //Else, use the account credentials of the machine
                //making the request. You might not be able to get
                //away with this if your production server does not have
                //rights to query Active Directory.
                //Then again, there are workarounds for anything.
                myDE = new DirectoryEntry(EntryString);
            }
            DirectorySearcher myDirectorySearcher = new DirectorySearcher(myDE);
            string query = "(&(objectCategory=person)(objectClass=user)(memberOf=*))";
            myDirectorySearcher.Filter = query;
            myDirectorySearcher.PropertiesToLoad.Add("memberOf");
            myDirectorySearcher.PropertiesToLoad.Add("name");
            myDirectorySearcher.PropertiesToLoad.Add("SAMAccountName");

            System.DirectoryServices.SearchResultCollection mySearchResultColl = myDirectorySearcher.FindAll();

            foreach (System.DirectoryServices.SearchResult result in mySearchResultColl)
            {
                foreach (string prop in result.Properties["memberOf"])
                {
                    if (prop.Contains(groupname))
                    {
                        lstMembers.Add(result.Properties["SAMAccountName"][0].ToString());
                    }
                }
            }


            return lstMembers;
        }

        public static UniParameterSet GetUniScanParameters(WorkflowStep step)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(step.Setting))
                {
                    return (UniParameterSet)Helper.DeSerializeObjectFromString(step.Setting, typeof(UniParameterSet));
                }

                return  new UniParameterSet();
            }
            catch(Exception ex)
            {
                return new UniParameterSet();
            }
        }

        private  static object  DeSerializeObject(byte[] arrByte, System.Type oType)
        {
            object oReturn = null;
            using (MemoryStream stream = new MemoryStream(arrByte))
            {
                XmlSerializer serializer = new XmlSerializer(oType);
                oReturn = serializer.Deserialize(stream);
                stream.Close();
            }

            return oReturn;
        }
    }
}
