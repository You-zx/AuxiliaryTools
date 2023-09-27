using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Hp.Data
{
    /// <summary>
    /// XML转换
    /// </summary>
    public class XmlSerializer
    {
        /// <summary>
        /// 将对象序列化为XML字符串
        /// </summary>
        /// <returns></returns>
        public static string ToXml(object obj)
        {

            StringWriter Output = new StringWriter(new StringBuilder());
            try
            {
                string Ret = String.Empty;
                System.Xml.Serialization.XmlSerializer s = new System.Xml.Serialization.XmlSerializer(obj.GetType());
                s.Serialize(Output, obj);
                Ret = Output.ToString();
                Ret = Ret.Replace(" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
                Ret = Ret.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "");
                Ret = Ret.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "<?xml version=\"1.0\" encoding=\"gb2312\"?>").Trim();
                Output.Close();
                return Ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Output != null)
                {
                    Output.Close();
                }
            }
        }

        /// <summary>
        /// 将XML字符串中反序列化为对象
        /// </summary>
        /// <param name="Xml">待反序列化的xml字符串</param>
        /// <returns></returns>
        public static T FromXml<T>(string xml) where T : class
        {
            StringReader stringReader = new StringReader(xml);
            XmlTextReader xmlReader = new XmlTextReader(stringReader);
            try
            {
                T obj = default(T);
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                if (serializer.CanDeserialize(xmlReader))
                {
                    obj = serializer.Deserialize(xmlReader) as T;
                }
                xmlReader.Close();
                stringReader.Close();
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (xmlReader != null)
                {
                    xmlReader.Close();
                }
                if (stringReader != null)
                {
                    stringReader.Close();
                }
            }
        }

        /// <summary>
        /// 从xml文件中反序列化对象
        /// </summary>
        /// <param name="xmlFileName">文件名</param>
        /// <returns>反序列化的对象，失败则返回null</returns>
        public static T FromXmlFile<T>(string xmlFileName) where T : class
        {
            Stream reader = null;
            try
            {
                T obj = default(T);
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                reader = new FileStream(xmlFileName, FileMode.Open, FileAccess.Read);
                XmlReader xmlReader = XmlReader.Create(reader);
                if (serializer.CanDeserialize(xmlReader))
                {
                    obj = serializer.Deserialize(xmlReader) as T;
                }
                reader.Close();
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        /// <summary>
        /// 将对象序列化到文件中
        /// </summary>
        /// <param name="xmlFileName">文件名</param>
        /// <returns>布尔型。True：序列化成功；False：序列化失败</returns>
        public static bool ToXmlFile<T>(string xmlFileName, T t)
        {
            TextWriter writer = null;
            try
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                writer = new StreamWriter(xmlFileName);
                serializer.Serialize(writer, t);
                writer.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }

        public static string ToXml_utf8(object obj)
        {
            StringWriter Output = new StringWriter(new StringBuilder());
            try
            {
                string Ret = String.Empty;
                System.Xml.Serialization.XmlSerializer s = new System.Xml.Serialization.XmlSerializer(obj.GetType());
                s.Serialize(Output, obj);
                Ret = Output.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                Output.Close();
                return Ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Output != null)
                {
                    Output.Close();
                }
            }
        }

        /// <summary>
        /// 将XML字符串中反序列化为对象
        /// </summary>
        /// <param name="strXml">待反序列化的xml字符串</param>
        /// <returns></returns>
        public static T FromXmlStr<T>(string strXml) where T : class
        {
            StringReader stringReader = new StringReader(strXml);
            XmlTextReader xmlReader = new XmlTextReader(stringReader);
            try
            {
                T obj = default(T);
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                if (serializer.CanDeserialize(xmlReader))
                {
                    obj = serializer.Deserialize(xmlReader) as T;
                }
                xmlReader.Close();
                stringReader.Close();
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (xmlReader != null)
                {
                    xmlReader.Close();
                }
                if (stringReader != null)
                {
                    stringReader.Close();
                }
            }
        }
        
    }
}
