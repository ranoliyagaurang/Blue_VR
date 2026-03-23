using UnityEngine;

public class PlayerBulletController : MonoBehaviour
{
    #region Unity_Callback
    void Start()
    {
        Invoke(nameof(AfterSomeTimeDestroyMe), 10);
    }
    private void AfterSomeTimeDestroyMe()
    {
        Destroy(gameObject);
    }
    #endregion
}
