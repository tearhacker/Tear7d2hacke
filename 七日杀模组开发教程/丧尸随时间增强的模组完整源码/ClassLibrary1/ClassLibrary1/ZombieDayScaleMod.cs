using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieDayScale
{
    /// <summary>
    /// 丧尸随天数强化 + 动态尸潮系统
    ///
    /// 功能:
    /// 1. 丧尸血量随天数增强 (每天 +0.1倍, 最高21倍)
    /// 2. 丧尸攻击力随天数增强 (每天 +0.05倍, 最高11倍)
    /// 3. 血月必定触发尸潮
    /// 4. 随机日夜间触发小规模尸潮
    ///
    /// V3.0修复:
    /// - 通知改用 XUiC_ChatOutput.AddMessage (V3.0标准API)
    /// - 反射调用 XUiC_InfoEntry 失败, 因该类在 V3.0 firstpass 中不存在
    /// </summary>
    public class ZombieDayScaleMod : IModApi
    {
        // ── 常量 ──
        private const int MAX_DAY = 200;
        private const float CHECK_INTERVAL = 1.0f;

        // ── 状态 ──
        private Mod _mod;
        private int _lastDay = -1;
        private float _nextCheck = 0f;
        private bool _initialized;

        // ── 丧尸血量追踪 ──
        private readonly Dictionary<int, float> _baseHealthMap = new Dictionary<int, float>();

        // ================================================================
        //  增强公式
        // ================================================================
        public static float GetHealthMultiplier(int day)
        {
            int d = Mathf.Clamp(day, 0, MAX_DAY);
            return 1f + d / 10f;
        }

        public static float GetAttackMultiplier(int day)
        {
            int d = Mathf.Clamp(day, 0, MAX_DAY);
            return 1f + d / 20f;
        }

        // ================================================================
        //  IModApi 入口
        // ================================================================
        public void InitMod(Mod _modInstance)
        {
            _mod = _modInstance;

            ModEvents.GameUpdate.RegisterHandler(OnGameUpdate);
            ModEvents.GameStartDone.RegisterHandler(OnGameStartDone);
            ModEvents.GameShutdown.RegisterHandler(OnGameShutdown);

            // 初始化完成通知 (确保日志能输出)
            try { Debug.Log("[ZombieDayScale] Mod initialized"); } catch { }
        }

        // ================================================================
        //  事件回调
        // ================================================================
        private void OnGameStartDone(ref ModEvents.SGameStartDoneData _data)
        {
            _initialized = false;
            _lastDay = -1;
            _baseHealthMap.Clear();
        }

        private void OnGameUpdate(ref ModEvents.SGameUpdateData _data)
        {
            if (Time.time < _nextCheck) return;
            _nextCheck = Time.time + CHECK_INTERVAL;

            World world = GameManager.Instance.World;
            if (world == null) return;

            EntityPlayerLocal player = world.GetPrimaryPlayer();
            if (player == null || player.IsDead()) return;

            int currentDay = world.WorldDay;

            // 首次进入
            if (!_initialized)
            {
                _lastDay = currentDay;
                _initialized = true;
                ApplyScaleToAllZombies(world, currentDay);
                ShowChatMessage(player, "丧尸强化+尸潮系统已激活", "00FF00");
                ShowZombieStats(player, currentDay, currentDay);
            }

            // 天数变更
            if (currentDay != _lastDay)
            {
                int oldDay = _lastDay;
                _lastDay = currentDay;
                ApplyScaleToAllZombies(world, currentDay);

                // 显示详细丧尸属性 + 尸潮信息
                ShowZombieStats(player, currentDay, oldDay);

                PlaySound(player);
            }

            // ── 尸潮系统 ──
            try
            {
                bool isBloodMoonTime = IsBloodMoonTime(world);
                bool isRandomHordeTime = HordeSystem.CanRandomHordeToday(currentDay, ref HordeSystem._lastRandomHordeDay, false);

                if (isBloodMoonTime)
                {
                    // 血月通知 (3行: 标题/数量/提示)
                    int bloodMoonCount = Mathf.RoundToInt(50f * (1f + currentDay / 10f));
                    ShowChatMessage(player, "=== 血月尸潮来袭! ===", "FF0000");
                    ShowChatMessage(player,
                        string.Format("目标数量: {0} 只  |  范围: 45-70米", bloodMoonCount),
                        "CC0000");
                    ShowChatMessage(player, "准备战斗!", "FF6666");
                }
                else if (isRandomHordeTime && currentDay >= 8)
                {
                    // 随机尸潮通知 (3行: 标题/数量/提示)
                    int randomCount = Mathf.RoundToInt(15f * (1f + currentDay / 10f));
                    float chance = Mathf.Min(0.5f, 0.1f + (currentDay - 8) * 0.01f);
                    if (UnityEngine.Random.value < chance)
                    {
                        ShowChatMessage(player, "=== 随机尸潮来袭! ===", "FF6600");
                        ShowChatMessage(player,
                            string.Format("目标数量: {0} 只  |  范围: 45-70米", randomCount),
                            "FF8800");
                        ShowChatMessage(player, "准备战斗!", "FFAA00");
                    }
                }

                HordeSystem.Tick(world, currentDay, isBloodMoonTime);
            }
            catch (Exception ex)
            {
                try { Debug.LogWarning("[ZombieDayScale] HordeSystem error: " + ex.Message); } catch { }
            }
        }

        private void OnGameShutdown(ref ModEvents.SGameShutdownData _data)
        {
            ModEvents.GameUpdate.UnregisterHandler(OnGameUpdate);
            ModEvents.GameStartDone.UnregisterHandler(OnGameStartDone);
            ModEvents.GameShutdown.UnregisterHandler(OnGameShutdown);
        }

        // ================================================================
        //  丧尸血量强化
        // ================================================================
        private void ApplyScaleToAllZombies(World world, int day)
        {
            float hpMul = GetHealthMultiplier(day);
            List<Entity> entities = world.Entities.list;
            if (entities == null) return;

            int count = 0;
            for (int i = 0; i < entities.Count; i++)
            {
                EntityAlive ea = entities[i] as EntityAlive;
                if (ea == null || !ea.IsAlive()) continue;
                if (!IsZombie(ea)) continue;

                try
                {
                    if (!_baseHealthMap.ContainsKey(ea.entityId))
                    {
                        _baseHealthMap[ea.entityId] = ea.Health;
                    }

                    float baseHp = _baseHealthMap[ea.entityId];
                    float newMax = baseHp * hpMul;

                    if (ea.Stats != null && ea.Stats.Health != null)
                    {
                        var health = ea.Stats.Health;
                        // V3.0: Stat.Max 是只读属性, 使用 SetMax 方法
                        var setMaxMethod = health.GetType().GetMethod("SetMax", new System.Type[] { typeof(float) });
                        if (setMaxMethod != null)
                        {
                            setMaxMethod.Invoke(health, new object[] { newMax });
                        }
                        health.Value = newMax;
                    }
                    count++;
                }
                catch (Exception ex)
                {
                    // 静默失败, 避免日志爆炸
                }
            }
        }

        private static bool IsZombie(EntityAlive ea)
        {
            if (ea == null) return false;
            string name = ea.EntityName ?? "";
            return name.StartsWith("zombie", StringComparison.OrdinalIgnoreCase);
        }

        // ================================================================
        //  血月检测
        // ================================================================
        private bool IsBloodMoonTime(World world)
        {
            try
            {
                int bloodMoonDay = GameStats.GetInt(EnumGameStats.BloodMoonDay);
                int currentDay = world.WorldDay;
                int hour = world.WorldHour;

                // V3.0: 血月从22:00开始到04:00结束
                if (currentDay == bloodMoonDay && hour >= 22) return true;
                if (currentDay == bloodMoonDay + 1 && hour < 4) return true;

                return false;
            }
            catch
            {
                return false;
            }
        }

        // ================================================================
        //  UI 通知 (使用 V3.0 标准 XUiC_ChatOutput API)
        // ================================================================

        /// <summary>
        /// 显示丧尸强化 + 尸潮详细信息 (3-4行通知)
        /// 第1行: 标题 (第N天 丧尸强化+尸潮)
        /// 第2行: 血量倍率 + 攻击倍率
        /// 第3行: 血月尸潮数量 + 随机尸潮数量
        /// 第4行: 随机尸潮触发概率 (新一天才显示)
        /// </summary>
        private void ShowZombieStats(EntityPlayerLocal player, int currentDay, int oldDay)
        {
            float hpMul = GetHealthMultiplier(currentDay);
            float atkMul = GetAttackMultiplier(currentDay);

            // 血月尸潮数量 = 50 × (1 + 天数/10)
            int bloodMoonCount = Mathf.RoundToInt(50f * (1f + currentDay / 10f));
            // 随机尸潮数量 = 15 × (1 + 天数/10)
            int randomCount = Mathf.RoundToInt(15f * (1f + currentDay / 10f));
            // 随机尸潮触发概率 = 10% + (天数-8) × 1%, 最高50%
            float randomChance = currentDay < 8 ? 0f
                : Mathf.Min(0.5f, 0.1f + (currentDay - 8) * 0.01f);

            // ── 第1行: 标题 ──
            ShowChatMessage(player,
                string.Format("=== 第 {0} 天 丧尸强化+尸潮 ===", currentDay),
                "FFA500");

            // ── 第2行: 强化倍率 ──
            ShowChatMessage(player,
                string.Format("血量倍率: x{0:F1}  |  攻击倍率: x{1:F1}", hpMul, atkMul),
                "FFFF00");

            // ── 第3行: 尸潮数量 ──
            ShowChatMessage(player,
                string.Format("血月尸潮: {0} 只  |  随机尸潮: {1} 只", bloodMoonCount, randomCount),
                "FF6666");

            // ── 第4行: 随机尸潮触发概率 (新一天才显示) ──
            if (currentDay != oldDay && oldDay > 0)
            {
                ShowChatMessage(player,
                    string.Format("随机尸潮触发概率: {0:F0}%", randomChance * 100f),
                    randomChance > 0 ? "00BFFF" : "888888");
            }
        }

        /// <summary>
        /// 在游戏内聊天框显示消息 (V3.0 XUiC_ChatOutput.AddMessage)
        /// V3.0 BBCode格式: [RRGGBB]文字[-] (如 [FFFFFF]白色文字[-])
        /// </summary>
        private void ShowChatMessage(EntityPlayerLocal player, string message, string colorHex = "FFFFFF")
        {
            try
            {
                // 获取玩家XUi实例
                LocalPlayerUI ui = LocalPlayerUI.GetUIForPlayer(player);
                if (ui == null) return;

                // V3.0聊天框BBCode格式: [RRGGBB]内容[-] (用 [-] 关闭颜色)
                string coloredMsg = string.Format("[{0}][ZombieDayScale] {1}[-]", colorHex, message);

                // 调用V3.0标准API
                XUiC_ChatOutput.AddMessage(
                    ui.xui,
                    EnumGameMessages.PlainTextLocal,
                    coloredMsg,
                    EChatType.Global,
                    EChatDirection.Inbound,
                    -1,
                    null,
                    null,
                    EMessageSender.Server
                );
            }
            catch
            {
                // 静默失败
            }
        }

        // ================================================================
        //  音效
        // ================================================================
        private static void PlaySound(EntityPlayerLocal player)
        {
            try
            {
                if (player != null)
                {
                    // V3.0: PlayOneShot 签名为 (string, bool, bool, bool, AnimationEvent, float)
                    player.PlayOneShot("quest_complete", false, false, false, null, 1f);
                }
            }
            catch { }
        }
    }
}
