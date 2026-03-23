using UnityEngine;

public class CodeDoorInstruction : MonoBehaviour
{
    #region Variables

    private bool isInstruction = true;
    [SerializeField] private Sprite itemSprite;
    public bool isDoorPuzzleOpen;

    #endregion

    #region Unity_Callback

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (isInstruction)
            {
                UnderWaterGamePlayManager.Instance.directionArrow.RemoveTarget();
                BlueGameUnderWaterUIManager.Instance.ShowItemInstruction("This code plate is hidden in one of the ship’s rooms. Find it to get the PIN and unlock the door.", itemSprite);
                isInstruction = false;
                UnderWaterGamePlayManager.Instance.OctopusActivator.SetActive(true);
            }
            if(isDoorPuzzleOpen)
            {
                BlueGameUnderWaterUIManager.Instance.ShowCodeEnterScreen();
            }
        }
    }

    #endregion
}
