/******************************************************************************
 *
 *    MIConvexHull, Copyright (C) 2013 David Sehnal, Matthew Campbell
 *
 *  This library is free software; you can redistribute it and/or modify it 
 *  under the terms of  the GNU Lesser General Public License as published by 
 *  the Free Software Foundation; either version 2.1 of the License, or 
 *  (at your option) any later version.
 *
 *  This library is distributed in the hope that it will be useful, 
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of 
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser 
 *  General Public License for more details.
 *  
 *****************************************************************************/

using System;
using System.Runtime.InteropServices;

namespace PCLLib
{
  public static class ScreenSaver
  {
    private const int SPI_GETSCREENSAVERACTIVE = 16;
    private const int SPI_SETSCREENSAVERACTIVE = 17;
    private const int SPI_GETSCREENSAVERTIMEOUT = 14;
    private const int SPI_SETSCREENSAVERTIMEOUT = 15;
    private const int SPI_GETSCREENSAVERRUNNING = 114;
    private const int SPIF_SENDWININICHANGE = 2;
    private const uint DESKTOP_WRITEOBJECTS = 128U;
    private const uint DESKTOP_READOBJECTS = 1U;
    private const int WM_CLOSE = 16;

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern bool SystemParametersInfo(int uAction, int uParam, ref int lpvParam, int flags);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern bool SystemParametersInfo(int uAction, int uParam, ref bool lpvParam, int flags);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern int PostMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr OpenDesktop(string hDesktop, int Flags, bool Inherit, uint DesiredAccess);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern bool CloseDesktop(IntPtr hDesktop);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern bool EnumDesktopWindows(IntPtr hDesktop, ScreenSaver.EnumDesktopWindowsProc callback, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern bool IsWindowVisible(IntPtr hWnd);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr GetForegroundWindow();

    public static bool GetScreenSaverActive()
    {
      bool lpvParam = false;
      ScreenSaver.SystemParametersInfo(16, 0, ref lpvParam, 0);
      return lpvParam;
    }

    public static void SetScreenSaverActive(int Active)
    {
      int lpvParam = 0;
      ScreenSaver.SystemParametersInfo(17, Active, ref lpvParam, 2);
    }

    public static int GetScreenSaverTimeout()
    {
      int lpvParam = 0;
      ScreenSaver.SystemParametersInfo(14, 0, ref lpvParam, 0);
      return lpvParam;
    }

    public static void SetScreenSaverTimeout(int Value)
    {
      int lpvParam = 0;
      ScreenSaver.SystemParametersInfo(15, Value, ref lpvParam, 2);
    }

    public static bool GetScreenSaverRunning()
    {
      bool lpvParam = false;
      ScreenSaver.SystemParametersInfo(114, 0, ref lpvParam, 0);
      return lpvParam;
    }

    public static void KillScreenSaver()
    {
      IntPtr hDesktop = ScreenSaver.OpenDesktop("Screen-saver", 0, false, 129U);
      if (hDesktop != IntPtr.Zero)
      {
        ScreenSaver.EnumDesktopWindows(hDesktop, new ScreenSaver.EnumDesktopWindowsProc(ScreenSaver.KillScreenSaverFunc), IntPtr.Zero);
        ScreenSaver.CloseDesktop(hDesktop);
      }
      else
        ScreenSaver.PostMessage(ScreenSaver.GetForegroundWindow(), 16, 0, 0);
    }

    private static bool KillScreenSaverFunc(IntPtr hWnd, IntPtr lParam)
    {
      if (ScreenSaver.IsWindowVisible(hWnd))
        ScreenSaver.PostMessage(hWnd, 16, 0, 0);
      return true;
    }

    private delegate bool EnumDesktopWindowsProc(IntPtr hDesktop, IntPtr lParam);
  }
}
