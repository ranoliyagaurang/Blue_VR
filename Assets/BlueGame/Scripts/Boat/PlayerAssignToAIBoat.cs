using UnityEngine;
using VRFPSKit;

public class PlayerAssignToAIBoat : MonoBehaviour
{
    [SerializeField] private Transform playerPos;
    [SerializeField] private PlayerAIBoat aIBoat;
    private Transform playerTans;

    public void SetPlayer(Transform player, Damageable damageable)
    {
        playerTans = player;
        playerTans.SetParent(playerPos);
        playerTans.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        aIBoat.playerHealth = damageable;
    }

    private void Update()
    {
        if (playerTans == null)
            return;

        playerTans.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }
}
