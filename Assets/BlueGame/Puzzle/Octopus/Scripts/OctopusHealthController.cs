using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class OctopusHealthController : MonoBehaviour
{
    #region Variables

    [SerializeField] private Image healthBar;
    [SerializeField] private GameObject healthBarCanvas;
    [SerializeField] private Animator _animator;

    private float currentHelth = 1;
    private readonly float octopusDamage = 0.05f;

    [SerializeField] private BoxCollider leftSideCol;
    [SerializeField] private BoxCollider rightSideCol;

    [SerializeField] private Swimmer swimmer;
    [SerializeField] private AudioSource octopusSound;

    #endregion

    #region Attack_Damage_Health

    public void OnOctopusAttack()
    {
        _animator.SetBool("Attack", true);
        healthBarCanvas.SetActive(true);
        octopusSound.Play();
    }

    public void TakeDamage()
    {
        if (_animator.GetBool("Attack"))
        {
            currentHelth -= octopusDamage;

            if (currentHelth <= 0)
            {
                octopusSound.Stop();
                healthBarCanvas.SetActive(false);
                _animator.SetBool("Attack", false);
                Debug.Log("Octopus Die");
                UnderWaterGamePlayManager.Instance.octopusSpotArea.SetActive(false);
                leftSideCol.enabled = false;
                rightSideCol.enabled = false;
                swimmer.IsOctopusActivate(false);
            }
            healthBar.DOFillAmount(currentHelth, 0.1f);
        }
    }

    #endregion
}
