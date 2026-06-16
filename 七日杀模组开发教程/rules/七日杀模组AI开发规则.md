# 七日杀 Mod AI 开发规则

> 本规则适用于所有七日杀 (7 Days to Die) 模组开发任务。
> AI 智能体必须严格遵守以下三条核心规则，确保 Mod 一次性开发完毕且正常使用。

---

## 规则一：必须基于真实游戏数据源开发

### 1.1 核心原则

**禁止凭空捏造任何游戏数据。** 所有 XML 节点名称、属性名、属性值、控件名称、buff 名称、被动效果名称等，必须从用户游戏安装目录的实际文件中获取和验证。

### 1.2 游戏数据目录

游戏核心数据位于安装根目录下的 `Data/` 文件夹：

```
<游戏根目录>/
└── Data/
    └── Config/                    ← 游戏配置数据源
        ├── buffs.xml              ← Buff 定义
        ├── items.xml              ← 物品定义
        ├── blocks.xml             ← 方块定义
        ├── recipes.xml            ← 配方定义
        ├── progression.xml        ← 技能树
        ├── entityclasses.xml      ← 实体属性
        ├── quests.xml             ← 任务定义
        ├── XUi/                   ← 游戏内 UI
        │   ├── windows.xml
        │   ├── controls.xml
        │   ├── styles.xml
        │   └── xui.xml
        └── XUi_Menu/             ← 主菜单 UI
            ├── windows.xml
            ├── controls.xml
            ├── styles.xml
            └── xui.xml
```

### 1.3 开发前必须执行的步骤

在开始任何 Mod 开发之前，AI 必须执行以下操作：

1. **确认游戏安装目录**：如果用户未提供游戏根目录，AI 必须主动询问：
   > "请问你的七日杀游戏安装根目录是哪里？例如：`D:\Steam\steamapps\common\7 Days To Die`"

2. **扫描并阅读相关数据文件**：根据 Mod 功能需求，读取 `Data/Config/` 下对应的原始 XML 文件，确认：
   - 节点结构和层级
   - 属性名称和可选值
   - 现有 buff/item/block 的名称和 ID
   - XUi 控件的实际定义和参数

3. **验证 XPath 路径**：在编写补丁前，必须确认目标 XPath 在原始文件中真实存在。

### 1.4 禁止行为

- 禁止假设或猜测 XML 节点名称
- 禁止使用未经验证的 buff 名称或被动效果名称
- 禁止根据用户随意编造的信息开发 Mod
- 禁止跳过数据源验证直接编写补丁

---

## 规则二：确保 XML 配置补丁格式正确，一次性可用

### 2.1 核心原则

**Mod 必须一次性开发完毕且正常使用。** 补丁文件的路径、格式、XPath 必须完全正确，不允许出现"补丁未应用"的错误。

### 2.2 补丁文件路径规则

补丁文件路径必须与游戏原始文件路径完全匹配：

| 游戏原始路径 | Mod 补丁路径 |
|-------------|-------------|
| `Data/Config/buffs.xml` | `Mods/MyMod/Config/buffs.xml` |
| `Data/Config/items.xml` | `Mods/MyMod/Config/items.xml` |
| `Data/Config/blocks.xml` | `Mods/MyMod/Config/blocks.xml` |
| `Data/Config/recipes.xml` | `Mods/MyMod/Config/recipes.xml` |
| `Data/Config/XUi/windows.xml` | `Mods/MyMod/Config/XUi/windows.xml` |
| `Data/Config/XUi/controls.xml` | `Mods/MyMod/Config/XUi/controls.xml` |
| `Data/Config/XUi/styles.xml` | `Mods/MyMod/Config/XUi/styles.xml` |
| `Data/Config/XUi_Menu/windows.xml` | `Mods/MyMod/Config/XUi_Menu/windows.xml` |
| `Data/Config/XUi_Menu/controls.xml` | `Mods/MyMod/Config/XUi_Menu/controls.xml` |

**关键**：子目录路径必须完全一致。例如 `XUi/windows.xml` 必须放在 `Config/XUi/windows.xml`，不能放在 `Config/windows.xml`。

### 2.3 补丁文件格式规范

1. 根元素必须是 `<configs>`，不是其他名称
2. 每个补丁操作必须指定正确的 `xpath` 属性
3. XPath 必须基于实际读取的原始 XML 结构编写
4. 使用 `remove` + `append` 组合替换整个元素时，确保 `remove` 和 `append` 的目标层级一致

### 2.4 ModInfo.xml 规范

必须使用 V2 格式（根元素 `<xml>`），V1 格式（根元素 `<ModInfo>`）已被废弃：

```xml
<?xml version="1.0" encoding="UTF-8"?>
<xml>
    <Name value="MyMod" />
    <DisplayName value="我的Mod" />
    <Version value="1.0.0" />
    <Description value="描述" />
    <Author value="作者" />
    <Website value="" />
</xml>
```

- `Name` 仅允许 `[0-9a-zA-Z_-]`，必须唯一
- `DisplayName` 非空

### 2.5 开发完成后的验证清单

在交付 Mod 之前，AI 必须自检以下项目：

- [ ] 所有补丁文件的路径是否与游戏原始路径匹配
- [ ] 所有 XPath 是否基于实际读取的原始 XML 结构
- [ ] `<configs>` 根元素是否正确
- [ ] ModInfo.xml 是否为 V2 格式
- [ ] 数据层（如 BagSize）与 UI 层（如 grid rows*cols）是否一致
- [ ] buff 触发机制是否可靠（推荐使用 `buffStatusCheck01` 常驻触发）

---

## 规则三：XUi 界面 Mod 开发原则

### 3.1 核心原则

**功能优先，美化按需。** UI 开发以实现用户要求的功能为第一优先级，美化程度根据用户需求调整。

### 3.2 功能实现标准

- 必须完整实现用户要求的所有功能
- 数据层和 UI 层必须同步修改（如背包格数 = BagSize = grid rows × cols）
- 控件名称（`name` 属性）必须与游戏控制器绑定的名称一致，不能随意改名
- 控制器（`controller` 属性）必须使用正确的类名

### 3.3 UI 美化策略

| 用户需求 | 美化程度 | 说明 |
|---------|---------|------|
| 仅要求功能 | 最小美化 | 保持原版风格，仅调整尺寸和布局以适配功能 |
| 提及风格/主题 | 中度美化 | 按用户指定的风格（科幻、暗黑、简约等）调整配色和装饰 |
| 明确要求美化 | 完整美化 | 全面设计：配色方案、装饰元素、动画效果、容量显示等 |

### 3.4 美化时的注意事项

- 修改窗口外观时，不能破坏控件名称和控制器绑定
- 格子模板（如 `backpack_item_stack`）替换时，必须保留所有必要的子元素（图标、耐久条、数量标签等）
- 缩小格子尺寸时，内部所有元素（图标、文字、进度条）必须按比例缩小
- 颜色格式为 `R,G,B,A`，每个分量 0-255

### 3.5 常用美化元素

```xml
<!-- 装饰线 -->
<sprite depth="1" pos="0,-1" width="920" height="2" color="0,220,255,200" type="sliced"/>

<!-- 半透明底色 -->
<sprite depth="0" pos="0,0" width="920" height="420" color="5,12,28,200" type="sliced"/>

<!-- 发光边框 -->
<sprite depth="8" sprite="menu_empty3px" color="0,160,200,120" type="sliced" fillcenter="false"/>

<!-- 容量进度条 -->
<sprite depth="2" name="capacityBarBg" width="200" height="6" color="15,30,55,255" type="sliced"/>
<filledsprite depth="3" name="capacityBarFill" width="200" height="6" color="0,220,255,220" fill="{encumbrance}" type="filled"/>
```

---

## 附：开发流程总览

```
用户提出 Mod 需求
  │
  ├─→ 1. 确认游戏安装目录（未提供则主动询问）
  │
  ├─→ 2. 读取 Data/Config/ 下相关原始文件
  │       - 确认节点结构、属性名、现有名称
  │       - 验证 XPath 目标存在
  │
  ├─→ 3. 编写 Mod 文件
  │       ├── ModInfo.xml (V2 格式)
  │       └── Config/
  │           ├── 数据层补丁 (buffs.xml, items.xml 等)
  │           └── UI 层补丁 (XUi/windows.xml 等，如需要)
  │
  ├─→ 4. 自检验证清单
  │       - 路径匹配、XPath 正确、数据/UI 一致
  │
  └─→ 5. 交付 Mod，告知用户部署方式和测试方法
```
