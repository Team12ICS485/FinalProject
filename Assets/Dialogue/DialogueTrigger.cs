using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public TextAsset inkJSON; // Assign this in the Inspector with your Ink JSON file

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TriggerDialogue();
        }
    }

    private void TriggerDialogue()
    {
        if (!DialogueManager.GetInstance().dialogueIsPlaying)
        {
            DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
        }
    }
}
