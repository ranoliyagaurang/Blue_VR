using UnityEngine;

public class DirectionArrow : MonoBehaviour
{
    private Transform target;

    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
        gameObject.SetActive(true);
    }

    public void RemoveTarget()
    {
        target = null;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (target == null)
            return;

        Vector3 direction = target.position - transform.position;
        direction.y = 0f; // Remove vertical tilt

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }
    }
}
