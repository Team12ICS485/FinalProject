using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Image actorImage;
    public Text actorName;
    public Text messageText;
    public RectTransform backgroundBox;

    Message[] currentMessages;
    Actor[] cuurentActors;
    int activeMessage = 0;
    public static bool dialogueActive = false;

    public void OpenDialogue(Message[] messages, Actor[] actors)
    {
        currentMessages = messages;
        cuurentActors = actors;
        activeMessage = 0;
        dialogueActive = true;
           
        Debug.Log("Open Dialogue" + messages.Length);
        DisplayMessage();
        
    }

    void DisplayMessage()
    {
        Message messageToDisplay = currentMessages[activeMessage];
        messageText.text = messageToDisplay.message; 
        
       Actor actorToDisplay = cuurentActors[messageToDisplay.actorId];
       actorName.text = actorToDisplay.name;
       actorImage.sprite = actorToDisplay.sprite;
    }

    public void NextMessage()
    {
        activeMessage++;
        if (activeMessage < currentMessages.Length)
        {
            DisplayMessage();
        }
        else
            {
            Debug.Log("End of Dialogue");
            dialogueActive = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && dialogueActive)
        {
            NextMessage();
        }
        
    }
}
