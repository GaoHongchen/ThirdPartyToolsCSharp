using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace GHCLibs
{
    /// <summary>
    /// Ini配置文件读写类
    /// </summary>
    public class FileIni
    {
        [System.Runtime.InteropServices.DllImport("kernel32")]
        public static extern long WritePrivateProfileString(
            string section, string key, string val, string filePath);

        [System.Runtime.InteropServices.DllImport("kernel32")]
        public static extern int GetPrivateProfileString(
            string section, string key, string def, StringBuilder retVal, int size, string filePath);

        private string pathFileIni;//INI文件名  

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="strIniPath">INI文件路径</param>
        public FileIni(string strIniPath)
        {
            this.pathFileIni = "";
            if (File.Exists(strIniPath))
            {
                this.pathFileIni = strIniPath;
            }
            else
            {
                Trace.WriteLine("文件路径错误：" + strIniPath + "文件不存在！");
            }
        }

        /// <summary>
        /// 写INI文件string
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public bool WriteValue(string Section, string Key, string Value)
        {
            if (File.Exists(this.pathFileIni))
            {
                long status = WritePrivateProfileString(Section, Key, Value, this.pathFileIni);
                if (status == 0)
                    return false;
                else
                    return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 读INI文件string
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public string ReadValue(string Section, string Key)
        {
            if (File.Exists(this.pathFileIni))
            {
                StringBuilder temp = new StringBuilder(255);
                int i = GetPrivateProfileString(Section, Key, "", temp, 255, this.pathFileIni);
                return temp.ToString();
            }
            else
            {
                return String.Empty;
            }
        }
    }
}
