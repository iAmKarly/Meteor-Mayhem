using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : MonoBehaviour
{
    public float damage = 25;

    [Tooltip("The animation of the bullet impact.")]
    [SerializeField] private GameObject basicImpactVFX;
    [Tooltip("The speed of the bullet.")]
    [SerializeField] private float speed = 0f;
    [Tooltip("The direction of the bullet.")]
    [SerializeField] private Vector3 direction;

    private bool collided;
    private Vector3 velocity;

    void Start(){
        tag = "bullet";
        GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
        GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;

        velocity = direction * speed;
    }

    void Update(){
        GetComponent<Rigidbody>().velocity = velocity;
    }

    void OnCollisionEnter (Collision co){
        // Destroy the bullet and particles after hitting a target or environment
        if(co.gameObject.tag != "bullet" && co.gameObject.tag != "Player" && co.gameObject.tag != "particle" && co.gameObject.tag != "LeftHandObj" && co.gameObject.tag != "RightHandObj" && co.gameObject.tag != "LeftHand" && co.gameObject.tag != "RightHand" && co.gameObject.tag != "Body"&& !collided){
            collided = true;
            var impact = Instantiate(basicImpactVFX, co.contacts[0].point, Quaternion.identity) as GameObject;
            Destroy(impact, 2);
            Destroy(gameObject);
        }
        if(co.gameObject.tag == "bullet"){
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
        GetComponent<BasicProjectile>().direction = direction;
        if (direction == null){
            success = false;
        }
        return success;
    }
}
