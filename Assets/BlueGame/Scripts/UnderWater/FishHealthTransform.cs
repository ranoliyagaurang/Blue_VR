using UnityEngine;

public class FishHealthTransform : MonoBehaviour
{
    #region Variables

    [SerializeField] Transform TargetTransform;
    [SerializeField] Vector3 LocalPositionOffset;
    [SerializeField] Vector3 RotationOffset;

    #endregion

    #region Unity_Callback

    private void Start()
    {
        TargetTransform = Camera.main.transform.parent;
    }
    void LateUpdate ()
    {
        transform.localRotation = Quaternion.LookRotation (transform.parent.InverseTransformDirection(TargetTransform.position - transform.position) + LocalPositionOffset, Vector3.up) * Quaternion.Euler (RotationOffset);
    }

    #endregion
}
