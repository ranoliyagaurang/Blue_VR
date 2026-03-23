using TMPro;
using UnityEngine;

public class CodeEnterController : MonoBehaviour
{
    #region Variables

    public TextMeshProUGUI codeTxt;

    private readonly string correctCode = "241989";

    #endregion

    #region Button_Clieck

    public void OnEnterCode(string code)
    {
        if (BlueGameSoundManager.Instance != null)
            BlueGameSoundManager.Instance.OnButtonClick();
        if (codeTxt.text.Length >= 6)
            return;

        codeTxt.text += code;
    }

    public void OnEraseClick()
    {
        if (BlueGameSoundManager.Instance != null)
            BlueGameSoundManager.Instance.OnButtonClick();
        if (!string.IsNullOrEmpty(codeTxt.text))
        {
            codeTxt.text = codeTxt.text.Substring(0, codeTxt.text.Length - 1);
        }
    }

    public void OnGoClick()
    {
        if (BlueGameSoundManager.Instance != null)
            BlueGameSoundManager.Instance.OnButtonClick();
        if (codeTxt.text.Equals(correctCode))
        {
            Debug.Log("Correct Code Enter");
            PlayerPrefs.SetInt("BlueGameCompletedLevel", 8);
            gameObject.SetActive(false);
            UnderWaterGamePlayManager.Instance.ShipDoorOpen();
        }
        else
        {
            BlueGameUnderWaterUIManager.Instance.ShowInstruction("You enter a wrong pin please try again");
        }
    }

    #endregion
}
