using UnityEngine;

public class SokobanPuzzlePlayer : Movable
{
    public override void Move(Vector3 direction, float distance, bool force = false)
    {
        base.Move(direction, distance, force);

        if(direction.x > 0)
        {
            transform.localScale = new Vector3(-0.2f, 0.2f, 0.2f);
        }
        else if(direction.x < 0)
        {
            transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        }
    }
}