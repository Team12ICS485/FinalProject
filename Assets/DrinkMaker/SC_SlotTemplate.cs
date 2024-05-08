using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SC_SlotTemplate : MonoBehaviour, IPointerClickHandler
{
    public Image container;
    public Image item;
    public Text count;

    [HideInInspector]
    public bool hasClicked = false;
    [HideInInspector]
    public SC_ItemCrafting craftingController;

    public void OnPointerClick(PointerEventData eventData)
    {
        hasClicked = true;
        craftingController.ClickEventRecheck();
    }

    void OnEnable()
    {
        Debug.Log("Slot Template enabled on " + gameObject.name);
    }
}
