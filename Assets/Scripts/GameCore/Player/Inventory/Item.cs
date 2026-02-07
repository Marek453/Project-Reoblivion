using GameCore.Facility;
using System;
using UnityEngine;

namespace GameCore.Player.Inventory
{
    [Serializable]
    public class Item 
    {
        public string nameItem;
        public float duraction;
        public AccessType accessType;
        public ItemType itemType;
        public string idSprite;
        public Sprite sprite;
        public GameObject firstPersonModel;
        public GameObject objectItem;

        public Item()
        {
            
        }

        public Item(Item item)
        {
            nameItem = item.nameItem;
            duraction = item.duraction;
            accessType = item.accessType;
            itemType = item.itemType;
            idSprite = item.idSprite;
            sprite = item.sprite;
            firstPersonModel = item.firstPersonModel;
            objectItem = item.objectItem;
        }
    }
}