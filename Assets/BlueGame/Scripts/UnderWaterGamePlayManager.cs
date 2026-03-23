using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UnderWaterGamePlayManager : MonoBehaviour
{
    #region Variables

    public static UnderWaterGamePlayManager Instance { get; private set; }

    [Space(2)]
    [Header("LocationMaker")]
    public DirectionArrow directionArrow;
    [SerializeField] private Transform PlayerTran;

    [Space(2)]
    [Header("PlayerData")]
    [SerializeField] private GameObject mainPlayer;
    [SerializeField] private Canvas playerCanvas;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private AudioSource swimmingSound;

    [Space(2)]
    [Header("LevelData")]
    public CurrentActivity currentActivity;
    public GameObject tunnleGate;
    [SerializeField] private Transform tunnelGateMovableTran;
    [SerializeField] private Transform targetTran;

    [Space(2)]
    [Header("CheckPoint")]
    public CheckPoint currentCheckPonit;

    [Space(2)]
    [Header("Puzzle")]
    public Transform sokobanPuzzleParent;
    public GameObject sokobanPuzzle;
    [SerializeField] private Sprite oxygenCylinderSprite;
    [SerializeField] private GameObject oxygenCylinder;
    private GameObject tmpPuzzle;

    [Space(2)]
    [Header("ShipData")]
    [SerializeField] private GameObject shipObj;
    [SerializeField] private GameObject shipMainDoor;
    [SerializeField] private Transform shipMainDoorTrnf;
    [SerializeField] private Transform shipMainDoorRotatorTrnf;
    [SerializeField] private GameObject chestObj;
    [SerializeField] private Transform keyAnimTranf;
    [SerializeField] private Transform chestDoorTranf;
    [SerializeField] private ParticleSystem treasureParticle;
    public GameObject chestKey;
    public GameObject chestKeyIntract;
    public bool isKeyFound;

    [Space(2)]
    [Header("CheckPoints")]
    public GameObject underwaterExplorationObj;
    public GameObject shipPathCheckPointObj;
    public GameObject shipExploreCheckPointObj;

    [Space(2)]
    [Header("Shooting")]
    [SerializeField] private HuntingController huntingController;

    [Space(2)]
    [Header("Octopus")]
    public GameObject octopus;
    public GameObject octopusSpotArea;
    public GameObject OctopusActivator;

    [SerializeField] private Transform fishParticle;

    #endregion

    #region Swimming

    public void PlaySwimmingSound()
    {
        if (!swimmingSound.isPlaying)
        {
            swimmingSound.Play();
        }
    }

    public void StopSwimmingSound()
    {
        if (swimmingSound.isPlaying)
        {
            swimmingSound.Stop();
        }
    }

    #endregion

    #region Unity_Callback

    private void Awake()
    {
        Instance = this;
        //StartCoroutine(UpdateFishPos());
    }

    private void Start()
    {
        PlayerPrefs.SetInt("BlueGameCompletedLevel", 7);
        SetLevelData();
    }

    #endregion

    #region FishHunting

    private int fishDeathCount = 0;
    private int targetCount = 6;
    private GameObject nextCheckPoint;

    public void SetFishHunting(int targetFishCount, GameObject nextPoint)
    {
        huntingController.SpawnFishForHunting(targetFishCount);
        nextCheckPoint = nextPoint;
        fishDeathCount = 0;
        targetCount = targetFishCount;
    }

    public void SharkDeath()
    {
        fishDeathCount++;

        if(fishDeathCount >= targetCount)
        {
            nextCheckPoint.SetActive(true);
            directionArrow.SetTarget(nextCheckPoint.transform);
        }
    }

    #endregion

    #region Puzzle

    private void SetLevelData()
    {
        Debug.Log("BlueGameCompletedLevel : " + PlayerPrefs.GetInt("BlueGameCompletedLevel"));

        switch (PlayerPrefs.GetInt("BlueGameCompletedLevel"))
        {
            case 0:
                BlueGameUnderWaterUIManager.Instance.missionFailedScreen.OnNewGameClick();
                break;
            case 1:
                BlueGameUnderWaterUIManager.Instance.missionFailedScreen.OnNewGameClick();
                break;
            case 2:
                underwaterExplorationObj.SetActive(true);
                directionArrow.SetTarget(underwaterExplorationObj.transform.GetChild(0));
                currentCheckPonit = CheckPoint.ExploreDeepSea;
                BlueGameUnderWaterUIManager.Instance.ShowInstruction("Collect the checkpoint to begin your deep underwater exploration.\r\nBut be careful—the Shark and Tylosaurus fish have spotted you and they’re closing in fast! Grab your weapon, fight back.\r\nShoot them down one by one to survive this deadly attack!");
                break;
            case 3:
                currentActivity = CurrentActivity.GotoTunnle;
                PlayerTran.position = new Vector3(-64, 3, -33);
                PlayerTran.rotation = Quaternion.Euler(0, 180, 0);
                BlueGameUnderWaterUIManager.Instance.ShowInstruction("Your oxygen cylinder level is critically low. Follow the arrow to find a new one.");
                break;
            case 4:
                currentActivity = CurrentActivity.DoorOpen;
                PlayerController.Instance.OxygenHealth();
                BlueGameUnderWaterUIManager.Instance.ShowItemInstruction("Hurry up! Your oxygen level is critically low. Follow the arrow to reach new oxygen cylinder inside the tunnel.", oxygenCylinderSprite);
                PlayerTran.position = new Vector3(-64, 3, -33);
                PlayerTran.rotation = Quaternion.Euler(0, 180, 0);
                directionArrow.SetTarget(oxygenCylinder.transform);
                break;
            case 5:
                currentActivity = CurrentActivity.FindShipBySaveLevel;
                PlayerTran.position = new Vector3(-65.5f, 14, -6);
                PlayerTran.rotation = Quaternion.identity;
                BlueGameUnderWaterUIManager.Instance.ShowInstruction("Follow the arrow, collect all checkpoints, and reveal the path to the legendary treasure ship.\r\nBut be careful—the Shark and Tylosaurus fish have spotted you and they’re closing in fast! Grab your weapon, fight back.\r\nShoot them down one by one to survive this deadly attack!");
                break;
            case 6:
                shipObj.SetActive(true);
                currentActivity = CurrentActivity.FindShipBySaveLevel;
                PlayerTran.position = new Vector3(-244, 20, 25);
                PlayerTran.rotation = Quaternion.Euler(0, -130, 0);
                currentActivity = CurrentActivity.ExploreShip;
                BlueGameUnderWaterUIManager.Instance.ShowInstruction("You’ve reached the shipwreck! Follow the arrow and collect all checkpoints to explore the treasure ship.\r\nBut be careful—the Shark and Tylosaurus fish have spotted you and they’re closing in fast! Grab your weapon, fight back.\r\nShoot them down one by one to survive this deadly attack!");
                break;
            case 7:
                shipObj.SetActive(true);
                PlayerTran.position = new Vector3(-244, 20, 25);
                PlayerTran.rotation = Quaternion.Euler(0, -130, 0);
                currentActivity = CurrentActivity.ShipMainDoor;
                BlueGameUnderWaterUIManager.Instance.ShowInstruction("Follow the arrow to reach the main entrance of the ship.");
                break;
            case 8:
                shipObj.SetActive(true);
                PlayerTran.position = new Vector3(-244, 20, 25);
                PlayerTran.rotation = Quaternion.Euler(0, -130, 0);
                BlueGameUnderWaterUIManager.Instance.ShowInstruction("Follow the arrow to reach the ship treasure.");
                directionArrow.SetTarget(chestObj.transform);
                ShipDoorOpen();
                break;
        }
    }

    public void SokobanPuzzleLoad()
    {
        BlueGameUnderWaterUIManager.Instance.blackScreen.DOFade(1, 0.5f).OnComplete(() =>
        {
            tunnleGate.SetActive(false);
            PlayerTran.SetPositionAndRotation(new Vector3(-65, 2.4f, -40), Quaternion.Euler(new Vector3(0, 190, 0)));
            tmpPuzzle = Instantiate(sokobanPuzzle, sokobanPuzzleParent);
            tmpPuzzle.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            BlueGameUnderWaterUIManager.Instance.blackScreen.DOFade(0, 0.5f);
        });
    }

    public void OnCompletedSokobanPuzzle()
    {
        Debug.Log("OnCompletedSokobanPuzzle");
        Destroy(tmpPuzzle);
        BlueGameUnderWaterUIManager.Instance.blackScreen.DOFade(0, 0.6f);
        Invoke(nameof(OxygenTunnelInsruction), 1f);
    }

    private void OxygenTunnelInsruction()
    {
        PlayerPrefs.SetInt("BlueGameCompletedLevel", 4);
        currentActivity = CurrentActivity.DoorOpen;
        PlayerController.Instance.OxygenHealth();
        BlueGameUnderWaterUIManager.Instance.ShowItemInstruction("Hurry up! Your oxygen level is critically low. Find a new oxygen cylinder inside the tunnel.", oxygenCylinderSprite);
    }

    public void OnTunnelDoorOpen()
    {
        oxygenCylinder.SetActive(true);
        StartCoroutine(MoveSmoothly(targetTran.position, 4));
    }

    public void PickUpOxygenCylinder()
    {
        oxygenCylinder.SetActive(false);

        BlueGameUnderWaterUIManager.Instance.blackScreen.DOFade(1, 0.7f).OnComplete(() =>
        {
            ShowShipFindInstruction();
            BlueGameUnderWaterUIManager.Instance.blackScreen.DOFade(0, 0.5f);
        });
    }

    private void ShowShipFindInstruction()
    {
        currentActivity = CurrentActivity.FindShip;
        BlueGameUnderWaterUIManager.Instance.ShowInstruction("Oxygen restored! Follow the arrow, collect all checkpoints, and reveal the path to the legendary treasure ship.\r\nBut be careful—the Shark and Tylosaurus fish have spotted you and they’re closing in fast! Grab your weapon, fight back.\r\nShoot them down one by one to survive this deadly attack!");
    }

    public void ShowShipPath()
    {
        shipObj.SetActive(true);
        shipPathCheckPointObj.SetActive(true);
        directionArrow.SetTarget(shipPathCheckPointObj.transform.GetChild(0));
        currentCheckPonit = CheckPoint.FindShip;
    }

    public void ShowShipPathBySaveData()
    {
        shipObj.SetActive(true);
        shipPathCheckPointObj.SetActive(true);
        shipPathCheckPointObj.transform.GetChild(0).gameObject.SetActive(false);
        shipPathCheckPointObj.transform.GetChild(3).gameObject.SetActive(true);
        directionArrow.SetTarget(shipPathCheckPointObj.transform.GetChild(3));
        currentCheckPonit = CheckPoint.FindShip;
    }

    public void ExploreShip()
    {
        shipObj.SetActive(true);
        shipPathCheckPointObj.SetActive(false);
        shipExploreCheckPointObj.SetActive(true);
        directionArrow.SetTarget(shipExploreCheckPointObj.transform.GetChild(0));
        currentCheckPonit = CheckPoint.ExploreShip;
    }

    public void GoToTheShipMainDoor()
    {
        shipMainDoor.SetActive(true);
        directionArrow.SetTarget(shipMainDoor.transform);
    }

    public void ShipDoorOpen()
    {
        chestObj.SetActive(true);
        shipMainDoor.SetActive(false);
        StartCoroutine(RotateObjectsSequentially());
    }

    public void OpenChestKeyAnim()
    {
        keyAnimTranf.gameObject.SetActive(true);
        StartCoroutine(RotateBackAndForthX());
    }

    public void OpenTheChest()
    {
        PlayerPrefs.SetInt("BlueGameCompletedLevel", 0);
        StartCoroutine(ChestOpen());
    }

    private void ShowCongratulationsMessage()
    {
        PlayerPrefs.SetInt("BlueGameCompletedLevel", 9);
        currentActivity = CurrentActivity.TakeTreasure;
        BlueGameUnderWaterUIManager.Instance.ShowInstruction("Congratulations! You’ve found Lady Blue’s Treasure. Now take the treasure to the sea surface — Sagar is waiting for you right above.");
    }

    #region IEnumerator

    private IEnumerator MoveSmoothly(Vector3 target, float duration)
    {
        Vector3 start = tunnelGateMovableTran.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            tunnelGateMovableTran.position = Vector3.Lerp(start, target, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        tunnelGateMovableTran.position = target;

        tunnelGateMovableTran.gameObject.SetActive(false);
    }

    private IEnumerator UpdateFishPos()
    {
        while(true)
        {
            fishParticle.position = new Vector3(mainPlayer.transform.position.x, 8, mainPlayer.transform.position.z);
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator ChestOpen()
    {
        yield return RotateX(keyAnimTranf, 360, 150);

        treasureParticle.Play();

        yield return RotateX(chestDoorTranf, 110, 100);

        Invoke(nameof(ShowCongratulationsMessage), 3);
    }

    private IEnumerator RotateBackAndForthX()
    {
        Vector3 originalEuler = keyAnimTranf.localEulerAngles;
        Vector3 targetEuler = new Vector3(-45, originalEuler.y, originalEuler.z);

        for (int i = 0; i < 3; i++)
        {
            yield return RotateToX(keyAnimTranf, targetEuler, 0.5f);
            yield return RotateToX(keyAnimTranf, originalEuler, 0.5f);
        }
        currentActivity = CurrentActivity.ChestPuzzle;
        BlueGameUnderWaterUIManager.Instance.ShowInstruction("The chest is stuck due to a jammed lock. Solve the gear rotator puzzle to repair the lock and open the chest.");
    }

    private IEnumerator RotateToX(Transform obj, Vector3 targetEuler, float time)
    {
        Vector3 startEuler = obj.localEulerAngles;

        // Convert to signed angles to handle negative rotation correctly
        startEuler.x = NormalizeAngle(startEuler.x);
        targetEuler.x = NormalizeAngle(targetEuler.x);

        float elapsed = 0f;

        while (elapsed < time)
        {
            float t = elapsed / time;
            float x = Mathf.Lerp(startEuler.x, targetEuler.x, t);
            obj.localEulerAngles = new Vector3(x, startEuler.y, startEuler.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        obj.localEulerAngles = new Vector3(targetEuler.x, startEuler.y, startEuler.z);
    }

    private float NormalizeAngle(float angle)
    {
        return (angle > 180) ? angle - 360 : angle;
    }

    private IEnumerator RotateObjectsSequentially()
    {
        yield return RotateY(shipMainDoorRotatorTrnf, 720);

        yield return RotateZ(shipMainDoorTrnf, 90);

        Debug.Log("Ship Main Door Open");
    }

    private IEnumerator RotateX(Transform target, float targetAngle, float speed)
    {
        float rotated = 0f;
        while (rotated < targetAngle)
        {
            float step = speed * Time.deltaTime;
            float rotateNow = Mathf.Min(step, targetAngle - rotated);

            target.Rotate(rotateNow, 0, 0);
            rotated += rotateNow;

            yield return null;
        }
    }

    private IEnumerator RotateY(Transform target, float targetAngle)
    {
        float rotated = 0f;
        while (rotated < targetAngle)
        {
            float step = 300 * Time.deltaTime;
            float rotateNow = Mathf.Min(step, targetAngle - rotated);

            target.Rotate(0, rotateNow, 0);
            rotated += rotateNow;

            yield return null;
        }
    }
    private IEnumerator RotateZ(Transform target, float targetAngle)
    {
        float rotated = 0f;
        while (rotated < targetAngle)
        {
            float step = 180 * Time.deltaTime;
            float rotateNow = Mathf.Min(step, targetAngle - rotated);

            target.Rotate(0, 0, rotateNow);
            rotated += rotateNow;

            yield return null;
        }
    }

    #endregion

    #endregion
}

public enum CurrentActivity
{
    None,
    GotoTunnle,
    TunnelDoorPuzzle,
    DoorOpen,
    OxygenCylinder,
    FindShip,
    FindShipBySaveLevel,
    ExploreShip,
    ShipMainDoor,
    ChestKey,
    ChestPuzzle,
    ChestOpen,
    TakeTreasure,
    Trap,
    OctopusTrap
}

public enum CheckPoint
{
    ExploreDeepSea,
    FindShip,
    ExploreShip
}
