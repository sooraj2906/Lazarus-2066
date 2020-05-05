using UnityEngine;

public class ItemPickup : Interactable
{

    public Item item;
    

    public override void Interact()
    {
        base.Interact();

        PickUp();
    }


    void PickUp()
    {
        Debug.Log("Picking up " + item.name);
        bool wasPickedUp = Inventory.instance.Add(item);    // Add to inventory
        switch (item.name)
        {
            case "Bandage":
                playerController.Bandage += 1;
                break;
            case "Med Kit":
                playerController.MedKit += 1;
                break;
            case "Medical Supply":
                playerController.MedicalSupply += 1;
                break;
            case "PainKillers":
                playerController.PainKiller += 1;
                break;
            case "Scrap Metal":
                playerController.ScrapMetal += 1;
                break;
        }
        // If successfully picked up
        if (wasPickedUp)
            Destroy(gameObject);    // Destroy item from scene
    }

}