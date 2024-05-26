using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterBasic : MonoBehaviour
{
    public HandTracker handTracker;
    public GameObject projectileBasic;
    private float timeToFire = 0;
    public float firerate = 2f;

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        // Firess a projectile when the ammount of finger up is more than 3 based on fire rate
        if(Time.time >= timeToFire){
            timeToFire = Time.time + 1/firerate;
            if(handTracker.checkShoot(true))
            {
                handTracker.firstHandFingerUp = 0;
                InstantiateProjectile(handTracker.firstHandOrigin, handTracker.firstHandDirection);
            }
            if(handTracker.checkShoot(false))
            {
                handTracker.secondHandFingerUp = 0;
                InstantiateProjectile(handTracker.secondHandOrigin, handTracker.secondHandDirection);
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
    GameObject InstantiateProjectile(Vector3 firepoint, Vector3 direction){
        var projectileBasicObj = Instantiate(projectileBasic, firepoint, Quaternion.identity) as GameObject;

        projectileBasicObj.GetComponent<BasicProjectile>().setProjectileData(direction);

        Destroy(projectileBasicObj, 5);
        return projectileBasicObj;
    }
}
