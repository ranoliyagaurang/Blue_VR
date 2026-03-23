using UnityEngine;
using System;
using System.Collections;

public class CameraPathMover : MonoBehaviour
{
    #region Variables

    [Header("Camera Path")]
    public Transform[] pathPoints; 
    public float moveDuration = 2f;
    public static Action OnPathCompleted;

    #endregion

    #region Beach_CutShort

    public void StartGame()
    {
        if (pathPoints == null || pathPoints.Length == 0)
        {
            Debug.LogWarning("No path points assigned to CameraPathMover!");
            return;
        }

        transform.SetPositionAndRotation(pathPoints[0].position, pathPoints[0].rotation);
        StartCoroutine(MoveThroughPath());
    }

    IEnumerator MoveThroughPath()
    {
        for (int i = 1; i < pathPoints.Length; i++)
        {
            yield return StartCoroutine(MoveToPoint(pathPoints[i]));
        }

        OnPathCompleted?.Invoke();
    }

    IEnumerator MoveToPoint(Transform target)
    {
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        Vector3 endPos = target.position;
        Quaternion endRot = target.rotation;

        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            float t = elapsed / moveDuration;

            transform.SetPositionAndRotation(Vector3.Lerp(startPos, endPos, t), Quaternion.Slerp(startRot, endRot, t));

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.SetPositionAndRotation(endPos, endRot);
    }

    #endregion
}
