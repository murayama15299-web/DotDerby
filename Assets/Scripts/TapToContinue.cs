using UnityEngine;
using UnityEngine.EventSystems;

namespace DotDerby
{
    public class TapToContinue : MonoBehaviour, IPointerClickHandler
    {
        public RaceUI raceUI;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (raceUI != null) raceUI.OnContinue();
        }
    }
}
