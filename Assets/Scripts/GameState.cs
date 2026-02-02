using System;

namespace DotDerby
{
    [Serializable]
    public class GameState
    {
        // 進行
        public int turn = 1;          // 1..36（12/24/36は行動後に目標レース）
        public int stamina = 100;     // 体力 0..100
        public int mood = 3;          // やる気 0..4

        // ステータス
        public int speed = 50;
        public int staminaStat = 50;
        public int power = 50;
        public int guts = 50;
        public int wisdom = 50;

        // 因子（持ち越し）
        public int factorSpeed = 0;
        public int factorStamina = 0;
        public int factorPower = 0;
        public int factorGuts = 0;
        public int factorWisdom = 0;

        // 目標レース勝敗（12/24/36）
        public bool race1Win = false;
        public bool race2Win = false;
        public bool race3Win = false;

        // 因子抽選を実行済みか（1周回につき1回）
        public bool factorsRolled = false;

        public int SumStats() => speed + staminaStat + power + guts + wisdom;

        public int GetStat(StatType t)
        {
            return t switch
            {
                StatType.Speed => speed,
                StatType.Stamina => staminaStat,
                StatType.Power => power,
                StatType.Guts => guts,
                StatType.Wisdom => wisdom,
                _ => 0
            };
        }

        public void AddStat(StatType t, int delta)
        {
            switch (t)
            {
                case StatType.Speed: speed += delta; break;
                case StatType.Stamina: staminaStat += delta; break;
                case StatType.Power: power += delta; break;
                case StatType.Guts: guts += delta; break;
                case StatType.Wisdom: wisdom += delta; break;
            }
            ClampStats();
        }

        public void ClampAll()
        {
            stamina = Math.Clamp(stamina, 0, 100);
            mood = Math.Clamp(mood, 0, 4);
            ClampStats();
        }

        void ClampStats()
        {
            speed = Math.Clamp(speed, 0, 999);
            staminaStat = Math.Clamp(staminaStat, 0, 999);
            power = Math.Clamp(power, 0, 999);
            guts = Math.Clamp(guts, 0, 999);
            wisdom = Math.Clamp(wisdom, 0, 999);
        }
    }
}
