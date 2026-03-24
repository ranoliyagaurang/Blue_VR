using UnityEngine;
using VRFPSKit;

public class Boat_RockDamage : MonoBehaviour
{
    #region Variables

    public Damageable damageable;

    [SerializeField] private ParticleSystem sparksParticle;
    [SerializeField] private ParticleSystem health75Particle;
    [SerializeField] private ParticleSystem health50Particle;
    [SerializeField] private ParticleSystem health25Particle;

    #endregion

    #region Unity_Callback

    private void OnCollisionStay(Collision collision)
    {
        if (BlueGameManager.Instance.isBoatRidingCompleted)
            return;

        if (collision.gameObject.CompareTag("Rock"))
        {
            Debug.Log("Collide with obstacle");
            damageable.health -= 1;

            if (damageable.health <= 0)
            {
                BlueGameUIManager.LevelFailked?.Invoke();
            }

            if(damageable.health > 50 && damageable.health < 75)
            {
                health75Particle.Play();
            }
            if (damageable.health > 25 && damageable.health < 50)
            {
                health75Particle.Stop();
                health50Particle.Play();
            }
            if (damageable.health < 25f)
            {
                health75Particle.Stop();
                health50Particle.Stop();
                health25Particle.Play();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Rock"))
        {
            sparksParticle.Play();
        }
    }

    #endregion
}
