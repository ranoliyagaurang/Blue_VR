using UnityEngine;

public class GunArrowController : MonoBehaviour
{
    #region Variables

    private Rigidbody rb;
    [SerializeField] private GameObject damageParticle;

    #endregion

    #region Unity_Callback

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Invoke(nameof(DestroyArrow), 7);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Instantiate(damageParticle, transform.position, Quaternion.identity);
            
            AISharkController ai = collision.gameObject.GetComponent<AISharkController>();
            if (ai != null)
            {
                transform.SetParent(ai.transform);
                ai.TakeDamage();
            }

            OctopusHealthController octopusHealth = collision.gameObject.GetComponent<OctopusHealthController>();
            if (octopusHealth != null)
            {
                octopusHealth.TakeDamage();
            }

            rb.linearVelocity = Vector3.zero;
            Invoke(nameof(DestroyArrow), 0.5f);
        }
        else
        {
            DestroyArrow();
        }
    }

    private void DestroyArrow()
    {
        Destroy(gameObject);
    }

    #endregion
}
