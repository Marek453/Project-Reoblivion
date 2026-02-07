using GameCore.Player.Inventory;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.UI.Inventory
{
    public class InventoryIU : MonoBehaviour
    {
        public UserMainInterface userMainInterface;
        public GameObject inventory;
        public Image cycle;
        public List<Cell> cells;
        public int currentCell = -1;

        public void SetupInventoryUI(Color color)
        {
            if (userMainInterface.lockalPlayer == null) return;
            userMainInterface.lockalPlayer.cursorManager.isInventory = false;
            inventory.SetActive(userMainInterface.lockalPlayer.cursorManager.isInventory);
            cycle.color = color;
            foreach (Cell cell in cells)
            {
                cell.Setup(color);
            }
        }

        public void Drop(int id)
        {
            if (!cells[id].itemCell.enabled) return;
            cells[id].isSelected = false;
            ColorBlock cvv = cells[id].button.colors;
            cvv.normalColor = new Color(cvv.normalColor.r, cvv.normalColor.g, cvv.normalColor.b, 0);
            cvv.selectedColor = new Color(cvv.normalColor.r, cvv.normalColor.g, cvv.normalColor.b, 0);
            cells[id].button.colors = cvv;
            userMainInterface.lockalPlayer.inventory.DropItem(id);
        }

        public void UpdateInventory(Item[] items)
        {
            foreach (var cell in cells)
            {
                cell.itemCell.enabled = false;
            }
            for (int i = 0; i < items.Length; i++)
            {
                cells[i].itemCell.enabled = true;
                cells[i].itemCell.sprite = userMainInterface.lockalPlayer.inventory.AnableItems.ToList().Find(ai => ai.idSprite == items[i].idSprite).sprite;
            }
        }

        public void SelectCell(int id)
        {
            if (cells[id].isSelected) return;
            foreach(Cell cell in cells)
            {
                cell.isSelected = false;
                ColorBlock cvv = cell.button.colors;
                cvv.normalColor = new Color(cvv.normalColor.r, cvv.normalColor.g, cvv.normalColor.b, 0);
                cvv.selectedColor = new Color(cvv.normalColor.r, cvv.normalColor.g, cvv.normalColor.b, 0);
                cell.button.colors = cvv;
            }
            if (cells[id].itemCell.enabled)
            {
                cells[id].isSelected = true;
                ColorBlock cb = cells[id].button.colors;
                cb.normalColor = new Color(cb.normalColor.r, cb.normalColor.g, cb.normalColor.b, 1);
                cb.selectedColor = new Color(cb.normalColor.r, cb.normalColor.g, cb.normalColor.b, 1);
                cells[id].button.colors = cb;
            }
            userMainInterface.lockalPlayer.inventory.SetItem(id);
            OnInventory();
        }

        public void OnInventory()
        {
            if (userMainInterface.lockalPlayer == null) return;
            if(userMainInterface.lockalPlayer.classManager.curRoleTypeId == Player.Class.RoleType.None || userMainInterface.lockalPlayer.classManager.curRoleTypeId == Player.Class.RoleType.Spectator)
            {
                return;
            }
            userMainInterface.lockalPlayer.cursorManager.isInventory = !userMainInterface.lockalPlayer.cursorManager.isInventory;
            inventory.SetActive(userMainInterface.lockalPlayer.cursorManager.isInventory);
        }
    }
}
