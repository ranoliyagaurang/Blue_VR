using UnityEngine;

public class UnderWaterCheckPoint : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject nextCheckPoint;
    public bool isFishHunting;
    public int fishCount;

    #endregion

    #region Unity_Callback

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (nextCheckPoint != null)
            {
                UnderWaterGamePlayManager.Instance.directionArrow.RemoveTarget();
                if(isFishHunting)
                {
                    gameObject.SetActive(false);
                    if (BlueGameSoundManager.Instance != null)
                        BlueGameSoundManager.Instance.OnCheckPointCollect();
                    UnderWaterGamePlayManager.Instance.SetFishHunting(fishCount, nextCheckPoint);
                    return;
                }
                nextCheckPoint.SetActive(true);
                UnderWaterGamePlayManager.Instance.directionArrow.SetTarget(nextCheckPoint.transform);
            }
            else
            {
                switch (UnderWaterGamePlayManager.Instance.currentCheckPonit)
                {
                    case CheckPoint.ExploreDeepSea:
                        PlayerPrefs.SetInt("BlueGameCompletedLevel", 3);
                        UnderWaterGamePlayManager.Instance.currentActivity = CurrentActivity.GotoTunnle;
                        BlueGameUnderWaterUIManager.Instance.ShowInstruction("Your oxygen cylinder level is critically low. Follow the arrow to find a new one.");
                        break;

                    case CheckPoint.FindShip:
                        PlayerPrefs.SetInt("BlueGameCompletedLevel", 6);
                        UnderWaterGamePlayManager.Instance.currentActivity = CurrentActivity.ExploreShip;
                        BlueGameUnderWaterUIManager.Instance.ShowInstruction("You’ve reached the shipwreck! Follow the arrow and collect all checkpoints to explore the treasure ship.\r\nBut be careful—the Shark and Tylosaurus fish have spotted you and they’re closing in fast! Grab your weapon, fight back.\r\nShoot them down one by one to survive this deadly attack!");
                        break;
                    case CheckPoint.ExploreShip:
                        PlayerPrefs.SetInt("BlueGameCompletedLevel", 7);
                        UnderWaterGamePlayManager.Instance.currentActivity = CurrentActivity.ShipMainDoor;
                        BlueGameUnderWaterUIManager.Instance.ShowInstruction("Follow the arrow to reach the main entrance of the ship.");
                        break;
                }
                UnderWaterGamePlayManager.Instance.directionArrow.RemoveTarget();
            }
            gameObject.SetActive(false);
            if (BlueGameSoundManager.Instance != null)
                BlueGameSoundManager.Instance.OnCheckPointCollect();
        }
    }

    #endregion
}
