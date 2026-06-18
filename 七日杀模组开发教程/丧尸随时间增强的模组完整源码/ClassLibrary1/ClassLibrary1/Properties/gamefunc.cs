using System;
using UnityEngine;

namespace ZombieDayScale
{
    /// <summary>
    /// 动态尸潮系统
    ///
    /// 功能:
    /// 1. 血月必定触发尸潮
    /// 2. 随机日夜间触发小规模尸潮
    /// 3. 尸潮规模随天数增长
    ///
    /// V3.0修复: 使用 EntityClass.GetId() 替代错误的 FromString()
    /// 僵尸名列表已根据 output/01_Entities.txt 验证, 移除不存在的实体名
    /// </summary>
    public static class HordeSystem
    {
        // ── 配置 ──
        private const int START_DAY_RANDOM = 8;
        private const float BASE_CHANCE = 0.1f;
        private const float CHANCE_PER_DAY = 0.01f;
        private const int COOLDOWN_DAYS = 2;
        private const int BASE_COUNT_RANDOM = 15;
        private const int BASE_COUNT_BLOODMOON = 50;
        private const float MIN_SPAWN_DIST = 45f;
        private const float MAX_SPAWN_DIST = 70f;

        // ── 常用僵尸实体类名 ──
        // V3.0验证 (来自 output/01_Entities.txt, 只保留 Category=Zombie 且 Spawnable=True)
        // 移除不存在的: zombieMale, zombieFemale, zombieOldMale, zombieOldFemale, zombieMiner, zombieChef, zombieHelper
        private static readonly string[] ZombieNames = {
            "zombieArlene", "zombieBiker", "zombieBoe", "zombieBusinessMan",
            "zombieDarlene", "zombieJoe", "zombieLumberjack",
            "zombieMarlene", "zombieMoe", "zombieNurse",
            "zombieSkateboarder", "zombieSoldier", "zombieUtilityWorker",
            "zombieFemaleFat", "zombieScreamer", "zombieLabCharged",
            "zombieLabRadiated", "zombieWightFeral"
        };

        // ── 状态 ──
        private static int _lastHordeDay = -100;
        public static int _lastRandomHordeDay = -100;
        private static float _nextCheckTime = 0f;
        private static float _checkInterval = 1.0f;
        private static readonly System.Random _rng = new System.Random();

        /// <summary>
        /// 获取随机尸潮触发概率
        /// </summary>
        public static float GetRandomChance(int day)
        {
            if (day < START_DAY_RANDOM) return 0f;
            return Mathf.Clamp(BASE_CHANCE + (day - START_DAY_RANDOM) * CHANCE_PER_DAY, 0f, 0.5f);
        }

        /// <summary>
        /// 获取随机尸潮数量
        /// </summary>
        public static int GetRandomCount(int day)
        {
            float multiplier = 1f + day / 10f;
            return Mathf.RoundToInt(BASE_COUNT_RANDOM * multiplier);
        }

        /// <summary>
        /// 获取血月尸潮数量
        /// </summary>
        public static int GetBloodMoonCount(int day)
        {
            float multiplier = 1f + day / 10f;
            return Mathf.RoundToInt(BASE_COUNT_BLOODMOON * multiplier);
        }

        /// <summary>
        /// 检查并触发尸潮 (由 ZombieDayScaleMod.OnGameUpdate 每秒调用)
        /// </summary>
        public static void Tick(World world, int currentDay, bool isBloodMoonTime)
        {
            if (Time.time < _nextCheckTime) return;
            _nextCheckTime = Time.time + _checkInterval;

            try
            {
                // 血月触发
                if (isBloodMoonTime && currentDay != _lastHordeDay)
                {
                    _lastHordeDay = currentDay;
                    int count = GetBloodMoonCount(currentDay);
                    SpawnHorde(world, count, true);
                }

                // 随机触发
                if (!isBloodMoonTime && CanRandomHorde(currentDay))
                {
                    float chance = GetRandomChance(currentDay);
                    if (UnityEngine.Random.value < chance)
                    {
                        _lastRandomHordeDay = currentDay;
                        int count = GetRandomCount(currentDay);
                        SpawnHorde(world, count, false);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning("[HordeSystem] Tick error: " + ex.Message);
            }
        }

        private static bool CanRandomHorde(int currentDay)
        {
            return (currentDay - _lastRandomHordeDay) >= COOLDOWN_DAYS;
        }

        /// <summary>
        /// 查询今天是否可以触发随机尸潮（不修改 _lastRandomHordeDay，仅做判断）
        /// </summary>
        public static bool CanRandomHordeToday(int currentDay, ref int lastRandomDay, bool dummy)
        {
            return (currentDay - lastRandomDay) >= COOLDOWN_DAYS;
        }

        /// <summary>
        /// 生成尸潮 — 在玩家周围环形区域刷出僵尸
        /// </summary>
        private static void SpawnHorde(World world, int count, bool isBloodMoon)
        {
            if (world == null) return;
            EntityPlayerLocal player = world.GetPrimaryPlayer();
            if (player == null) return;

            Vector3 playerPos = player.GetPosition();
            string hordeType = isBloodMoon ? "血月" : "随机";
            int spawned = 0;
            int failed = 0;

            for (int i = 0; i < count; i++)
            {
                try
                {
                    Vector3 spawnPos = GetSpawnPosition(world, playerPos);
                    if (spawnPos == Vector3.zero) continue;

                    // 随机选择一个僵尸类型
                    string zombieName = ZombieNames[_rng.Next(ZombieNames.Length)];

                    // V3.0: 使用 EntityClass.GetId() 获取正确的实体ID
                    int entityId = EntityClass.GetId(zombieName);
                    if (entityId <= 0)
                    {
                        failed++;
                        continue;
                    }

                    Entity entity = EntityFactory.CreateEntity(entityId, spawnPos);
                    if (entity != null)
                    {
                        entity.SetSpawnerSource(EnumSpawnerSource.Dynamic);
                        world.SpawnEntityInWorld(entity);
                        spawned++;
                    }
                    else
                    {
                        failed++;
                    }
                }
                catch (Exception ex)
                {
                    failed++;
                }
            }

            Debug.Log("[HordeSystem] " + hordeType + "尸潮: 成功 " + spawned + " / 失败 " + failed);
        }

        /// <summary>
        /// 计算生成位置 (在玩家周围 MIN-MAX 格的环形区域)
        /// </summary>
        private static Vector3 GetSpawnPosition(World world, Vector3 center)
        {
            float angle = UnityEngine.Random.Range(0f, 360f);
            float dist = UnityEngine.Random.Range(MIN_SPAWN_DIST, MAX_SPAWN_DIST);
            float x = center.x + Mathf.Cos(angle * Mathf.Deg2Rad) * dist;
            float z = center.z + Mathf.Sin(angle * Mathf.Deg2Rad) * dist;

            // 获取地面高度
            int bx = Mathf.FloorToInt(x);
            int bz = Mathf.FloorToInt(z);
            // V3.0: World.GetHeight 签名改为 (int x, int z)
            float y = (float)world.GetHeight(bx, bz);

            return new Vector3(x, y + 1f, z);
        }
    }
}
