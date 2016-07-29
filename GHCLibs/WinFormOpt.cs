using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace GHCLibs
{
    class WinFormOpt
    {
        public enum ButtonStyle { Rectangle, RectangleRoundedCorners, Ellipse, Transparent };

        Form m_Form;

        int nScreenWidth;
        int nScreenHeight;

        public WinFormOpt(Form form)
        {
            m_Form = form;

            //分辨率
            nScreenWidth = Screen.PrimaryScreen.Bounds.Width;
            nScreenHeight = Screen.PrimaryScreen.Bounds.Height;
        }

        public void FullScreen()
        {
            m_Form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            //m_Form.WindowState = FormWindowState.Maximized;
            int nWidthBorder = (m_Form.Width - m_Form.ClientRectangle.Width) / 2;
            int nHeightTitle = m_Form.Height - m_Form.ClientRectangle.Height - nWidthBorder;
            m_Form.Left = -nWidthBorder;
            m_Form.Top = -nHeightTitle;
            m_Form.Width = nScreenWidth + nWidthBorder * 2;
            m_Form.Height = nScreenHeight + nHeightTitle + nWidthBorder;
            /*
            Int32 hWnd = FindWindow("Shell_TrayWnd", null);//获取任务栏的句柄
            if (hWnd == 0)
            {
                return;
            }
            else
            {
                ShowWindow(hWnd, SW_SHOW);//隐藏任务栏

                Rectangle rectOld = Rectangle.Empty;
                //获取屏幕范围
                SystemParametersInfo(SPI_GETWORKAREA, 0, ref rectOld, SPIF_UPDATEINIFILE);
                Rectangle rectFull = Screen.PrimaryScreen.Bounds;
                //窗体全屏幕显示
                SystemParametersInfo(SPI_SETWORKAREA, 0, ref rectFull, SPIF_UPDATEINIFILE);
            }*/
        }

        public void SetButtonStyle(Button btn, ButtonStyle styleBtn)
        {
            switch (styleBtn)
            {
                case ButtonStyle.Rectangle:
                    break;
                case ButtonStyle.RectangleRoundedCorners:
                    int nMinSize = (btn.Height <= btn.Width) ? (btn.Height - 10) : (btn.Width - 10);
                    GraphicsPath path = new GraphicsPath();
                    path.AddArc(new Rectangle(5, 5, nMinSize, nMinSize), 90, 180);
                    path.AddLine(5 + nMinSize / 2, 5, btn.Width - nMinSize / 2 - 5, 5);
                    path.AddArc(new Rectangle(btn.Width - nMinSize - 5, 5, nMinSize, nMinSize), -90, 180);
                    path.AddLine(btn.Width - nMinSize / 2 - 5, btn.Height - 5, 5 + nMinSize / 2, btn.Height - 5);

                    btn.Region = new Region(path);
                    path.Dispose();
                    break;
                case ButtonStyle.Ellipse:
                    GraphicsPath pathEllipse = new GraphicsPath();
                    pathEllipse.AddEllipse(5, 5, btn.Width - 10, btn.Height - 10);
                    btn.Region = new Region(pathEllipse);
                    pathEllipse.Dispose();
                    break;
                case ButtonStyle.Transparent:
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.ForeColor = Color.Transparent;
                    btn.BackColor = Color.Transparent;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.FlatAppearance.MouseOverBackColor = Color.Transparent;
                    btn.FlatAppearance.MouseDownBackColor = Color.Transparent;
                    break;
            }
        }
    }
}
