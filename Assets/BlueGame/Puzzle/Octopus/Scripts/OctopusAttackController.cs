using UnityEngine;

public class OctopusAttackController : MonoBehaviour
{
    #region Unity_Callback

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController.Instance.PlayerTakeDamage(8f);
        }
    }

    #endregion
}
