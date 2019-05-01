﻿using IMIP.SharedComponent.CustomMapping;
using IMIP.UniversalScan.Data;
using IMIP.UniversalScan.Def;
using IMIP.UniversalScan.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCMS.ImportController.Data;
using UCMS.RestClient;

namespace UCMS.ImportController
{
    public class GetData
    {
        public static List<Branch> GetBranch(UCMSApiClient oUCMSApiClient)
        {
            List<Branch> list = new List<Branch>();
            Model.CustomSetting customsetting = oUCMSApiClient.Setting.GetCustomSetting("USCBranches");
            if (customsetting != null && customsetting.Value != "")
            {
                ActivityConfiguration oActivityConfiguration = Common.DeSerializeFromString<ActivityConfiguration>(customsetting.Value, "ActivityConfiguration");
                if (oActivityConfiguration != null && oActivityConfiguration.Branches != null)
                {
                    foreach (Branch item in oActivityConfiguration.Branches)
                    {
                        if (item.Users.Find(x => x.Name.Equals(Common.Username)|| x.Name.Equals("Everyone")) != null)
                        {
                            list.Add(item);
                        }
                    }
                }
            }
            return list;
        }

        public static List<Model.Folder> GetFolder(UCMSApiClient oUCMSApiClient)
        {
            var ListFolder = oUCMSApiClient.Folder.GetFolders("");
            if (ListFolder == null || ListFolder.Items == null)
            {
                return new List<Model.Folder>();
            }
            return ListFolder.Items;
        }

        public static List<Model.Workflow> GetWorkflow(UCMSApiClient oUCMSApiClient, String FolderId)
        {
            List<Model.Workflow> ListFlow = oUCMSApiClient.Workflow.GetAll(FolderId);
            if (ListFlow == null)
            {
                return new List<Model.Workflow>();
            }
            return ListFlow;
        }

        public static ActivityConfiguration GetContentType(UCMSApiClient oUCMSApiClient, string WorkflowStepId)
        {
            Model.WorkflowStepSetting workFlowStep = oUCMSApiClient.Workflow.GetStepSetting(WorkflowStepId);
            ActivityConfiguration UniFormType = new ActivityConfiguration();
            if (workFlowStep != null && !string.IsNullOrEmpty(workFlowStep.Setting))
            {
                UniFormType = Common.DeSerializeFromString<ActivityConfiguration>(workFlowStep.Setting, "ActivityConfiguration");
            }
            return UniFormType==null? new ActivityConfiguration(): UniFormType;
        }

        public static List<UniFieldDefinition> GetListContentField(UCMSApiClient oUCMSApiClient, UniFormType oUniFormType)
        {
            Model.ContentType contentType = oUCMSApiClient.ContentType.GetById(oUniFormType.ExternalID);
            List<UniFieldDefinition> Listfield = new List<UniFieldDefinition>();
            foreach (UniFieldDefinition item in oUniFormType.FieldDefs)
            {
                if(contentType.Fields.SingleOrDefault(x=>x.Name.Equals(item.Name)) != null)
                {
                    Listfield.Add(item);
                }
            }
            return Listfield;
        }

        public static List<UniFieldDefinition> GetListLibraryField(UCMSApiClient oUCMSApiClient, UniFormType oUniFormType, String FolderId)
        {
            Model.Library library = oUCMSApiClient.Folder.GetLibrary(FolderId);
            List<UniFieldDefinition> Listfield = new List<UniFieldDefinition>();
            foreach (UniFieldDefinition item in oUniFormType.FieldDefs)
            {
                if (library.Fields.SingleOrDefault(x => x.Name.Equals(item.Name)) != null)
                {
                    Listfield.Add(item);
                }
            }
            return Listfield;
        }

        public static String Naming(string ContentTypeName, BatchNamingProfile oBatchNamingProfile)
        {
            String tempName = "";
            if (oBatchNamingProfile != null && oBatchNamingProfile.Enabled && oBatchNamingProfile.BatchNamingSettings != null)
            {
                BatchNamingSetting oNaming = oBatchNamingProfile.BatchNamingSettings.SingleOrDefault(x => x.DocumentTypeName.Equals(ContentTypeName));
                if (oNaming != null)
                {
                    foreach (SourceField oSourceField in oNaming.BatchNamingTemplate)
                    {
                        switch (oSourceField.Type)
                        {
                            case SourceFieldType.DocVariable:
                                tempName += ContentTypeName;
                                break;
                            case SourceFieldType.System:
                                if (oSourceField.StaticName.Equals("Date"))
                                {
                                    tempName += DateTime.Now.ToString(oNaming.DateFormat);
                                }
                                else if (oSourceField.StaticName.Equals("Time"))
                                {
                                    tempName += DateTime.Now.ToString(oNaming.TimeFormat);
                                }
                                else if (oSourceField.StaticName.Equals("Machine Name"))
                                {
                                    tempName += DateTime.Now.ToString(Environment.MachineName.ToString());
                                }
                                else if (oSourceField.StaticName.Equals("User Name"))
                                {
                                    tempName += DateTime.Now.ToString(Common.Username);
                                }
                                else
                                {
                                    tempName += oSourceField.DisplayName;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            return tempName!=""? tempName:ContentTypeName + DateTime.Now.ToString("yyMMddHHmmssff");
        }
    }
}