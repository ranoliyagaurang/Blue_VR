using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlueGameUIManager : MonoBehaviour
{
    #region Variables

    public static BlueGameUIManager Instance { get; private set; }
    public static Action LevelFailked;

    [Header("Game_Instruction")]
    public BlueGameStartingScreen startingScreen;
    [SerializeField] private GameObject instructionPanel;
    [SerializeField] private GameObject gameCompleteMessage;
    [SerializeField] private TextMeshProUGUI instructionTxt;
    [SerializeField] private BeachLevelController beachController;
    [SerializeField] private MissionFailed missionFailed;

    //[Header("SettingScreen")]
    //public BlueGameSettingScreen settingScreen;
    //public GameObject settingbtn;

    [Header("SagarInstruction")]
    public GameObject sagarInstruction;

    [Header("CallingUI")]
    [SerializeField] private GameObject phoneCallUI;
    [SerializeField] private GameObject callConversationsUI;
    [SerializeField] private CameraPathMover pathMover;
    [SerializeField] private GameObject uiCamera;

    public bool isBoatFight;
    private PlayerAIBoat playerAIBoat;

    #endregion

    #region Unity_Callback

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //settingScreen.SetSensitivity();
    }


    private void OnEnable()
    {
        LevelFailked += MissionFailed;
    }

    private void OnDisable()
    {
        LevelFailked -= MissionFailed;
    }

    #endregion

    #region Event_Callback

    private void MissionFailed()
    {
        missionFailed.gameObject.SetActive(true);
    }

    #endregion

    #region Calling

    public void OnCallRinging()
    {
        beachController.themeSoung.Stop();
        phoneCallUI.SetActive(true);
        uiCamera.SetActive(true);
    }

    public void OnCallReceive()
    {
        phoneCallUI.SetActive(false);
        callConversationsUI.SetActive(true);
    }

    public void ConversationsEnd()
    {
        callConversationsUI.SetActive(false);
        sagarInstruction.SetActive(true);
        beachController.cameraMover.SetActive(false);
    }

    public void SagarInstructionClose()
    {
        sagarInstruction.SetActive(false);
        uiCamera.SetActive(false);
        BlueGameManager.Instance.PlayerMovement(true);
    }

    #endregion

    #region Button_Click

    public void StartGame()
    {
        beachController.themeSoung.Play();
        startingScreen.gameObject.SetActive(false);
        pathMover.StartGame();
    }

    public void OnSettingOpen()
    {
        //settingScreen.gameObject.SetActive(true);
    }

    #endregion

    #region Instruction

    public void OnCompleteUI()
    {
        gameCompleteMessage.SetActive(true);
    }

    public void OnExitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        missionFailed.OnNewGameClick();
    }

    public void ShowBoatFightIns(PlayerAIBoat pb)
    {
        isBoatFight = true;
        playerAIBoat = pb;
        ShowInstruction("Enemy have spotted you with Lady Blue’s Treasure. they’re closing in fast! Grab your weapon, fight back.\r\nShoot them down one by one to survive this deadly attack!");
    }

    public void ShowInstruction(string instruction)
    {
        instructionTxt.text = instruction;
        instructionPanel.SetActive(true);
    }

    public void OnInstructionClick()
    {
        if (BlueGameSoundManager.Instance != null)
            BlueGameSoundManager.Instance.OnButtonClick();
        instructionPanel.SetActive(false);
        uiCamera.SetActive(false);

        if (BlueGameManager.Instance.isBoatRidingCompleted)
        {
            BlueGameManager.Instance.isBoatRidingCompleted = false;
            BlueGameManager.Instance.OnBoatRidingComplete();
        }

        if (isBoatFight)
        {
            playerAIBoat.EnemySpawn();
            isBoatFight = false;
        }
    }

    #endregion

}
