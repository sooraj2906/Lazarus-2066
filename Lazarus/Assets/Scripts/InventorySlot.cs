using UnityEngine;
using UnityEngine.UI;

/* Sits on all InventorySlots. */

public class InventorySlot : MonoBehaviour
{

    public Image icon;
    public PlayerController playerController;
    public UIManager uiManager;

    Item item;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        uiManager = FindObjectOfType<UIManager>();
    }
    // Add item to the slot
    public void AddItem(Item newItem)
    {
        item = newItem;

        icon.sprite = item.icon;
        icon.enabled = true;
    }

    // Clear the slot
    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
    }

    // Called when the remove button is pressed
    public void OnRemoveButton()
    {
        Inventory.instance.Remove(item);
    }

    // Called when the item is pressed
    public void UseItem()
    {
        if (item != null)
        {
            item.Use();

            switch (item.name)
            {
                case "Bandage":
                    playerController.currentHealth += 10;
                    uiManager.UpdateHealth(playerController.currentHealth);
                    break;
                case "Med Kit":
                    playerController.currentHealth += 50;
                    uiManager.UpdateHealth(playerController.currentHealth);
                    break;
                case "PainKillers":
                    playerController.currentHealth += 25;
                    uiManager.UpdateHealth(playerController.currentHealth);
                    break;
            }
        }
    }

}