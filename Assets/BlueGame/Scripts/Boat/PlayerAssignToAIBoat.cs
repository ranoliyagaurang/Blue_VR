using UnityEngine;
using VRFPSKit;

public class PlayerAssignToAIBoat : MonoBehaviour
{
    [SerializeField] private Transform playerPos;
    [SerializeField] private PlayerAIBoat aIBoat;
    private bool isFightComplete = false;
    private Transform playerTans;

    public void SetPlayer(Transform player, Damageable damageable)
    {
        playerTans = player;
        playerTans.SetParent(null);
        playerTans.SetPositionAndRotation(playerPos.position, playerPos.rotation);
        aIBoat.playerHealth = damageable;
    }

    public void FightComplete()
    {
        isFightComplete = true;
        playerTans.SetPositionAndRotation(playerPos.position, Quaternion.Euler(new Vector3(playerPos.rotation.x, playerPos.rotation.y + 180, playerPos.rotation.z)));
    }

    private void LateUpdate()
    {
        if ((playerTans == null))
            return;

        if(isFightComplete)
            playerTans.SetPositionAndRotation(playerPos.position, Quaternion.Euler(new Vector3(playerPos.rotation.x, playerPos.rotation.y + 180, playerPos.rotation.z)));
        else
            playerTans.SetPositionAndRotation(playerPos.position, playerPos.rotation);
    }
}
