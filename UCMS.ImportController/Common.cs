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

namespace UCMS.ImportController
{
    public class Common
    {
        public static String Username = ConfigurationSettings.AppSettings["Username"].ToString();
        public static String Password = ConfigurationSettings.AppSettings["Password"].ToString();
        public static String UCMSWebAPIEndPoint = ConfigurationSettings.AppSettings["UCMSWebAPIEndPoint"].ToString();
        public static String UCMSAuthorizationServer = ConfigurationSettings.AppSettings["UCMSAuthorizationServer"].ToString();

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

        public static void LogToFile(string message)
        {
            try
            {
                message = DateTime.Now.ToString("HH:mm:ss") + " " + message;
                string logPath = string.Empty;
                logPath = @"C:\UCMSLog\" + DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("HH");
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
