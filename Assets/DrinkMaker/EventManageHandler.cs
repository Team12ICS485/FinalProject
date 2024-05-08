using UnityEngine;
using UnityEngine.EventSystems;

public class EventManager : MonoBehaviour
{
    [System.Obsolete]
    void Awake()
    {
        EventSystem[] eventSystems = FindObjectsOfType<EventSystem>();
        if (eventSystems.Length > 1)
        {
            for (int i = 0; i < eventSystems.Length; i++)
            {
                // Only preserve the first EventSystem found, destroy the others
                if (i > 0)
                    Destroy(eventSystems[i].gameObject);
            }
        }
        else if (eventSystems.Length == 0)
        {
            // If no EventSystem is found, create one dynamically
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }
    }
}
