using UnityEngine;

public class TunnleGateCollide : MonoBehaviour
{
    #region Unity_Callback

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            UnderWaterGamePlayManager.Instance.currentActivity = CurrentActivity.TunnelDoorPuzzle;
            UnderWaterGamePlayManager.Instance.directionArrow.RemoveTarget();
            BlueGameUnderWaterUIManager.Instance.ShowInstruction("Complete the puzzle to unlock the door.");
        }
    }

    #endregion
}
