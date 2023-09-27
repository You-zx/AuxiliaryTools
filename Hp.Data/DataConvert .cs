using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Web.Script.Serialization;

namespace Hp.Data
{
    public static class Data
    {
        #region 校验数据格式
        /// <summary>
        /// 校验是否位json字符串
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static bool IsJson(this string json)
        {
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                dynamic obj = serializer.Deserialize(json, typeof(object));
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 判断字符串是否为空
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static bool IsEmpty(this string Str)
        {
            if (Str == null || Str == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 数据格式类型转换
        /// <summary>
        /// string转换为int
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static int ToInt(this string Str)
        {
            return int.Parse(Str);
        }

        /// <summary>
        /// string转换为int或空
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static int? ToIntOrNull(this string Str)
        {
            try
            {
                if (Str.IsEmpty())
                {
                    return null;
                }
                else
                {
                    return int.Parse(Str);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 从string转换为decima
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string Str)
        {
            return decimal.Parse(Str);
        }

        /// <summary>
        /// 从string转换为decima或null
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static decimal? ToDecimalOrNull(this string Str)
        {
            try
            {
                if (Str.IsEmpty())
                {
                    return null;
                }
                else
                {
                    return decimal.Parse(Str);
                }
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        /// <summary>
        /// decimal?保留指定位数小数
        /// </summary>
        /// <param name="num">原始数量</param>
        /// <param name="scale">保留小数位数</param>
        /// <returns>截取指定小数位数后的数量字符串</returns>
        public static string ToAppointString(this decimal? num, int scale)
        {
            string numToString = num.ToString();

            int index = numToString.IndexOf(".");
            int length = numToString.Length;

            if (index != -1)
            {
                return string.Format("{0}.{1}", numToString.Substring(0, index), numToString.Substring(index + 1, Math.Min(length - index - 1, scale)));
            }
            else
            {
                return num.ToString();
            }
        }

        /// <summary>
        /// 字符串转为DateTime类型的年月日 时分秒
        /// </summary>
        /// <param name="Str">yyyy-MM-dd HH:mm:ss</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string Str)
        {
            //int num = Regex.Matches(Str, "-").Count;
            DateTime dt = DateTime.ParseExact(Str, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);
            return Convert.ToDateTime(Str);
        }

        /// <summary>
        /// 字符串转为DateTime类型的年月日
        /// </summary>
        /// <param name="Str">yyyy-MM-dd</param>
        /// <returns></returns>
        public static DateTime ToDate(this string Str)
        {
            DateTime dt = DateTime.ParseExact(Str, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
            return Convert.ToDateTime(Str);
        }

        /// <summary>
        /// DataTable转成指定List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ToDataList<T>(this DataTable dt)
        {
            var list = new List<T>();
            var plist = new List<PropertyInfo>(typeof(T).GetProperties());

            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }

            foreach (DataRow item in dt.Rows)
            {
                T s = Activator.CreateInstance<T>();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    PropertyInfo info = plist.Find(p => p.Name == dt.Columns[i].ColumnName);
                    if (info != null)
                    {
                        try
                        {
                            if (!Convert.IsDBNull(item[i]))
                            {
                                object v = null;
                                if (info.PropertyType.ToString().Contains("System.Nullable"))
                                {
                                    v = Convert.ChangeType(item[i], Nullable.GetUnderlyingType(info.PropertyType));
                                }
                                else
                                {
                                    v = Convert.ChangeType(item[i], info.PropertyType);
                                }
                                info.SetValue(s, v, null);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("字段[" + info.Name + "]转换出错," + ex.Message);
                        }
                    }
                }
                list.Add(s);
            }
            return list;
        }

        /// <summary>
        /// 把字符串按照分隔符转换成 List
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="speater">分隔符</param>
        /// <param name="toLower">是否转换为小写</param>
        /// <returns></returns>
        public static List<string> GetStrArray(this string str, char speater, bool toLower)
        {
            List<string> list = new List<string>();
            string[] ss = str.Split(speater);
            foreach (string s in ss)
            {
                if (!string.IsNullOrEmpty(s) && s != speater.ToString())
                {
                    string strVal = s;
                    if (toLower)
                    {
                        strVal = s.ToLower();
                    }
                    list.Add(strVal);
                }
            }
            return list;
        }

        /// <summary>
        /// 把 List<string>按照分隔符组装成 string
        /// </summary>
        /// <param name="list"></param>
        /// <param name="speater">分隔符</param>
        /// <returns></returns>
        public static string GetArrayStr(List<string> list, string speater)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1)
                {
                    sb.Append(list[i]);
                }
                else
                {
                    sb.Append(list[i]);
                    sb.Append(speater);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 把 List<int>按照分隔符组装成 string
        /// </summary>
        /// <param name="list"></param>
        /// <param name="speater">分隔符</param>
        /// <returns></returns>
        public static string GetArrayStr(List<int> list, string speater)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1)
                {
                    sb.Append(list[i].ToString());
                }
                else
                {
                    sb.Append(list[i]);
                    sb.Append(",");
                }
            }
            return sb.ToString();
        }
        #endregion


        /// <summary>
        /// 两个实体之间相同属性的映射
        /// </summary>
        /// <typeparam name="R">目标实体</typeparam>
        /// <typeparam name="T">数据源实体</typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static R EntityMapping<R, T>(T model)
        {
            R result = Activator.CreateInstance<R>();
            foreach (PropertyInfo info in typeof(R).GetProperties())
            {
                PropertyInfo pro = typeof(T).GetProperty(info.Name);
                if (pro != null)
                    info.SetValue(result, pro.GetValue(model));
            }
            return result;
        }
       
    }
}