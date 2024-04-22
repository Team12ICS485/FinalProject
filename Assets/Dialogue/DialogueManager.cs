using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    private bool dialogueIsPlaying;
    public bool DialogueIsPlaying
    {
        get { return dialogueIsPlaying; }
        private set { dialogueIsPlaying = value; }
    }

    private static DialogueManager instance;
    public static DialogueManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("DialogueManager instance is not initialized yet.");
            }
            return instance;
        }
    }

    private Story currentStory;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("Another instance of DialogueManager already exists.");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            Debug.Log("DialogueManager instance created");
        }
    }

    private void Start()
    {
        DialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        // Initialize choices text
        choicesText = new TextMeshProUGUI[choices.Length];
        for (int i = 0; i < choices.Length; i++)
        {
            TextMeshProUGUI choiceText = choices[i].GetComponentInChildren<TextMeshProUGUI>();
            if (choiceText != null)
            {
                choicesText[i] = choiceText;
            }
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        if (inkJSON != null)
        {
            currentStory = new Story(inkJSON.text);
            DialogueIsPlaying = true;
            dialoguePanel.SetActive(true);
            ContinueStory();
        }
        else
        {
            Debug.LogError("Ink JSON asset is null.");
        }
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();
            DisplayChoices();
        }
        else
        {
            ExitDialogueMode();
        }
    }

    private void Update()
    {
        if (!DialogueIsPlaying)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ContinueStory();
        }
    }

    private void ExitDialogueMode()
    {
        DialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        foreach (GameObject choice in choices)
        {
            choice.SetActive(false);  // Ensure all choice buttons are hidden when not in use
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;
        if (currentChoices.Count > choicesText.Length)
        {
            Debug.LogError("More choices than UI can handle");
            return;  // Exit the function to avoid index out of range errors
        }

        for (int i = 0; i < currentChoices.Count; i++)
        {
            choices[i].SetActive(true);
            choicesText[i].text = currentChoices[i].text;
        }

        // Hide unused choices
        for (int i = currentChoices.Count; i < choicesText.Length; i++)
        {
            choices[i].SetActive(false);
        }
    }
}
