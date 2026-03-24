using UnityEngine;

public class OctopusSpotArea : MonoBehaviour
{
    #region Unity_Callback

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            UnderWaterGamePlayManager.Instance.currentActivity = CurrentActivity.OctopusTrap;
            BlueGameUnderWaterUIManager.Instance.ShowInstruction("A giant octopus has trapped you in its territory!\r\nIt’s not moving, but its massive presence blocks your escape.\r\nGrab your weapon and fight back — destroying it is your only chance to break free!");
            gameObject.SetActive(false);
        }
    }

    #endregion
}
