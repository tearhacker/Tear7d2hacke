using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using System.Windows.Threading;
using tearhacker7daystodieStarter.Helpers;
using WinForms = System.Windows.Forms;

namespace tearhacker7daystodieStarter;

public partial class MainWindow : Window
{
    private string? _gamePath;
    private bool _isInstalled;
    private bool _isBusy;

    // 启动覆盖层运行时状态
    private DispatcherTimer? _launchTimer;
    private DispatcherTimer? _dotsTimer;
    private DispatcherTimer? _logTimer;
    private DispatcherTimer? _caretTimer;
    private DispatcherTimer? _progressTimer;
    private DispatcherTimer? _gameDetectTimer;
    private DateTime _launchStart;
    private double _launchProgress; // 0~1
    private int _stageIndex;
    private bool _gameDetected;
    private readonly Random _rng = new();

    public MainWindow()
    {
        InitializeComponent();
        Loaded += MainWindow_Loaded;
    }

    // ===== 初始化 =====

    private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        StartBreathAnimation();
        RefreshAllText();

        SetStatus(Lang.Get("status_detecting"), "#FFD700");
        PathText.Text = Lang.Get("path_detecting");

        _gamePath = await System.Threading.Tasks.Task.Run(() => GamePathDetector.FindGamePath());

        if (!string.IsNullOrEmpty(_gamePath))
        {
            PathText.Text = _gamePath;
            SetStatus(Lang.Get("status_path_found"), "#00E5FF");
            RefreshModStatus();
        }
        else
        {
            PathText.Text = Lang.Get("path_not_found");
            SetStatus(Lang.Get("status_no_path"), "#FF6B6B");
            _isInstalled = false;
            UpdateInstallButton();
        }
    }

    // ===== 语言切换 =====

    private void LangBtn_Click(object sender, RoutedEventArgs e)
    {
        Lang.Toggle();
        RefreshAllText();
    }

    private void RefreshAllText()
    {
        // 语言按钮
        var langTemplate = LangBtn.Template;
        if (langTemplate.FindName("LangBtnText", LangBtn) is TextBlock langText)
            langText.Text = Lang.Get("lang_label");

        // 灵动岛
        if (!_isBusy)
        {
            if (string.IsNullOrEmpty(_gamePath))
                SetStatus(Lang.Get("status_no_path"), "#FF6B6B");
            else if (_isInstalled)
                SetStatus(Lang.Get("status_installed"), "#00E5FF");
            else
                SetStatus(Lang.Get("status_ready"), "#00E5FF");
        }

        // 路径
        if (string.IsNullOrEmpty(_gamePath))
            PathText.Text = Lang.Get("path_not_found");

        // Mod 状态
        ModStatusText.Text = _isInstalled ? Lang.Get("mod_installed") : Lang.Get("mod_not_installed");

        // 按钮
        UpdateInstallButton();

        // 启动游戏按钮
        var launchTemplate = LaunchBtn.Template;
        if (launchTemplate.FindName("LaunchBtnText", LaunchBtn) is TextBlock launchText)
            launchText.Text = Lang.Get("btn_launch");

        // 官网按钮
        var websiteTemplate = WebsiteBtn.Template;
        if (websiteTemplate.FindName("WebsiteBtnText", WebsiteBtn) is TextBlock websiteText)
            websiteText.Text = Lang.Get("btn_website");
    }

    // ===== 灵动岛状态 =====

    private void SetStatus(string text, string hexColor)
    {
        StatusText.Text = text;
        var color = (Color)ColorConverter.ConvertFromString(hexColor);
        StatusText.Foreground = new SolidColorBrush(color);
        DynamicIsland.BorderBrush = new SolidColorBrush { Color = color, Opacity = 0.4 };
    }

    // ===== 进度条 =====

    private void ShowProgress(double percent)
    {
        ProgressPanel.Visibility = Visibility.Visible;
        ProgressFill.Width = 280 * Math.Clamp(percent, 0, 1);
    }

    private void HideProgress()
    {
        ProgressPanel.Visibility = Visibility.Collapsed;
        ProgressFill.Width = 0;
    }

    // ===== Mod 状态刷新 =====

    private void RefreshModStatus()
    {
        if (string.IsNullOrEmpty(_gamePath)) return;

        _isInstalled = ModDeployer.IsModInstalled(_gamePath);

        if (_isInstalled)
        {
            ModStatusDot.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00E5FF"));
            ModStatusText.Text = Lang.Get("mod_installed");
            SetStatus(Lang.Get("status_installed"), "#00E5FF");
        }
        else
        {
            ModStatusDot.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6B6B"));
            ModStatusText.Text = Lang.Get("mod_not_installed");
            SetStatus(Lang.Get("status_ready"), "#00E5FF");
        }

        UpdateInstallButton();
    }

    private void UpdateInstallButton()
    {
        if (_isBusy) return;

        if (string.IsNullOrEmpty(_gamePath))
        {
            BtnText.Text = Lang.Get("btn_no_path");
            InstallBtn.IsEnabled = false;
            UninstallBtn.IsEnabled = false;
            return;
        }

        InstallBtn.IsEnabled = true;
        BtnText.Text = _isInstalled ? Lang.Get("btn_reinstall") : Lang.Get("btn_install");

        // 卸载按钮：仅在已安装时可用
        UninstallBtn.IsEnabled = _isInstalled;
        UninstallBtnText.Text = Lang.Get("btn_uninstall");
        UninstallBtnText.Opacity = _isInstalled ? 0.7 : 0.3;
    }

    // ===== 安装按钮 =====

    private async void InstallBtn_Click(object sender, RoutedEventArgs e)
    {
        if (_isBusy || string.IsNullOrEmpty(_gamePath)) return;

        _isBusy = true;
        InstallBtn.IsEnabled = false;
        BtnText.Text = Lang.Get("btn_installing");
        LogText.Text = "";
        ShowProgress(0);

        // 如果游戏运行中，先显示关闭状态
        if (ModDeployer.IsGameRunning())
        {
            SetStatus(Lang.Get("status_closing"), "#FFD700");
            ShowProgress(0.1);
        }

        string gamePath = _gamePath;
        int step = 0;
        int totalSteps = 5;

        var result = await System.Threading.Tasks.Task.Run(() =>
            ModDeployer.Install(gamePath, msg =>
                Dispatcher.Invoke(() =>
                {
                    step++;
                    ShowProgress((double)step / totalSteps);
                    LogText.Text = msg;
                }))
        );

        _isBusy = false;
        HideProgress();

        switch (result)
        {
            case ModDeployer.InstallResult.Success:
                _isInstalled = true;
                SetStatus(Lang.Get("status_success"), "#00FF88");
                ModStatusDot.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00FF88"));
                ModStatusText.Text = Lang.Get("mod_installed");
                LogText.Text = Lang.Get("log_launch_hint");
                UpdateInstallButton();
                break;

            case ModDeployer.InstallResult.PermissionDenied:
                SetStatus(Lang.Get("status_permission"), "#FF6B6B");
                LogText.Text = Lang.Get("log_permission");
                UpdateInstallButton();
                break;

            default:
                SetStatus(Lang.Get("status_failed"), "#FF6B6B");
                LogText.Text = Lang.Get("log_error");
                UpdateInstallButton();
                break;
        }
    }

    // ===== 卸载按钮 =====

    private async void UninstallBtn_Click(object sender, RoutedEventArgs e)
    {
        if (_isBusy || string.IsNullOrEmpty(_gamePath) || !_isInstalled) return;

        _isBusy = true;
        InstallBtn.IsEnabled = false;
        UninstallBtn.IsEnabled = false;
        UninstallBtnText.Text = Lang.Get("btn_uninstalling");
        SetStatus(Lang.Get("status_uninstalling"), "#FF6B6B");
        LogText.Text = "";

        string gamePath = _gamePath;

        // 如果游戏运行中，先关闭
        if (ModDeployer.IsGameRunning())
        {
            SetStatus(Lang.Get("status_closing"), "#FFD700");
            await System.Threading.Tasks.Task.Run(() => ModDeployer.KillGameProcess());
        }

        bool success = await System.Threading.Tasks.Task.Run(() => ModDeployer.Uninstall(gamePath));

        _isBusy = false;

        if (success)
        {
            _isInstalled = false;
            SetStatus(Lang.Get("status_uninstalled"), "#00FF88");
            ModStatusDot.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6B6B"));
            ModStatusText.Text = Lang.Get("mod_not_installed");
            LogText.Text = Lang.Get("log_uninstall_done");
            UpdateInstallButton();

            // 灵动岛弹窗：卸载成功
            ShowUninstallIsland();
        }
        else
        {
            SetStatus(Lang.Get("status_uninstall_fail"), "#FF6B6B");
            LogText.Text = Lang.Get("log_uninstall_fail");
            UpdateInstallButton();
        }
    }

    // ===== 卸载成功灵动岛弹窗 =====

    private DispatcherTimer? _uninstallIslandTimer;

    private void ShowUninstallIsland()
    {
        // 创建灵动岛弹窗
        var island = new Border
        {
            CornerRadius = new CornerRadius(20),
            Padding = new Thickness(24, 8, 24, 8),
            Background = new SolidColorBrush(Color.FromArgb(230, 0x1A, 0x1F, 0x2E)),
            BorderThickness = new Thickness(1),
            BorderBrush = new SolidColorBrush(Color.FromArgb(180, 0x00, 0xFF, 0x88)),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new Thickness(0, 60, 0, 0),
            Opacity = 0,
            Effect = new DropShadowEffect { Color = Color.FromArgb(255, 0x00, 0xFF, 0x88), BlurRadius = 20, ShadowDepth = 0, Opacity = 0.5 }
        };

        var stack = new StackPanel { Orientation = Orientation.Horizontal };

        // 绿色圆点
        var dot = new Ellipse
        {
            Width = 10, Height = 10,
            Fill = new SolidColorBrush(Color.FromArgb(255, 0x00, 0xFF, 0x88)),
            VerticalAlignment = VerticalAlignment.Center
        };
        dot.Effect = new DropShadowEffect { Color = Color.FromArgb(255, 0x00, 0xFF, 0x88), BlurRadius = 8, ShadowDepth = 0 };
        stack.Children.Add(dot);

        // 文字
        var titleText = new TextBlock
        {
            Text = Lang.Current == Lang.LangType.CN ? "卸载成功" : "UNINSTALLED",
            FontSize = 12, FontWeight = FontWeights.Medium,
            Foreground = new SolidColorBrush(Color.FromArgb(255, 0x00, 0xFF, 0x88)),
            FontFamily = new FontFamily("Consolas"),
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(10, 0, 0, 0)
        };
        stack.Children.Add(titleText);

        var descText = new TextBlock
        {
            Text = Lang.Current == Lang.LangType.CN ? "  恢复正常游戏效果" : "  Game restored",
            FontSize = 11,
            Foreground = new SolidColorBrush(Color.FromArgb(200, 0xAA, 0xBB, 0xCC)),
            FontFamily = new FontFamily("Consolas"),
            VerticalAlignment = VerticalAlignment.Center
        };
        stack.Children.Add(descText);

        island.Child = stack;

        // 添加到主Grid
        var mainGrid = (Grid)((Border)Content).Child;
        Grid.SetRowSpan(island, 3);
        Grid.SetColumnSpan(island, 3);
        mainGrid.Children.Add(island);

        // 淡入动画
        var fadeIn = new DoubleAnimation
        {
            From = 0, To = 1,
            Duration = TimeSpan.FromMilliseconds(400),
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
        };
        island.BeginAnimation(OpacityProperty, fadeIn);

        // 从上方滑入
        var slideIn = new DoubleAnimation
        {
            From = -40, To = 0,
            Duration = TimeSpan.FromMilliseconds(400),
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
        };
        var transform = new TranslateTransform();
        island.RenderTransform = transform;
        transform.BeginAnimation(TranslateTransform.YProperty, slideIn);

        // 3秒后淡出并移除
        _uninstallIslandTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
        _uninstallIslandTimer.Tick += (_, _) =>
        {
            _uninstallIslandTimer.Stop();

            var fadeOut = new DoubleAnimation
            {
                From = 1, To = 0,
                Duration = TimeSpan.FromMilliseconds(500),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn }
            };
            fadeOut.Completed += (_, _) =>
            {
                mainGrid.Children.Remove(island);
            };
            island.BeginAnimation(OpacityProperty, fadeOut);
        };
        _uninstallIslandTimer.Start();
    }

    // ===== 手动选择路径 =====

    private void BrowseBtn_Click(object sender, RoutedEventArgs e)
    {
        using var dialog = new WinForms.FolderBrowserDialog
        {
            Description = Lang.Get("path_browse_tip"),
            UseDescriptionForTitle = true,
            ShowNewFolderButton = false
        };

        if (dialog.ShowDialog() == WinForms.DialogResult.OK)
        {
            string selected = dialog.SelectedPath;

            if (GamePathDetector.IsValidGameDir(selected))
            {
                _gamePath = selected;
                PathText.Text = _gamePath;
                SetStatus(Lang.Get("status_path_set"), "#00E5FF");
                RefreshModStatus();
            }
            else
            {
                SetStatus(Lang.Get("status_invalid"), "#FF6B6B");
                LogText.Text = Lang.Get("log_invalid_dir");
            }
        }
    }

    // ===== 启动游戏 =====

    private void LaunchBtn_Click(object sender, RoutedEventArgs e)
    {
        // 切换到全屏启动覆盖层
        ShowLaunchingOverlay();

        // 启动游戏（后台），UI 进入科技动画 + 进程检测
        try
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "steam://run/251570",
                UseShellExecute = true
            });
            PushLog("steam://run/251570", "#00E5FF");
        }
        catch
        {
            // 回退：直接启动 exe
            if (!string.IsNullOrEmpty(_gamePath))
            {
                try
                {
                    string exePath = System.IO.Path.Combine(_gamePath, "7DaysToDie.exe");
                    if (System.IO.File.Exists(exePath))
                    {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = exePath,
                            WorkingDirectory = _gamePath,
                            UseShellExecute = true
                        });
                        PushLog($"exec {exePath}", "#00E5FF");
                    }
                    else
                    {
                        PushLog(Lang.Get("log_exe_not_found"), "#FF6B6B");
                    }
                }
                catch (Exception ex)
                {
                    PushLog(Lang.Get("log_launch_fail") + ex.Message, "#FF6B6B");
                }
            }
        }
    }

    // ===== GitHub =====

    private void GithubBtn_Click(object sender, RoutedEventArgs e)
    {
        OpenUrl("https://github.com/tearhacker/");
    }

    // ===== 泪心官网 =====

    private void WebsiteBtn_Click(object sender, RoutedEventArgs e)
    {
        OpenUrl("http://teargamestorem.top/");
    }

    // ===== 窗口交互 =====

    private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ButtonState == MouseButtonState.Pressed)
            DragMove();
    }

    private void CloseBtn_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    // ===== 呼吸光效动画 =====

    private void StartBreathAnimation()
    {
        var anim = new DoubleAnimation
        {
            From = 0.15,
            To = 0.45,
            Duration = TimeSpan.FromSeconds(4),
            AutoReverse = true,
            RepeatBehavior = RepeatBehavior.Forever,
            EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
        };
        BgGlow.BeginAnimation(OpacityProperty, anim);
    }

    private static void OpenUrl(string url)
    {
        try
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(url)
            {
                UseShellExecute = true
            });
        }
        catch { }
    }

    // ============================================================
    // ===================== 启动覆盖层逻辑 ========================
    // ============================================================

    private static readonly string[] StagesEN =
    {
        "[ STAGE 1/4 ] BOOT SEQUENCE",
        "[ STAGE 2/4 ] LOADING CORE MOD",
        "[ STAGE 3/4 ] CONNECTING STEAM",
        "[ STAGE 4/4 ] WAITING FOR PROCESS",
    };

    private static readonly string[] StagesCN =
    {
        "[ 阶段 1/4 ] 引导序列",
        "[ 阶段 2/4 ] 加载核心模组",
        "[ 阶段 3/4 ] 连接 Steam",
        "[ 阶段 4/4 ] 等待进程就绪",
    };

    private static readonly string[] LogPool =
    {
        "[OK] init kernel module: Tear7d2d.dll",
        "[OK] hooking d3d11_present @ 0x7FFE3A1C2",
        "[OK] resolving steam_api64.dll exports",
        "[..] mapping memory regions  0x140000000 - 0x1700FFFFF",
        "[OK] anti-cheat bypass layer ready",
        "[..] verifying mod signature  sha256:9d4e...edd",
        "[OK] injecting overlay (cyan) into render pipeline",
        "[..] establishing IPC channel  pipe://tear7d2d.core",
        "[OK] preloading textures (2048x2048 x 64)",
        "[..] waiting steam handshake  steamapi#251570",
        "[OK] frame budget: 16.6ms / target 60fps",
        "[..] spawning watchdog thread tid=0x1A2C",
        "[OK] graphics adapter: NVIDIA RTX (PCIe x16)",
        "[..] mounting world chunks  region.0.0 ~ region.7.7",
    };

    private void ShowLaunchingOverlay()
    {
        _gameDetected = false;
        _launchProgress = 0;
        _stageIndex = 0;
        _launchStart = DateTime.Now;

        MainPanel.Visibility = Visibility.Collapsed;
        LaunchingOverlay.Visibility = Visibility.Visible;
        LaunchingOverlay.Opacity = 0;

        // 入场淡入
        LaunchingOverlay.BeginAnimation(OpacityProperty, new DoubleAnimation
        {
            From = 0,
            To = 1,
            Duration = TimeSpan.FromMilliseconds(450),
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
        });

        // 中央放射光呼吸
        LaunchGlow.BeginAnimation(OpacityProperty, new DoubleAnimation
        {
            From = 0.25,
            To = 0.55,
            Duration = TimeSpan.FromSeconds(2.2),
            AutoReverse = true,
            RepeatBehavior = RepeatBehavior.Forever,
            EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
        });

        // 雷达旋转
        var rotate = new DoubleAnimation
        {
            From = 0,
            To = 360,
            Duration = TimeSpan.FromSeconds(2.8),
            RepeatBehavior = RepeatBehavior.Forever
        };
        RadarRotate.BeginAnimation(RotateTransform.AngleProperty, rotate);

        // 顶部扫描线上下扫描
        StartScanLineAnimation();

        // 网格背景
        BuildCyberGrid();

        // 文字阶段刷新
        UpdateStageText();
        LaunchSubtitle.Text = Lang.Current == Lang.LangType.CN ? "正在唤醒游戏进程" : "Waking up game process";
        LaunchTitle.Text = Lang.Current == Lang.LangType.CN ? "游戏正在启动中" : "GAME IS LAUNCHING";
        LaunchIslandText.Text = Lang.Current == Lang.LangType.CN ? "游戏启动中" : "GAME LAUNCHING";

        // 启动各项 timer
        StartLaunchTimers();
    }

    private void HideLaunchingOverlay()
    {
        StopLaunchTimers();
        LaunchingOverlay.Visibility = Visibility.Collapsed;
        MainPanel.Visibility = Visibility.Visible;
    }

    private void StartLaunchTimers()
    {
        // 主计时（时钟 + 假进度推进 + 阶段轮转）
        _launchTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
        _launchTimer.Tick += LaunchTimer_Tick;
        _launchTimer.Start();

        // 副标题动态点
        _dotsTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(360) };
        _dotsTimer.Tick += (_, _) =>
        {
            int n = (LaunchDots.Text.Length % 3) + 1;
            LaunchDots.Text = new string('.', n);
        };
        _dotsTimer.Start();

        // 日志推送
        _logTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(420) };
        _logTimer.Tick += (_, _) =>
        {
            string line = LogPool[_rng.Next(LogPool.Length)];
            PushLog(line, line.StartsWith("[OK]") ? "#00FF88" : "#00E5FF");
        };
        _logTimer.Start();

        // 光标闪烁
        _caretTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
        _caretTimer.Tick += (_, _) =>
        {
            Caret.Opacity = Caret.Opacity > 0.5 ? 0.1 : 1.0;
        };
        _caretTimer.Start();

        // 进度条平滑动画
        _progressTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(60) };
        _progressTimer.Tick += (_, _) =>
        {
            double width = LaunchingOverlay.ActualWidth > 0 ? 380 : 380;
            LaunchProgressFill.Width = width * Math.Clamp(_launchProgress, 0, 1);
            LaunchPercentText.Text = $"{(int)(_launchProgress * 100)}%";
        };
        _progressTimer.Start();

        // 进程检测：每 800ms 看一次
        _gameDetectTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(800) };
        _gameDetectTimer.Tick += GameDetect_Tick;
        _gameDetectTimer.Start();
    }

    private void StopLaunchTimers()
    {
        _launchTimer?.Stop(); _launchTimer = null;
        _dotsTimer?.Stop(); _dotsTimer = null;
        _logTimer?.Stop(); _logTimer = null;
        _caretTimer?.Stop(); _caretTimer = null;
        _progressTimer?.Stop(); _progressTimer = null;
        _gameDetectTimer?.Stop(); _gameDetectTimer = null;
    }

    private void LaunchTimer_Tick(object? sender, EventArgs e)
    {
        // 时钟
        var dt = DateTime.Now - _launchStart;
        LaunchClock.Text = $"{(int)dt.TotalMinutes:D2}:{dt.Seconds:D2}";

        // 假进度推进：未检测到游戏时缓慢逼近 0.9，检测到后立刻冲到 1.0
        if (!_gameDetected)
        {
            // 越接近 0.9 速度越慢
            double remaining = 0.9 - _launchProgress;
            if (remaining > 0)
                _launchProgress += Math.Max(0.001, remaining * 0.012);
        }

        // 阶段切换：按时间和进度
        int newStage = _launchProgress switch
        {
            < 0.25 => 0,
            < 0.55 => 1,
            < 0.85 => 2,
            _ => 3,
        };
        if (newStage != _stageIndex)
        {
            _stageIndex = newStage;
            UpdateStageText();
        }
    }

    private void UpdateStageText()
    {
        var arr = Lang.Current == Lang.LangType.CN ? StagesCN : StagesEN;
        LaunchStageText.Text = arr[Math.Clamp(_stageIndex, 0, arr.Length - 1)];

        if (_stageIndex == 3)
        {
            LaunchSubtitle.Text = Lang.Current == Lang.LangType.CN
                ? "等待游戏窗口接入"
                : "Waiting for game window";
        }
    }

    private void GameDetect_Tick(object? sender, EventArgs e)
    {
        if (_gameDetected) return;

        if (ModDeployer.IsGameRunning())
        {
            _gameDetected = true;

            // 立刻冲满进度
            _launchProgress = 1.0;
            LaunchIslandText.Text = Lang.Current == Lang.LangType.CN ? "游戏已启动" : "GAME ONLINE";
            LaunchIslandDot.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00FF88"));
            LaunchTitle.Text = Lang.Current == Lang.LangType.CN ? "启动完毕" : "LAUNCH COMPLETE";
            LaunchSubtitle.Text = Lang.Current == Lang.LangType.CN ? "祝你好运，幸存者" : "Good luck, survivor";
            LaunchDots.Text = "";
            PushLog(Lang.Current == Lang.LangType.CN
                ? "[OK] 游戏进程已就绪，启动器即将关闭"
                : "[OK] game process online, closing launcher", "#00FF88");

            // 1.6 秒后关闭启动器
            var closer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(1600) };
            closer.Tick += (_, _) =>
            {
                closer.Stop();
                FadeOutAndClose();
            };
            closer.Start();
        }
    }

    private void FadeOutAndClose()
    {
        StopLaunchTimers();
        var fade = new DoubleAnimation
        {
            From = 1.0,
            To = 0.0,
            Duration = TimeSpan.FromMilliseconds(600),
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn }
        };
        fade.Completed += (_, _) => Application.Current.Shutdown();
        this.BeginAnimation(OpacityProperty, fade);
    }

    // ===== 扫描线 =====
    private void StartScanLineAnimation()
    {
        // Y 从 0 -> 484 (整窗高度大致)
        var anim = new DoubleAnimation
        {
            From = 0,
            To = 484,
            Duration = TimeSpan.FromSeconds(2.6),
            RepeatBehavior = RepeatBehavior.Forever,
            EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
        };
        ScanLineTransform.BeginAnimation(TranslateTransform.YProperty, anim);

        // 透明度也呼吸
        ScanLine.BeginAnimation(OpacityProperty, new DoubleAnimation
        {
            From = 0.4,
            To = 0.95,
            Duration = TimeSpan.FromSeconds(1.3),
            AutoReverse = true,
            RepeatBehavior = RepeatBehavior.Forever
        });
    }

    // ===== 赛博网格 =====
    private void BuildCyberGrid()
    {
        GridCanvas.Children.Clear();

        const int spacing = 36;
        double w = 744; // 整窗 760 - 边距
        double h = 484;
        var brush = new SolidColorBrush(Color.FromArgb(255, 0, 229, 255));

        for (double x = 0; x <= w; x += spacing)
        {
            var line = new Line
            {
                X1 = x, Y1 = 0, X2 = x, Y2 = h,
                Stroke = brush,
                StrokeThickness = 0.5,
                Opacity = 0.35
            };
            GridCanvas.Children.Add(line);
        }
        for (double y = 0; y <= h; y += spacing)
        {
            var line = new Line
            {
                X1 = 0, Y1 = y, X2 = w, Y2 = y,
                Stroke = brush,
                StrokeThickness = 0.5,
                Opacity = 0.35
            };
            GridCanvas.Children.Add(line);
        }
    }

    // ===== 日志推送 (3 行滚动) =====
    private void PushLog(string text, string hexColor)
    {
        if (LogLine1 == null) return;
        LogLine1.Text = LogLine2.Text;
        LogLine2.Text = LogLine3.Text;
        LogLine3.Text = $"> {text}";

        try
        {
            var color = (Color)ColorConverter.ConvertFromString(hexColor);
            LogLine3.Foreground = new SolidColorBrush(color);
        }
        catch { }
    }
}
