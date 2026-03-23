using UnityEngine;

public class CameraRotationController : MonoBehaviour
{
    [Header("Rotation Limits")]
    [SerializeField] private float minXRotation = -75f;
    [SerializeField] private float maxXRotation = 75f;
    [SerializeField] private bool enableYRotationLimit = false;
    [SerializeField] private float minYRotation = 0f;
    [SerializeField] private float maxYRotation = 360f;

    [Header("Sensitivity Settings")]
    [Range(0f, 3f)]
    [SerializeField] private float sensitivityX = 2f;
    [Range(0f, 3f)]
    [SerializeField] private float sensitivityY = 2f;

    [Header("Platform Scaling")]
    [SerializeField] private float mobileScaleFactor = 0.6f;
    [SerializeField] private float pcScaleFactor = 1f;
    [SerializeField] private float touchNormalizationFactor = 0.1f;

    [Header("Smoothing")]
    [SerializeField] private bool enableSmoothing = true;
    [SerializeField] private float smoothSpeed = 10f;
    [SerializeField] private float dampingFactor = 0.9f;

    [Header("Input Settings")]
    [SerializeField] private bool invertXAxis = false;
    [SerializeField] private bool invertYAxis = false;
    [SerializeField] private bool requireMouseButton = true;
    [SerializeField] private int mouseButtonIndex = 0; // 0 = Left, 1 = Right, 2 = Middle

    [Header("Mobile Settings")]
    [SerializeField] private bool enableMobileInput = true;
    [SerializeField] private int touchIndex = 0;

    [Header("Dynamic Sensitivity")]
    [SerializeField] private bool enableDynamicSensitivity = true;
    [SerializeField] private AnimationCurve sensitivityCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    // Private variables
    private float currentXRotation = 0f;
    private float currentYRotation = 0f;
    private float targetXRotation = 0f;
    private float targetYRotation = 0f;

    private Vector2 rotationVelocity = Vector2.zero;
    private Vector2 lastInputDelta = Vector2.zero;

    private Camera playerCamera;
    private bool isInitialized = false;
    private bool isMobilePlatform = false;

    // Input state
    private bool isRotating = false;
    private Vector2 lastInputPosition = Vector2.zero;

    // Properties for runtime access
    public float SensitivityX
    {
        get { return sensitivityX; }
        set { sensitivityX = Mathf.Clamp(value, 0f, 3f); }
    }

    public float SensitivityY
    {
        get { return sensitivityY; }
        set { sensitivityY = Mathf.Clamp(value, 0f, 3f); }
    }

    public float MobileScaleFactor
    {
        get { return mobileScaleFactor; }
        set { mobileScaleFactor = Mathf.Clamp(value, 0.1f, 2f); }
    }

    public float PCScaleFactor
    {
        get { return pcScaleFactor; }
        set { pcScaleFactor = Mathf.Clamp(value, 0.1f, 2f); }
    }

    private void Awake()
    {
        InitializeCamera();
        DetectPlatform();
    }

    private void Start()
    {
        if (!isInitialized)
        {
            InitializeCamera();
        }

        // Initialize rotation values
        Vector3 currentEuler = transform.localEulerAngles;
        currentXRotation = NormalizeAngle(currentEuler.x);
        currentYRotation = NormalizeAngle(currentEuler.y);
        targetXRotation = currentXRotation;
        targetYRotation = currentYRotation;
    }

    private void InitializeCamera()
    {
        playerCamera = GetComponent<Camera>();
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
        isInitialized = true;
    }

    private void DetectPlatform()
    {
        isMobilePlatform = Application.platform == RuntimePlatform.Android ||
                          Application.platform == RuntimePlatform.IPhonePlayer ||
                          Application.isMobilePlatform;
    }

    private void Update()
    {
        HandleInput();
        ApplyRotation();
    }

    private void HandleInput()
    {
        Vector2 inputDelta = Vector2.zero;
        bool inputDetected = false;

        // Handle input based on platform
        if (isMobilePlatform && enableMobileInput)
        {
            inputDetected = HandleTouchInput(out inputDelta);
        }
        else
        {
            inputDetected = HandleMouseInput(out inputDelta);
        }

        // Process input delta
        if (inputDetected)
        {
            ProcessInputDelta(inputDelta);
        }
        else
        {
            // Apply damping when no input
            ApplyDamping();
        }
    }

    private bool HandleMouseInput(out Vector2 inputDelta)
    {
        inputDelta = Vector2.zero;

        if (requireMouseButton)
        {
            if (Input.GetMouseButtonDown(mouseButtonIndex))
            {
                isRotating = true;
                lastInputPosition = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(mouseButtonIndex))
            {
                isRotating = false;
            }

            if (!isRotating)
                return false;
        }

        // Get mouse delta
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        if (mouseDelta.magnitude > 0.001f)
        {
            // Apply unified sensitivity with PC scaling
            inputDelta = new Vector2(
                mouseDelta.x * sensitivityX * pcScaleFactor,
                mouseDelta.y * sensitivityY * pcScaleFactor
            );
            return true;
        }

        return false;
    }

    private bool HandleTouchInput(out Vector2 inputDelta)
    {
        inputDelta = Vector2.zero;

        if (Input.touchCount > touchIndex)
        {
            Touch touch = Input.GetTouch(touchIndex);

            if (touch.phase == TouchPhase.Began)
            {
                isRotating = true;
                lastInputPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isRotating = false;
            }
            else if (touch.phase == TouchPhase.Moved && isRotating)
            {
                Vector2 touchDelta = touch.deltaPosition;

                // Normalize touch delta for consistent sensitivity across devices
                float screenFactor = Screen.dpi > 0 ? Screen.dpi / 160f : 1f;
                touchDelta /= screenFactor;

                // Apply unified sensitivity with mobile scaling
                inputDelta = new Vector2(
                    touchDelta.x * sensitivityX * mobileScaleFactor * touchNormalizationFactor,
                    touchDelta.y * sensitivityY * mobileScaleFactor * touchNormalizationFactor
                );
                return true;
            }
        }
        else
        {
            isRotating = false;
        }

        return false;
    }

    private void ProcessInputDelta(Vector2 inputDelta)
    {
        // Apply inversion
        if (invertXAxis) inputDelta.x = -inputDelta.x;
        if (invertYAxis) inputDelta.y = -inputDelta.y;

        // Apply dynamic sensitivity curve
        if (enableDynamicSensitivity)
        {
            float inputMagnitude = inputDelta.magnitude;
            float curveValue = sensitivityCurve.Evaluate(Mathf.Clamp01(inputMagnitude / 5f));
            inputDelta *= curveValue;
        }

        // Update target rotations
        targetYRotation += inputDelta.x;
        targetXRotation -= inputDelta.y; // Negative for natural mouse look

        // Clamp X rotation
        targetXRotation = Mathf.Clamp(targetXRotation, minXRotation, maxXRotation);

        // Handle Y rotation limits
        if (enableYRotationLimit)
        {
            targetYRotation = Mathf.Clamp(targetYRotation, minYRotation, maxYRotation);
        }
        else
        {
            // Normalize Y rotation to prevent accumulation issues
            targetYRotation = NormalizeAngle(targetYRotation);
        }

        // Store input for velocity calculation
        lastInputDelta = inputDelta;
    }

    private void ApplyDamping()
    {
        if (enableSmoothing)
        {
            rotationVelocity *= dampingFactor;

            // Apply residual velocity
            if (rotationVelocity.magnitude > 0.01f)
            {
                ProcessInputDelta(rotationVelocity * Time.deltaTime);
            }
        }
    }

    private void ApplyRotation()
    {
        if (enableSmoothing)
        {
            // Smooth rotation interpolation
            currentXRotation = Mathf.LerpAngle(currentXRotation, targetXRotation, smoothSpeed * Time.deltaTime);
            currentYRotation = Mathf.LerpAngle(currentYRotation, targetYRotation, smoothSpeed * Time.deltaTime);

            // Update velocity for momentum
            rotationVelocity = Vector2.Lerp(rotationVelocity, lastInputDelta, Time.deltaTime * 5f);
        }
        else
        {
            // Immediate rotation
            currentXRotation = targetXRotation;
            currentYRotation = targetYRotation;
        }

        // Apply rotation to transform
        transform.localRotation = Quaternion.Euler(currentXRotation, currentYRotation, 0f);
    }

    private float NormalizeAngle(float angle)
    {
        while (angle > 180f) angle -= 360f;
        while (angle < -180f) angle += 360f;
        return angle;
    }

    // Public methods for runtime control
    public void SetSensitivity(float xSensitivity, float ySensitivity)
    {
        SensitivityX = xSensitivity;
        SensitivityY = ySensitivity;
    }

    public void SetPlatformScaling(float mobileScale, float pcScale)
    {
        MobileScaleFactor = mobileScale;
        PCScaleFactor = pcScale;
    }

    public void SetRotationLimits(float minX, float maxX, float minY = 0f, float maxY = 360f)
    {
        minXRotation = minX;
        maxXRotation = maxX;
        minYRotation = minY;
        maxYRotation = maxY;
    }

    public void ResetRotation()
    {
        currentXRotation = 0f;
        currentYRotation = 0f;
        targetXRotation = 0f;
        targetYRotation = 0f;
        transform.localRotation = Quaternion.identity;
    }

    public void SetSmoothing(bool enabled, float speed = 10f)
    {
        enableSmoothing = enabled;
        smoothSpeed = speed;
    }

    // Get current rotation values
    public Vector2 GetCurrentRotation()
    {
        return new Vector2(currentXRotation, currentYRotation);
    }

    public Vector2 GetTargetRotation()
    {
        return new Vector2(targetXRotation, targetYRotation);
    }

    public bool IsMobilePlatform()
    {
        return isMobilePlatform;
    }

    // Get effective sensitivity (with platform scaling applied)
    public Vector2 GetEffectiveSensitivity()
    {
        float scaleFactor = isMobilePlatform ? mobileScaleFactor : pcScaleFactor;
        return new Vector2(sensitivityX * scaleFactor, sensitivityY * scaleFactor);
    }
}
