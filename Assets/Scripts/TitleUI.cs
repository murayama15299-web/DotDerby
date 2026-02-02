using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DotDerby
{
    public class TitleUI : MonoBehaviour
    {
        public Button continueButton;        // 「続きから」ボタン
        public TextMeshProUGUI statusText;   // 任意：状態表示（NoneでOK）

        void Start()
        {
            Refresh();
        }

        void Refresh()
        {
            var gm = GameManager.I;
            bool hasSave = gm.HasSaveData();

            if (continueButton != null)
                continueButton.interactable = hasSave;

            if (statusText != null)
                statusText.text = hasSave ? "保存データがあります。" : "保存データがありません。";
        }

        // 続きから（保存データの続きで再開）
        public void OnContinue()
        {
            var gm = GameManager.I;
            var s = gm.State;

            // ★完走済みなら結果画面に戻す（Trainingの36ターン目で止まらない）
            if (s.turn >= GameManager.MaxTurns)
            {
                gm.Go("Result");
                return;
            }

            gm.Go("Training");
        }

        // 新しく育成（因子引き継ぎでターン1から）
        public void OnNewRun()
        {
            GameManager.I.NewRunKeepFactors();
            GameManager.I.Go("Training");
        }

        // データリセット（因子含め完全初期化）
        public void OnResetAll()
        {
            GameManager.I.ResetAll();
            Refresh();
        }

        public void OnQuit()
        {
            Application.Quit();
        }
    }
}
