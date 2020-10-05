using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

using Newtonsoft.Json;

namespace ARMInfo
{
    public class Report
    {
        public int page_number;
        public int code;
    }
    public class Report<T> : Report
    {
        public List<T> data;
        public List<T> Load(string uri)
        {
            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                var json = client.DownloadString(uri);
                return JsonConvert.DeserializeObject<Report<T>>(json).data;
            }
        }

        public class JsonResultModel
        {
            public string ErrorMessage { get; set; }
            public bool IsSuccess { get; set; }
            public string Results { get; set; }
        }

        public JsonResultModel Update(string Url, T Data)
        {
            JsonResultModel model = new JsonResultModel();
            string Out = String.Empty;
            string Error = String.Empty;
            System.Net.WebRequest req = System.Net.WebRequest.Create(Url);

            try
            {
                req.Method = "PUT";
                req.Timeout = 100000;
                req.ContentType = "application/json";
                var serializedData = JsonConvert.SerializeObject(Data);
                byte[] sentData = Encoding.UTF8.GetBytes(serializedData);
                req.ContentLength = sentData.Length;

                using (System.IO.Stream sendStream = req.GetRequestStream())
                {
                    sendStream.Write(sentData, 0, sentData.Length);
                    sendStream.Close();
                }

                System.Net.WebResponse res = req.GetResponse();
                System.IO.Stream ReceiveStream = res.GetResponseStream();
                using (System.IO.StreamReader sr = new
                System.IO.StreamReader(ReceiveStream, Encoding.UTF8))
                {

                    Char[] read = new Char[256];
                    int count = sr.Read(read, 0, 256);

                    while (count > 0)
                    {
                        String str = new String(read, 0, count);
                        Out += str;
                        count = sr.Read(read, 0, 256);
                    }
                }
            }
            catch (ArgumentException ex)
            {
                Error = string.Format("HTTP_ERROR :: The second HttpWebRequest object has raised an Argument Exception as 'Connection' Property is set to 'Close' :: {0}", ex.Message);
            }
            catch (WebException ex)
            {
                Error = string.Format("HTTP_ERROR :: WebException raised! :: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                Error = string.Format("HTTP_ERROR :: Exception raised! :: {0}", ex.Message);
            }

            model.Results = Out;
            model.ErrorMessage = Error;
            if (!string.IsNullOrEmpty(Out.Trim()))
            {
                var res = JsonConvert.DeserializeObject<Report>(model.Results);

                if (res.code == 0)
                {
                    model.IsSuccess = true;
                }
                else { model.IsSuccess = false; }
            }
            return model;
        }

        public JsonResultModel Post(string Url, T Data)
        {
            JsonResultModel model = new JsonResultModel();
            string Out = String.Empty;
            string Error = String.Empty;
            System.Net.WebRequest req = System.Net.WebRequest.Create(Url);

            try
            {
                req.Method = "POST";
                req.Timeout = 100000;
                req.ContentType = "application/json";
                var serializedData = JsonConvert.SerializeObject(Data);
                byte[] sentData = Encoding.UTF8.GetBytes(serializedData);
                req.ContentLength = sentData.Length;

                using (System.IO.Stream sendStream = req.GetRequestStream())
                {
                    sendStream.Write(sentData, 0, sentData.Length);
                    sendStream.Close();
                }

                System.Net.WebResponse res = req.GetResponse();
                System.IO.Stream ReceiveStream = res.GetResponseStream();
                using (System.IO.StreamReader sr = new
                System.IO.StreamReader(ReceiveStream, Encoding.UTF8))
                {

                    Char[] read = new Char[256];
                    int count = sr.Read(read, 0, 256);

                    while (count > 0)
                    {
                        String str = new String(read, 0, count);
                        Out += str;
                        count = sr.Read(read, 0, 256);
                    }
                }
            }
            catch (ArgumentException ex)
            {
                Error = string.Format("HTTP_ERROR :: The second HttpWebRequest object has raised an Argument Exception as 'Connection' Property is set to 'Close' :: {0}", ex.Message);
            }
            catch (WebException ex)
            {
                Error = string.Format("HTTP_ERROR :: WebException raised! :: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                Error = string.Format("HTTP_ERROR :: Exception raised! :: {0}", ex.Message);
            }

            model.Results = Out;
            model.ErrorMessage = Error;
            if (!string.IsNullOrEmpty(Out.Trim()))
            {
                var res = JsonConvert.DeserializeObject<Report>(model.Results);

                if (res.code == 0)
                {
                    model.IsSuccess = true;
                }
                else { model.IsSuccess = false; }
            }
            return model;
        }
    }
}
