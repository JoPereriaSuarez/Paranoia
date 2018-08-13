using UnityEngine;

namespace PointClick.Interactable
{
    public class BriefcaseItem : MultipleInteractableObject
    {
        [SerializeField] BriefcaseController controller;

        protected override void Start()
        {
            base.Start();
            controller.OnConfirm += Controller_OnConfirm;
        }

        private void Controller_OnConfirm(object sender, bool e)
        {
            if (e)
            {
                IsOpen = e;
            }
        }

        public override void Interact()
        {
            if (!IsOpen)
            {
                controller.DisplayPuzzle();
            }
        }
    }
}
