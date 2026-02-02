using TMPro;
using UnityEngine;

namespace DotDerby
{
    public class RaceUI : MonoBehaviour
    {
        public TextMeshProUGUI titleText;
        public TextMeshProUGUI resultText;

        void Start() => RunRace();

        void RunRace()
        {
            var gm = GameManager.I;
            var s = gm.State;

            int raceIndex = gm.GetTargetRaceIndex(s.turn);
            if (raceIndex < 0) raceIndex = 0;

            titleText.text = $"目標レース {raceIndex + 1}（ターン {s.turn}）";

            bool win = RaceLogic.RunTargetRace(gm, raceIndex);

            if (raceIndex == 0) s.race1Win = win;
            if (raceIndex == 1) s.race2Win = win;
            if (raceIndex == 2) s.race3Win = win;
            gm.Save();

            resultText.text =
                (win ? "勝利！" : "敗北…") + "\n\n" +
                $"体力: {s.stamina}/100    やる気: {s.mood}/4\n" +
                $"ステータス合計: {s.SumStats()}\n\n" +
                "画面タップで続行";
        }

        public void OnContinue()
        {
            if (GameManager.I.State.turn >= GameManager.MaxTurns)
            {
                GameManager.I.Go("Result");
                return;
            }

            TrainingLogic.NextTurn(GameManager.I);
            GameManager.I.Go("Training");
        }
    }
}
