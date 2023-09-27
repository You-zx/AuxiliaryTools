using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace Hp.Base
{
    /// <summary>
    /// 请求
    /// </summary>
    public class RTS
    {

        /// <summary>
        /// 设置请求参数
        /// </summary>
        /// <param name="Name">参数名集合</param>
        /// <param name="Values">参数值集合</param>
        /// <returns>返回结构</returns>
        public static string SetArgs(string[] Name, string[] Values)
        {
            string result = string.Empty;
            for (int i = 0; i < Name.Length; i++)
            {
                result += i == 0 ? "" : "&";
                result += string.Format("{0}={1}", Name[i], HttpUtility.UrlEncode(FormatValue(Values[i]).ToString().Trim()));
            }
            return result;
        }

        /// <summary>
        /// 基础格式转换
        /// </summary>
        /// <param name="Value">需要转换的参数</param>
        /// <param name="isNum">true 空值返回0，false 空值返回 ""</param>
        /// <returns></returns>
        public static object FormatValue(object Value, bool isNum = false)
        {
            return Value == null || string.IsNullOrEmpty(Value.ToString()) ? (isNum ? "0" : "") : Value;
        }

        /// <summary>
        /// GET 请求
        /// </summary>
        /// <param name="Url">请求地址</param>
        /// <param name="postDataStr">请求参数</param>
        /// <returns></returns>
        public static string GET(string Url, string postDataStr = "")
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        /// <summary>
        /// POST 请求
        /// </summary>
        /// <param name="Url">请求地址</param>
        /// <param name="postDataStr">请求参数</param>
        /// <returns></returns>
        public static string POST(string Url, string postDataStr = "")
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postDataStr.Length;
            StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.ASCII);
            writer.Write(postDataStr);
            writer.Flush();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string encoding = response.ContentEncoding;
            if (encoding == null || encoding.Length < 1)
            {
                encoding = "UTF-8";
            }
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
            string retString = reader.ReadToEnd();
            return retString;
        }

    }

}
