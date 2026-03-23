using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class BlueGameSettingScreen : MonoBehaviour
{
    #region Variables

    [SerializeField] private Color selectedColor;

    [Header("Option")]
    [SerializeField] private List<SettingOptionButton> buttonElementLinks;

    [Header("Sensitivity")]
    [SerializeField] private TextMeshProUGUI xSensitivityPerTxt;
    [SerializeField] private TextMeshProUGUI ySensitivityPerTxt;
    public Slider xSensitivitySlider;
    public Slider ySensitivitySlider;

    [SerializeField] private GameObject sensitivityPopUp;

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
        Destroy(BlueGameSoundManager.Instance.gameObject);
    }

    public void OnBeachNewGameClick()
    {
        PlayerPrefs.SetInt("BlueGameCompletedLevel", 0);
        Invoke(nameof(WaitForNewGame), 1.5f);
    }

    public void OnUnderWaterNewGameClick()
    {
        PlayerPrefs.SetInt("BlueGameCompletedLevel", 0);
        Invoke(nameof(WaitForNewGame), 1.5f);
    }

    private void WaitForNewGame()
    {
        //NetworkManager.Instance.Shutdown(ShemarooConstant.BlueBeachGamePlay);
    }

    public void OnBeachRestartMissionClick()
    {
        Invoke(nameof(WaitForNewGame), 1.5f);
    }

    public void OnUnderWaterRestartMissionClick()
    {
        Invoke(nameof(WaitForRestartMission), 1.5f);
    }

    private void WaitForRestartMission()
    {
        //NetworkManager.Instance.Shutdown(ShemarooConstant.BlueUnderWaterGamePlay);
    }

    public void OnCloseClick()
    {
        if (BlueGameSoundManager.Instance != null)
            BlueGameSoundManager.Instance.OnButtonClick();
        gameObject.SetActive(false);
    }

    public void ShowElement(TextMeshProUGUI element)
    {
        foreach (var elementSection in buttonElementLinks)
        {
            if (elementSection.element == element)
                elementSection.lineImage.SetActive(true);
            else
                elementSection.lineImage.SetActive(false);

            if(elementSection.element == element)
                elementSection.element.color = selectedColor;
            else
                elementSection.element.color = Color.white;
        }

        sensitivityPopUp.SetActive(buttonElementLinks[2].lineImage.activeSelf);
    }

    #endregion
}
