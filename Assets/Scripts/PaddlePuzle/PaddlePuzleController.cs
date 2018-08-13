using System;
using UnityEngine;
using UnityEngine.UI;

namespace PointClick.Interactable
{
    public class PaddlePuzleController : InteractableObject
    {
        public event EventHandler<PuzzleConfirmEventArgs> OnPuzzleConfirm;
        public event EventHandler OnPuzzleClose;

        const string finalPuzzle = "PuzzleRoom04";
        const string itemPuzzle = "PuzzleDetail2";

        public Inventory.InventoryItemModel[] requirements;
        [SerializeField] private Paddle[] paddles;
        [SerializeField] private GameObject puzzleContainer;
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private Sprite paddleOn;
        [SerializeField] private Sprite paddleOff;

        protected sealed override void Start()
        {
            base.Start();
            for (int i = 0; i < paddles.Length; i++)
            {
                paddles[i].OnSprite = paddleOn;
                paddles[i].OffSprite = paddleOff;
            }
        }

        public void ConfirmPuzzle()
        {
            for (int i = 0; i < paddles.Length; i++)
            {
                if(!paddles[i].State)
                {
                    Debug.Log("Puzzle failed");
                    OnPuzzleConfirm?.Invoke(this, new PuzzleConfirmEventArgs(false, null));
                    return;
                }
            }
            Debug.Log("PUZZLE CORRECT");

            InteractionType i_type = (ObjectName == finalPuzzle) ? InteractionType.Victory : InteractionType.InventoryItem;
            OnPuzzleConfirm?.Invoke(this, new PuzzleConfirmEventArgs(true, new InteractEventArgs(i_type, NextDoor,  Items),-1) );
            coll.enabled = false;
            Hide();
        }

        public void Display()
        {
            puzzleContainer.SetActive(true);
            confirmButton.gameObject.SetActive(true);
            confirmButton.onClick.AddListener(() => ConfirmPuzzle());

            closeButton.gameObject.SetActive(true);
            closeButton.onClick.AddListener(() => Hide());

            coll.enabled = false;
        }
        public void Hide()
        {
            puzzleContainer.SetActive(false);
            
            confirmButton.gameObject.SetActive(false);
            confirmButton.onClick.RemoveAllListeners();

            closeButton.gameObject.SetActive(false);
            closeButton.onClick.RemoveAllListeners();

            coll.enabled = true;

            OnPuzzleClose?.Invoke(this, EventArgs.Empty);
        }
    }

    public class PuzzleConfirmEventArgs : EventArgs
    {
        public readonly bool acoplished;
        public readonly InteractEventArgs onInteract;
        public readonly int maxMovements;

        public PuzzleConfirmEventArgs(bool acoplished, InteractEventArgs onInteract, int maxMovements = -1)
        {
            this.acoplished = acoplished;
            this.onInteract = onInteract;
            this.maxMovements = maxMovements;
        }
    }
}
