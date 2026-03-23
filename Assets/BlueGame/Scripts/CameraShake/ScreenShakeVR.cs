using UnityEngine;
using System.Collections.Generic;

public class ScreenShakeVR : MonoBehaviour
{
    public static ScreenShakeVR instance;

    [Header("Material Settings")]
    [Tooltip("Drag your Mat_VRScreenShake Material here")]
    public Material screenShakeMaterial;
    public float magnitude;
    public float length;

    [Tooltip("The 'Reference' name from your Shader Graph property")]
    public string shakePropertyReference = "_ShakeFactor";


    [Header("Shake Settings")]
    public float shakeMagnitude = 0.1f;
    public float shakeFrequency = 20f;

    [Tooltip("Shake the screen when the space key is pressed")]
    public bool debug = false;

    // --- The rest of this is from the original script ---

    private float shakeVal;
    private float shakeCumulation;
    public List<ShakeEvent> activeShakes = new List<ShakeEvent>();

    public class ShakeEvent
    {
        public float magnitude;
        public float length;
        private float exponent;

        public bool finished { get { return time >= length; } }
        public float currentStrength { get { return magnitude * Mathf.Clamp01(1 - time / length); } }

        public ShakeEvent(float mag, float len, float exp = 2)
        {
            magnitude = mag;
            length = len;
            exponent = exp;
        }

        private float time;

        public void Update(float deltaTime)
        {
            time += deltaTime;
        }
    }

    void Awake()
    {
        instance = this;

        if (screenShakeMaterial == null)
        {
            Debug.LogError("ScreenShakeVR Error: No Material assigned!");
        }
    }

    private void OnDisable()
    {
        // IMPORTANT: Reset the shake when we stop playing or disable the object
        if (screenShakeMaterial != null)
        {
            screenShakeMaterial.SetFloat(shakePropertyReference, 0f);
        }
    }

    public void Shake(float magnitude, float length, float exponent = 2)
    {
        activeShakes.Add(new ShakeEvent(magnitude, length, exponent));
    }

    public static void TriggerShake(float magnitude, float length, float exponent = 2)
    {
        if (instance == null)
        {
            Debug.LogWarning("No ScreenShakeVR Component in scene.");
        }
        else
        {
            instance.Shake(magnitude, length, exponent);
        }
    }

    public void ScreenShakeButton()
    {
        //Debug.Log("Trigger Shake Called");
        TriggerShake(magnitude, length);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && debug)
        {
            // Shake(0.5f, 1.0f);
            Shake(magnitude, length);
        }

        // Calculate the shake value (same as original script)
        shakeCumulation = 0;
        for (int i = activeShakes.Count - 1; i >= 0; i--)
        {
            activeShakes[i].Update(Time.deltaTime);
            shakeCumulation += activeShakes[i].currentStrength;
            if (activeShakes[i].finished)
            {
                activeShakes.RemoveAt(i);
            }
        }

        if (shakeCumulation > 0)
        {
            shakeVal = Mathf.PerlinNoise(Time.time * shakeFrequency, 10.234896f) * shakeCumulation * shakeMagnitude;
        }
        else
        {
            shakeVal = 0;
        }

        // --- This is the new part ---
        // Instead of OnRenderImage, we just set the material property.
        // The URP Renderer Feature will do the rest of the work!
        if (screenShakeMaterial != null)
        {
            screenShakeMaterial.SetFloat(shakePropertyReference, shakeVal);
        }
    }
}