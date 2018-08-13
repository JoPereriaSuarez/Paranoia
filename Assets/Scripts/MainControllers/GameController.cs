using PointClick.Interactable;
using PointClick.Inventory;
using PointClick.UI;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PointClick
{

    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }

        public event EventHandler<bool> OnPaused;
        public event EventHandler<RoomChangedEventArgs> OnRoomChanged;

        [SerializeField] private GameUI ui;
        public GameUI MainUI { get { return ui; } }
        [SerializeField] private InventoryController normalInventory;
        public InventoryController NormalInventory { get { return normalInventory; } }

        [SerializeField] private InventoryController documentInventory;
        public InventoryController DocumentInventory { get { return documentInventory; } }

        Camera cam;
        public PaddlePuzleController currentPuzzle;

        [SerializeField] private int lifes = 3;
        public int Lifes
        {
            get { return lifes; }
            set
            {
                value = Mathf.Clamp(value, 0, 4);
                lifes = value;
                if (lifes <= 0)
                { GameOver(); }
            }
        }

        [Header("Rooms List")]
        [SerializeField] private Transform[] roomList;

        [Header("Scenes")]
        [SerializeField] private string GameOverScene = "GameOverScene";
        [SerializeField] private string MainMenuScene = "MainMenuScene";
        [SerializeField] private string GameScene = "GameScene";
        [SerializeField] private string VictoryScene = "VictoryScene";

        private void Awake()
        {
            Instance = this;
            if (normalInventory == null)
            {
                normalInventory = FindObjectOfType<InventoryController>();
            }
        }

        private void Start()
        {
            cam = Camera.main;
        }

        public void GameOver()
        {
            SceneManager.LoadScene(GameOverScene);
        }
        public void Victory()
        {
            SceneManager.LoadScene(VictoryScene);
        }

        public void AddInteractableObjectListener(InteractableObject item)
        {
            item.OnInteract += Item_OnInteract;
        }
        public void RemoveInteractableObjectListener(InteractableObject item)
        {
            item.OnInteract -= Item_OnInteract;
        }

        private void Item_OnInteract(object sender, InteractEventArgs e)
        {
            switch(e.interactionType)
            {
                case InteractionType.DoNothing:
                    break;
                case InteractionType.GoTo:
                    /// Check if object has a requirement to intereact with
                    if(e.inventoryItem != null)
                    {
                        /// Check if the item selected by the player is equal to the requierement
                        if(e.inventoryItem.Length == 0 || e.inventoryItem[0] == normalInventory.UseItem)
                        {
                            //inventory.RemoveFromInventory(inventory.UseItem);
                            Transform room;
                            if (TryFindRoom(e.nextDoor, out room))
                            {
                                ChangeRoom(room);
                            }
                            else
                            {
                                Debug.LogError($"There's no room index as {e.nextDoor}, please check name or add it on GameController");
                                return;
                            }
                        }
                    }
                    /// Go to different Room if the object has no requirements
                    else
                    {
                        Transform room;
                        
                        if(TryFindRoom(e.nextDoor, out room))
                        {
                            Debug.Log("CHANGE SCENE TO " + e.nextDoor);
                            ChangeRoom(room);
                        }
                        else
                        {
                            Debug.LogError($"There's no room index as {e.nextDoor}, please check name or add it on GameController");
                            return;
                        }
                    }
                    normalInventory.UseItem = null;
                    break;

                case InteractionType.InventoryItem:

                    ///HAZLO MEJOR PARA LA OTRA
                    for(int i = 0; i < e.inventoryItem.Length; i++)
                    {
                        switch (e.inventoryItem[i].type)
                        {
                            case InventoryType.Item:
                            if (normalInventory.AddToInventory(e.inventoryItem[i]))
                            {
                                InteractableObject interactable = (InteractableObject)sender;
                                //Debug.Log($"THE ITEM {e.inventoryItem} has been added to you inventory");
                                Destroy(interactable.gameObject);
                            }
                            break;
                            case InventoryType.ItemWithAudio:
                            if(normalInventory.AddToInventory(e.inventoryItem[i]))
                            {
                                InteractableObject interactable = (InteractableObject)sender;
                                SoundManager.Instance.VoiceController.Play(e.Audio);
                                //Debug.Log($"THE ITEM {e.inventoryItem} has been added to you inventory");
                                Destroy(interactable.gameObject);
                            }
                            break;

                            case InventoryType.Document:
                            if (documentInventory.AddToInventory(e.inventoryItem[i]))
                            {
                                InteractableObject interactable = (InteractableObject)sender;
                                //Debug.Log($"THE ITEM {e.inventoryItem} has been added to you inventory");
                                Destroy(interactable.gameObject);
                            }
                            break;

                            case InventoryType.DocumentWithAudio:
                            if (documentInventory.AddToInventory(e.inventoryItem[i]))
                            {
                                InteractableObject interactable = (InteractableObject)sender;
                                SoundManager.Instance.VoiceController.Play(e.Audio);
                                //Debug.Log($"THE ITEM {e.inventoryItem} has been added to you inventory");
                                Destroy(interactable.gameObject);
                            }
                            break;
                        }
                    }

                    break;

                case InteractionType.Puzzle:
                    /// We have only 1 puzzle so, no checking values here.
                    PaddlePuzleController puzzle = (PaddlePuzleController)sender;
                    if(puzzle == null) { return; }

                    currentPuzzle = puzzle;
                    // Check if the inventory contain the puzzle requirements
                    // if so, delete requirements from puzzle and objects from inventory
                    if(currentPuzzle.requirements != null && currentPuzzle.requirements.Length > 0)
                    {
                        InventoryItemModel[] requirements = new InventoryItemModel[puzzle.requirements.Length];
                        for(int i = 0; i < puzzle.requirements.Length; i++)
                        {
                            requirements[i] = normalInventory.FindByItemName(puzzle.requirements[i].itemName);
                            if(requirements[i] == null)
                            {
                                DisplayText("No Cumples con los Requisitos");
                                return;
                            }
                        }
                        
                        puzzle.requirements = null;
                        //for(int i = 0; i < requirements.Length; i++)
                        //{
                        //    inventory.RemoveFromInventory(requirements[i]);
                        //}
                        requirements = null;
                    }

                    /// Open the puzzle
                    currentPuzzle.GetComponent<Collider2D>().enabled = false;
                    currentPuzzle.OnPuzzleConfirm += Puzzle_OnPuzzleConfirm;
                    currentPuzzle.OnPuzzleClose += Puzzle_OnPuzzleClose;
                    currentPuzzle.Display();
                    break;
                case InteractionType.Victory:
                    Victory();
                    break;
            }
        }

        public bool TryFindRoom(string name, out Transform room)
        {
            try
            {
                room = roomList.Where(_room => _room.name.ToLower() == name.ToLower()).First();
            }
            catch(InvalidOperationException)
            {
                room = null;
            }
            return (room != null);
        }

        string currentRoom = "room01";
        private void ChangeRoom(Transform room)
        {
            /// Add effects and delay time later
            cam.transform.position = new Vector3(room.position.x, room.position.y, cam.transform.position.z);
            cam.transform.rotation = room.rotation;
            string previousRoom = currentRoom;
            currentRoom = room.name.ToLower() ;
            OnRoomChanged?.Invoke(this, new RoomChangedEventArgs(currentRoom, previousRoom));
        }

        private void Puzzle_OnPuzzleClose(object sender, EventArgs e)
        {
            currentPuzzle.OnPuzzleConfirm -= Puzzle_OnPuzzleConfirm;
            currentPuzzle.OnPuzzleClose -= Puzzle_OnPuzzleClose;
        }

        private void Puzzle_OnPuzzleConfirm(object sender, PuzzleConfirmEventArgs e)
        {
            if(e.acoplished && e.onInteract != null)
            {
                Debug.Log("DO SOMETHING");
//                if(e.onInteract.interactionType == InteractionType.Victory)
//                {
//                    Victory();
//                }
//                else
//                { 
//}
                Item_OnInteract(sender, e.onInteract);
            }
            else if(!e.acoplished)
            {
                Lifes--;
                if(ui != null)
                {
                    ui.RemoveLife();
                }
            }
        }

        public void DisplayText(string text)
        {
            string[] resultedTexts = text.Split('/');

            /// Display text on UI
            if(ui != null)
            {
                ui.OpenPopupMessage(resultedTexts);
            }
            /// Display Text on Console
            else
            {
                Debug.Log(text);
            }
        }
        public void DisplayText(string text, Sprite image)
        {
            DisplayText(text);
            if(image != null)
            {
                ui.DisplayImage(image);
            }
        }

        public void PauseGame()
        {
            Time.timeScale = 0.0f;
            OnPaused?.Invoke(this, true);
        }
        public void UnPauseGame()
        {
            Time.timeScale = 1.0f;
            OnPaused?.Invoke(this, false);
        }
    }

    public class RoomChangedEventArgs : EventArgs
    {
        public readonly string roomName;
        public readonly string previousRoom;

        public RoomChangedEventArgs(string roomName, string previousRoom)
        {
            this.roomName = roomName;
            this.previousRoom = previousRoom;
        }
    }

}
