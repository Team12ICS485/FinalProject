using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private Vector3 startPosition;
    private Transform originalParent;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) // Ensure there is a CanvasGroup component
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        canvas = GetComponentInParent<Canvas>(); // Get the Canvas component from parent
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = rectTransform.position; // Store the starting position
        originalParent = rectTransform.parent; // Store the original parent
        canvasGroup.alpha = 0.6f; // Make the item semi-transparent
        canvasGroup.blocksRaycasts = false; // Disable raycast to allow drop event detection

        // Optional: Bring to front if needed
        rectTransform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas.renderMode == RenderMode.WorldSpace)
        {
            rectTransform.position = Camera.main.ScreenToWorldPoint(eventData.position);
            rectTransform.position = new Vector3(rectTransform.position.x, rectTransform.position.y, startPosition.z);
        }
        else
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        rectTransform.position = startPosition; // Reset position to the start (this line can be adjusted based on your needs)
        canvasGroup.alpha = 1.0f; // Reset transparency
        canvasGroup.blocksRaycasts = true; // Enable raycasts again

        // Check if dropped on a valid slot
        if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.CompareTag("Slot"))
        {
            // Set the item's new parent to the slot it was dropped on
            rectTransform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform, false);
            rectTransform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;
        }
        else
        {
            // Reset parent to the original parent if not dropped on a valid slot
            rectTransform.SetParent(originalParent, false);
        }
    }
}
