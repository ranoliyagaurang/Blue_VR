using UnityEngine;

public class CheckPointController : MonoBehaviour
{
    [SerializeField] private GameObject nextCheckPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("boat") || other.gameObject.CompareTag("Projectile"))
        {
            if(nextCheckPoint != null)
            {
                nextCheckPoint.SetActive(true);
            }
            else
            {
                Debug.Log("Completed");
                BlueGameManager.Instance.isBoatRidingCompleted = true;
                BlueGameUIManager.Instance.ShowInstruction("You have reached the shipwreck location. Now, dive deep underwater to explore.");
            }
            gameObject.SetActive(false);
            if (BlueGameSoundManager.Instance != null)
                BlueGameSoundManager.Instance.OnCheckPointCollect();
        }
    }
}
