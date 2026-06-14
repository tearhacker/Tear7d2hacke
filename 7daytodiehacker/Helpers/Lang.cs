namespace tearhacker7daystodieStarter.Helpers;

/// <summary>
/// 中英文双语支持
/// </summary>
public static class Lang
{
    public enum LangType { EN, CN }

    public static LangType Current { get; set; } = LangType.CN;

    private static readonly Dictionary<string, string[]> Strings = new()
    {
        // 灵动岛状态
        { "status_ready",       ["SYSTEM READY",    "系统就绪"] },
        { "status_detecting",   ["DETECTING...",    "正在检测..."] },
        { "status_path_found",  ["PATH FOUND",      "路径已找到"] },
        { "status_no_path",     ["PATH NOT FOUND",  "路径未找到"] },
        { "status_path_set",    ["PATH SET",        "路径已设置"] },
        { "status_invalid",     ["INVALID PATH",    "无效路径"] },
        { "status_installed",   ["MOD INSTALLED",   "模组已安装"] },
        { "status_installing",  ["INSTALLING...",   "正在安装..."] },
        { "status_closing",     ["CLOSING GAME...", "正在关闭游戏..."] },
        { "status_success",     ["INSTALL SUCCESS", "安装成功"] },
        { "status_uninstalling",["UNINSTALLING...","正在卸载..."] },
        { "status_uninstalled", ["UNINSTALL SUCCESS","卸载成功"] },
        { "status_uninstall_fail",["UNINSTALL FAILED","卸载失败"] },
        { "status_failed",      ["INSTALL FAILED",  "安装失败"] },
        { "status_permission",  ["ACCESS DENIED",   "权限不足"] },
        { "status_launched",    ["GAME LAUNCHED",   "游戏已启动"] },
        { "status_launch_fail", ["LAUNCH FAILED",   "启动失败"] },
        { "status_no_exe",      ["EXE NOT FOUND",   "未找到EXE"] },

        // 按钮
        { "btn_install",        ["INSTALL MOD",     "安装模组"] },
        { "btn_reinstall",      ["REINSTALL MOD",   "重新安装"] },
        { "btn_uninstall",      ["UNINSTALL MOD",   "卸载模组"] },
        { "btn_uninstalling",   ["UNINSTALLING...","卸载中..."] },
        { "btn_installing",     ["INSTALLING...",   "安装中..."] },
        { "btn_no_path",        ["NO PATH",         "无路径"] },
        { "btn_launch",         ["LAUNCH GAME",     "启动游戏"] },

        // 路径区
        { "path_detecting",     ["Detecting game path...",       "正在检测游戏路径..."] },
        { "path_not_found",     ["Game path not found, select manually", "未找到游戏路径，请手动选择"] },
        { "path_browse_tip",    ["Select 7 Days To Die root folder",     "选择七日杀游戏根目录"] },

        // Mod 状态
        { "mod_installed",      ["Mod Installed",   "模组已安装"] },
        { "mod_not_installed",  ["Mod Not Installed","模组未安装"] },

        // 日志
        { "log_creating_mods",  ["Creating Mods directory...",    "正在创建 Mods 目录..."] },
        { "log_creating_mod",   ["Creating Mod directory...",     "正在创建模组目录..."] },
        { "log_extracting_dll", ["Extracting Mod DLL...",         "正在释放模组核心文件..."] },
        { "log_extracting_xml", ["Writing Mod config...",         "正在写入模组配置文件..."] },
        { "log_done",           ["Mod installed successfully",    "模组安装完成"] },
        { "log_launch_hint",    ["Launch game via Steam / WeGame","请通过 Steam / other 启动游戏"] },
        { "log_close_game",     ["Closing game process...",       "正在关闭游戏进程..."] },
        { "log_game_closed",    ["Game closed",                   "游戏已关闭"] },
        { "log_dir_not_exist",  ["Game directory not found",      "游戏目录不存在"] },
        { "log_permission",     ["Run as Administrator required", "请以管理员身份运行启动器"] },
        { "log_invalid_dir",    ["Not a valid 7 Days To Die directory", "所选目录不是有效的七日杀游戏目录"] },
        { "log_exe_not_found",  ["7DaysToDie.exe not found",     "未找到 7DaysToDie.exe"] },
        { "log_launch_fail",    ["Launch failed: ",               "启动失败："] },
        { "log_error",          ["Error: ",                       "错误："] },
        { "log_uninstall_done", ["Mod uninstalled, game restored","模组已卸载，恢复正常游戏效果"] },
        { "log_uninstall_fail", ["Uninstall failed, try again",  "卸载失败，请重试"] },

        // 语言切换
        { "lang_label",         ["中文", "EN"] },
        { "btn_website",        ["TEAR WEB",        "泪心官网"] },
    };

    public static string Get(string key)
    {
        int idx = Current == LangType.EN ? 0 : 1;
        return Strings.TryGetValue(key, out var arr) ? arr[idx] : key;
    }

    public static void Toggle()
    {
        Current = Current == LangType.EN ? LangType.CN : LangType.EN;
    }
}
