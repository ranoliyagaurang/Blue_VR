using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlueGameTrapController : MonoBehaviour
{
    #region Variables

    //[SerializeField] private vThirdPersonInput thirdPersonInput;
    //[SerializeField] private vThirdPersonCamera thirdPersonCamera;

    [SerializeField] private Transform trapPoint;
    [SerializeField] private Transform trap;
    [SerializeField] private Transform leftHook;
    [SerializeField] private Transform rightHook;
    [SerializeField] private Transform hook;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Image trapFillbar;
    [SerializeField] private GameObject trapUI;

    private bool isActiveTrap = false;
    private readonly float decreaseSpeed = 0.1f;
    private readonly float increaseAmount = 0.05f;

    #endregion

    #region Unity_Callback

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            OnTrapPlayer();
        }
    }

    private void Update()
    {
        if (isActiveTrap)
        {
            trapFillbar.fillAmount -= decreaseSpeed * Time.deltaTime;

            if (trapFillbar.fillAmount <= 0f)
            {
                trapFillbar.fillAmount = 0f;
                isActiveTrap = false;
                //BlueGameUnderWaterUIManager.Instance.missionFailedScreen.gameObject.SetActive(true);
            }
        }
    }

    #endregion

    #region Private_Method

    private void IstrapIdle()
    {
        playerAnimator.SetBool("Trap", false);
        UnderWaterGamePlayManager.Instance.currentActivity = CurrentActivity.Trap;
        BlueGameUnderWaterUIManager.Instance.ShowInstruction("You are trapped! \r\nTap the unlock button repeatedly to fill the bar and unlock the trap.\r\nIf the bar empties completely, you will die!");
    }

    private void IsSwimmingActive()
    {
        playerAnimator.SetBool("TrapUnlock", false);
        gameObject.SetActive(false);
        //thirdPersonInput.enabled = true;
        //thirdPersonCamera.enabled = true;
        trapUI.SetActive(false);
    }

    #endregion

    #region Public_Method

    [ContextMenu("OnPuzzleActivate")]
    public void OnPuzzleActivate()
    {
        trapUI.SetActive(true);
        isActiveTrap = true;
    }

    public void OnUnlock()
    {
        if (isActiveTrap)
        {
            trapFillbar.fillAmount += increaseAmount;

            if (trapFillbar.fillAmount >= 1f)
            {
                trapFillbar.fillAmount = 1f;
                isActiveTrap = false;
                StartCoroutine(RotateY(rightHook, -45));
                StartCoroutine(RotateY(leftHook, 45));
                playerAnimator.SetBool("TrapUnlock", true);
                Invoke(nameof(IsSwimmingActive), 1.2f);
                Debug.Log("Win");
            }
        }
    }

    [ContextMenu("OnTrapPlayer")]
    public void OnTrapPlayer()
    {
        //thirdPersonInput.enabled = false;
        //thirdPersonCamera.enabled = false;
        StartCoroutine(MoveSmoothly());
        playerAnimator.SetBool("Trap", true);
        Invoke(nameof(IstrapIdle), 1.2f);
    }

    #endregion

    #region IEnumerator

    //private IEnumerator MoveCamera()
    //{
    //    Vector3 startPos = thirdPersonCamera.transform.localPosition;
    //    Quaternion startRot = thirdPersonCamera.transform.localRotation;

    //    Vector3 targetPos = new Vector3(1, 1, 1);
    //    Quaternion targetRot = Quaternion.Euler(25, -130, 0);

    //    float duration = 2f;
    //    float elapsed = 0f;

    //    while (elapsed < duration)
    //    {
    //        float t = elapsed / duration;

    //        thirdPersonCamera.transform.SetLocalPositionAndRotation(Vector3.Lerp(startPos, targetPos, t), Quaternion.Slerp(startRot, targetRot, t));
    //        elapsed += Time.deltaTime;
    //        yield return null;
    //    }

    //    thirdPersonCamera.transform.SetLocalPositionAndRotation(targetPos, targetRot);
    //    thirdPersonCamera.transform.SetParent(null);
    //}


    private IEnumerator MoveSmoothly()
    {
        Vector3 start = trap.position;
        float elapsed = 0f;

        while (elapsed < 1)
        {
            trap.position = Vector3.Lerp(start, trapPoint.position, elapsed / 1);
            elapsed += Time.deltaTime;
            yield return null;
        }

        trap.position = trapPoint.position;
        StartCoroutine(RotateY(rightHook, 45f));
        StartCoroutine(RotateY(leftHook, -45f));
        hook.localPosition = new Vector3(hook.localPosition.x, trap.localPosition.y, trap.localPosition.z);

        //thirdPersonCamera.transform.SetParent(playerAnimator.transform);
        //StartCoroutine(MoveCamera());
    }

    private IEnumerator RotateY(Transform target, float targetAngle)
    {
        float rotated = 0f;
        while (Mathf.Abs(rotated) < Mathf.Abs(targetAngle))
        {
            float step = 300 * Time.deltaTime;
            float rotateNow = Mathf.Min(step, Mathf.Abs(targetAngle) - Mathf.Abs(rotated));

            float signedStep = Mathf.Sign(targetAngle) * rotateNow;

            target.Rotate(0, signedStep, 0);
            rotated += signedStep;

            yield return null;
        }
    }

    #endregion
}
