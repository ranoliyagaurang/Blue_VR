using UnityEngine;

public class PickBoat : MonoBehaviour
{

    [SerializeField] private bool isPerfact;
    [SerializeField] private BeachLevelController beachLevelController;
    [SerializeField] private string boatInstruction;
    [SerializeField] private Transform instructionPos;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Boat Collide : " + other.gameObject.name);
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            beachLevelController.ShowBoatInstruction(boatInstruction, isPerfact, instructionPos);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            beachLevelController.boatInstructionPanel.SetActive(false);
        }
    }
}
