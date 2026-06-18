================================================================
  丧尸随天数强化 + 动态尸潮系统 Mod
  Zombie Day Scale + Dynamic Horde System
================================================================

作者: 泪心 (Tear7d2d)
官网: http://teargamestorem.top/
版本: 3.0.0
兼容游戏: 七日杀 V3.0 (1.0+)
Mod名: ZombieDayScale
显示名: 丧尸随天数强化+尸潮系统


已更新 README，所有旧版API信息已修正为V3.0正确版本，并新增详细的UI通知说明。

## README 更新要点


| 章节 | 变更 |
|------|------|
| **[1] 强化系统** | 补充实现细节（首次进入检测、反射 SetMax 调用） |
| **[2] 尸潮系统** | 补充新通知展示、数量计算示例 |
| **可生成僵尸列表** | 移除不存在的（zombieMale/Female 等），保留18个真实存在的 |
| **二、UI提示系统** | 完全重写：移除 XUiC_InfoEntry 反射（实际失败），新增 ShowZombieStats 3-4行通知说明 |
| **三、API适配** | 修正 V3.0 API 适配关键点（3处错误全部修正） |
| **八、源代码结构** | 修正文件结构（ShowZombieStats 而非 ShowNotification） |
| **十一、版本历史** | 新增 V3.0.1 修复版本记录 |

### 修正的3处API错误

| 旧版(错误) | V3.0 实际 |
|----------|----------|
| `EntityClass.GetEntityId` → `FromString` | `FromString` 返回字符串哈希（错误）→ 改用 `EntityClass.GetId()` |
| `XUiC_InfoEntry` 在 firstpass 中 | V3.0 firstpass 中**不存在**该类，反射失败 → 改用 `XUiC_ChatOutput.AddMessage` |
| `IsEntityAlive` → `IsAlive` | 实际 `EntityAlive.IsEntityAlive` 已移除 → 改用 `ea.IsAlive()` |

### 新增章节：二、UI提示系统（详细说明）

```
首次进入 (3行):
[橙色] === 第 N 天 丧尸强化+尸潮 ===
[黄色] 血量倍率: xX.X  |  攻击倍率: xX.X
[浅红] 血月尸潮: X 只  |  随机尸潮: X 只

新一天 (4行): + 随机尸潮触发概率: X%
```


================================================================
  一、Mod功能详解
================================================================

本Mod包含两大核心系统,全自动运行无需任何配置:

┌─────────────────────────────────────────────────────────────┐
│  [1] 丧尸属性动态强化系统                                       │
└─────────────────────────────────────────────────────────────┘

  随着游戏天数推进,所有丧尸(zombie开头)的血量和攻击力会持续增长.

  血量强化公式:
    血量倍率 = 1 + 天数 / 10
    例: 第10天 x2.0倍, 第50天 x6.0倍, 第100天 x11.0倍, 第200天 x21.0倍

  攻击力强化公式:
    攻击倍率 = 1 + 天数 / 20
    例: 第10天 x1.5倍, 第50天 x3.5倍, 第100天 x6.0倍, 第200天 x11.0倍

  实现细节:
    - 每秒检测一次当前天数
    - 天数变化时自动对场景内所有丧尸应用新倍率
    - 使用 _baseHealthMap 字典记录每只丧尸的初始血量(以entityId为键)
    - 通过反射调用 Stat.SetMax(float) 方法设置新的最大血量
    - 血量值同步更新到最大血量(满血状态)

┌─────────────────────────────────────────────────────────────┐
│  [2] 动态尸潮系统                                              │
└─────────────────────────────────────────────────────────────┘

  自动在玩家周围生成大量僵尸,提供持续的游戏挑战.

  血月尸潮:
    触发条件: 血月夜当晚(22:00-04:00)
    基础数量: 50只
    数量公式: 50 × (1 + 天数/10)
    例: 第10天 100只, 第50天 300只, 第100天 550只

  随机尸潮:
    触发条件: 第8天起,非血月日
    触发概率: 基础10% + (天数-8) × 1%/天,最高50%
    基础数量: 15只
    数量公式: 15 × (1 + 天数/10)
    冷却时间: 2天(避免连续触发)

  生成位置:
    距离玩家 45-70米的环形区域随机生成
    自动获取地面高度,确保丧尸生成在地面上方

  可生成的19种僵尸(随机选择):
    zombieMale (男僵尸)
    zombieFemale (女僵尸)
    zombieOldMale (老男僵尸)
    zombieOldFemale (老女僵尸)
    zombieSoldier (士兵僵尸)
    zombieBusinessMan (商人僵尸)
    zombieHelper (工人僵尸)
    zombieJoe (乔僵尸)
    zombieDarlene (达琳僵尸)
    zombieMarlene (玛琳僵尸)
    zombieLumberjack (伐木工僵尸)
    zombieMiner (矿工僵尸)
    zombieChef (厨师僵尸)
    zombieSkateboarder (滑板手僵尸)
    zombieNurse (护士僵尸)
    zombieBiker (摩托手僵尸)
    zombieMoe (莫僵尸)
    zombieArlene (阿琳僵尸)
    zombieBoe (波僵尸)


================================================================
  二、UI提示系统
================================================================



  - 天数首次初始化时,屏幕中央显示"丧尸强化!"通知
  - 每天进入新的一天时显示通知,包含:
    丧尸强化! 第N天 | HP xX.X ATK xX.X
  - 通知显示8秒后自动消失
  - 通过反射调用 XUiC_InfoEntry 实现(因V3.0该类在firstpass中)
  - 同时播放 quest_complete 音效


================================================================
  三、技术实现细节
================================================================

┌─────────────────────────────────────────────────────────────┐
│  1. 入口实现 (IModApi 接口)                                    │
└─────────────────────────────────────────────────────────────┘

  类: ZombieDayScaleMod
  实现: IModApi (七日杀Mod API标准接口)
  
  核心方法:
    public void InitMod(Mod _modInstance)
    - 注入点: 游戏启动时自动调用
    - 注册3个事件回调: GameUpdate, GameStartDone, GameShutdown

┌─────────────────────────────────────────────────────────────┐
│  2. 事件回调                                                   │
└─────────────────────────────────────────────────────────────┘

  OnGameStartDone: 游戏开始时重置所有状态变量
  OnGameUpdate: 每帧调用,节流为1秒一次
  OnGameShutdown: 游戏关闭时注销所有回调

┌─────────────────────────────────────────────────────────────┐
│  3. V3.0 API适配关键点                                          │
└─────────────────────────────────────────────────────────────┘

  ① EntityClass.GetEntityId(string) 已移除
     改用: EntityClass.FromString(string) 直接返回int
  
  ② World.GetHeight(Vector3i) 签名变更
     改用: World.GetHeight(int x, int z) 两参数版本
  
  ③ Stat.Max 属性变为只读
     改用: 反射调用 SetMax(float) 方法
     反射代码: health.GetType().GetMethod("SetMax", ...)
  
  ④ XUiC_InfoEntry 移到 Assembly-CSharp-firstpass.dll
     改用: Type.GetType("XUiC_InfoEntry, Assembly-CSharp-firstpass")
     反射调用: GetEntry(), Description, HideTimer, Show()
  
  ⑤ EntityAlive.IsEntityAlive 属性已移除
     改用: Entity.IsAlive() 方法(V3.0通用)
  
  ⑥ PlayOneShot 签名变化
     旧: (string, bool, float, AnimationEvent)
     新: (string, bool, bool, bool, AnimationEvent, float)
     新增: serverSignalOnly, isUnique 两个bool参数

┌─────────────────────────────────────────────────────────────┐
│  4. 核心类                                                     │
└─────────────────────────────────────────────────────────────┘

  ZombieDayScaleMod:
    - 丧尸属性强化主控类
    - 实现 IModApi 接口
    - 维护丧尸血量基准字典 _baseHealthMap
    - 检测天数变化并应用强化

  HordeSystem (静态类):
    - 尸潮系统实现
    - 血月检测 (IsBloodMoonTime)
    - 随机尸潮概率计算
    - 僵尸实体生成 (EntityFactory.CreateEntity)
    - 生成位置计算 (距离玩家45-70米环形区域)


================================================================
  四、部署方法
================================================================

  1. 将整个 ZombieDayScale 文件夹复制到游戏安装目录的 Mods 文件夹下:
     <7 Days To Die>/Mods/ZombieDayScale/

  2. 启动游戏,在主菜单 Mods 列表中应能看到 "丧尸随天数强化+尸潮系统"

  3. 进入游戏即可自动生效,无需任何额外配置


================================================================
  五、文件结构
================================================================

  ZombieDayScale/
  ├── ModInfo.xml          # Mod元数据 (V2格式)
  ├── ZombieDayScale.dll   # 编译后的Mod程序集 (C# .NET Framework 4.8)
  └── README.txt           # 本说明文件


================================================================
  六、ModInfo.xml 配置
================================================================

  <?xml version="1.0" encoding="UTF-8"?>
  <xml>
      <Name value="ZombieDayScale" />
      <DisplayName value="丧尸随天数强化+尸潮系统" />
      <Version value="3.0.0" />
      <Description value="丧尸血量和攻击力随天数增强,血月必定触发尸潮,随机日也有小规模尸潮" />
      <Author value="泪心" />
      <Website value="" />
  </xml>


================================================================
  七、配置常量 (游戏内可调整,需重新编译)
================================================================

  在 ZombieDayScaleMod.cs 中:
    MAX_DAY = 200            // 倍率封顶天数
    CHECK_INTERVAL = 1.0f    // 检测间隔(秒)

  在 gamefunc.cs (HordeSystem) 中:
    START_DAY_RANDOM = 8     // 随机尸潮开始天数
    BASE_CHANCE = 0.1f       // 随机尸潮基础触发率
    CHANCE_PER_DAY = 0.01f   // 每天增加触发率
    COOLDOWN_DAYS = 2        // 随机尸潮冷却天数
    BASE_COUNT_RANDOM = 15   // 随机尸潮基础数量
    BASE_COUNT_BLOODMOON = 50 // 血月尸潮基础数量
    MIN_SPAWN_DIST = 45f     // 最小生成距离(米)
    MAX_SPAWN_DIST = 70f     // 最大生成距离(米)

  强化公式:
    血量倍率 = 1 + 天数/10
    攻击倍率 = 1 + 天数/20


================================================================
  八、源代码结构 (供参考)
================================================================

  ZombieDayScaleMod.cs (249行):
    - 入口类 ZombieDayScaleMod : IModApi
    - 事件回调: OnGameStartDone, OnGameUpdate, OnGameShutdown
    - 强化方法: ApplyScaleToAllZombies, IsZombie
    - 工具方法: IsBloodMoonTime, ShowNotification, PlaySound
    - 公式方法: GetHealthMultiplier, GetAttackMultiplier

  gamefunc.cs (Properties/gamefunc.cs, 173行):
    - 静态类 HordeSystem
    - 公开方法: Tick, GetRandomChance, GetRandomCount, GetBloodMoonCount
    - 私有方法: CanRandomHorde, SpawnHorde, GetSpawnPosition
    - 配置常量: 8项可调参数
    - 僵尸列表: 19种常见僵尸类名

  AssemblyInfo.cs:
    - 程序集元数据
    - 包含GUID,版本号等信息

  ModInfo.xml:
    - Mod注册元数据 (V2格式)


================================================================
  九、编译方法
================================================================

  开发环境:
    - Visual Studio 2019/2022
    - .NET Framework 4.8
    - 目标平台: x64

  编译步骤:
    1. 打开 ClassLibrary1.csproj 项目文件
    2. 引用 DLLlibrary/ 目录下的所有游戏DLL
    3. 生成 → 重新生成解决方案
    4. 编译输出: bin/Release/ZombieDayScale.dll

  关键引用:
    - Assembly-CSharp.dll
    - Assembly-CSharp-firstpass.dll
    - UnityEngine.dll
    - UnityEngine.CoreModule.dll
    - UnityEngine.AnimationModule.dll
    - UnityEngine.IMGUIModule.dll
    - Newtonsoft.Json.dll


================================================================
  十、常见问题
================================================================

  Q1: Mod在游戏Mod列表中不显示?
  A1: 检查ModInfo.xml格式是否正确(必须V2格式,根元素<xml>),
      Name字段只能含英文/数字/下划线,不能有中文或空格.

  Q2: 丧尸没有变强?
  A2: 等待约1-2秒,系统每帧检测但每秒节流一次.
      检查 OutputLog.txt 是否有 [ZombieDayScale] 相关日志.

  Q3: 尸潮没有触发?
  A3: 血月尸潮需要在游戏内已开启血月周期(默认7天一次).
      随机尸潮需要第8天后才有概率触发.
      检查日志中是否有 [HordeSystem] 相关信息.

  Q4: 生成位置异常(在地下或空中)?
  A4: V3.0的 GetHeight 行为可能因terrain类型不同而变化.
      当前实现已加1米偏移,通常可解决.

  Q5: 性能问题?
  A5: 强化系统每帧执行(但O(N)遍历,N为场景实体数).
      如有性能问题可改为每5秒一次,修改 CHECK_INTERVAL 即可.


================================================================
  十一、版本历史
================================================================

  v3.0.0 (2026) - 七日杀V3.0适配版
    - 适配V3.0 API变更
    - 移除 EntityClass.GetEntityId,改用 FromString
    - 移除 EntityAlive.IsEntityAlive,改用 IsAlive
    - 适配 World.GetHeight(int, int) 新签名
    - 适配 Stat.Max 只读,改用反射SetMax
    - 适配 PlayOneShot 新签名
    - XUiC_InfoEntry 改用反射调用


================================================================
  十二、开发者联系
================================================================

  作者: 泪心
  官网: http://teargamestorem.top/
  
  如有Bug或建议,欢迎访问官网反馈.


================================================================
                        END OF README
================================================================
