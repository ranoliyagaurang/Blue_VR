using BoatAttack;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BoatSteering : MonoBehaviour
{
    [Header("Steering Limits")]
    public float minY = -45f;
    public float maxY = 45f;
    public float rotateSpeed = 500f;
    public Transform steering;

    private XRSimpleInteractable interactable;

    private Transform grabbingHand;
    private float lastHandY;
    private bool isGrabbed;

    private BoxCollider boxCollider;

    [SerializeField] private HumanController humanController;

    private void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();
        boxCollider = GetComponent<BoxCollider>();

        interactable.selectEntered.AddListener(OnGrab);
        interactable.selectExited.AddListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        grabbingHand = args.interactorObject.transform;
        lastHandY = grabbingHand.position.y;
        isGrabbed = true;
        boxCollider.enabled = false;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        grabbingHand = null;
        isGrabbed = false;
        boxCollider.enabled = true;
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }


    private void Update()
    {
        // 🚫 Absolutely no movement without grab
        if (!isGrabbed || grabbingHand == null)
            return;

        float currentHandY = grabbingHand.position.y;
        float deltaY = currentHandY - lastHandY;
        lastHandY = currentHandY;

        // Detect left/right hand
        float direction = grabbingHand.name.ToLower().Contains("left") ? -1f : 1f;

        float currentY = steering.localEulerAngles.z;
        if (currentY > 180) currentY -= 360;

        float targetY = Mathf.Clamp(
            currentY + (deltaY * rotateSpeed * direction),
            minY,
            maxY
        );

        steering.localRotation = Quaternion.Euler(28, 0, targetY);

        float steerNormalized = Mathf.InverseLerp(minY, maxY, targetY) * 2f - 1f;

        humanController._steering = -steerNormalized;
    }
}
