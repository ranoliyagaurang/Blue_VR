using DG.Tweening;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
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
    [SerializeField] private ContinuousMoveProvider continuousMove;
    [SerializeField] private ContinuousTurnProvider continuousTurn;
    [SerializeField] private GameObject gravityObj;
    [SerializeField] private GameObject jumpObj;
    [SerializeField] private Transform beachPos;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject leftHandRay;
    [SerializeField] private GameObject rightHandRay;
    [SerializeField] private GameObject vest;
    [SerializeField] private XRSpawnWithInteractable xRSpawnWithInteractable;
    public Image blackScreen;


    private GameObject tmpBoatRideObj;

    private bool pistolDrop;

    public bool isBoatRidingCompleted;

    public PlayerAIBoat playerAIBoat;

    #endregion

    #region Unity_Callback

    private void Awake()
    {
        Instance = this;

        PlayerPrefs.SetInt("BlueGameCompletedLevel", 0);
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
        int level = PlayerPrefs.GetInt("BlueGameCompletedLevel");

        if (level == 0)
            BlueGameUIManager.Instance.StartGame();
        else if (level == 1)
            OnBoatRide();
        else if (level == 9)
            OnBoatFight();
        else
            SceneManager.LoadSceneAsync("BlueUnderWaterGamePlay");
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
        Destroy(beachObj);

        vest.SetActive(true);

        GameObject go = Instantiate(boatFight);
        playerAIBoat = go.GetComponentInChildren<PlayerAIBoat>();

        PlayerMovement(false);

        PlayerAssignToAIBoat PAB = go.GetComponent<PlayerAssignToAIBoat>();

        PAB.SetPlayer(playerTransform, playerTransform.gameObject.GetComponent<Damageable>());

        blackScreen.DOFade(0, 1);
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

    #region Hand Ray Control

    public void LeftHandGrab(SelectEnterEventArgs args)
    {
        leftHandRay.SetActive(false);

        if (args.interactableObject.transform.name == "M17 9MM(Clone)")
            pistolDrop = false;
    }

    public void LeftHandGrabRelease(SelectExitEventArgs args)
    {
        leftHandRay.SetActive(true);

        if (args.interactableObject.transform.gameObject.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        if ((args.interactableObject.transform.name == "M17 9MM(Clone)"))
        {
            pistolDrop = true;
            Invoke(nameof(PistolRepos), 2);
        }
    }

    public void RightHandGrab(SelectEnterEventArgs args)
    {
        rightHandRay.SetActive(false);

        if (args.interactableObject.transform.name == "M17 9MM(Clone)")
            pistolDrop = false;
    }


    public void RightHandGrabRelease(SelectExitEventArgs args)
    {
        rightHandRay.SetActive(true);

        if (args.interactableObject.transform.gameObject.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        if ((args.interactableObject.transform.name == "M17 9MM(Clone)"))
        {
            pistolDrop = true;
            Invoke(nameof(PistolRepos), 2);
        }
    }

    private void PistolRepos()
    {
        if (pistolDrop)
        {
            xRSpawnWithInteractable.ReSetGunPos();
            pistolDrop = false;
        }
    }

    #endregion
}
