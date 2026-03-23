using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OctopusHealthController : MonoBehaviour
{
    #region Variables

    [SerializeField] private Image healthBar;
    [SerializeField] private Animator _animator;

    private float currentHelth = 1;
    private readonly float octopusDamage = 0.03f;
    private readonly float fillSpeed = 4f;

    [SerializeField] private BoxCollider leftSideCol;
    [SerializeField] private BoxCollider rightSideCol;

    #endregion

    #region Attack_Damage_Health

    public void OnOctopusAttack()
    {
        _animator.SetBool("Attack", true);
    }

    public void TakeDamage()
    {
        currentHelth -= octopusDamage;

        if (currentHelth <= 0)
        {
            if (_animator.GetBool("Attack"))
            {
                _animator.SetBool("Attack", false);
                Debug.Log("Octopus Die");
                UnderWaterGamePlayManager.Instance.octopusSpotArea.SetActive(false);
                leftSideCol.enabled = false;
                rightSideCol.enabled = false;
            }
        }

        SetFillAmount(currentHelth);
    }

    public void SetFillAmount(float targetFill)
    {
        StopAllCoroutines();
        StartCoroutine(ChangeFill(targetFill));
    }

    private IEnumerator ChangeFill(float targetFill)
    {
        float startFill = healthBar.fillAmount;
        float time = 0f;

        while (Mathf.Abs(healthBar.fillAmount - targetFill) > 0.001f)
        {
            time += Time.deltaTime * fillSpeed;
            healthBar.fillAmount = Mathf.Lerp(startFill, targetFill, time);
            yield return null;
        }

        healthBar.fillAmount = targetFill;
    }

    #endregion
}
