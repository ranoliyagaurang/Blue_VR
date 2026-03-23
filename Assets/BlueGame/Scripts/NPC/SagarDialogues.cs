using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SagarDialogues : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject boats;
    [SerializeField] private List<SagarConversations> dialogueList = new();
    [SerializeField] private TMP_Text dialogue_Text;
    [SerializeField] private TMP_Text nextBtn_Text;
    [SerializeField] private Button next_Btn;
    [SerializeField] private Animator sagarAnim;
    private int _currentDialogueIndex = 0;
    private bool _isTyping = false;
    private Coroutine _typingCoroutine;
    private float letterDelay = 0.05f;
    private float pauseTime;

    #endregion

    #region Unity_Callback

    private void Start()
    {
        next_Btn.onClick.AddListener(Next_Button);
    }

    private void OnEnable()
    {
        StartDialogue();
    }

    #endregion

    #region Sagar_Conversations

    public void StartDialogue()
    {
        if (dialogueList.Count > 0)
        {
            nextBtn_Text.text = "Next";
            _currentDialogueIndex = 0;
            DisplayDialogue(dialogueList[_currentDialogueIndex].dialogue);
        }
        else
        {
            Debug.LogWarning("Dialogue list is empty!");
        }
    }

    private void Next_Button()
    {
        if (BlueGameSoundManager.Instance != null)

        if (_isTyping)
        {
            SkipTyping();
        }
        else
        {
            _currentDialogueIndex++;

            if (_currentDialogueIndex == dialogueList.Count - 1)
            {
                nextBtn_Text.text = "Okay";
            }

            if (_currentDialogueIndex < dialogueList.Count)
            {
                DisplayNextDialogue();
            }
            else
            {
                EndDialogue();
            }
        }
    }

    private void DisplayNextDialogue()
    {
        DisplayDialogue(dialogueList[_currentDialogueIndex].dialogue);
        //Sagar Animation
    }

    private void DisplayDialogue(string dialogue)
    {
        if (_typingCoroutine != null)
        {
            StopCoroutine(_typingCoroutine);
        }

        _typingCoroutine = StartCoroutine(TypeText(dialogue));
    }

    private IEnumerator TypeText(string dialogue)
    {
        dialogue_Text.text = "<b>" + dialogueList[_currentDialogueIndex].name + "</b>";
        if (dialogueList[_currentDialogueIndex].name.Equals("Sagar : "))
            sagarAnim.SetBool("Talk", true);
        sagarAnim.Play("Talking", 0, pauseTime);
        _isTyping = true;

        for (int i = 0; i < dialogue.Length; i++)
        {
            dialogue_Text.text += dialogue[i];
            yield return new WaitForSeconds(letterDelay);
        }

        _isTyping = false;
        pauseTime = sagarAnim.GetNormalizedTime(0);
        sagarAnim.SetBool("Talk", false);
    }
    
    private void SkipTyping()
    {
        if (_typingCoroutine != null)
        {
            StopCoroutine(_typingCoroutine);
        }

        dialogue_Text.text = dialogue_Text.text = "<b>" + dialogueList[_currentDialogueIndex].name + "</b>" + dialogueList[_currentDialogueIndex].dialogue;
        _isTyping = false;
        pauseTime = sagarAnim.GetNormalizedTime(0);
        sagarAnim.SetBool("Talk", false);
    }

    private void EndDialogue()
    {
        boats.SetActive(true);
        gameObject.SetActive(false);
        Debug.Log("Talk Complete");
    }

    #endregion
}

[Serializable]
public class SagarConversations
{
    public string name;
    public string dialogue;
}