using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    #region Variables

    private float speed;
    private GameObject targetEnemy;
    [SerializeField] private float hitDistance = 1.5f;
    private PlayerAIBoat playerAIBoat;

    #endregion

    #region Unity_Callback 

    void Update()
    {
        Vector3 dir = (targetEnemy.transform.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;

        float distance = Vector3.Distance(transform.position, targetEnemy.transform.position);
        if (distance < hitDistance)
        {
            HitEnemy();
        }
    }

    #endregion

    #region Bullet_&_Damage

    public void Initialize(float spd, PlayerAIBoat aiBoat)
    {
        speed = spd;
        targetEnemy = aiBoat.gameObject;
        playerAIBoat = aiBoat;
        Invoke(nameof(AfterSomeTimeDestroyMe), 10);
    }

    private void HitEnemy()
    {
        Debug.Log("HitEnemy");
        playerAIBoat.TakeDamage();
        Destroy(gameObject);
    }

    private void AfterSomeTimeDestroyMe()
    {
        Destroy(gameObject);
    }

    #endregion
}
