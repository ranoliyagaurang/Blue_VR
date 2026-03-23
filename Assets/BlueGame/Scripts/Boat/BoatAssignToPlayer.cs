using UnityEngine;
using VRFPSKit;

public class BoatAssignToPlayer : MonoBehaviour
{
    [SerializeField] private Transform playerPos;
    [SerializeField] private BoatAccelerator boatAccelerator;
    [SerializeField] private Boat_RockDamage rockDamage;
    private Transform playerTans;

    public void SetPlayer(Transform player, Damageable damageable)
    {
        playerTans = player;
        playerTans.SetParent(playerPos);
        playerTans.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        boatAccelerator.readyToDrive = true;
        rockDamage.damageable = damageable;
    }

    private void Update()
    {
        if (playerTans == null)
            return;

        playerTans.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }
}
