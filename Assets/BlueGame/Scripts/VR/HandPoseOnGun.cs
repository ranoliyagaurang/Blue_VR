using UnityEngine;

public class HandPoseOnGun : MonoBehaviour
{
    [SerializeField] private GameObject leftPose;
    [SerializeField] private GameObject rightPose;

    public void DisablePose()
    {
        leftPose.SetActive(false);
        rightPose.SetActive(false);
    }

    public void EnablePose(string pose)
    {
        if (pose.Equals("Left"))
            leftPose.SetActive(true);
        else
            rightPose.SetActive(true);
    }
}
