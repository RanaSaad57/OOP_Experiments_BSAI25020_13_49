using UnityEngine;
using UnityEngine.EventSystems;

namespace OOPLab.Modules.FreeFallSetup
{
    public class FreeFallClickTarget : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private FreeFallController controller;

        public void SetController(FreeFallController target)
        {
            controller = target;
        }

        private void OnMouseDown()
        {
            controller?.ShowMenu();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            controller?.ShowMenu();
        }
    }
}
