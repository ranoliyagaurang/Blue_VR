using UnityEngine;

public class ChestKeyIntract : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            BlueGameUnderWaterUIManager.Instance.ShowInstruction("You’ve found an Chest Key! Click to PickUp, then head to the chest to unlock it.", "PickUp");
            UnderWaterGamePlayManager.Instance.chestKey.SetActive(false);
            UnderWaterGamePlayManager.Instance.chestKeyIntract.SetActive(false);
            UnderWaterGamePlayManager.Instance.isKeyFound = true;
        }
    }
}
