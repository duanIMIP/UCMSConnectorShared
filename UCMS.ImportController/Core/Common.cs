using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Xml;

namespace UCMS.ImportController
{
    public class Common
    {
        public static String Username = ConfigurationSettings.AppSettings["Username"].ToString();
        public static String Password = ConfigurationSettings.AppSettings["Password"].ToString();
        public static String UCMSWebAPIEndPoint = ConfigurationSettings.AppSettings["UCMSWebAPIEndPoint"].ToString();
        public static String UCMSAuthorizationServer = ConfigurationSettings.AppSettings["UCMSAuthorizationServer"].ToString();
        public static Int32 PoolTime = Convert.ToInt32(ConfigurationSettings.AppSettings["PoolTime"]);

        public static Int32 MaxTimeUpdate = Convert.ToInt32(ConfigurationSettings.AppSettings["MaxTimeUpdate"]);

        public static String SettingReferenceDefault = "--- Own Setting ---";

        public static String[] WFStepProcessAuto = ConfigurationSettings.AppSettings["WFStepProcessAuto"].Split(';');

        public static string SerializeToString(System.Type oType, object objectToSerialize)
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

        public static object DeSerializeFromByte(byte[] arrByte, System.Type oType)
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

        public static object DeSerializeFromFile(string filename, System.Type oType)
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

        public static T DeSerializeFromString<T>(string value, string ElementName) where T : class
        {
            TextReader textReader = null;
            try
            {
                XmlRootAttribute xRoot = new XmlRootAttribute();
                xRoot.ElementName = ElementName;
                xRoot.IsNullable = true;
                XmlSerializer deserializer = new XmlSerializer(typeof(T), xRoot);
                textReader = new StringReader(value);
                return deserializer.Deserialize(textReader) as T;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetBaseException());
                return null;
            }
            finally
            {
                if (textReader != null)
                    textReader.Close();
            }
        }

        public static T DeserializeFromNode<T>(XmlNode node) where T : class
        {
            TextReader textReader = null;
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(T));
                textReader = new StringReader(node.OuterXml);
                return deserializer.Deserialize(textReader) as T;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (textReader != null)
                    textReader.Close();
            }
        }

        public static void LogToFile(string message)
        {
            try
            {
                message = DateTime.Now.ToString("HH:mm:ss") + " " + message;
                string logPath = string.Empty;
                logPath = @"UCMSLog\" + DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("HH");
                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }
                logPath = logPath + @"\EXCEPTION.log";

                TextWriterTraceListener listener = new TextWriterTraceListener(logPath);
                listener.WriteLine(message);
                listener.Flush();
                listener.Close();
            }
            catch
            {

            }
        }

        public static String WriteToFile(string fileName, String message)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    return "File name exists";
                }
                TextWriterTraceListener listener = new TextWriterTraceListener(fileName);
                listener.WriteLine(message);
                listener.Flush();
                listener.Close();
                return "Write file successfully";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static ResponseObject GetBy(string requestUri)
        {
            ResponseObject res = new ResponseObject();
            HttpResponseMessage response = null;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(UCMSWebAPIEndPoint);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    //client.DefaultRequestHeaders.Add("Key", Key);
                    response = client.GetAsync(requestUri).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        res.Value = response.Content.ReadAsAsync<Object>().Result;
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = ex.Message;
            }
            return res;
        }
        public static ResponseObject GetAll(string requestUri)
        {
            ResponseObject res = new ResponseObject();
            HttpResponseMessage response = null;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(UCMSWebAPIEndPoint);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    //client.DefaultRequestHeaders.Add("Key", Key);
                    client.DefaultRequestHeaders.Add("Authorization", string.Format("Basic {0}", ""));
                    response = client.GetAsync(requestUri).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        res.ListValue = response.Content.ReadAsAsync<List<Object>>().Result;
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = ex.Message;
            }
            return res;
        }

        public static ResponseObject POST(string requestUri, object requestBody)
        {
            ResponseObject res = new ResponseObject();
            HttpResponseMessage response = null;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(UCMSWebAPIEndPoint);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    //client.DefaultRequestHeaders.Add("Key", Key);
                    response = client.PostAsJsonAsync(requestUri, requestBody).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        res = response.Content.ReadAsAsync<ResponseObject>().Result;
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = ex.Message;
            }
            return res;
        }
    }

    public class ResponseObject
    {
        public string Code { get; set; }
        public List<object> ListValue { get; set; }
        public string Message { get; set; }
        public string Time { get; set; }
        public object Value { get; set; }
    }
}
