using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR; // NEW: Required for direct hardware polling

public class Swimmer : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] float swimForce = 4f;
    [SerializeField] float dragForce = 2f;
    [SerializeField] float rotationSensitivity = 50f;
    [SerializeField] float minForce;
    [SerializeField] float minTimeBetweenStrokes;

    [Header("References")]
    [SerializeField] InputActionReference leftControllerSwimReference;
    [SerializeField] InputActionReference rightControllerSwimReference;


    private bool isOctopusActive = false;
    private bool isDead = false;
    Rigidbody _rigidbody;
    float _cooldownTimer;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = false;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Start()
    {
        PlayerController.Instance.playerHealth.DeathEvent += MissioFaield;
    }

    private void OnDestroy()
    {
        if(PlayerController.Instance != null)
            PlayerController.Instance.playerHealth.DeathEvent -= MissioFaield;
    }

    private void MissioFaield() => isDead = true;

    // --- NEW: DIRECT HARDWARE VELOCITY METHOD ---
    private Vector3 GetHardwareVelocity(XRNode node)
    {
        UnityEngine.XR.InputDevice device = InputDevices.GetDeviceAtXRNode(node);
        if (device.isValid && device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceVelocity, out Vector3 velocity))
        {
            return velocity;
        }
        return Vector3.zero;
    }

    public void IsOctopusActivate(bool isActive)
    {
        if (isActive)
            _rigidbody.linearVelocity = Vector3.zero;

        isOctopusActive = isActive;
    }

    private void FixedUpdate()
    {
        if (isDead || isOctopusActive) return;

        _cooldownTimer += Time.fixedDeltaTime;

        // 1. GET PERFECT HARDWARE VELOCITY (Works on PC and Quest 3 APK perfectly)
        Vector3 leftHandVelocity = GetHardwareVelocity(XRNode.LeftHand);
        Vector3 rightHandVelocity = GetHardwareVelocity(XRNode.RightHand);

        // 2. DRAG & SOUND
        bool isMoving = _rigidbody.linearVelocity.sqrMagnitude > 0.01f;
        if (isMoving)
        {
            _rigidbody.AddForce(-_rigidbody.linearVelocity * dragForce, ForceMode.Acceleration);
            UnderWaterGamePlayManager.Instance.PlaySwimmingSound();
        }
        else
        {
            if (!leftControllerSwimReference.action.IsPressed() && !rightControllerSwimReference.action.IsPressed())
                UnderWaterGamePlayManager.Instance.StopSwimmingSound();
        }

        HandleRotation(leftHandVelocity, rightHandVelocity);
        HandleSwimming(leftHandVelocity, rightHandVelocity);
    }

    private void HandleRotation(Vector3 leftVel, Vector3 rightVel)
    {
        bool leftPressed = leftControllerSwimReference.action.IsPressed();
        bool rightPressed = rightControllerSwimReference.action.IsPressed();

        if (leftPressed ^ rightPressed)
        {
            Vector3 handVel = leftPressed ? leftVel : rightVel;

            float rotationAmount = -handVel.x * rotationSensitivity * Time.fixedDeltaTime;
            Quaternion deltaRotation = Quaternion.Euler(0, rotationAmount, 0);
            _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
        }
    }

    private void HandleSwimming(Vector3 leftVel, Vector3 rightVel)
    {
        if (_cooldownTimer > minTimeBetweenStrokes
            && leftControllerSwimReference.action.IsPressed()
            && rightControllerSwimReference.action.IsPressed())
        {
            Vector3 localVelocity = -(leftVel + rightVel);

            if (localVelocity.sqrMagnitude > minForce * minForce)
            {
                Vector3 worldVelocity = transform.TransformDirection(localVelocity);
                _rigidbody.AddForce(worldVelocity * swimForce, ForceMode.Acceleration);
                _cooldownTimer = 0f;
                UnderWaterGamePlayManager.Instance.PlaySwimmingSound();
            }
        }
    }
}