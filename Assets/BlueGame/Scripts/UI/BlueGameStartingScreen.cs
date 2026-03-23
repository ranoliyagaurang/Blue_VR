using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class BlueGameStartingScreen : MonoBehaviour
{
    #region Variable

    [Header("Option")]
    [SerializeField] private List<SettingOptionButton> buttonElementLinks;
    [SerializeField] private Color selectedColor;

    #endregion

    #region Unity_Callback

    private void Start()
    {
        foreach (var elementButtonLink in buttonElementLinks)
        {
            if (elementButtonLink.button == null)
            {
                continue;
            }
            elementButtonLink.button.onClick.AddListener(() =>
            {
                ShowElement(elementButtonLink.element);
            });
        }

        ShowElement(buttonElementLinks[0].element);
    }

    #endregion

    #region Button_Click

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void OnExitBlueGame()
    {
        Debug.Log("Quit BlueGame");
        //LoadingStatus.Instance.ChangeLoadingStatus(CurrentLoadingStatus.LOADING, LoaderType.SIMPLE);
        //NetworkManager.Instance.ShutdownAndJoinRoom(PlayerInfo.Instance.bollywoodBoulervardZoneName, ShemarooConstant.BollywoodSceneKey);
        Destroy(BlueGameSoundManager.Instance.gameObject);
    }

    public void ShowElement(TextMeshProUGUI element)
    {
        foreach (var elementSection in buttonElementLinks)
        {
            if (elementSection.element == element)
                elementSection.lineImage.SetActive(true);
            else
                elementSection.lineImage.SetActive(false);

            if (elementSection.element == element)
                elementSection.element.color = selectedColor;
            else
                elementSection.element.color = Color.white;
        }
    }

    #endregion
}
