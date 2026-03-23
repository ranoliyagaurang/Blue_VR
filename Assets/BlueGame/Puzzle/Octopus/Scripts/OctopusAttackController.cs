using UnityEngine;

public class OctopusAttackController : MonoBehaviour
{
    #region Unity_Callback

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerController.Instance.PlayerTakeDamage(8f);
            Debug.Log("Player Damage");
        }
    }

    #endregion
}
