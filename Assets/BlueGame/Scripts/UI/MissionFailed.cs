using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MissionFailed : MonoBehaviour
{
    #region Variables

    [SerializeField] private Color selectedColor;

    [Header("Option")]
    [SerializeField] private List<SettingOptionButton> buttonElementLinks;

    [SerializeField] private Image blackScreen;

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

    public void OnExitBlueGame()
    {
        Debug.Log("Quit BlueGame");
        blackScreen.DOFade(1, 1).OnComplete(() =>
        {
            Application.Quit();
        });
    }

    public void OnNewGameClick()
    {
        blackScreen.DOFade(1, 1).OnComplete(() =>
        {
            SceneManager.LoadSceneAsync("BlueBeachGamePlay");
        });
    }

    public void OnRestartMissionClick()
    {
        blackScreen.DOFade(1, 1).OnComplete(() =>
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        });
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

[Serializable]
public class SettingOptionButton
{
    public Button button;
    public TextMeshProUGUI element;
    public GameObject lineImage;
}