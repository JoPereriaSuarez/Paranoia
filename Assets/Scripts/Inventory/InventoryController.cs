using System.Collections.Generic;
using UnityEngine;

namespace PointClick.Inventory
{

    public class InventoryController : MonoBehaviour
    {
        public const string WATCH_INSTRUCTION = "watch";
        public const string USE_INSTRUCTION = "use";

        [SerializeField] InventoryType inventoryType = InventoryType.Item;
        public InventoryType InventoryType { get; }

        [SerializeField] private InventoryItemModel[] defaultInventory;
        [SerializeField] private InventoryView view;
        
        private Dictionary<int, InventoryItemModel> inventory;        
        public InventoryItemModel UseItem { get; set; }

        private void Start()
        {
            GameController.Instance.MainUI.OnMessagePopUp += MainUI_OnMessagePopUp;
            GameController.Instance.OnRoomChanged += Instance_OnRoomChanged;
            inventory = new Dictionary<int, InventoryItemModel>();
            view.OnOptionClicked += View_OnItemClicked;
            for(int i = 0; i < defaultInventory.Length; i++)
            {
                AddToInventory(defaultInventory[i]);
            }
        }

        private void Instance_OnRoomChanged(object sender, RoomChangedEventArgs e)
        {
            view.Hide();
        }

        private void MainUI_OnMessagePopUp(object sender, UI.MessagePopUpEventArgs e)
        {
            view.Hide();
        }

        private void View_OnItemClicked(object sender, OptionClickEventArgs e)
        {
            try
            {
                switch(e.Instruction.ToLower())
                {
                    case USE_INSTRUCTION:
                        UseItem = inventory[e.ItemIndex];
                        break;
                    case WATCH_INSTRUCTION:
                        GameController.Instance.DisplayText(inventory[e.ItemIndex].description, inventory[e.ItemIndex].image);
                        if(inventory[e.ItemIndex].type == InventoryType.DocumentWithAudio ||
                            inventory[e.ItemIndex].type == InventoryType.ItemWithAudio)
                        {
                            SoundManager.Instance.VoiceController.Play(inventory[e.ItemIndex].audioClip);
                        }
                        break;
                    default:
                        Debug.LogError($"{e.Instruction} is not a valid instruction to InventoryController class");
                        break;
                }
                view.RemoveAllListener();
                view.ActiveOptionButtons(false);
            }
            catch(KeyNotFoundException)
            {
                Debug.Log("ITEM IS NULL");
                view.RemoveAllListener();
                view.ActiveOptionButtons(false);
                return;
            }
        }

        public bool AddToInventory(InventoryItemModel item, bool showNow = false)
        {
            /// -1 slot is empty
            int index = -1;
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i] == item)
                {
                    index = i;
                    break;
                }
            }

            if(index != -1)
            {
                if(inventory[index].amount + item.amount < item.maxAmount)
                {
                    inventory[index].amount += item.amount;
                }
            }

            else
            {
                int key = view.AddItem(item.icon);
                if(key == -1) { return false; }
                
                inventory.Add(key, item);
            }

            view.Display();
            return true;
        }

        public void RemoveFromInventory(InventoryItemModel item)
        {
            int index = -1;
            for (int i = 0; i < inventory.Count; i++)
            {
                if(inventory[i] == item)
                {
                    index = i;
                    break;
                }
            }

            if(index != -1)
            {
                if(inventory[index].amount -item.amount > 0)
                {
                    inventory[index].amount -= item.amount;
                }
                else
                {
                    view.RemoveItem(index);
                    inventory.Remove(index);
                }
            }

        }

        public InventoryItemModel FindByItemName(string name)
        {
            if (name == default(string))
            {
                return null;
            }

            InventoryItemModel item = null;
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i].itemName == name)
                {
                    item = inventory[i];
                    break;
                }
            }

            return item;
        }

        public void Delete()
        {
            inventory.Clear();
        }

    }

}
