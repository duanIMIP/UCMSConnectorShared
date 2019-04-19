using System.Collections.Generic;
using System.Linq;
using IMIP.UniversalScan.Profile;
using IMIP.UniversalScan.Def;
using UCMS.Model;
using IMIP.UniversalScan.Common;
using UCMS.Model.Enum;

namespace IMIP.UniversalScan.Connector.UCMSConnectorShared
{
    public class DataHelper
    {
        public static UniFormType GetUniFormType(ContentType contentType, List<LibraryField> lstLibFields)
        {
            UniFormType oType = new UniFormType() { Name = contentType.Name };
            oType.ExternalID = contentType.Id;

            for (int i = 0; i < lstLibFields.Count; i++)
            {
                var fieldDefinition = lstLibFields[i];
                var uniDefField = DataHelper.CreateUniFieldDefinition(fieldDefinition);

                uniDefField.Index = i;
                oType.FieldDefs.Add(uniDefField);
            }

            for (int i = 0; i < contentType.Fields.Count; i++)
            {
                var fieldDefinition = contentType.Fields[i];
                var uniDefField = DataHelper.CreateUniFieldDefinition(fieldDefinition);

                uniDefField.Index = i + lstLibFields.Count;
                oType.FieldDefs.Add(uniDefField);
            }
            return oType;
        }

        public static UniFormType GetUniFormType(ContentType contentType)
        {
            UniFormType oType = new UniFormType() { Name = contentType.Name };
            oType.ExternalID = contentType.Id;

            for (int i = 0; i < contentType.Fields.Count; i++)
            {
                var fieldDefinition = contentType.Fields[i];
                var uniDefField = DataHelper.CreateUniFieldDefinition(fieldDefinition);

                uniDefField.Index = i;
                oType.FieldDefs.Add(uniDefField);
            }
            return oType;
        }

        public static List<UniPreDefineField> GetPreDefineField(List<ContentType> lstContentTypes)
        {
            List<UniPreDefineField> lstPreDef = new List<UniPreDefineField>();
            foreach (var contentType in lstContentTypes)
            {
                foreach (var field in contentType.Fields)
                {
                    if (field.DataType == DataType.MultiSelect || field.DataType == DataType.Picklist)
                    {
                        UniPreDefineField preDef = new UniPreDefineField();
                        preDef.Name = field.Id;
                        preDef.UniqueID = field.Id;
                        preDef.PreDefineValues = field.Items;
                        lstPreDef.Add(preDef);
                    }

                    if (field.DataType == DataType.Lookup)
                    {
                        if (field.Items != null)
                        {
                            UniPreDefineField preDef = new UniPreDefineField();
                            preDef.Name = field.Id;
                            preDef.UniqueID = field.Id;
                            preDef.PreDefineValues = field.Items;
                            lstPreDef.Add(preDef);
                        }
                    }
                }
            }

            return lstPreDef;
        }

        public static List<UniFormType> GetUniFormTypes(List<ContentType> lstContentTypes, List<LibraryField> lstLibFields)
        {
            var doctypes = new List<UniFormType>();
            foreach (var item in lstContentTypes)
            {
                UniFormType oType = GetUniFormType(item, lstLibFields);
                doctypes.Add(oType);
            }

            return doctypes;
        }
        private static void SyncDocFields(UniFormType originalDocType, UniFormType paraType)
        {
            for (int i = 0; i < originalDocType.FieldDefs.Count; i++)
            {
                UniFieldDefinition originalField = originalDocType.FieldDefs[i];

                var paraField = paraType.FieldDefs.Where(f => f.Name == originalField.Name).FirstOrDefault();
                if (paraField != null)
                {
                    originalField.DefaultValue = paraField.DefaultValue;
                    originalField.Mandatory = paraField.Mandatory;
                    originalField.Visible = paraField.Visible;
                    originalField.ReadOnly = paraField.ReadOnly;
                    originalField.ValueFromBarcode = paraField.ValueFromBarcode;
                    originalField.ValidationFormat = paraField.ValidationFormat;
                    //originalField.Index = 0;
                    //paraField.Index = 0;
                    originalField.Index = paraField.Index > 0 ? paraField.Index : originalField.Index;
                    paraField = originalField;
                }
            }
        }

        public static string SynchronizeDocumentType(UniParameterSet oUniParameterSet, List<UniFormType> lstOriginalUniDocTypes)
        {
            for (int i = oUniParameterSet.DocumentTypeProfileSetting.UniFormtypeList.Count - 1; i >= 0 ; i--)
            {
                UniFormType paraType = oUniParameterSet.DocumentTypeProfileSetting.UniFormtypeList[i];

                UniFormType originalType = lstOriginalUniDocTypes.Where(t => t.Name == paraType.Name).FirstOrDefault();
                
                //sychronyzine doc field
                if (originalType != null)
                {
                    paraType.ExternalID = originalType.ExternalID;
                    SyncDocFields(originalType, paraType);

                    paraType.FieldDefs = originalType.FieldDefs;
                    paraType.FieldDefs.SortByIndex();

                    //oUniParameterSet.DocumentTypeProfileSetting.UniFormtypeList[i] = originalType;
                }
                else
                {
                    oUniParameterSet.DocumentTypeProfileSetting.UniFormtypeList.Remove(paraType);
                }
            }

            return "";
        }

        private static bool DoesListContainFormType(string sDocID, List<UniFormType> oList)
        {
            var ret = oList.Where(d => d.ExternalID == sDocID).FirstOrDefault();
            return ret != null;

        }

        private static bool UpdateUniFieldDefinition(UniFieldDefinition oUniFieldDef, UniFieldDefinition originalField)
        {
            bool result = false;

            if (oUniFieldDef.Name != originalField.Name)
            {
                oUniFieldDef.Name = originalField.Name;
                result = true;
            }

            if (oUniFieldDef.DataType != originalField.DataType)
            {
                oUniFieldDef.DataType = originalField.DataType;
                result = true;
            }

            return result;
        }

        private static bool SynchronizeFieldDefinition(UniFormType oUniFormType, UniFormType originalDocType)
        {
            List<UniFieldDefinition> onewFieldCol = new List<UniFieldDefinition>();
            bool result = false;

            //check if uni field def exist in primus, compare via external id
            foreach (UniFieldDefinition oUniFieldDef in oUniFormType.FieldDefs)
            {
                //DtoStorageDefinition oxBoundFieldDef = GetxBoundFieldDefinition(oUniFieldDef.ExternalID, originalDocType);

                UniFieldDefinition originalField = originalDocType.FieldDefs.Where(f => f.ExternalID == oUniFieldDef.ExternalID).FirstOrDefault();
                if (originalField != null)
                {
                    result = UpdateUniFieldDefinition(oUniFieldDef, originalField);
                    //prevent duplicate
                    if (!DoesListContainField(oUniFieldDef.ExternalID, onewFieldCol))
                        onewFieldCol.Add(oUniFieldDef);                        
                }               
            }

            //check if new field just added to xbound which does not exist in uni field list yet
            foreach (var originalField in originalDocType.FieldDefs)
            {
                string externalID = originalField.ExternalID;
                if (!DoesListContainField(externalID, oUniFormType.FieldDefs))
                {
                    //UniFieldDefinition oUniFieldDef = CreateUniFieldDefinition(oxBoundFieldDef);
                    //onewFieldCol.Add(oUniFieldDef);

                    onewFieldCol.Add(originalField);
                    result = true;
                }
            }

            oUniFormType.FieldDefs = onewFieldCol;
            

            return result;
        }

        private static bool DoesListContainField(string fielExternalID, List<UniFieldDefinition> oFieldList)
        {
            
            foreach (UniFieldDefinition oField in oFieldList)
                if (oField.ExternalID == fielExternalID)
                    return true;

            return false;
        }

        public static UniFieldDefinition CreateUniFieldDefinition(Field fieldDefinition)
        {
            UniFieldDefinition uniDefField = new UniFieldDefinition();
            uniDefField.Name = fieldDefinition.Name;
            uniDefField.DisplayName = fieldDefinition.Label;
            uniDefField.ExternalID = fieldDefinition.Id;

            switch (fieldDefinition.DataType)
            {
                case DataType.Text:
                    uniDefField.DataType = UniFieldDataType.StringType;
                    break;
                case DataType.TextArea:
                    uniDefField.DataType = UniFieldDataType.StringType;
                    uniDefField.MultiLine = true;
                    break;
                case DataType.Checkbox:
                    uniDefField.DataType = UniFieldDataType.BoolType;
                    break;
                case DataType.Number:
                    uniDefField.DataType = UniFieldDataType.FloatType;
                    break;
                case DataType.Date:
                    uniDefField.DataType = UniFieldDataType.DateTimeType;
                    break;
                case DataType.DateTime:
                    uniDefField.DataType = UniFieldDataType.DateTimeType;
                    break;
                case DataType.Picklist:
                    uniDefField.DataType = UniFieldDataType.StringType;
                    uniDefField.PreDefineListID = fieldDefinition.Id;
                    break;
                case DataType.MultiSelect:
                    uniDefField.DataType = UniFieldDataType.StringType;
                    uniDefField.MultiChoice = true;
                    uniDefField.PreDefineListID = fieldDefinition.Id;
                    break;
                case DataType.Lookup:
                    uniDefField.DataType = UniFieldDataType.LookupType;
                    uniDefField.PreDefineListID = fieldDefinition.LookupField + "|" + fieldDefinition.LookupType;
                    break;
                case DataType.Table:
                    uniDefField.DataType = UniFieldDataType.Table;
                    for (int i = 0; i < fieldDefinition.Items.Count; i++)
                    {
                        UniColumn oCol = new Def.UniColumn() { ColumnName = fieldDefinition.Items[i], ColumnType = UniFieldDataType.StringType };
                        uniDefField.Columns.Add(oCol);
                    }
                    break;
                default:
                    uniDefField.DataType = UniFieldDataType.StringType;
                    break;
            }

            return uniDefField;
        }

    }
}
