using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingManager : MonoBehaviour
{
    public Image customCursor;           // Custom cursor to show selected item
    public Slots[] craftingSlots;        // Array of slots for crafting items
    public List<Item> itemList;          // List of items currently in slots
    public string[] recipes;             // Array of recipes in string format
    public Item[] recipeResults;         // Array of resulting items from recipes
    public Slots resultSlots;            // Slot for displaying the result of crafting

    private Item currentItem;            // Currently selected item

    void Awake()
    {
        // Initialize itemList based on the number of crafting slots
        itemList = new List<Item>(new Item[craftingSlots.Length]);
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0) && currentItem != null)
        {
            DropCurrentItem();
        }
    }

    public void OnMouseDownItem(Item item)
    {
        currentItem = item;
        customCursor.gameObject.SetActive(true);
        customCursor.sprite = currentItem.itemIcon;  // Set the custom cursor icon
    }

    private void DropCurrentItem()
    {
        customCursor.gameObject.SetActive(false);
        FindNearestSlot();
        currentItem = null;  // Reset current item after it has been dropped
    }

    private void FindNearestSlot()
    {
        Slots nearestSlot = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Slots slot in craftingSlots)
        {
            float distance = Vector2.Distance(Input.mousePosition, Camera.main.WorldToScreenPoint(slot.transform.position));
            if (distance < shortestDistance)
            {
                nearestSlot = slot;
                shortestDistance = distance;
            }
        }

        if (nearestSlot != null && currentItem != null)
        {
            nearestSlot.SetItem(currentItem);
            itemList[nearestSlot.index] = currentItem;  // Update itemList at the slot's index
            CheckForCreatedRecipes();
        }
    }

    public void CheckForCreatedRecipes()
    {
        string currentRecipe = string.Join("+", itemList.FindAll(item => item != null).ConvertAll(item => item.itemName));
        Debug.Log("Current Recipe String: " + currentRecipe);  // Debugging current recipe string

        bool recipeFound = false;
        for (int i = 0; i < recipes.Length; i++)
        {
            if (recipes[i] == currentRecipe)
            {
                Debug.Log("Recipe Match Found: " + recipes[i]);  // Confirm recipe match found
                resultSlots.SetItem(recipeResults[i]);  // Set the resulting item in the result slot
                recipeFound = true;
                break;
            }
        }

        if (!recipeFound)
        {
            Debug.Log("No matching recipe found.");
            resultSlots.SetItem(null);  // Optionally clear the result slot if no recipe matches
        }
    }
}
