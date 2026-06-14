using Microsoft.Win32;
using System.IO;

namespace tearhacker7daystodieStarter.Helpers;

/// <summary>
/// 七日杀游戏路径自动检测
/// </summary>
public static class GamePathDetector
{
    private const string SteamAppId = "251570";

    /// <summary>
    /// 自动查找七日杀安装目录
    /// </summary>
    public static string? FindGamePath()
    {
        // 1. Steam 注册表
        string? steamPath = GetSteamGamePath();
        if (!string.IsNullOrEmpty(steamPath) && Directory.Exists(steamPath))
            return steamPath;

        // 2. Steam libraryfolders.vdf 解析
        string? libPath = GetSteamLibraryPath();
        if (!string.IsNullOrEmpty(libPath) && Directory.Exists(libPath))
            return libPath;

        // 3. WeGame 常见路径
        string[] wegameCandidates =
        {
            @"D:\WeGameApps\7DaysToDie",
            @"D:\WeGameApps\PC逆向完整技术\七日杀C绘制源码IMGUI",
            @"C:\WeGameApps\7DaysToDie",
            @"E:\WeGameApps\7DaysToDie",
            @"F:\WeGameApps\7DaysToDie"
        };

        foreach (var path in wegameCandidates)
        {
            if (Directory.Exists(path) && IsValidGameDir(path))
                return path;
        }

        // 4. 全盘快速扫描常见目录
        string? scanPath = QuickScan();
        if (!string.IsNullOrEmpty(scanPath))
            return scanPath;

        return null;
    }

    /// <summary>
    /// 校验是否为有效的七日杀游戏目录
    /// </summary>
    public static bool IsValidGameDir(string path)
    {
        if (!Directory.Exists(path)) return false;
        // 七日杀根目录特征：存在 7DaysToDie.exe 或 7DaysToDie_Data
        return File.Exists(Path.Combine(path, "7DaysToDie.exe"))
            || Directory.Exists(Path.Combine(path, "7DaysToDie_Data"));
    }

    private static string? GetSteamGamePath()
    {
        try
        {
            using var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Valve\Steam");
            if (key == null) return null;

            string? steamInstallPath = key.GetValue("InstallPath")?.ToString();
            if (string.IsNullOrEmpty(steamInstallPath)) return null;

            string defaultGamePath = Path.Combine(steamInstallPath, @"steamapps\common\7 Days To Die");
            return IsValidGameDir(defaultGamePath) ? defaultGamePath : null;
        }
        catch { return null; }
    }

    private static string? GetSteamLibraryPath()
    {
        try
        {
            using var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Valve\Steam");
            if (key == null) return null;

            string? steamInstallPath = key.GetValue("InstallPath")?.ToString();
            if (string.IsNullOrEmpty(steamInstallPath)) return null;

            string vdfPath = Path.Combine(steamInstallPath, @"steamapps\libraryfolders.vdf");
            if (!File.Exists(vdfPath)) return null;

            foreach (string line in File.ReadAllLines(vdfPath))
            {
                string trimmed = line.Trim();
                if (trimmed.Contains("\"path\""))
                {
                    int start = trimmed.IndexOf('"', trimmed.IndexOf('"') + 1) + 1;
                    int end = trimmed.LastIndexOf('"');
                    if (start > 0 && end > start)
                    {
                        string libPath = trimmed.Substring(start, end - start).Replace(@"\\", @"\");
                        string gamePath = Path.Combine(libPath, @"steamapps\common\7 Days To Die");
                        if (IsValidGameDir(gamePath)) return gamePath;
                    }
                }
            }
            return null;
        }
        catch { return null; }
    }

    private static string? QuickScan()
    {
        string[] commonDirs =
        {
            @"C:\Program Files (x86)\Steam\steamapps\common\7 Days To Die",
            @"D:\SteamLibrary\steamapps\common\7 Days To Die",
            @"E:\SteamLibrary\steamapps\common\7 Days To Die",
            @"D:\Games\7 Days To Die",
            @"E:\Games\7 Days To Die",
        };

        foreach (var dir in commonDirs)
        {
            if (IsValidGameDir(dir)) return dir;
        }
        return null;
    }
}
