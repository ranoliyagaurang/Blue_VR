using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AISharkController : MonoBehaviour
{
    #region Variables

    public bool IsShark;

    [SerializeField] private Image healthBar;
    private Transform playerTransform;
    [SerializeField] private Animator _animator;
    private float fishDamage = 0.2f;

    private float fishSpeed = 2.7f;
    private float currentHelth = 1;
    private bool isStopSwim;

    private bool isCloseOnece = false;

    private bool isDamage = true;

    #endregion

    #region Unity_Callback

    void Update()
    {
        if (playerTransform == null || isStopSwim)
            return;

        MoveFishTowardPlayer();
        CheckFishDistance();
    }

    public void SetTarget(Transform target)
    {
        playerTransform = target;
    }

    private void Start()
    {
        if(IsShark)
        {
            fishSpeed = 2f;
            fishDamage = 0.35f;
        }
        else
        {
            fishSpeed = 2.5f;
            fishDamage = 0.2f;
        }
    }

    #endregion

    #region Fish_Movement

    void MoveFishTowardPlayer()
    {
        Vector3 pos = new Vector3(playerTransform.position.x, playerTransform.position.y + 0.8f, playerTransform.position.z);

        Vector3 direction = (pos - transform.position).normalized;

        Collider[] neighbors = Physics.OverlapSphere(transform.position, 2f);
        foreach (Collider col in neighbors)
        {
            if (col.gameObject != gameObject && col.CompareTag("Enemy"))
            {
                Vector3 away = transform.position - col.transform.position;
                direction += away.normalized * 1.2f;
            }
        }

        direction.Normalize();

        transform.position = Vector3.MoveTowards(
            transform.position,
            transform.position + direction,
            fishSpeed * Time.deltaTime
        );

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    #endregion

    #region Player_And_Fish_Damage

    void CheckFishDistance()
    {
        float dist = Vector3.Distance(transform.position, playerTransform.position);
        if (dist <= 7f)
        {
            if (!_animator.GetBool("Attack"))
                _animator.SetBool("Attack", true);
            Debug.Log("Fish Close Animation");
        }
        else
        {
            if (_animator.GetBool("Attack"))
                _animator.SetBool("Attack", false);
        }
        if (dist <= 2f)
        {
            Debug.Log("Fish to Much Close");
            isCloseOnece = true;
            if ((!_animator.GetBool("Death")) && isDamage)
            {
                isDamage = false;
                PlayerController.Instance.PlayerTakeDamage(15);
                Invoke(nameof(WaitForNextDamage), 1);
            }
        }
        else
        {
            if (isCloseOnece)
                isCloseOnece = false;
        }
    }

    private void WaitForNextDamage()
    {
        isDamage = true;
    }

    public void TakeDamage()
    {
        currentHelth -= fishDamage;

        if(currentHelth <= 0)
        {
            if (!_animator.GetBool("Death"))
            {
                isStopSwim = true;
                _animator.SetBool("Death", true);
                UnderWaterGamePlayManager.Instance.SharkDeath();
                Invoke(nameof(AfterDeathDestroyFish), 1.5f);
                Debug.Log("Fish Die");
            }
        }
        healthBar.DOFillAmount(currentHelth, 0.5f);
    }

    private void AfterDeathDestroyFish()
    {
       Destroy(gameObject);
    }
    #endregion
}
