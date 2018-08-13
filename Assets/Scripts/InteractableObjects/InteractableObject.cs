using System;
using PointClick.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PointClick.Interactable
{

    [RequireComponent(typeof(Collider2D))]
    public class InteractableObject : MonoBehaviour
    {
        public event EventHandler<InteractEventArgs> OnInteract;
        public event EventHandler OnClick;

        [SerializeField] private string objectName = "InteractableObject";
        protected string ObjectName { get { return objectName; } }

        [SerializeField] private InteractionType interactionType = InteractionType.InventoryItem;
        protected InteractionType InteractionType { get { return interactionType; } }

        [SerializeField] private InventoryItemModel[] items;
        protected InventoryItemModel[] Items{ get { return items; } }

        [SerializeField] private string nextDoor = "door.next";
        protected string NextDoor { get { return nextDoor; }  set { nextDoor = value; } }

        //[SerializeField] protected AudioClip audioClip;
        //public AudioClip Audio
        //{
        //    get { return audioClip; }
        //}

        [SerializeField] private bool interactOnce = false;

        protected Collider2D coll;

        private bool hasClick;
        private bool hasInteract;

        public void CancelClick()
        {
            if(!hasClick) { return; }
            hasClick = false;
        }

        void Awake()
        {
            coll = GetComponent<Collider2D>();
        }

        protected virtual void Start()
        {
            if(GameController.Instance == null) { return; }

            GameController.Instance.AddInteractableObjectListener(this);
        }

        private void OnDestroy()
        {
            if (GameController.Instance == null) { return; }

            GameController.Instance.RemoveInteractableObjectListener(this);
        }

        /// <summary>
        /// Interact with this object. An event is raised
        /// </summary>
        public virtual void Interact()
        {
            if(hasInteract && interactOnce) { return; }

            hasInteract = true;

            /// A listener must decide what happens with every object
            InteractEventArgs interactArgs = (items.Length > 0) ?
                new InteractEventArgs(InteractionType, nextDoor, items[0].audioClip, Items) :
                new InteractEventArgs(InteractionType, nextDoor, items);
            OnInteract?.Invoke(this, interactArgs);
        }


        /// <summary>
        /// Enable or Disable collider
        /// </summary>
        /// <param name="value"></param>
        public void ActiveObject(bool value)
        {
            coll.enabled = false;
        }

        /// <summary>
        /// Delete Requirement for this object.
        /// </summary>
        public void RemoveItem()
        {
            items = null;
        }

        ///// <summary>
        ///// Calls Interact when the object is click
        ///// </summary>
        private void OnMouseDown()
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (hasClick)
                {
                    Interact();
                }
                else 
                {
                    hasClick = true;
                    OnClick?.Invoke(this, EventArgs.Empty);
                }
            }
        }

    }

    public enum InteractionType
    {
        InventoryItem,
        GoTo,
        Puzzle,
        Victory, 
        GameOver,
        DoNothing,
    }

    public class InteractEventArgs : EventArgs
    {
        public readonly InteractionType interactionType;
        public readonly string nextDoor;
        public readonly InventoryItemModel[] inventoryItem;
        public Sprite Image { get; set; }
        public AudioClip Audio { get; set; }

        public InteractEventArgs(InteractionType senderId, string nextDoor, params InventoryItemModel[] inventoryItem)
        {
            this.interactionType = senderId;
            this.nextDoor = nextDoor;
            this.inventoryItem = inventoryItem;
        }
        public InteractEventArgs(InteractionType senderId, string nextDoor, AudioClip audio, params InventoryItemModel[] inventoryItem)
        {
            this.interactionType = senderId;
            this.nextDoor = nextDoor;
            this.inventoryItem = inventoryItem;
            Audio = audio;
        }
    }

}
