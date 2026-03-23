using UnityEngine;

namespace VRFPSKit {
    public class VRBodyVestTargetTracking : MonoBehaviour
    {
        public VRBodySkinBinder skinBinder;
        [SerializeField] private Transform mainCamera;

        // Update is called once per frame
        void Update()
        {
            if (skinBinder.GetSkin() is not { } skin) return;
            if (skin.vestTarget is not { } vestTarget)
            {
                Debug.LogWarning($"Player skin '{skin.name}' doesn't have a vest target assigned. Cannot track vest.");
                return;
            }

            // Follow chest position
            transform.position = vestTarget.position;

            // Get the headset rotation but only yaw (Y-axis)
            float yaw = mainCamera.eulerAngles.y;

            // Apply yaw to vest rotation while keeping vest's original forward direction
            transform.rotation = Quaternion.Euler(
                vestTarget.eulerAngles.x,   // keep original chest X rotation
                yaw,                        // follow headset left-right rotation
                vestTarget.eulerAngles.z    // keep original chest Z rotation
            );
        }
    }
}