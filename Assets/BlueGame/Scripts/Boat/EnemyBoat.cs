using System.Collections;
using System.Collections.Generic;
using BoatAttack;
using UnityEngine;
using UnityEngine.UI;
using VRFPSKit;

public class EnemyBoat : MonoBehaviour
{
    #region Variables

    public Engine engine;

    private float _throttle;
    private float _steering;
    [SerializeField] private Image healthFillbar;
    [SerializeField] private Animator _animator;
    

    public List<Transform> aiBoatCheckPoint = new();
    public PlayerAIBoat playerBoat;
    public int currentIndex = 0;
    public bool startEngin;
    public bool isShooting;
    [SerializeField] private float checkpointReachDistance;

    [SerializeField] private GameObject bulletPre;
    [SerializeField] private Transform bulletspawnPoint;

    [Header("Particle System")]
    [SerializeField] private ParticleSystem sparksParticle;
    [SerializeField] private ParticleSystem health75Particle;
    [SerializeField] private ParticleSystem health50Particle;
    [SerializeField] private ParticleSystem health25Particle;
    [SerializeField] private List<ParticleSystem> shootParticle;

    #endregion

    #region Unity_Callback

    private void OnEnable()
    {
        Bullet.EnemyDamage += TakeDamage;
    }

    private void OnDisable()
    {
        Bullet.EnemyDamage -= TakeDamage;
    }

    void FixedUpdate()
    {
        if (!startEngin)
            return;

        Transform target = aiBoatCheckPoint[currentIndex];

        _throttle = 1f;

        Vector3 localTarget = transform.InverseTransformPoint(target.position);
        float steerDir = localTarget.x / localTarget.magnitude;
        _steering = Mathf.Clamp(steerDir, -1f, 1f);

        engine.Accelerate(_throttle);
        engine.Turn(_steering);

        float distance = Vector3.Distance(transform.position, target.position);
        if (distance < checkpointReachDistance)
        {
            currentIndex++;

            if (currentIndex >= aiBoatCheckPoint.Count)
                currentIndex = 0;
        }
    }

    #endregion

    #region Shooting_&_Damange

    public void ShootingStart()
    {
        StartCoroutine(ShootBullet());
    }

    private IEnumerator ShootBullet()
    {
        while (true)
        {
            EnemyBulletController enemyBullet =  Instantiate(bulletPre, bulletspawnPoint.position, bulletspawnPoint.rotation).GetComponent<EnemyBulletController>();

            //Vector3 direction = (playerBoat.transform.position - bulletspawnPoint.position).normalized;

            Debug.Log("Shoot Bullet HitEnemy");

            enemyBullet.Initialize(60, playerBoat);

            for (int i = 0; i < shootParticle.Count; i++)
            {
                shootParticle[i].Play();
            }
            yield return new WaitForSeconds(Random.Range(4, 8));
        }
    } 

    public void TakeDamage(GameObject go)
    {
        if (go == gameObject)
        {
            healthFillbar.fillAmount -= 0.1f;

            if (healthFillbar.fillAmount <= 0)
            {
                if (!_animator.GetBool("Death"))
                {
                    startEngin = false;
                    _animator.SetBool("Death", true);
                    Invoke(nameof(AfterSomeTimeDestroyMe), 2);
                    playerBoat.EnemySpawn();
                    sparksParticle.Play();
                }
            }

            if (healthFillbar.fillAmount > 0.5f && healthFillbar.fillAmount < 0.75f)
            {
                health75Particle.Play();
            }
            if (healthFillbar.fillAmount > 0.25f && healthFillbar.fillAmount < 0.50f)
            {
                health75Particle.Stop();
                health50Particle.Play();
            }
            if (healthFillbar.fillAmount < 0.25f)
            {
                health75Particle.Stop();
                health50Particle.Stop();
                health25Particle.Play();
            }
        }
    }

    private void AfterSomeTimeDestroyMe()
    {
        Destroy(gameObject);
    }

    #endregion
}