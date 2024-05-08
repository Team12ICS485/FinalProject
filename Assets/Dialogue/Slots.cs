using UnityEngine;
using UnityEngine.UI;

public class Slots : MonoBehaviour
{
    public Item item;         // Current item in the slot
    public int index;         // Index of the slot in the array
    public Image slotImage;   // UI component to display the item's icon
    public CraftingManager craftingManager;  // Reference to the CraftingManager

    private void Start()
    {
        if (item != null)
        {
            UpdateUI();
        }
    }

    public void SetItem(Item newItem)
    {
        item = newItem;
        UpdateUI();
        if (craftingManager != null)
        {
            craftingManager.CheckForCreatedRecipes();  // Call to check recipes whenever an item is set
        }
        else
        {
            Debug.LogError("CraftingManager reference not set on " + gameObject.name);
        }
    }

    private void UpdateUI()
    {
        if (item != null)
        {
            slotImage.sprite = item.itemIcon;
        }
        else
        {
            slotImage.sprite = null;
        }
    }
}
