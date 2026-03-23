using UnityEngine;

public class BlueEnemyShootingController : MonoBehaviour
{
    #region Variables

    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float bulletSpeed = 50f;
    [SerializeField] private ParticleSystem shootParticle;

    #endregion

    #region Unity_Callback

    void Update()
    {

#if UNITY_EDITOR || UNITY_WEBGL

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
#endif

    }

    #endregion

    #region Shooting 

    public void Shoot()
    {
        if (BlueGameSoundManager.Instance != null)
            BlueGameSoundManager.Instance.OnGunShoot();

        shootParticle.Play();
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.LookRotation(ray.direction));

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = ray.direction * bulletSpeed;
    }

    #endregion
}
