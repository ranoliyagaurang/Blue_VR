using UnityEngine;

public class OctopusActivator : MonoBehaviour
{
    #region Variable

    [SerializeField] private GameObject codeplate;
    [SerializeField] private CodeDoorInstruction instruction;

    #endregion

    #region Unity_Callback

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Name : " + other.name + "  Layer : " + other.gameObject.layer);
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            codeplate.SetActive(true);
            UnderWaterGamePlayManager.Instance.octopus.SetActive(true);
            UnderWaterGamePlayManager.Instance.octopusSpotArea.SetActive(true);
            instruction.isDoorPuzzleOpen = true;
            gameObject.SetActive(false);
        }
    }

    #endregion
}
