using UnityEngine;

namespace PointClick.Inventory
{

    [CreateAssetMenu(fileName = "InteractableItem", menuName = "Inventory Item", order = 1)]
    /// Is the model data for any object that can be added to inventory
    public class InventoryItemModel : ScriptableObject
    {
        public InventoryType type = InventoryType.Item;
        public AudioClip audioClip;

        public string itemName = "Item";
        [TextArea] public string description = "Usa los caracteres /" +
            " para que el texto posterior este en otro cuadro de dialogo";

        public Sprite icon;
        public Sprite image;

        public int amount = 1;
        public int maxAmount = 1;
    }

    public enum InventoryType
    {
        Item = 1,
        ItemWithAudio = 2,
        Document = 4,
        DocumentWithAudio = 5,
    }
}
