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
        playerTans.SetParent(playerPos);
        playerTans.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        aIBoat.playerHealth = damageable;
    }

    public void FightComplete()
    {
        isFightComplete = true;
        playerTans.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(new Vector3(0,180,0)));
    }

    private void Update()
    {
        if ((playerTans == null) || isFightComplete)
            return;

        playerTans.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }
}
