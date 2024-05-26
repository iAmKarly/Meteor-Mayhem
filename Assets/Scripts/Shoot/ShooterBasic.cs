using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterBasic : MonoBehaviour
{
    public FingerCounter fingerCounter;
    public HandTracker handTracker;
    // private PalmDirection palmDirection;
    public GameObject projectileBasic;
    public GameObject launchParticleBasic;
    public Transform firepoint0, firepoint1;
    public float projectileSpeedBasic = 30;
    public float firerate = 2;
    public float damage = 50;
    private Vector3 destination;
    private float timeToFire = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Firess a projectile when the ammount of finger up is more than 3 based on fire rate
        if(Time.time >= timeToFire)
        {
            timeToFire = Time.time + 1/firerate;
            if(fingerCounter.finger0 > 3)
            {
                fingerCounter.finger0 = 0;
                ShootBasicProjectile0();
            }
            if(fingerCounter.finger1 > 3)
            {
                fingerCounter.finger1 = 0;
                ShootBasicProjectile1();
            }
        }
    }

    void ShootBasicProjectile0()
    {
        //shoot projectile at aimed direction
        // Ray ray = new Ray(firepoint0.position, handTracker.perpendicularDirection0);
        // RaycastHit hit;
        // if(Physics.Raycast(ray, out hit))
        // {
        //     destination = hit.point;
        // }
        // else
        // {
        //     destination = ray.GetPoint(1000);
        // }
        
        calcPath(firepoint0, handTracker.perpendicularDirection0);

        InstantiateProjectile(firepoint0);
    }
    void ShootBasicProjectile1()
    {
        //shoot projectile at aimed direction
        // Ray ray = new Ray(firepoint1.position, handTracker.perpendicularDirection1);
        // RaycastHit hit;
        // if(Physics.Raycast(ray, out hit))
        // {
        //     destination = hit.point;
        // }
        // else
        // {
        //     destination = ray.GetPoint(1000);
        // }

        calcPath(firepoint1, handTracker.perpendicularDirection1);

        InstantiateProjectile(firepoint1);
    }

    void InstantiateProjectile(Transform firepoint)
    {
        // Creates a projecttile and launch it at the direction
        var projectileBasicObj = Instantiate(projectileBasic, firepoint.position, Quaternion.identity) as GameObject;
        projectileBasicObj.GetComponent<BasicProjectile>().damage = damage;
        projectileBasicObj.GetComponent<Rigidbody>().velocity = (destination - firepoint.position).normalized * projectileSpeedBasic;
        Destroy(projectileBasicObj, 5);
    }

    void calcPath(Transform firepoint, Vector3 perpendicularDirection)
    {
        Ray ray = new Ray(firepoint.position, perpendicularDirection);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            destination = hit.point;
        }
        else
        {
            destination = ray.GetPoint(1000);
        }
    }
}
