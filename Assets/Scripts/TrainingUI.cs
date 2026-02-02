using TMPro;
using UnityEngine;

namespace DotDerby
{
    public class TrainingUI : MonoBehaviour
    {
        public TextMeshProUGUI titleText;
        public TextMeshProUGUI statsText;
        public TextMeshProUGUI logText;

        void Start()
        {
            // 完走済みデータでTrainingに来たらResultへ
            if (GameManager.I.State.turn >= GameManager.MaxTurns)
            {
                GameManager.I.Go("Result");
                return;
            }

            Refresh();
        }

        void Refresh()
        {
            var gm = GameManager.I;
            var s = gm.State;

            titleText.text = $"ドットでダービー！  育成 {s.turn}/36";

            statsText.text =
                $"体力: {s.stamina}/100\n" +
                $"やる気: {s.mood}/4\n" +
                $"\n" +
                $"スピード: {s.speed}\n" +
                $"スタミナ: {s.staminaStat}\n" +
                $"パワー: {s.power}\n" +
                $"根性: {s.guts}\n" +
                $"賢さ: {s.wisdom}\n";

            logText.text = string.IsNullOrEmpty(gm.lastLog)
                ? "行動を選択してください。"
                : gm.lastLog;
        }

        void AfterAction()
        {
            var gm = GameManager.I;
            var s = gm.State;

            if (gm.IsTargetRaceTurn(s.turn))
            {
                gm.Go("Race");
                return;
            }

            TrainingLogic.NextTurn(gm);

            if (gm.State.turn > GameManager.MaxTurns)
            {
                gm.Go("Result");
                return;
            }

            Refresh();
        }

        public void OnTrain()
        {
            TrainingLogic.DoTrain(GameManager.I);
            AfterAction();
        }

        public void OnRest()
        {
            TrainingLogic.DoRest(GameManager.I);
            AfterAction();
        }

        public void OnOuting()
        {
            TrainingLogic.DoOuting(GameManager.I);
            AfterAction();
        }

        // 中断（保存してタイトルへ）
        public void OnSuspend()
        {
            var gm = GameManager.I;
            gm.Save();
            gm.lastLog = "育成を中断しました。";
            gm.Go("Title");
        }
    }
}
