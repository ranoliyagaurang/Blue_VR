using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using VRFPSKit;

public class PlayerController : MonoBehaviour
{
    #region Variables

    public static PlayerController Instance { get; private set; }
    [SerializeField] private Image playerDamageImg_Left;
    [SerializeField] private Image playerDamageImg_Right;
    [SerializeField] private TextMeshProUGUI o2Level;

    [SerializeField] private GameObject leftHandRay;
    [SerializeField] private GameObject rightHandRay;

    public Damageable playerHealth;
    private Coroutine oxygenCoroutine;
    private int oxygenCount = 100;

    #endregion

    #region Unity_Callback

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    #region Hand Ray Control

    public void LeftHandGrab(SelectEnterEventArgs args)
    {
        leftHandRay.SetActive(false);
    }

    public void LeftHandGrabRelease(SelectExitEventArgs args)
    {
        leftHandRay.SetActive(true);
    }

    public void RightHandGrab(SelectEnterEventArgs args)
    {
        rightHandRay.SetActive(false);
    }


    public void RightHandGrabRelease(SelectExitEventArgs args)
    {
        rightHandRay.SetActive(true);
    }

    #endregion

    #region Player_Damage

    public void PlayerTakeDamage(float damage)
    {
        playerDamageImg_Left.DOFade(1, 0.1f).OnComplete(() =>
        {
            playerDamageImg_Left.DOFade(0, 0.5f);
        });
        playerDamageImg_Right.DOFade(1, 0.1f).OnComplete(() =>
        {
            playerDamageImg_Right.DOFade(0, 0.5f);
        });

        PlayerDamage(damage);
        //ScreenShakeVR.instance.ScreenShakeButton();
    }

    private void PlayerDamage(float damage)
    {
        playerHealth.TakeDamage(damage);
    }

    #endregion

    #region Oxygen_Logic

    public void OxygenHealth()
    {
        oxygenCoroutine = StartCoroutine(OxygenHealthDecrease());
    }

    public void GetOxygen()
    {
        if (oxygenCoroutine != null)
        {
            StopCoroutine(oxygenCoroutine);
            oxygenCount = 100;
            o2Level.text = "O2: " + oxygenCount + "%";
        }
    }

    private IEnumerator OxygenHealthDecrease()
    {
        while (true)
        {
            oxygenCount -= 1;
            o2Level.text = "O2: " + oxygenCount + "%";

            if (oxygenCount <= 0)
            {
                PlayerDamage(100);
                yield break;
            }

            yield return new WaitForSeconds(2f);
        }
    }

    #endregion
}
