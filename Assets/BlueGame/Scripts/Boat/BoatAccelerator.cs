using BoatAttack;
using UnityEngine;
using UnityEngine.InputSystem;
using VRFPSKit;

public class BoatAccelerator : MonoBehaviour
{
    [SerializeField] private HumanController humanController;
    public bool readyToDrive;

    private void OnEnable()
    {
        Player.trigerCallback += TriggerCallback;
    }

    private void OnDisable()
    {
        Player.trigerCallback -= TriggerCallback;
    }

    private void TriggerCallback(string arg1, bool arg2)
    {
        if (!readyToDrive)
            return;

        if (arg1.Equals("Right"))
        {
            if (arg2)
            {
                Debug.Log("Right Trigger Press");
                humanController._throttle = 1;
            }
            else
            {
                Debug.Log("Right Trigger Relese");
                humanController._throttle = 0;
            }
        }
    }
}
