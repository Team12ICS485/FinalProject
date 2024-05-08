using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private float typingSpeed = 0.05f;  // Speed at which the dialogue text is displayed.
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    [SerializeField] private TextMeshProUGUI[] choicesText;

    private Story currentStory;
    private Coroutine displayLineCoroutine;
    public bool dialogueIsPlaying { get; private set; }

    public bool canContinueToNextLine = false;

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
            for (int i = 0; i < choices.Length; i++)
            {
                if (choices[i] != null)
                    choicesText[i] = choices[i].GetComponentInChildren<TextMeshProUGUI>();
            }
        }
    }

    private void Update()
    {
        if (canContinueToNextLine && dialogueIsPlaying && currentStory.currentChoices.Count == 0 && Input.GetMouseButtonDown(0))
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

    private IEnumerator DisplayLine(string line)
    {
        dialogueText.text = "";

        canContinueToNextLine = false;

        foreach (char letter in line.ToCharArray())
        {
            if (Input.GetMouseButtonDown(0))
            {
                dialogueText.text = line;
                break;
            }
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        canContinueToNextLine = true;
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);  // This could include a fade out or other animation.

        // Check if the dialoguePanel still exists before trying to access it.
        if (dialoguePanel != null)
        {
            dialogueIsPlaying = false;
            dialoguePanel.SetActive(false);
            dialogueText.text = "";
        }
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }
            displayLineCoroutine = StartCoroutine(DisplayLine(currentStory.Continue()));
            DisplayChoices();
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
            //LoadNextScene();
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
        if (canContinueToNextLine)
        {
            Debug.Log($"Making choice {choiceIndex}");
            currentStory.ChooseChoiceIndex(choiceIndex);
            ContinueStory();
        }
    }

    /*private void LoadNextScene()
    {
        SceneManager.LoadScene("DrinkMakerScene3");
    }

    // Added methods for scene transition based on Ink outcomes
    public void LoadDrinkMakerScene()
    {
        SceneManager.LoadScene("DrinkMakerScene3");
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("NPC1(Tiara)");
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainScene")
        {
            int drinkResult = PlayerPrefs.GetInt("DrinkResult", 0);
            HandleDialogueBasedOnResult(drinkResult);
        }
    }

   private void HandleDialogueBasedOnResult(int result)
    {
        if (result == 1)
        {
            currentStory.ChoosePathString("success_path");
        }
        else
        {
            currentStory.ChoosePathString("failure_path");
        }
        ContinueStory();
   }*/
}
