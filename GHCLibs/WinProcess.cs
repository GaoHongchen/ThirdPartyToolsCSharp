using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GHCLibs
{
    /// <summary>
    /// 进程调用类
    /// </summary>
    public class WinProcess
    {
        public static void OpenExeFile(string strExePath)
        {
            System.Diagnostics.Process.Start(strExePath);
        }

        public static void OpenCalc()
        {
            System.Diagnostics.Process.Start("calc.exe");//计算器
        }

        public static void OpenNotepad()
        {
            System.Diagnostics.Process.Start("notepad.exe");//记事本
        }

        public static void OpenUrl()
        {
            System.Diagnostics.Process.Start("http://www.baidu.com");
        }

        public static void OpenCtrlPanel()
        {
            System.Diagnostics.Process.Start("rundll32.exe", @"shell32.dll,Control_RunDLL appwiz.cpl,1"); //控制面板
        }

        public static void OpenDisc()
        {
            System.Diagnostics.Process.Start("explorer.exe", @"c:");
        }

        public static void ReBoot()
        {
            System.Diagnostics.Process.Start("shutdown", @"/r");//重启
        }

        public static void LogOff()
        {
            System.Diagnostics.Process.Start("shutdown", @"/l");//注销计算机 
        }
    }
}
