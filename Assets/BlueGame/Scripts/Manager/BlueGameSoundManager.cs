using UnityEngine;

public class BlueGameSoundManager : MonoBehaviour
{
    #region Variable

    public static BlueGameSoundManager Instance {  get; private set; }

    [Header("AudioSource")]
    [SerializeField] private AudioSource sfx;

    [Header("Audio Clip")]
    [SerializeField] private AudioClip checkPoint;
    [SerializeField] private AudioClip buttonClick;
    [SerializeField] private AudioClip gunSHoot;
    [SerializeField] private AudioClip crossbowFiring;

    #endregion

    #region Unity_Callback

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    #region SFX_Play

    public void PlaySFX(AudioClip clip)
    {
        sfx.clip = clip; 
        sfx.Play();
    }

    public void OnCheckPointCollect()
    {
        PlaySFX(checkPoint);
    }

    public void OnButtonClick()
    {
        PlaySFX(buttonClick);
    }

    public void OnGunShoot()
    {
        PlaySFX(gunSHoot);
    }

    public void OnCrossbowFiring()
    {
        PlaySFX(crossbowFiring);
    }

    #endregion
}
