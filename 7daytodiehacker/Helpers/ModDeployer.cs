using System.IO;
using System.Reflection;

namespace tearhacker7daystodieStarter.Helpers;

/// <summary>
/// Mod 内嵌资源释放与安装
/// </summary>
public static class ModDeployer
{
    private const string DllResourceName = "tearhacker7daystodieStarter.Tear7d2d.Tear7d2d.dll";
    private const string XmlResourceName = "tearhacker7daystodieStarter.Tear7d2d.ModInfo.xml";
    private const string ModFolderName = "Tear7d2d";

    public enum InstallResult
    {
        Success,
        GameDirNotFound,
        PermissionDenied,
        ResourceNotFound,
        UnknownError
    }

    public static bool IsModInstalled(string gameRootPath)
    {
        string modDir = Path.Combine(gameRootPath, "Mods", ModFolderName);
        return File.Exists(Path.Combine(modDir, "Tear7d2d.dll"))
            && File.Exists(Path.Combine(modDir, "ModInfo.xml"));
    }

    public static bool IsGameRunning()
    {
        try
        {
            return System.Diagnostics.Process.GetProcessesByName("7DaysToDie").Length > 0;
        }
        catch { return false; }
    }

    /// <summary>
    /// 强制关闭游戏进程
    /// </summary>
    public static bool KillGameProcess(Action<string>? logCallback = null)
    {
        try
        {
            bool killed = false;
            foreach (var proc in System.Diagnostics.Process.GetProcessesByName("7DaysToDie"))
            {
                logCallback?.Invoke(Lang.Get("log_close_game"));
                proc.Kill();
                proc.WaitForExit(5000);
                killed = true;
                logCallback?.Invoke(Lang.Get("log_game_closed"));
            }
            return killed;
        }
        catch { return false; }
    }

    /// <summary>
    /// 执行完整 Mod 安装（自动关闭游戏）
    /// </summary>
    public static InstallResult Install(string gameRootPath, Action<string>? logCallback = null)
    {
        try
        {
            if (!Directory.Exists(gameRootPath))
            {
                logCallback?.Invoke(Lang.Get("log_dir_not_exist"));
                return InstallResult.GameDirNotFound;
            }

            // 自动关闭游戏
            if (IsGameRunning())
            {
                KillGameProcess(logCallback);
            }

            string modTargetDir = Path.Combine(gameRootPath, "Mods", ModFolderName);

            if (!Directory.Exists(Path.Combine(gameRootPath, "Mods")))
            {
                Directory.CreateDirectory(Path.Combine(gameRootPath, "Mods"));
                logCallback?.Invoke(Lang.Get("log_creating_mods"));
            }

            if (!Directory.Exists(modTargetDir))
            {
                Directory.CreateDirectory(modTargetDir);
                logCallback?.Invoke(Lang.Get("log_creating_mod"));
            }

            logCallback?.Invoke(Lang.Get("log_extracting_dll"));
            ExtractResource(DllResourceName, Path.Combine(modTargetDir, "Tear7d2d.dll"));

            logCallback?.Invoke(Lang.Get("log_extracting_xml"));
            ExtractResource(XmlResourceName, Path.Combine(modTargetDir, "ModInfo.xml"));

            logCallback?.Invoke(Lang.Get("log_done"));
            return InstallResult.Success;
        }
        catch (UnauthorizedAccessException)
        {
            logCallback?.Invoke(Lang.Get("log_permission"));
            return InstallResult.PermissionDenied;
        }
        catch (Exception ex)
        {
            logCallback?.Invoke(Lang.Get("log_error") + ex.Message);
            return InstallResult.UnknownError;
        }
    }

    public static bool Uninstall(string gameRootPath)
    {
        try
        {
            string modDir = Path.Combine(gameRootPath, "Mods", ModFolderName);
            if (Directory.Exists(modDir))
            {
                Directory.Delete(modDir, true);
                return true;
            }
            return false;
        }
        catch { return false; }
    }

    private static void ExtractResource(string resourceFullName, string targetFilePath)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        using Stream? resourceStream = assembly.GetManifestResourceStream(resourceFullName);

        if (resourceStream == null)
            throw new Exception($"Embedded resource not found: {resourceFullName}");

        using FileStream fs = new FileStream(targetFilePath, FileMode.Create, FileAccess.Write);
        resourceStream.CopyTo(fs);
    }
}
