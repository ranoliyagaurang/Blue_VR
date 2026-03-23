using UnityEngine;

public class BlueGameDolphin : MonoBehaviour
{
    #region Variables

    [SerializeField] private Transform player;
    [SerializeField] private float swimRadius;
    [SerializeField] private float swimSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float targetChangeInterval;

    private Vector3 targetPos;
    private float timer;

    #endregion

    #region Unity_Callback

    void Start()
    {
        PickNewTarget();
    }

    void Update()
    {
        if (!player) return;

        timer -= Time.deltaTime;
        if (timer <= 0f || Vector3.Distance(transform.position, targetPos) < 1f)
        {
            PickNewTarget();
        }

        Vector3 dir = (targetPos - transform.position).normalized;
        transform.forward = Vector3.Slerp(transform.forward, dir, turnSpeed * Time.deltaTime);
        transform.position += transform.forward * swimSpeed * Time.deltaTime;
    }

    #endregion

    #region PickRandomTarget

    void PickNewTarget()
    {
        timer = targetChangeInterval + Random.Range(-1f, 1f);

        Vector3 randomDir = Random.insideUnitSphere;
        randomDir.y = 0f;
        randomDir.Normalize();

        targetPos = player.position + randomDir * Random.Range(swimRadius * 0.5f, swimRadius);
    }

    #endregion
}
