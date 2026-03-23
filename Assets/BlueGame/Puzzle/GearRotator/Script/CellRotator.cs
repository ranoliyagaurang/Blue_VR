using UnityEngine;

public class CellRotator : MonoBehaviour
{
    #region Variables

    public bool isRotating;
    public float speedRot = 0.3f;
    public bool isClockWise;

    public CellController cellController;

    #endregion

    #region Unity_Callback

    void Update()
    {
        if (isRotating)
        {
            float rot = Time.deltaTime * speedRot;
            transform.Rotate(0f, 0f, isClockWise ? -rot : rot);
        }
    }

    #endregion
}
