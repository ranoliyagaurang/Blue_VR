using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BeachLevelController : MonoBehaviour
{
    #region Variables

    public GameObject cameraMover;
    public GameObject boatInstructionPanel;
    [SerializeField] private TextMeshProUGUI boatInstructionTxt;
    [SerializeField] private Button boatPickbtn;
    [SerializeField] private TextMeshProUGUI boatPickbtnTxt;
    public AudioSource themeSoung;

    #endregion

    #region BeachLevel_Control

    [ContextMenu("Test")]
    public void Test()
    {
        OnBoatPick();
    }

    public void ShowBoatInstruction(string instruction, bool isPerfact, Transform uiParent)
    {
        boatInstructionTxt.text = instruction;
        boatPickbtn.gameObject.SetActive(true);
        boatInstructionPanel.transform.SetParent(uiParent, false);
        boatInstructionPanel.transform.localPosition = Vector3.zero;
        boatInstructionPanel.SetActive(true);
        if (isPerfact)
        {
            boatPickbtnTxt.text = "Pick";
            boatPickbtn.onClick.RemoveAllListeners();
            boatPickbtn.onClick.AddListener(OnBoatPick);
        }
        else
        {
            boatPickbtnTxt.text = "Okay";
            boatPickbtn.onClick.RemoveAllListeners();
            boatPickbtn.onClick.AddListener(OnHideBoatInstruction);
        }
    }
    private void OnHideBoatInstruction()
    {
        if (BlueGameSoundManager.Instance != null)
            BlueGameSoundManager.Instance.OnButtonClick();
        boatInstructionPanel.SetActive(false);
    }

    private void OnBoatPick()
    {
        if (BlueGameSoundManager.Instance != null)
            BlueGameSoundManager.Instance.OnButtonClick();
        boatInstructionPanel.SetActive(false);
        BlueGameManager.Instance.OnBoatRideActive();
    }

    #endregion
}
