using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;
using VRFPSKit;

public class BlueGameManager : MonoBehaviour
{
    #region Variables

    public static BlueGameManager Instance { get; private set; }

    [SerializeField] private GameObject beachObj;
    [SerializeField] private GameObject boatRide;
    [SerializeField] private GameObject boatFight;
    [SerializeField] private Material skyBox;
    [SerializeField] private ContinuousMoveProvider continuousMove;
    [SerializeField] private ContinuousTurnProvider continuousTurn;
    [SerializeField] private GameObject gravityObj;
    [SerializeField] private GameObject jumpObj;
    [SerializeField] private Transform beachPos;
    [SerializeField] private Transform playerTransform;
    public Image blackScreen;


    private GameObject tmpBoatRideObj;

    public bool isBoatRidingCompleted;

    public PlayerAIBoat playerAIBoat;

    #endregion

    #region Unity_Callback

    private void Awake()
    {
        Instance = this;

        //PlayerPrefs.SetInt("BlueGameCompletedLevel", 1);

        RenderSettings.skybox = skyBox;
    }

    private void OnEnable()
    {
        CameraPathMover.OnPathCompleted += WaitForRinging;
    }

    private void OnDisable()
    {
        CameraPathMover.OnPathCompleted -= WaitForRinging;
    }

    private void Start()
    {
        switch (PlayerPrefs.GetInt("BlueGameCompletedLevel"))
        {
            case 0:
                //BlueGameUIManager.Instance.startingScreen.Show();
                //BlueGameUIManager.Instance.settingbtn.SetActive(false);
                BlueGameUIManager.Instance.StartGame();
                break;
            case 1:
                OnBoatRide();
                break;
            case 9:
                OnBoatFight();
                break;
        }
    }

    #endregion

    #region Level_Controll

    private void WaitForRinging()
    {
        blackScreen.DOFade(1, 1).OnComplete(() =>
        {
            playerTransform.SetParent(null);
            playerTransform.SetPositionAndRotation(beachPos.position, beachPos.rotation);
            gravityObj.SetActive(true);
            jumpObj.SetActive(true);
            blackScreen.DOFade(0, 0.5f).OnComplete(() =>
            {
                BlueGameUIManager.Instance.OnCallRinging();
            });
        });
    }

    public void PlayerMovement(bool isActive)
    {
        continuousMove.enabled = isActive;
        continuousTurn.enabled = isActive;
    }

    public void OnBoatRideActive()
    {
        blackScreen.DOFade(1, 1).OnComplete(OnBoatRide);
    }

    private void OnBoatFight()
    {
        GameObject vThirdPersonCamera = GameObject.Find("ThirdPersonCamera");
        GameObject vInputType = GameObject.Find("vInputType");
        GameObject MousePositionHandler = GameObject.Find("MousePositionHandler");
        GameObject objectContainer = GameObject.Find("Object Container");

        Destroy(beachObj);

        if (vThirdPersonCamera != null)
            Destroy(vThirdPersonCamera);
        if (vInputType != null)
            Destroy(vInputType);
        if (MousePositionHandler != null)
            Destroy(MousePositionHandler);
        if (objectContainer != null)
            Destroy(objectContainer);

        GameObject go = Instantiate(boatFight);
        playerAIBoat = go.GetComponentInChildren<PlayerAIBoat>();
    }

    private void OnBoatRide()
    {
        PlayerPrefs.SetInt("BlueGameCompletedLevel", 1);

        Destroy(beachObj);

        tmpBoatRideObj = Instantiate(boatRide);

        PlayerMovement(false);

        BoatAssignToPlayer BAP = tmpBoatRideObj.GetComponent<BoatAssignToPlayer>();

        BAP.SetPlayer(playerTransform, playerTransform.gameObject.GetComponent<Damageable>());

        blackScreen.DOFade(0, 1);
    }

    [ContextMenu("OnBoatRidingComplete")]
    public void OnBoatRidingComplete()
    {
        blackScreen.DOFade(1, 1).OnComplete(() =>
        {
            Destroy(tmpBoatRideObj);
            PlayerPrefs.SetInt("BlueGameCompletedLevel", 2);
            SceneManager.LoadSceneAsync("BlueUnderWaterGamePlay");
        });
    }

    #endregion
}
