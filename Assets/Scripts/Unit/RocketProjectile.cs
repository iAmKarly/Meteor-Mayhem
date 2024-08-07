using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketProjectile : MonoBehaviour
{
    public float damage = 25;

    [Tooltip("The animation of the bullet impact.")]
    [SerializeField] private GameObject basicImpactVFX;
    [Tooltip("The speed of the bullet.")]
    [SerializeField] private static float speed = 50f;
    [Tooltip("The direction of the bullet.")]
    [SerializeField] private Vector3 direction;

    private bool collided;
    private Vector3 velocity;

    private Vector3 origin;

    void Start(){
        tag = "bullet";
        GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
        GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;

        velocity = direction * speed;
        
    }

    void Update(){
        GetComponent<Rigidbody>().velocity = velocity;
        transform.rotation = Quaternion.LookRotation(origin, direction);
    }

    // public LayerMask layerMask = 1;
    void OnCollisionEnter (Collision co){
        // Destroy the bullet and particles after hitting a target or environment
        if(co.gameObject.tag != "bullet" && co.gameObject.tag != "Player" && co.gameObject.tag != "particle" && co.gameObject.tag != "LeftHandObj" && co.gameObject.tag != "RightHandObj" && co.gameObject.tag != "LeftHand" && co.gameObject.tag != "RightHand" && co.gameObject.tag != "RocketExplosion" && co.gameObject.tag != "Body"  && !collided){
            collided = true;
            var impact = Instantiate(basicImpactVFX, co.contacts[0].point, Quaternion.identity) as GameObject;
            Destroy(impact, 2);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Set the direction where projectile goes
    /// </summary>
    /// <param name="direction">The direction of the projectile.</param>
    /// <returns>Returns bool if direction is succesfully set.</returns>
    public bool setProjectileData(Vector3 direction){
        bool success = true;
        GetComponent<RocketProjectile>().direction = direction;
        if (direction == null){
            success = false;
        }
        return success;
    }

    /// <summary>
    /// Set the origin of the rocket
    /// </summary>
    /// <param name="origin">The origin of the rocket.</param>
    public void setOrigin(Vector3 origin){
        this.origin = origin;
    }

    /// <summary>
    /// Set speed of the bullet.
    /// </summary>
    /// <param name="newSpeed">The ammount of speed set </param>
    public static void setSpeed(float newSpeed){
        RocketProjectile.speed = newSpeed;
    }

    /// <summary>
    /// Lower speed of the meteor.
    /// </summary>
    /// <return>Returns the speed.</return>
    public static float getSpeed(){
        return RocketProjectile.speed;
    }
}
