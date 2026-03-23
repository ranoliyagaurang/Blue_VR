using UnityEngine;

public class ChestIntract : MonoBehaviour
{
    #region Variables

    [SerializeField] private Sprite itemSprite;

    #endregion

    #region Unity_Callback

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (!UnderWaterGamePlayManager.Instance.isKeyFound)
            {
                UnderWaterGamePlayManager.Instance.directionArrow.RemoveTarget();
                BlueGameUnderWaterUIManager.Instance.ShowItemInstruction("This Chest Key is hidden in one of the ship’s rooms. Find it unlock the Chest.", itemSprite);
                UnderWaterGamePlayManager.Instance.chestKey.SetActive(true);
                UnderWaterGamePlayManager.Instance.chestKeyIntract.SetActive(true);
            }
            else
            {
                UnderWaterGamePlayManager.Instance.currentActivity = CurrentActivity.ChestKey;
                BlueGameUnderWaterUIManager.Instance.ShowInstruction("Great! You found the chest key. Please wait while the chest is being unlocked.");
                gameObject.SetActive(false);
            }
        }
    }

    #endregion
}
