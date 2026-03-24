using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntingController : MonoBehaviour
{
    private int fishCount;
    private readonly float spawnRadius = 45f;
    private readonly float minSpawnDistance = 25f;
    private readonly int spawnMinWait = 4;
    private readonly int spawnMaxWait = 8;
    private Coroutine fishSpawnCoroutine;

    [SerializeField] private Transform playerTransf;
    [SerializeField] private List<GameObject> fishPrefList = new();
    private List<AISharkController> fishList = new();

    #region Unity_Callback

    private void Start()
    {
        PlayerController.Instance.playerHealth.DeathEvent += OnPlayerDead;
    }

    private void OnDestroy()
    {
        PlayerController.Instance.playerHealth.DeathEvent -= OnPlayerDead;
    }

    #endregion

    #region Enemy_Spawn

    public void SpawnFishForHunting(int fc)
    {
        fishCount = fc;
        fishSpawnCoroutine = StartCoroutine(SpawnFish());
    }

    IEnumerator SpawnFish()
    {
        fishList.Clear();
        for (int i = 0; i < fishCount; i++)
        {
            Vector3 randomPos;

            do
            {
                randomPos = playerTransf.position + Random.insideUnitSphere * spawnRadius;
                randomPos.y = playerTransf.position.y;
            }
            while (Vector3.Distance(randomPos, playerTransf.position) < minSpawnDistance);

            GameObject fish = Instantiate(fishPrefList[Random.Range(0, fishPrefList.Count)], randomPos, Quaternion.identity);

            AISharkController sharkAI = fish.GetComponent<AISharkController>();

            fishList.Add(sharkAI);

            if (sharkAI != null)
            {
                sharkAI.SetTarget(playerTransf);
            }

            yield return new WaitForSeconds(Random.Range(spawnMinWait, spawnMaxWait));
        }
    }

    public void OnPlayerDead()
    {
        if (fishSpawnCoroutine != null)
            StopCoroutine(fishSpawnCoroutine);

        for (int i = 0; i < fishList.Count; i++)
        {
            if (fishList[i] != null)
            {
                fishList[i].SetTarget(UnderWaterGamePlayManager.Instance.chestKey.transform);
            }
        }
    }

    #endregion
}
