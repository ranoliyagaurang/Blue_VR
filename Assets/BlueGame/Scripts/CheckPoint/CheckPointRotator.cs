using UnityEngine;

public class CheckPointRotator : MonoBehaviour
{
    public float speedRot = 0.3f;

    void Update()
    {
        float rot = Time.deltaTime * speedRot;
        transform.Rotate(new Vector3(rot, 0, 0));
        //transform.Rotate(new Vector3(rot, transform.rotation.y, transform.rotation.z));
        transform.localRotation = Quaternion.Euler(rot, transform.rotation.y, transform.rotation.z);
    }
}
