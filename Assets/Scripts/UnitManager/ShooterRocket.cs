using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShooterRocket : MonoBehaviour
{
    [Tooltip("Source of hand data.")]
    [SerializeField] private HandTracker handTracker;
    [Tooltip("Projectile prefab.")]
    [SerializeField] private GameObject projectileRocket;
    [Tooltip("How often can the player shoot.")]
    [SerializeField] private static float firerate = 1f;

    [Tooltip("Audio clip for shooting sound.")]
    [SerializeField] private AudioClip shootingSound;
    [Tooltip("Animator Script.")]
    [SerializeField] private HandAnimator handAnimator;

    private float timeToFire = 0;
    private AudioSource audioSource;

    void Start()
    {
        firerate = 1f;
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
                timeToFire = Time.time + 2 / firerate ;
                if (handTracker.checkShootRocket(true))
                {
                    InstantiateProjectile(handTracker.firstHandOrigin, handTracker.firstHandDirection);
                    if(String.Equals(handTracker.checkHandType(true), "Left")){
                        handAnimator.shootLeft();
                    }
                    if(String.Equals(handTracker.checkHandType(true), "Right")){
                        handAnimator.shootRight();
                    }
                }
                if (handTracker.checkShootRocket(false))
                {
                    InstantiateProjectile(handTracker.secondHandOrigin, handTracker.secondHandDirection);
                    if(String.Equals(handTracker.checkHandType(false), "Left")){
                        handAnimator.shootLeft();
                    }
                    if(String.Equals(handTracker.checkHandType(false), "Right")){
                        handAnimator.shootRight();
                    }
                }
                handAnimator.idleLeft();
                handAnimator.idleRight();   
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
        Vector3 newFirePoint = firepoint;
        newFirePoint.z = newFirePoint.z - 1;
        GameObject projectileRocketObj = Instantiate(projectileRocket, newFirePoint, Quaternion.LookRotation(newFirePoint, direction)) as GameObject;

        projectileRocketObj.GetComponent<RocketProjectile>().setProjectileData(direction);
        projectileRocketObj.GetComponent<RocketProjectile>().setOrigin(firepoint);

        Destroy(projectileRocketObj, 4);

        // Play shooting sound
        if (shootingSound != null)
        {
            audioSource.PlayOneShot(shootingSound);
        }

        return projectileRocketObj;
    }

    /// <summary>
    /// Set firerate of the shooter.
    /// </summary>
    /// <param name="newFirerate">The ammount of speed set </param>
    public static void setFireRate(float newFirerate)
    {
        ShooterRocket.firerate = newFirerate;
    }

    /// <summary>
    /// Return firerate of the shooter.
    /// </summary>
    /// <return>Returns the speed.</return>
    public static float getFireRate()
    {
        return ShooterRocket.firerate;
    }
}
