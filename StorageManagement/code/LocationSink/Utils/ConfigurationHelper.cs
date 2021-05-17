using System;
using System.Configuration;

namespace Utils
{
    public class ConfigurationHelper
    {
        #region 添加或更新配置文件（并Save）
        /// <summary>
        /// 更新App.config配置文件，如果节点不存在则创建节点
        /// </summary>
        /// <param name="key">App.config中的key</param>
        /// <param name="value">App.config中的value</param>
        public static void AddUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException ex)
            {
                Console.WriteLine("写入配置错误：" + ex);
            }
        }
        #endregion
    }
}
