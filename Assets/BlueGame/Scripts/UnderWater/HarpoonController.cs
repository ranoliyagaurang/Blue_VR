using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class HarpoonController : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;
    private bool isGrab = false;
    private bool isLoaded = false;
    private bool gunReset = false;

    [SerializeField] private GameObject arrowObj;
    [SerializeField] private List<ParticleSystem> shootParticle;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowSpawnPoint;
    [SerializeField] private float arrowSpeed = 50f;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
        isLoaded = true;
    }

    private void Start()
    {
        grabInteractable.selectEntered.AddListener(HarpoonGunGrab);
        grabInteractable.selectExited.AddListener(HarpoonGunGrabRelease);

        grabInteractable.activated.AddListener(HarpoonGunTriggerActivate);

        PlayerController.Instance.playerHealth.DeathEvent += GunDisable;
    }

    private void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(HarpoonGunGrab);
        grabInteractable.selectExited.RemoveListener(HarpoonGunGrabRelease);

        grabInteractable.activated.RemoveListener(HarpoonGunTriggerActivate);

        PlayerController.Instance.playerHealth.DeathEvent -= GunDisable;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Gun Holster Socket")
        {
            gunReset = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Gun Holster Socket")
        {
            gunReset = false;
        }
    }

    private void GunDisable()
    {
        gameObject.SetActive(false);
    }

    private void HarpoonGunGrab(SelectEnterEventArgs args)
    {
        isGrab = true;
    }

    private void HarpoonGunGrabRelease(SelectExitEventArgs args)
    {
        if (grabInteractable.interactorsSelecting.Count == 0)
        {
            isGrab = false;
            rb.useGravity = true;
            rb.isKinematic = false;

            if (gunReset)
                ReSetGun();
            else
                Invoke(nameof(ReSetGun), 3);
        }
    }

    private void HarpoonGunTriggerActivate(ActivateEventArgs args)
    {
        if (!isGrab) return;

        IXRActivateInteractor interactor = args.interactorObject;

        if ((grabInteractable.interactorsSelecting.Count > 0) &&
            (interactor == grabInteractable.interactorsSelecting[0]) && isLoaded)
        {
            isLoaded = false;
            Fire();
        }
    }

    private void ReSetGun()
    {
        rb.useGravity = false;
        rb.isKinematic = true;
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    private void Fire()
    {
        if (BlueGameSoundManager.Instance != null)
            BlueGameSoundManager.Instance.OnCrossbowFiring();

        arrowObj.SetActive(false);

        Invoke(nameof(WaitFoeNextShoot), 0.3f);

        for (int i = 0; i < shootParticle.Count; i++)
        {
            shootParticle[i].Play();
        }

        GameObject bullet = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = bullet.transform.forward * arrowSpeed;
    }

    private void WaitFoeNextShoot()
    {
        isLoaded = true;
        arrowObj.SetActive(true);
    }
}
