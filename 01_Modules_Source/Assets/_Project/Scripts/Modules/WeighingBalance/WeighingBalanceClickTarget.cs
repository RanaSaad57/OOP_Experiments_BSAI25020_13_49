using UnityEngine;

namespace OOPLab.Modules.WeighingBalance
{
    public class WeighingBalanceClickTarget : MonoBehaviour
    {
        [SerializeField] private WeighingBalanceController controller;

        public void SetController(WeighingBalanceController weighingBalanceController)
        {
            controller = weighingBalanceController;
        }

        private void OnMouseDown()
        {
            if (controller != null)
            {
                controller.ShowPanel();
            }
        }
    }
}
