using UnityEngine;
using UnityEngine.SceneManagement;

namespace DotDerby
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager I { get; private set; }

        public const int MaxTurns = 36;
        public static readonly int[] TargetRaceTurns = { 12, 24, 36 };

        public GameState State { get; private set; } = new GameState();

        // UIログ
        public string lastLog = "";

        const string SaveKey = "DDD_SAVE";

        void Awake()
        {
            if (I != null && I != this) { Destroy(gameObject); return; }
            I = this;
            DontDestroyOnLoad(gameObject);

            LoadOrNew();
        }

        public bool HasSaveData()
        {
            return PlayerPrefs.HasKey(SaveKey) && !string.IsNullOrEmpty(PlayerPrefs.GetString(SaveKey));
        }

        public bool IsTargetRaceTurn(int turn)
        {
            foreach (var t in TargetRaceTurns) if (t == turn) return true;
            return false;
        }

        // 12->0 / 24->1 / 36->2
        public int GetTargetRaceIndex(int turn)
        {
            for (int i = 0; i < TargetRaceTurns.Length; i++)
                if (TargetRaceTurns[i] == turn) return i;
            return -1;
        }

        public void Go(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void Save()
        {
            var json = JsonUtility.ToJson(State);
            PlayerPrefs.SetString(SaveKey, json);
            PlayerPrefs.Save();
        }

        void LoadOrNew()
        {
            if (PlayerPrefs.HasKey(SaveKey))
            {
                var json = PlayerPrefs.GetString(SaveKey);
                try
                {
                    var loaded = JsonUtility.FromJson<GameState>(json);
                    State = loaded ?? new GameState();
                }
                catch
                {
                    State = new GameState();
                }
            }
            else
            {
                State = new GameState();
            }

            State.ClampAll();
        }

        // データリセット（因子含め完全初期化）
        public void ResetAll()
        {
            // 保存データを完全削除
            PlayerPrefs.DeleteKey(SaveKey);
            PlayerPrefs.Save();

            // メモリ上も初期化
            State = new GameState();
            lastLog = "";
        }


        // 新しく育成（因子引き継ぎでターン1から）
        public void NewRunKeepFactors()
        {
            // 因子保持
            int fs = State.factorSpeed;
            int fsta = State.factorStamina;
            int fp = State.factorPower;
            int fg = State.factorGuts;
            int fw = State.factorWisdom;

            // 周回リセット（因子だけ引き継ぎ）
            State = new GameState
            {
                factorSpeed = fs,
                factorStamina = fsta,
                factorPower = fp,
                factorGuts = fg,
                factorWisdom = fw,
                factorsRolled = false
            };

            // 因子を初期ステへ反映
            State.speed += State.factorSpeed;
            State.staminaStat += State.factorStamina;
            State.power += State.factorPower;
            State.guts += State.factorGuts;
            State.wisdom += State.factorWisdom;

            lastLog = "新しい育成を開始しました。因子を反映しました。";
            Save();
        }
    }
}
