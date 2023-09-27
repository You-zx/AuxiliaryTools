using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hp.Base
{
    /// <summary>
    /// Appsetting辅助类
    /// </summary>
    class Setting
    {

        /// <summary>
        /// 默认缓存保存时间
        /// </summary>
        private readonly object CachTime = ConfigurationManager.AppSettings["CachTime"];

        /// <summary>
        /// 向AppSettings写入配置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="kind">操作类别 1：读取指定key的value；2：向指定key写入；3：添加新的Settings；4：删除指定key</param>
        public static string AccessAppSettings(string key, string value, int kind)
        {
            //获取Configuration对象
            Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            string val = "";
            switch (kind)
            {
                case 1:
                    //根据Key读取<add>元素的Value
                    val = config.AppSettings.Settings[key].Value;
                    break;
                case 2:
                    //写入<add>元素的Value
                    val = config.AppSettings.Settings[key].Value = value;
                    break;
                case 3:
                    //增加<add>元素
                    config.AppSettings.Settings.Add(key, value);
                    val = "ok";
                    break;
                case 4:
                    //删除<add>元素
                    config.AppSettings.Settings.Remove(key);
                    val = "ok";
                    break;
                default:
                    break;
            }
            //一定要记得保存，写不带参数的config.Save()也可以
            config.Save(ConfigurationSaveMode.Modified);
            //刷新，否则程序读取的还是之前的值（可能已装入内存）
            System.Configuration.ConfigurationManager.RefreshSection("appSettings");
            return val;
        }

        /// <summary>
        /// 根据key获取value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetSettingValue(string key)
        {
            //获取Configuration对象
            Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            return config.AppSettings.Settings[key].Value;
        }

        /// <summary>
        /// 向已存在的key写入value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SetValue(string key, string value)
        {
            //获取Configuration对象
            Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            return config.AppSettings.Settings[key].Value = value;
        }

        /// <summary>
        /// 添加新的setting项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void AddSettings(string key, string value)
        {
            // 获取Configuration对象
            Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Add(key, value);
        }
        
        /// <summary>
        /// 删除指定key的setting
        /// </summary>
        /// <param name="key"></param>
        public static void DeleteSettings(string key)
        {
            // 获取Configuration对象
            Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove(key);
        }
    }
}
