using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using JetBrains.Annotations;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    [SerializeField] private TextMeshProUGUI[] choicesText;

    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }

    public static DialogueManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one DialogueManager instance found!");
            Destroy(gameObject);  // Destroy the extra instance.
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject); // Make it persist across scenes.
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (choices != null && choices.Length > 0)
        {
            choicesText = new TextMeshProUGUI[choices.Length];
            for (int index = 0; index < choices.Length; index++)
            {
                if (choices[index] != null)
                    choicesText[index] = choices[index].GetComponentInChildren<TextMeshProUGUI>();
            }
        }
    }

    private void Update()
    {
        if (dialogueIsPlaying && currentStory.currentChoices.Count == 0 && Input.GetMouseButtonDown(0))
        {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        ContinueStory();
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);  // This could include a fade out or other animation.
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
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
            StartCoroutine(ExitDialogueMode());
        }
    }

    private void InitializeChoices()
    {
        for (int i = 0; i < choices.Length; i++)
        {
            int choiceIndex = i;
            choices[i].GetComponent<Button>().onClick.AddListener(() => MakeChoice(choiceIndex));
        }
    }

    public void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;
        for (int i = 0; i < choices.Length; i++)
        {
            if (i < currentChoices.Count)
            {
                choices[i].SetActive(true);
                choicesText[i].text = currentChoices[i].text;
            }
            else
            {
                choices[i].SetActive(false);
            }
        }

        InitializeChoices();
    }

    public void MakeChoice(int choiceIndex)
    {
        Debug.Log($"Making choice {choiceIndex}");
        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }
}
