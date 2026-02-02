using UnityEngine;

namespace DotDerby
{
    public class SceneBootstrap : MonoBehaviour
    {
        void Awake()
        {
            if (GameManager.I == null)
            {
                var go = new GameObject("GameManager");
                go.AddComponent<GameManager>();
            }
        }
    }
}
