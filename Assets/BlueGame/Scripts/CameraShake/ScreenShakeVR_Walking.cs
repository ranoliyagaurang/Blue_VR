using UnityEngine;

public class ScreenShakeVR_Walking : MonoBehaviour
{
    public static ScreenShakeVR_Walking instance;

    [Header("Material Settings")]
    public Material screenShakeMaterial;
    public string shakePropertyReference = "_ShakeFactor";

    [Header("Walking Shake Settings")]
    
    [Tooltip("Turn this ON/OFF externally")]
    public bool isWalking = false;     // Turn this ON/OFF externally
    
    [Tooltip("Shake strength")]
    public float walkIntensity = 0.03f;           // Shake strength
    
    [Tooltip("Steps per second (jogging = 3–4)")]
    public float walkFrequency = 2.0f;            // Steps per second (jogging = 3–4)
   
   [Tooltip("Smooth fade-in/out")]
    public float fadeSpeed = 4f;                  // Smooth fade-in/out

    private float currentShake = 0f;
    private float shakeTarget = 0f;

    void Awake()
    {
        instance = this;

        if (screenShakeMaterial == null)
        {
            Debug.LogError("Walking Shake: No material assigned!");
        }
    }

    private void OnDisable()
    {
        if (screenShakeMaterial != null)
            screenShakeMaterial.SetFloat(shakePropertyReference, 0f);
    }

    void Update()
    {
        // Determine the target shake value based on whether player is walking
        if (isWalking)
        {
            // Smooth sinusoidal oscillation
            shakeTarget = Mathf.Sin(Time.time * walkFrequency * Mathf.PI * 2f)
                          * walkIntensity;
        }
        else
        {
            shakeTarget = 0f;
        }

        // Smoothly transition (no snapping)
        currentShake = Mathf.Lerp(currentShake, shakeTarget, Time.deltaTime * fadeSpeed);

        // Apply to material
        if (screenShakeMaterial != null)
        {
            screenShakeMaterial.SetFloat(shakePropertyReference, currentShake);
        }
    }

    // Optional: call this from your movement script
    public void SetWalking(bool walking)
    {
        isWalking = walking;
    }
}
