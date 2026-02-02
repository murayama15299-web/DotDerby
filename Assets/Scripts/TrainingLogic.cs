using UnityEngine;

namespace DotDerby
{
    public static class TrainingLogic
    {
        public static int CalcFailRate(GameState s)
        {
            int rate = 10;

            if (s.stamina < 70) rate += 5;
            if (s.stamina < 40) rate += 10;
            if (s.stamina < 20) rate += 10;

            int moodPenalty = (4 - s.mood) * 3; // 0..12
            rate += moodPenalty;

            return Mathf.Clamp(rate, 0, 80);
        }

        public static void DoTrain(GameManager gm)
        {
            var s = gm.State;
            var stat = (StatType)Random.Range(0, 5);

            int failRate = CalcFailRate(s);
            bool fail = Random.Range(0, 100) < failRate;

            int baseGain = 8;
            int staminaCost = (stat == StatType.Wisdom) ? 8 : 12;

            if (!fail)
            {
                s.AddStat(stat, baseGain);
                s.stamina -= staminaCost;
                gm.lastLog = $"練習：{ToJP(stat)} 成功！ +{baseGain} / 体力 -{staminaCost}（失敗率 {failRate}%）";
            }
            else
            {
                int gain = 3;
                s.AddStat(stat, gain);
                s.stamina -= staminaCost + 8;
                s.mood = Mathf.Max(0, s.mood - 1);
                gm.lastLog = $"練習：{ToJP(stat)} 失敗… +{gain} / 体力 -{staminaCost + 8} / やる気 -1（失敗率 {failRate}%）";
            }

            s.ClampAll();
            gm.Save();
        }

        public static void DoRest(GameManager gm)
        {
            var s = gm.State;
            int heal = 35;
            s.stamina += heal;

            bool moodUp = Random.Range(0, 100) < 40;
            if (moodUp) s.mood = Mathf.Min(4, s.mood + 1);

            gm.lastLog = moodUp ? $"休憩：体力 +{heal} / やる気 +1" : $"休憩：体力 +{heal}";
            s.ClampAll();
            gm.Save();
        }

        public static void DoOuting(GameManager gm)
        {
            var s = gm.State;
            int r = Random.Range(0, 100);

            if (r < 50)
            {
                s.mood = Mathf.Min(4, s.mood + 1);
                gm.lastLog = "おでかけ：リフレッシュ！ やる気 +1";
            }
            else if (r < 80)
            {
                s.stamina += 15;
                gm.lastLog = "おでかけ：気分転換。体力 +15";
            }
            else
            {
                var stat = (StatType)Random.Range(0, 5);
                s.AddStat(stat, 5);
                gm.lastLog = $"おでかけ：良い刺激！ {ToJP(stat)} +5";
            }

            s.ClampAll();
            gm.Save();
        }

        public static void NextTurn(GameManager gm)
        {
            gm.State.turn++;
            gm.State.ClampAll();
            gm.Save();
        }

        static string ToJP(StatType t)
        {
            return t switch
            {
                StatType.Speed => "スピード",
                StatType.Stamina => "スタミナ",
                StatType.Power => "パワー",
                StatType.Guts => "根性",
                StatType.Wisdom => "賢さ",
                _ => t.ToString()
            };
        }
    }
}
