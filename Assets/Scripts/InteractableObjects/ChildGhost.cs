using System;
using UnityEngine;

namespace PointClick.Interactable
{
    public class ChildGhost : InteractableObject
    {
        [TextArea] public string message;

        public override void Interact()
        {
            GameController.Instance.DisplayText(message);
        }
    }
}
