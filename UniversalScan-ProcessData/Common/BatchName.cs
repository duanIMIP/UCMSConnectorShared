using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMIP.SharedComponent.CustomMapping;
using IMIP.UniversalScan.Data;
using IMIP.UniversalScan.Common;
using IMIP.UniversalScan.Connector;
using IMIP.UniversalScan.Def;
using IMIP.UniversalScan.Profile;

namespace IMIP.UniversalScan.Common
{
    public class DocumentNameBuilder
    {
        public virtual string BuildDocumentName(UniDocument oDoc)
        {
            if (oDoc.UniFormType == null || !oDoc.UniFormType.EnableDocumentNaming())
                return "";

            string batchName = "";
            foreach (SourceField exportValue in oDoc.UniFormType.DocumentNamingSetting.BatchNamingTemplate)
            {
                switch (exportValue.Type)
                {
                    case SourceFieldType.DocVariable:
                        batchName += GetDocumentVariableValue(oDoc, exportValue.StaticName);
                        break;

                    case SourceFieldType.DocIndexField:
                        batchName += GetIndexFieldValue(oDoc.Fields, exportValue.StaticName);
                        break;

                    case SourceFieldType.System:
                        batchName += GetSystemVariableValue(oDoc, exportValue.StaticName);
                        break;

                    case SourceFieldType.TextConstant:
                        //If text constant, the value must get by DisplayName
                        batchName += exportValue.DisplayName;
                        break;
                }
            }

            //if (!string.IsNullOrEmpty(batchName))
            oDoc.Name = batchName;

            return batchName;
        }

        public virtual string GetIndexFieldValue(List<UniField> fields, string indexField)
        {
            foreach (UniField field in fields)
            {
                if (field.Name == indexField)
                    return field.Value;
            }

            return "";
        }

        public virtual string GetSystemVariableValue(UniDocument oDoc, string pVariable)
        {
            string sValue = "";

            if (pVariable == ScanCommon.ConstantString.SystemMachineName)
            {
                sValue = System.Environment.MachineName;
            }
            else if (pVariable == ScanCommon.ConstantString.SystemUserName)
            {
                sValue = System.Environment.UserName;
            }
            else if (pVariable == ScanCommon.ConstantString.SystemDate)
            {
                sValue = oDoc.CreationDate.ToString(oDoc.UniFormType.DocumentNamingSetting.DateFormat);
            }
            else if (pVariable == ScanCommon.ConstantString.SystemTime)
            {
                sValue = oDoc.CreationDate.ToString(oDoc.UniFormType.DocumentNamingSetting.TimeFormat);
            }
            return sValue;
        }

        public virtual string GetDocumentVariableValue(UniDocument oDoc, string pVariable)
        {
            string sValue = "";

            if (pVariable == ScanCommon.ConstantString.DocType)
            {
                sValue = oDoc.UniFormType.Name;
            }
            else if (pVariable == ScanCommon.ConstantString.DocName)
            {
                sValue = oDoc.Name;
            }
            else if (pVariable == ScanCommon.ConstantString.DocSequence)
            {
                sValue = (oDoc.Index + 1).ToString();
            }

            return sValue;
        }

    }
}
