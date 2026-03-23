using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;
using VRFPSKit;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    [SerializeField] private Image playerDamageImg_Left;
    [SerializeField] private Image playerDamageImg_Right;
    [SerializeField] private TextMeshProUGUI o2Level;
    public Damageable playerHealth;
    private Coroutine oxygenCoroutine;
    private int oxygenCount = 100;

    private void Awake()
    {
        Instance = this;
    }

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
}
