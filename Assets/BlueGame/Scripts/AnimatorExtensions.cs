using UnityEngine;

public static class AnimatorExtensions
{
    public static float GetNormalizedTime(this Animator animator, int layer, int round = 2)
    {
        return (float)System.Math.Round(((animator.IsInTransition(layer) ? animator.GetNextAnimatorStateInfo(layer).normalizedTime : animator.GetCurrentAnimatorStateInfo(layer).normalizedTime) % 1), round);
    }
}