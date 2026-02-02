using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DotDerby
{
    public class ResultUI : MonoBehaviour
    {
        public TextMeshProUGUI summaryText;
        public Button rollButton;   // 因子抽選ボタン

        void Start()
        {
            Refresh();
        }

        void Refresh()
        {
            var s = GameManager.I.State;

            int wins =
                (s.race1Win ? 1 : 0) +
                (s.race2Win ? 1 : 0) +
                (s.race3Win ? 1 : 0);

            // 評価スコア
            int score = s.SumStats() + wins * 200;

            // ランク判定
            string rank =
                score >= 900 ? "S" :
                score >= 750 ? "A" :
                score >= 600 ? "B" :
                score >= 450 ? "C" : "D";

            // ランクコメント
            string rankComment =
                rank == "S" ? "伝説級の育成！" :
                rank == "A" ? "かなり優秀な結果！" :
                rank == "B" ? "安定した育成。" :
                rank == "C" ? "もう一息。" :
                "厳しい結果…";

            string rolledMsg = s.factorsRolled
                ? "因子抽選は完了しました。次の育成へ進めます。"
                : "ボタンを押して因子抽選を行いましょう。";

            summaryText.text =
                "結果\n" +
                $"勝利数: {wins}/3\n" +
                $"ステータス合計: {s.SumStats()}\n" +
                $"評価: {score}（{rank}ランク）\n" +
                $"{rankComment}\n\n" +
                "因子（現在）\n" +
                $"スピード: {s.factorSpeed}\n" +
                $"スタミナ: {s.factorStamina}\n" +
                $"パワー: {s.factorPower}\n" +
                $"根性: {s.factorGuts}\n" +
                $"賢さ: {s.factorWisdom}\n\n" +
                rolledMsg;

            // ★ 抽選済みなら因子抽選ボタンを消す
            if (rollButton != null)
            {
                rollButton.gameObject.SetActive(!s.factorsRolled);
            }
        }

        int RollFactorValue(int score)
        {
            int r = Random.Range(0, 100);

            if (score >= 900)
            {
                if (r < 25) return 3;
                if (r < 70) return 2;
                return 1;
            }
            if (score >= 750)
            {
                if (r < 15) return 3;
                if (r < 60) return 2;
                return 1;
            }

            if (r < 5) return 3;
            if (r < 40) return 2;
            return 1;
        }

        public void OnRollFactors()
        {
            var gm = GameManager.I;
            var s = gm.State;

            // 念のためガード
            if (s.factorsRolled)
                return;

            int wins =
                (s.race1Win ? 1 : 0) +
                (s.race2Win ? 1 : 0) +
                (s.race3Win ? 1 : 0);

            int score = s.SumStats() + wins * 200;

            // 3枠抽選
            for (int i = 0; i < 3; i++)
            {
                var stat = (StatType)Random.Range(0, 5);
                int v = RollFactorValue(score);

                switch (stat)
                {
                    case StatType.Speed: s.factorSpeed += v; break;
                    case StatType.Stamina: s.factorStamina += v; break;
                    case StatType.Power: s.factorPower += v; break;
                    case StatType.Guts: s.factorGuts += v; break;
                    case StatType.Wisdom: s.factorWisdom += v; break;
                }
            }

            s.factorsRolled = true;
            gm.Save();
            Refresh();
        }

        public void OnNextRun()
        {
            GameManager.I.NewRunKeepFactors();
            GameManager.I.Go("Training");
        }

        public void OnBackToTitle()
        {
            GameManager.I.Go("Title");
        }
    }
}
