using System;
using UnityEngine;
using UnityEngine.UI;

namespace PointClick.Inventory
{

    /// <summary>
    /// Contain the UI element of the inventory.
    /// The values are setting on Unity Inspector
    /// </summary>
    public class InventoryView : MonoBehaviour
    {
        const string SHOW_ANIM_TAG = "show";

        /// <summary>
        /// Event when an Item option is click
        /// </summary>
        public event EventHandler<OptionClickEventArgs> OnOptionClicked;

        [SerializeField] MenuNavigation orientation = MenuNavigation.Horizontal;
        [SerializeField] int length = 5;
        /// <summary>
        /// Amount of object in inventory
        /// </summary>
        public int Length { get { return length; } }

        /// <summary>
        /// Default icon when the slot is empty
        /// </summary>
        [SerializeField] Sprite emptyItemIcon;

        /// <summary>
        /// Slots for inventory
        /// </summary>
        [SerializeField] Button[] inventoryButtons;

        /// <summary>
        /// Buttons that represents the options when an object is selected
        /// </summary>
        [SerializeField] Button useButton, watchButton;

        RectTransform watchButtonTransform, useButtonTransform;
        RectTransform[] inventoryButtonsTransform;

        int clickedItemIndex = 0;

        [SerializeField] Animator anim;
        //[SerializeField] Button displayButton;

        private void Start()
        {
            RemoveAllListener();
            inventoryButtonsTransform = new RectTransform[inventoryButtons.Length];
            for (int i = 0; i < inventoryButtons.Length; i++)
            {
                inventoryButtonsTransform[i] = inventoryButtons[i].GetComponent<RectTransform>();
            }
            watchButtonTransform = watchButton.GetComponent<RectTransform>();
            useButtonTransform = useButton.GetComponent<RectTransform>();
        }

        /// <summary>
        /// Creates option buttons when item is clicked.
        /// </summary>
        /// <param name="index"></param>
        public void OnIconClick(int index)
        {
            if(orientation == MenuNavigation.Horizontal)
            {
                watchButtonTransform.position = new Vector2(inventoryButtonsTransform[index].position.x, watchButtonTransform.position.y);
                useButtonTransform.position = new Vector2(inventoryButtonsTransform[index].position.x, useButtonTransform.position.y);
            }
            else
            {
                watchButtonTransform.position = new Vector2(watchButtonTransform.position.x, inventoryButtonsTransform[index].position.y);
                useButtonTransform.position = new Vector2(useButtonTransform.position.x, inventoryButtonsTransform[index].position.y-0.5f);
            }

            RemoveAllListener();
            ActiveOptionButtons(true);

            useButton.onClick.AddListener(() 
                => OnOptionClicked?.Invoke(this, new OptionClickEventArgs
                {
                    ItemIndex = index, Instruction = InventoryController.USE_INSTRUCTION
                }));
            watchButton.onClick.AddListener(() 
                => OnOptionClicked?.Invoke(this, new OptionClickEventArgs { ItemIndex = index, Instruction = InventoryController.WATCH_INSTRUCTION } ));
        }

        /// <summary>
        /// Remove all listener from Option UI Button.
        /// The Options still visibles
        /// </summary>
        public void RemoveAllListener()
        {
            useButton.onClick.RemoveAllListeners();
            watchButton.onClick.RemoveAllListeners();   
        }

        /// <summary>
        /// Disable or enable UI option for items.
        /// The listeners still there.
        /// </summary>
        /// <param name="value"></param>
        public void ActiveOptionButtons(bool value)
        {
            watchButton.gameObject.SetActive(value);
            useButton.gameObject.SetActive(value);
        }

        /// <summary>
        /// Add an sprite to inventory buttons.
        /// Returns a integer containing the index of the item added.
        /// </summary>
        /// <param name="sprite"></param>
        /// <returns></returns>
        public int AddItem(Sprite sprite)
        {
            int index = -1;
            for(int i= 0; i< inventoryButtons.Length; i++)
            {
                if(inventoryButtons[i].image.sprite == emptyItemIcon || inventoryButtons[i].image.sprite == null)
                {
                    inventoryButtons[i].image.sprite = sprite;
                   // inventoryButtons[i].image.SetNativeSize();
                    index = i;
                    break;
                }
            }
            return index;
        }

        /// <summary>
        /// Remove an non-default sprite at a specific slot
        /// </summary>
        /// <param name="index"></param>
        public void RemoveItem(int index)
        {
            if(index < 0 || index >= inventoryButtons.Length) { return; }

            inventoryButtons[index].image.sprite = emptyItemIcon;
            //inventoryButtons[index].image.SetNativeSize();
        }

        bool isDsplayed = false;
        public void SwitchDisplay()
        {
            if (isDsplayed) { Hide(); }
            else
            {
                Display();
            }
        }

        public void Display()
        {
            if(anim!=null)
            {
                RemoveAllListener();
                ActiveOptionButtons(false);
                isDsplayed = true;
                anim.SetBool(SHOW_ANIM_TAG, true);
            }
        }
        public void Hide()
        {
            if (anim != null)
            {
                RemoveAllListener();
                ActiveOptionButtons(false);
                isDsplayed = false;
                anim.SetBool(SHOW_ANIM_TAG, false);
            }
        }
    }

    public class OptionClickEventArgs : EventArgs
    {
        public int ItemIndex { get; set; }
        public string Instruction { get; set; }
    }

    public enum MenuNavigation
    {
        Vertical, 
        Horizontal,
    }

}
