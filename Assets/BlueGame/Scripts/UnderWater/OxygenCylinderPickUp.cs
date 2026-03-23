using UnityEngine;

public class OxygenCylinderPickUp : MonoBehaviour
{
    #region Unity_Callback

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerPrefs.SetInt("BlueGameCompletedLevel", 5);
            UnderWaterGamePlayManager.Instance.currentActivity = CurrentActivity.OxygenCylinder;
            PlayerController.Instance.GetOxygen();
            BlueGameUnderWaterUIManager.Instance.ShowInstruction("You’ve found an oxygen cylinder! Click to PickUp and refill your oxygen.", "PickUp");
        }
    }

    #endregion
}
