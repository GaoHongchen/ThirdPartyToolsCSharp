// ===============================================================================
// File Name            :       WinAPI.cs
// File Description     :       This file is a CSharp Library which will be in 
//                              favor of Programming created by myself.
// ===============================================================================
// Author               :       GaoHongchen
// Create Time          :       2015/09/07
// Update Time          :       2015/11/22
// ===============================================================================
// Copyright @ GaoHongchen 2015 . All rights reserved.
// ===============================================================================
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Net;
using System.Drawing;

namespace GHCLibs
{
    /// <summary>
    /// Windows系统API
    /// </summary>
    public class WinAPI
    {
        [DllImport("kernal32.dll")]
        public static extern bool Beep(int freq, int duration);


        [DllImport("winmm")]
        public static extern bool PlaySound(string szSound, IntPtr hmod, int fdwSound);


        #region user32.dll系统函数

        [DllImport("user32.dll")]
        public static extern bool MessageBeep(uint uType);

        [DllImport("user32.dll")]
        public static extern int MessageBox(IntPtr hParent, string msg, string caption, int type);

        [DllImport("user32")]
        public static extern long ExitWindowsEx(long uFlags, long dwReserved);

        [DllImport("user32.dll", EntryPoint = "ShowWindow")]
        public static extern Int32 ShowWindow(Int32 hwnd, Int32 nCmdShow);
        public const Int32 SW_SHOW = 5; public const Int32 SW_HIDE = 0;

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        public static extern Int32 FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "AnimateWindow")]
        public static extern bool AnimateWindow(IntPtr handle, int ms, int flags);

        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        public static extern Int32 SystemParametersInfo(Int32 uAction, Int32 uParam, ref Rectangle lpvParam, Int32 fuWinIni);
        public const Int32 SPIF_UPDATEINIFILE = 0x1;
        public const Int32 SPI_SETWORKAREA = 47;
        public const Int32 SPI_GETWORKAREA = 48;

        #endregion


        #region kernel32.dll系统函数

        [DllImport("kernel32")]
        public static extern long WritePrivateProfileString(
            string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(
            string section, string key, string def, StringBuilder retVal, int size, string filePath);

        [DllImport("Kernel32.dll")]
        static extern bool QueryPerformanceCounter(ref long count);

        [DllImport("Kernel32.dll")]
        static extern bool QueryPerformanceFrequency(ref long freq);

        #endregion


        public void test()
        {
            //隐藏动画
            //base.OnFormClosing(e);
            //AnimateWindow(this.Handle, 1000, 0x10010);   // 居中逐渐隐藏
            //AnimateWindow(this.Handle, 1000, 0x90000); // 淡入淡出效果
            //AnimateWindow(this.Handle, 1000, 0x50008); // 自下而上
            //AnimateWindow(this.Handle, 1000, 0x10008); // 自下而上

            /*
            //显示动画
            AnimateWindow(this.Handle, 1000, 0x20010); // 居中逐渐显示
            AnimateWindow(this.Handle, 1000, 0xA0000); // 淡入淡出效果
            AnimateWindow(this.Handle, 1000, 0x60004); // 自上向下
            AnimateWindow(this.Handle, 1000, 0x20004); // 自上向下
            */

        }
    }
}
