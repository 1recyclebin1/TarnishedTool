// 

using System;
using System.Diagnostics;
using TarnishedTool.Memory;

namespace TarnishedTool.Utilities;

public static class PatchManager
{
    public static bool Initialize()
    {
        var exePath = ExeManager.GetExePath();
        if (exePath is null)
        {
            MsgBox.Show("Could not find Elden Ring installation.");
            return false;
        }

        var versionInfo = FileVersionInfo.GetVersionInfo(exePath);
        var fileVersion = versionInfo.FileVersion;

        if (!Offsets.Initialize(fileVersion))
        {
            MsgBox.Show($"Unsupported game version: {fileVersion}");
            return false;
        }

        return true;
    }
}