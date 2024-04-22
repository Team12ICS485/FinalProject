using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private TextAsset inkJSON; // Drag your Ink JSON file here in the Inspector{
    private void OnMouseOver()
    {
        // Check if the right mouse button was clicked
        if (Input.GetMouseButtonDown(0)) // 1 is the button index for the right mouse button
        {
            TriggerDialogue();
        }
    }

    private void TriggerDialogue()
    {
        DialogueManager.Instance.EnterDialogueMode(inkJSON); // Make sure to assign your Ink JSON file correctly
    }
}
