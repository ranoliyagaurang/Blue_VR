using UnityEngine;

public class SagarIntract : MonoBehaviour
{
    [SerializeField] private GameObject DialoguePanel;
    [SerializeField] private BoxCollider intracionCollider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            DialoguePanel.SetActive(true);
            intracionCollider.enabled = false;
        }
    }
}
