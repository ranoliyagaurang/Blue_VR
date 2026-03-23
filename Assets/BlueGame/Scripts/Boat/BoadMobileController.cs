using BoatAttack;
using Shemarooverse.MobileControls;
using UnityEngine;

public class BoadMobileController : AControlUIButton
{
    [SerializeField] private HumanController humanController;
    [SerializeField] private float inputValue;
    [SerializeField] private bool isAccelerate;

    public override void OnPointerDown()
    {
        if (isAccelerate)
        {
            humanController._throttle = inputValue;
        }
        else
        {
            humanController._steering = inputValue;
        }
    }

    public override void OnPointerUp()
    {
        if (isAccelerate)
        {
            humanController._throttle = 0;
        }
        else
        {
            humanController._steering = 0;
        }
    }
}
