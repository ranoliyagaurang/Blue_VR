using System.Collections;
using UnityEngine;

public class ExplosionParticle : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }
}
