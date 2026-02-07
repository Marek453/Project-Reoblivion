using GameCore.UI;
using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameCore.Player.Inventory
{
    public class Inventory : NetworkBehaviour
    {
        public Item[] AnableItems;
        public SyncList<Item> ItemsInInventory = new SyncList<Item>();

        [SyncVar]
        public int currentItem;


        public void AddItem(int ID, float duraction)
        {
            if (isLocalPlayer)
            {
                CmdAddItem(ID, duraction);
            }
        }

        public void DropItem(int id)
        {
            if (isLocalPlayer)
            {
                ItemsInInventory[id].firstPersonModel.SetActive(false);
                CmdDropItem(id);
            }
        }

        private void Start()
        {
            ItemsInInventory.Callback += OnInventoryUpdated;
        }

        [Command]
        private void CmdDropItem(int ID)
        {
            Item item = ItemsInInventory[ID];
            GameObject objectItem = Instantiate(item.objectItem);
            objectItem.transform.position = base.transform.position;
            ObjectItem objectItm = objectItem.GetComponent<ObjectItem>();
            objectItm.id = AnableItems.ToList().FindIndex(itm => itm.nameItem == ItemsInInventory[ID].nameItem);
            objectItm.duraction = ItemsInInventory[ID].duraction;
            NetworkServer.Spawn(objectItem);
            ItemsInInventory.Remove(item);
        }

        [Command]
        private void CmdAddItem(int ID, float duraction)
        {
            Item item = new Item(AnableItems[ID]);
            item.duraction = duraction;
            item.sprite = null;
            ItemsInInventory.Add(item);
        }


        private void OnInventoryUpdated(SyncList<Item>.Operation op, int index, Item oldItem, Item newItem)
        {
            if (isLocalPlayer)
            {
                UserMainInterface.singlenton.inventory.UpdateInventory(ItemsInInventory.ToArray());
            }
        }

        public void SetItem(int id)
        {
            foreach(var item in ItemsInInventory)
            {
                item.firstPersonModel.SetActive(false);
            }
            if(id < ItemsInInventory.Count)
            {
                currentItem = id;
                ItemsInInventory[id].firstPersonModel.SetActive(true);
            }
        }
    }
}
