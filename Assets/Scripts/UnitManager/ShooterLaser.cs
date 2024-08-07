using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShooterLaser : MonoBehaviour
{
    [Tooltip("Source of hand data.")]
    [SerializeField] private HandTracker handTracker;
    [Tooltip("How often can the player shoot.")]
    [SerializeField] private static float firerate = 20f;

    [Tooltip("Audio clip for shooting sound.")]
    [SerializeField] private AudioClip shootingSound;
    [Tooltip("Animator Script.")]
    [SerializeField] private HandAnimator handAnimator;
    [Tooltip("Damage.")]
    [SerializeField] private int damage = 10;


    private float timeToFire = 0;
    private AudioSource audioSource;
    private GameObject firstHandLine;
    private GameObject secondHandLine;
    private LineRenderer firstHandLineLR;
    private LineRenderer secondHandLineLR;
    private float startTimeAudio = 2f;
    private float endTimeAudio = 3f;

    private Coroutine laserSoundCoroutine;
    private bool shouldPlayLaser = false;

    void Start()
    {
        firerate = 20f;
        // Get or add an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        firstHandLine = new GameObject("FirstHandDirection");
        firstHandLineLR = firstHandLine.AddComponent<LineRenderer>();
        firstHandLineLR.startWidth = 0.2f;
        firstHandLineLR.endWidth = 0.2f;
        firstHandLineLR.SetPosition(0, new Vector3(0,0,-1000));
        firstHandLineLR.SetPosition(1, new Vector3(0,0,-1000));
        firstHandLine.tag = "Player";

        secondHandLine = new GameObject("SecondHandDirection");
        secondHandLineLR = secondHandLine.AddComponent<LineRenderer>();
        secondHandLineLR.startWidth = 0.1f;
        secondHandLineLR.endWidth = 0.1f;
        secondHandLineLR.SetPosition(0, new Vector3(0,0,-1000));
        secondHandLineLR.SetPosition(1, new Vector3(0,0,-1000));
        secondHandLine.tag = "Player";
    }

    // Update is called once per frame
    void Update()
    {
        // Fires a projectile when the ammount of finger up is more than 3 based on fire rate
        if (handTracker.checkHandRecognized())
        {
            if (handTracker.checkShootLaser(true))
                {
                    shootLaser(true, handTracker.firstHandOrigin, handTracker.firstHandDirection);
                }
            else
            {
                firstHandLineLR.enabled = false;
                StopLaserSound();
            }
            if (handTracker.checkShootLaser(false))
                {
                    shootLaser(false, handTracker.secondHandOrigin, handTracker.secondHandDirection);
                }
            else
            {
                StopLaserSound();
                secondHandLineLR.enabled = false;
            }
            handAnimator.idleLeft();
            handAnimator.idleRight();  
        }
        else{
            StopLaserSound();
            firstHandLineLR.enabled = false;
            secondHandLineLR.enabled = false;
        }
    }

    /// <summary>
    /// Set firerate of the shooter.
    /// </summary>
    /// <param name="newFirerate">The ammount of speed set </param>
    public static void setFireRate(float newFirerate)
    {
        ShooterLaser.firerate = newFirerate;
    }

    /// <summary>
    /// Return firerate of the shooter.
    /// </summary>
    /// <return>Returns the speed.</return>
    public static float getFireRate()
    {
        return ShooterLaser.firerate;
    }

    /// <summary>
    /// Play the laser sound effect
    /// </summary>
    public void StartLaserSound(AudioClip laserSound)
    {
        if (laserSoundCoroutine == null)
        {
            shouldPlayLaser = true;
            laserSoundCoroutine = StartCoroutine(PlayLaserSound(laserSound));
        }
    }

    public void StopLaserSound()
    {
        shouldPlayLaser = false;
        
    }

    private IEnumerator PlayLaserSound(AudioClip laserSound)
    {
        if (laserSound == null)
        {
            Debug.LogError("No laser sound clip provided");
            yield break;
        }

        audioSource.clip = laserSound;
        audioSource.loop = false;  // We'll handle looping manually

        while (shouldPlayLaser || audioSource.isPlaying)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.time = 1;
                audioSource.Play();
            }

            if (audioSource.time >= endTimeAudio)
            {
                if (shouldPlayLaser)
                {
                    audioSource.time = startTimeAudio;
                }
                else
                {
                    break;  // Exit the loop if we've reached the end and shouldn't continue
                }
            }

            yield return null;
        }
        audioSource.Stop();
        laserSoundCoroutine = null;
    }

    void shootLaser(bool first, Vector3 origin, Vector3 direction){
        StartLaserSound(shootingSound);
                    
        Ray handRay = new Ray(origin, direction);
        Vector3 endPosition = origin + direction.normalized * 300;

        if(Physics.Raycast(handRay, out RaycastHit hit, 300)){
            endPosition = hit.point;
            
            if (Time.time >= timeToFire)
            {
                timeToFire = Time.time + 2 / firerate ;
                MeteorBasic meteor = hit.transform.gameObject.GetComponent<MeteorBasic>();
                BasicPowerUp BasicPowerUp = hit.transform.gameObject.GetComponent<BasicPowerUp>();
                if(meteor != null){
                    meteor.dealDamage(10);
                }
                if(BasicPowerUp != null){
                    BasicPowerUp.activatePowerUp();
                }
            }
        }

        if(first){
            firstHandLineLR.enabled = true;
            firstHandLineLR.SetPosition(0, origin);
            firstHandLineLR.SetPosition(1, endPosition);
        }
        else{
            secondHandLineLR.enabled = true;
            secondHandLineLR.SetPosition(0, origin);
            secondHandLineLR.SetPosition(1, endPosition);
        }
    }
}
