/****************************************************************************
'*   (c) Copyright IMIP Technology And Solution Consultancy JSC., 2010. All rights reserved.
'*   Unauthorized use, duplication or distribution is strictly prohibited.
'*****************************************************************************
'*
'*   File:       Helper.cs
'*
'*   Purpose:    Helper class to serialize and deserialize objects
*********************************************************************************/
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using IMIP.UniversalScan.Data;
using System;
using IMIP.UniversalScan.Def;

namespace IMIP.UniversalScan.Common
{
    public class SortUniFieldIndex : IComparer<UniField>
    {
        int IComparer<UniField>.Compare(UniField x, UniField y)
        {
            if (x.FieldDef == null || y.FieldDef == null)
            {
                return 0;
            }

            return x.FieldDef.Index.CompareTo(y.FieldDef.Index);
        }
    }

    public class SortUniFieldDefinitionByIndex : IComparer<UniFieldDefinition>
    {
        int IComparer<UniFieldDefinition>.Compare(UniFieldDefinition x, UniFieldDefinition y)
        {
            return x.Index.CompareTo(y.Index);
        }
    }

    public static class Extensions
    {
        public static object UscClone(this object oObject)
        {
            Type oType = oObject.GetType();
            string strSetting = UniversalScan.Common.Helper.SerializeObjectToString(oType, oObject);
            return UniversalScan.Common.Helper.DeSerializeObjectFromString(strSetting, oType);
        }

        public static void SortByIndex(this List<UniFieldDefinition> lstFieldDefs)
        {
            lstFieldDefs.Sort(new SortUniFieldDefinitionByIndex());
        }

        public static void SortByIndex(this List<UniField> lstField)
        {
            lstField.Sort(new SortUniFieldIndex());
        }
    }

    public class Helper
    {
        public static void SerializeObject(string filename, System.Type oType, object objectToSerialize)
        {
            TextWriter textWriter = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(oType);
                textWriter = new StreamWriter(filename);
                serializer.Serialize(textWriter, objectToSerialize);
                
            }
            finally
            {
                if (textWriter != null)
                    textWriter.Close();
            }
        }

        public static object DeSerializeObject(string filename, System.Type oType)
        {
            TextReader textReader = null;
            object oBatch;
            try
            {
                XmlSerializer deserializer = new XmlSerializer(oType);
                textReader = new StreamReader(filename);

                oBatch = deserializer.Deserialize(textReader);
            }
            finally
            {
                if (textReader != null)
                    textReader.Close();
            }

            return oBatch;
        }

        public static string SerializeObjectToString(System.Type oType, object objectToSerialize)
        {
            TextWriter textWriter = null;
            MemoryStream oMemoryStream = new MemoryStream();
            try
            {
                XmlSerializer serializer = new XmlSerializer(oType);
                textWriter = new StreamWriter(oMemoryStream);
                serializer.Serialize(textWriter, objectToSerialize);
                return System.Text.Encoding.UTF8.GetString(oMemoryStream.ToArray());
            }
            finally
            {
                if (textWriter != null)
                    textWriter.Close();
            }
        }

        public static object DeSerializeObjectFromString(string value, System.Type oType)
        {
            TextReader textReader = null;
            object oBatch;
            try
            {
                XmlSerializer deserializer = new XmlSerializer(oType);
                textReader = new StringReader(value);

                oBatch = deserializer.Deserialize(textReader);
            }
            finally
            {
                if (textReader != null)
                    textReader.Close();
            }

            return oBatch;
        }

        public static object DeSerializeObject(byte[] arrByte, System.Type oType)
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

        public static void CopyDirectory(string srcDirName, string destDirName)
        {
            if (!Directory.Exists(destDirName))
                Directory.CreateDirectory(destDirName);

            String[] files = Directory.GetFileSystemEntries(srcDirName);

            foreach (string strFile in files)
            {
                string strDest = Path.Combine(destDirName, Path.GetFileName(strFile));
                // Sub directories
                if (Directory.Exists(strFile))
                    CopyDirectory(strFile, strDest);
                // Files in directory
                else
                    File.Copy(strFile, strDest, true);
            }
        }

        public static object CloneObject(object oObject, Type oType)
        {
            //Serialize object to string
            string object2String = SerializeObjectToString(oType, oObject);

            //Deserilize string to new object
            return DeSerializeObjectFromString(object2String, oType);
        }
    }
}
