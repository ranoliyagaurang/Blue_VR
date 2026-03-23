using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIFadeController : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private const float FADE_DURATION = 1f;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void FadeIn()
    {
        StartCoroutine(FadeCanvasGroup(_canvasGroup, _canvasGroup.alpha, 1f, FADE_DURATION));
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }

    public void FadeOut()
    {
        StartCoroutine(FadeCanvasGroup(_canvasGroup, _canvasGroup.alpha, 0f, FADE_DURATION));
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    } 

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float startAlpha, float endAlpha, float duration)
    {
        var time = 0f;

        // Loop over time and update alpha gradually
        while (time < duration)
        {
            time += Time.deltaTime;
            var newAlpha = Mathf.Lerp(startAlpha, endAlpha, time / duration);
            cg.alpha = newAlpha; 
            yield return null; // Wait until the next frame
        }
        cg.alpha = endAlpha;
    }
}
