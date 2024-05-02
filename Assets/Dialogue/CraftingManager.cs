using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CraftingManager : MonoBehaviour
{
    private Item currentItem;
    public UnityEngine.UI.Image customCursor;
    public Slots[] craftingSlots;

    public List<Item> itemList;
    public string[] recipes;
    public Item[] recipeResults;
    public Slots resultSlots;

    public void OnMouseDownItem(Item item)
    {
      if(currentItem == null)
        {
        currentItem = item;
        customCursor.gameObject.SetActive(true);
        customCursor.sprite = currentItem.GetComponent<UnityEngine.UI.Image>().sprite;
      }
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if(currentItem != null)
            {
                customCursor.gameObject.SetActive(false);
                currentItem = null;

                Slots nearestSlot = null;
                float shortestDistance = float.MaxValue;

                foreach (Slots slot in craftingSlots)
                {
                   float dist = Vector2.Distance(Input.mousePosition, slot.transform.position);
                    if(dist < shortestDistance)
                    {
                            nearestSlot = slot;
                            shortestDistance = dist;
                      }
                }
                if(nearestSlot != null)
                {
                    nearestSlot.gameObject.SetActive(true);
                    nearestSlot.GetComponent<UnityEngine.UI.Image>().sprite = currentItem.GetComponent<UnityEngine.UI.Image>().sprite;
                    nearestSlot.item = currentItem;
                    itemList[nearestSlot.index] = currentItem;

                    currentItem = null;
                    CheckForCreatedRecipes();
                }

            }
        }

    }
    void CheckForCreatedRecipes()
    {
      resultSlots.gameObject.SetActive(false);
        resultSlots.item = null;

        string currentRecipeString = "";
        foreach(Item item in itemList)
        {
            if(item != null)
            {
                currentRecipeString += item.itemName;
            }
            else
            {
                currentRecipeString += "null";
            } 
        }
        for(int i =0; i <recipes.Length; i++)
        {
            //f (recipes[i] == currentRecipesString)
            {
                resultSlots.gameObject.SetActive(true);
                resultSlots.GetComponent<UnityEngine.UI.Image>().sprite = recipeResults[i].GetComponent<UnityEngine.UI.Image>().sprite;
                resultSlots.item = recipeResults[i];
                break;
            }
        }
    }
      public void OnClickSlot(Slots slot){
        slot.item = null;
        itemList[slot.index] = null;
        slot.gameObject.SetActive(false);
        CheckForCreatedRecipes();
    }
}
