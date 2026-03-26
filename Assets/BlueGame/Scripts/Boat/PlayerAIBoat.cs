using System.Collections;
using System.Collections.Generic;
using BoatAttack;
using DG.Tweening;
using UnityEngine;
using VRFPSKit;

public class PlayerAIBoat : MonoBehaviour
{
    #region Variables

    public Damageable playerHealth;
    [SerializeField] private GameObject akChar;
    [SerializeField] private GameObject treasure;
    [SerializeField] private Transform treasureDoor;
    [SerializeField] private PlayerAssignToAIBoat playerAssignToAI;

    public Engine engine;

    public float _throttle;
    public float _steering;

    [SerializeField] private List<Transform> aiPlayerBoatCheckPoint = new();
    [SerializeField] private float checkpointReachDistance = 3f;
    private int currentIndex = 0;

    [SerializeField] private Transform skybox;

    [Header("Particle System")]
    [SerializeField] private ParticleSystem health75Particle;
    [SerializeField] private ParticleSystem health50Particle;
    [SerializeField] private ParticleSystem health25Particle;

    [Header("Enemy")]
    [SerializeField] private GameObject enemy1Pref;
    [SerializeField] private GameObject enemy2Pref;
    private Transform enemyBoat;
    [SerializeField] private float enemyDistance;
    private EnemyBoat eb;

    [Header("Beach")]
    [SerializeField] private GameObject beach;
    [SerializeField] private GameObject bigStone;
    [SerializeField] private Transform beachPoint;
    private bool isBeach = false;
    private bool isReachBeach = false;

    [Header("Enviornment")]
    [SerializeField] private MeshRenderer skyMesh;
    [SerializeField] private MeshRenderer envMesh;
    [SerializeField] private Material skyMaterial;
    [SerializeField] private Material envMaterial;


    private int enemyCount = 0;

    #endregion

    #region Unity_Callback

    private void Start()
    {
        BlueGameUIManager.Instance.ShowBoatFightIns(this);
    }

    void FixedUpdate()
    {
        SkyboxPosSet();

        if (isBeach)
        {
            if (isReachBeach)
            {
                _throttle = 0f;
                _steering = 0f;
                engine.Accelerate(_throttle);
                engine.Turn(_steering);
                return;
            }

            engine.horsePower = 30;
            _throttle = 1f;
            Vector3 beachTarget = transform.InverseTransformPoint(beachPoint.position);
            float steerDirbeach = beachTarget.x / beachTarget.magnitude;
            _steering = Mathf.Clamp(steerDirbeach, -1f, 1f);

            engine.Accelerate(_throttle);
            engine.Turn(_steering);

            float beachDistance = Vector3.Distance(transform.position, beachPoint.position);
            if (beachDistance < checkpointReachDistance)
            {
                isReachBeach = true;
                IsGameComplete();
            }
            return;
        }

        if (enemyBoat != null)
        {
            float edDistance = Vector3.Distance(transform.position, enemyBoat.position);

            if (edDistance <= 30)
            {
                if (eb != null && !eb.isShooting)
                {
                    eb.isShooting = true;
                    eb.ShootingStart();
                }
            }

            if (edDistance <= enemyDistance)
            {
                engine.horsePower = 30;
            }
            else
            {
                engine.horsePower = 15;
            }
        }

        Transform target = aiPlayerBoatCheckPoint[currentIndex];

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

            if (currentIndex >= aiPlayerBoatCheckPoint.Count)
                currentIndex = 0;
        }
    }

    #endregion

    #region Player_Boat_Logic

    private IEnumerator RotateToX(Transform obj, Vector3 targetEuler, float duration)
    {
        Quaternion startRot = obj.localRotation;
        Quaternion targetRot = Quaternion.Euler(targetEuler);

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            obj.localRotation = Quaternion.Slerp(startRot, targetRot, t);

            yield return null;
        }

        obj.localRotation = targetRot;
    }

    private bool isEnemy1;
    public void EnemySpawn()
    {
        enemyCount++;

        if(enemyCount > 4)
        {
            BlueGameManager.Instance.blackScreen.DOFade(1, 1f).OnComplete(() =>
            {
                CutScene();
                playerAssignToAI.FightComplete();
            });
            return;
        }

        GameObject go;
        if (isEnemy1)
        {
            go = enemy1Pref;
            isEnemy1 = false;
        }
        else
        {
            go = enemy2Pref;
            isEnemy1 = true;
        }

        Vector3 spawnPos = transform.position - transform.forward * 60f;

        eb = Instantiate(go, spawnPos, transform.rotation).GetComponent<EnemyBoat>();

        eb.aiBoatCheckPoint = aiPlayerBoatCheckPoint;
        eb.currentIndex = currentIndex;
        eb.playerBoat = this;
        eb.startEngin = true;
        enemyBoat = eb.transform;
    }

    private void CutScene()
    {
        beach.SetActive(true);
        bigStone.SetActive(false);
        akChar.SetActive(true);
        treasure.SetActive(true);
        RenderSettings.skybox = null;
        skyMesh.material = skyMaterial;
        envMesh.material = envMaterial;
        StartCoroutine(RotateToX(treasureDoor, new Vector3(120, 0, 0), 2));
        Invoke(nameof(ShowIns), 9);
        isBeach = true;
        BlueGameManager.Instance.blackScreen.DOFade(0, 0.5f);
    }

    private void ShowIns()
    {
        BlueGameUIManager.Instance.ShowInstruction("Congratulations! You’ve defeated all the enemies and claimed the Lady Blue’s Treasure.\r\nYour journey was worth it!");
    }

    public void IsGameComplete()
    {
        BlueGameUIManager.Instance.OnInstructionClick();
        BlueGameUIManager.Instance.OnCompleteUI();
        Invoke(nameof(GoToMainGame), 1f);
    }
    private void GoToMainGame()
    {
        PlayerPrefs.SetInt("BlueGameCompletedLevel", 0);
    }

    private void SkyboxPosSet()
    {
        skybox.position = transform.position;
    }

    public void TakeDamage()
    {
        playerHealth.health -= 5f;

        if (playerHealth.health <= 0)
        {
            BlueGameUIManager.LevelFailked?.Invoke();
        }

        if (playerHealth.health > 50f && playerHealth.health < 75f)
        {
            health75Particle.Play();
        }
        if (playerHealth.health > 25f && playerHealth.health < 50f)
        {
            health75Particle.Stop();
            health50Particle.Play();
        }
        if (playerHealth.health < 25f)
        {
            health75Particle.Stop();
            health50Particle.Stop();
            health25Particle.Play();
        }
    }

    #endregion
}

