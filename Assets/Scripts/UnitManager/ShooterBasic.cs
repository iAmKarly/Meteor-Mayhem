using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterBasic : MonoBehaviour
{
    [Tooltip("The source of firepoints")]
    [SerializeField] private Transform LHFirePoint, RHFirePoint;
    [Tooltip("Source of mouse data.")]
    [SerializeField] private MouseLook mouseLook;
    [Tooltip("Source of hand data.")]
    [SerializeField] private HandTracker handTracker;
    [Tooltip("Projectile prefab.")]
    [SerializeField] private GameObject projectileBasic;
    [Tooltip("How often can the player shoot.")]
    [SerializeField] private static float firerate = 5f;

    [Tooltip("Audio clip for shooting sound.")]
    [SerializeField] private AudioClip shootingSound;

    private float timeToFire = 0;
    private bool leftHand;
    private AudioSource audioSource;

    void Start()
    {
        // Get or add an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Fires a projectile when the ammount of finger up is more than 3 based on fire rate
        if (handTracker.checkHandRecognized())
        {
            if (Time.time >= timeToFire)
            {
                timeToFire = Time.time + 1 / firerate;
                if (handTracker.checkShoot(true))
                {
                    InstantiateProjectile(handTracker.firstHandOrigin, handTracker.firstHandDirection);
                }
                if (handTracker.checkShoot(false))
                {
                    InstantiateProjectile(handTracker.secondHandOrigin, handTracker.secondHandDirection);
                }
            }
        }
        else
        {
            if (mouseLook.checkShoot())
            {
                if (Time.time >= timeToFire)
                {
                    timeToFire = Time.time + 1 / firerate;
                    if (leftHand)
                    {
                        leftHand = false;
                        InstantiateProjectile(LHFirePoint.position, mouseLook.viewDirection);
                    }
                    else
                    {
                        leftHand = true;
                        InstantiateProjectile(RHFirePoint.position, mouseLook.viewDirection);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Instatntiates a projectile at the firepoint and launches it in a direction.
    /// After 5 seconds, destroys the projectile.
    /// </summary>
    /// <param name="firepoint">The origin point of the projectile.</param>
    /// <param name="direction">The direction of the projectile.</param>
    /// <returns>Returns the projectile created</returns>
    GameObject InstantiateProjectile(Vector3 firepoint, Vector3 direction)
    {
        GameObject projectileBasicObj = Instantiate(projectileBasic, firepoint, Quaternion.identity) as GameObject;

        projectileBasicObj.GetComponent<BasicProjectile>().setProjectileData(direction);

        Destroy(projectileBasicObj, 4);

        // Play shooting sound
        if (shootingSound != null)
        {
            audioSource.PlayOneShot(shootingSound);
        }

        return projectileBasicObj;
    }

    /// <summary>
    /// Set firerate of the shooter.
    /// </summary>
    /// <param name="newFirerate">The ammount of speed set </param>
    public static void setFireRate(float newFirerate)
    {
        ShooterBasic.firerate = newFirerate;
    }

    /// <summary>
    /// Return firerate of the shooter.
    /// </summary>
    /// <return>Returns the speed.</return>
    public static float getFireRate()
    {
        return ShooterBasic.firerate;
    }
}
