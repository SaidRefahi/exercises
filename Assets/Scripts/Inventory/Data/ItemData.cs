using UnityEngine;

namespace Inventory.Data
{
    [CreateAssetMenu(fileName = "NewItemData", menuName = "Game Data/Inventory/Item Data")]
    public class ItemData : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private Sprite _icon;
        [SerializeField] private int _maxStackSize = 64;

        public string Id => _id;
        public Sprite Icon => _icon;
        public int MaxStackSize => _maxStackSize;
    }
}
