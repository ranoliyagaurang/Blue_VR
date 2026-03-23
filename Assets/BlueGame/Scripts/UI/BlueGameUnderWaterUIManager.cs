using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlueGameUnderWaterUIManager : MonoBehaviour
{
    #region Variables
    public static BlueGameUnderWaterUIManager Instance { get; private set; }

    [Header("Camera")]
    [SerializeField] private GameObject uiCamera;
    public Image blackScreen;

    [Header("Game_Instruction")]
    [SerializeField] private GameObject instructionPanel;
    [SerializeField] private TextMeshProUGUI instructionTxt;
    [SerializeField] private TextMeshProUGUI instructionBtnTxt;

    [Header("Item_Instruction")]
    [SerializeField] private GameObject itemInstructionPanel;
    [SerializeField] private TextMeshProUGUI itemInstructionTxt;
    [SerializeField] private Image itemImage;

    [Header("CodeEnterScreen")]
    [SerializeField] private GameObject codeEnterScreen;

    [Header("GearRotator")]
    [SerializeField] private GameObject gearRotatorPuzzle;

    [Header("TrapUI")]
    [SerializeField] private BlueGameTrapController trapController;

    [Header("MissionFailed")]
    public MissionFailed missionFailedScreen;

    [Header("Octopus")]
    [SerializeField] private Vector3 octopusTrapPos;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private OctopusHealthController octopusHealth;

    #endregion

    #region Unity_Callback

    private void Awake()
    {
        Instance = this;
        blackScreen.DOFade(0, 1f);
    }

    #endregion

    #region Game_Instruction

    public void ShowInstruction(string instruction, string btnStr = "Okay")
    {
        instructionTxt.text = instruction;
        instructionBtnTxt.text = btnStr;
        instructionPanel.SetActive(true);
        uiCamera.SetActive(true);
    }

    public void ShowItemInstruction(string instruction, Sprite image)
    {
        itemInstructionTxt.text = instruction;
        itemImage.sprite = image;
        itemInstructionPanel.SetActive(true);
        uiCamera.SetActive(true);
    }

    public void HideItemInstruction()
    {
        itemInstructionPanel.SetActive(false);
        uiCamera.SetActive(false);
        if(UnderWaterGamePlayManager.Instance.currentActivity == CurrentActivity.DoorOpen)
        {
            UnderWaterGamePlayManager.Instance.OnTunnelDoorOpen();
            UnderWaterGamePlayManager.Instance.currentActivity = CurrentActivity.None;
        }
    }

    public void OnInstructionClick()
    {
        if (BlueGameSoundManager.Instance != null)
            BlueGameSoundManager.Instance.OnButtonClick();

        Debug.Log("currentActivity : " + UnderWaterGamePlayManager.Instance.currentActivity);

        switch (UnderWaterGamePlayManager.Instance.currentActivity)
        {
            case CurrentActivity.GotoTunnle:
                UnderWaterGamePlayManager.Instance.tunnleGate.SetActive(true);
                UnderWaterGamePlayManager.Instance.directionArrow.SetTarget(UnderWaterGamePlayManager.Instance.tunnleGate.transform);
                break;
            case CurrentActivity.TunnelDoorPuzzle:
                UnderWaterGamePlayManager.Instance.SokobanPuzzleLoad();
                break;
            case CurrentActivity.DoorOpen:
                UnderWaterGamePlayManager.Instance.OnTunnelDoorOpen();
                break;
            case CurrentActivity.Trap:
                trapController.OnPuzzleActivate();
                break;
            case CurrentActivity.OxygenCylinder:
                UnderWaterGamePlayManager.Instance.PickUpOxygenCylinder();
                break;
            case CurrentActivity.FindShip:
                UnderWaterGamePlayManager.Instance.ShowShipPath();
                break;
            case CurrentActivity.FindShipBySaveLevel:
                UnderWaterGamePlayManager.Instance.ShowShipPathBySaveData();
                break;
            case CurrentActivity.ExploreShip:
                UnderWaterGamePlayManager.Instance.ExploreShip();
                break;
            case CurrentActivity.ShipMainDoor:
                UnderWaterGamePlayManager.Instance.GoToTheShipMainDoor();
                break;
            case CurrentActivity.ChestKey:
                UnderWaterGamePlayManager.Instance.OpenChestKeyAnim();
                break;
            case CurrentActivity.ChestPuzzle:
                gearRotatorPuzzle.SetActive(true);
                break;
            case CurrentActivity.ChestOpen:
                UnderWaterGamePlayManager.Instance.OpenTheChest();
                break;
            case CurrentActivity.TakeTreasure:
                TakeTreasure();
                break;
            case CurrentActivity.OctopusTrap:
                blackScreen.DOFade(1, 1f).OnComplete(() =>
                {
                    SetPlayerOctopusTrapPos();
                    OnOctopusSpot();
                    blackScreen.DOFade(0, 1f);
                });
                break;
        }
        UnderWaterGamePlayManager.Instance.currentActivity = CurrentActivity.None;
        instructionPanel.SetActive(false);
        uiCamera.SetActive(false);
    }

    #endregion

    #region Puzzle_UI_Control

    public void OnPlayerDead()
    {
        missionFailedScreen.gameObject.SetActive(true);
        //blueGameShootingScreen.OnPlayerDead();
    }

    public void OnSettingOpen()
    {
        //settingScreen.gameObject.SetActive(true);
    }

    public void ShowCodeEnterScreen()
    {
        codeEnterScreen.SetActive(true);
    }

    private void TakeTreasure()
    {
        blackScreen.DOFade(1, 1f).OnComplete(() =>
        {
            WaitForNewGame();
            blackScreen.DOFade(0, 1f);
        });
    }

    private void WaitForNewGame()
    {
        missionFailedScreen.OnNewGameClick();
    }

    private void SetPlayerOctopusTrapPos()
    {
        playerTransform.position = octopusTrapPos;
    }

    private void OnOctopusSpot()
    {
        octopusHealth.OnOctopusAttack();
    }

    #endregion
}
