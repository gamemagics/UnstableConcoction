using Yarn.Unity;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text dialogue = null;

    private Vector2 pos0;

    private DialogueUI dialogueUI = null;

    private TMPro.TMP_Text[] options;

    private int optionSize;
    private int currentOption;

    private bool isOptionDisplayed;

    // Start is called before the first frame update
    void Start()
    {
        //pos0 = dialogueBubble.anchoredPosition;
        // Flag needed to check when the Options are displayed to enable the controls for them
        isOptionDisplayed = false;
        // Get a reference to the DialogueUI
        dialogueUI = FindObjectOfType<DialogueUI>();
    }

    // Update is called once per frame
    void Update()
    {
        ControlOptions();
    }

    private void ControlOptions()
    {
        if (isOptionDisplayed)
        {
            ChangeOption();
            SelectOption();
        }
        else
        {
            SkipDialogue();
        }
    }

    private void ChangeOption()
    {

        if (Input.GetKeyDown(KeyCode.S))
        {
            dialogueUI.optionButtons[currentOption].transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            currentOption = (currentOption + 1) % optionSize;
            //dialogue.SetText(options[currentOption].text);
            dialogueUI.optionButtons[currentOption].transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            dialogueUI.optionButtons[currentOption].transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            //Move to the previous option
            if (currentOption == 0)
                currentOption = optionSize - 1;
            else
                currentOption = (Mathf.Abs(currentOption - 1) % optionSize);

            //dialogue.SetText(options[currentOption].text);
            dialogueUI.optionButtons[currentOption].transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }
    }

    private void SkipDialogue()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            dialogueUI.MarkLineComplete();
        }
    }

    private void SelectOption()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            dialogueUI.SelectOption(currentOption);
            ResetCurrentOption();
        }
    }

    private void ResetCurrentOption()
    {
        currentOption = 0;
    }

    public void SetStartingOption()
    {
        dialogue.SetText(options[0].text);
    }

    public void SetOptionDisplayed(bool flag)
    {
        isOptionDisplayed = flag;
        dialogueUI.optionButtons[currentOption].transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }
}