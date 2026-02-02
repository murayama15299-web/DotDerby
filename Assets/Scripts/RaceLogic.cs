using UnityEngine;

namespace DotDerby
{
    public static class RaceLogic
    {
        public static bool RunTargetRace(GameManager gm, int raceIndex)
        {
            var s = gm.State;

            float a = 1.2f, b = 1.0f, c = 1.1f, d = 0.9f, e = 0.8f;
            float basePower = s.speed * a + s.staminaStat * b + s.power * c + s.guts * d + s.wisdom * e;

            float moodMul = 0.90f + 0.05f * s.mood; // 0.90..1.10
            basePower *= moodMul;

            float rand = Random.Range(0.93f, 1.07f); // }7%
            float finalPower = basePower * rand;

            float difficulty = raceIndex switch
            {
                0 => 420f,
                1 => 470f,
                2 => 520f,
                _ => 450f
            };

            bool win = finalPower >= difficulty;

            s.stamina -= 18;
            if (!win && Random.Range(0, 100) < 30) s.mood = Mathf.Max(0, s.mood - 1);

            s.ClampAll();
            gm.Save();
            return win;
        }
    }
}
