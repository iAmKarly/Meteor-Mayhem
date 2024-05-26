using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : MonoBehaviour
{
    private bool collided;
    private Vector3 velocity;
    public GameObject basicImpactVFX;
    public float damage = 25;
    public float speed = 100;
    public Vector3 direction;

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
        if(co.gameObject.tag != "bullet" && co.gameObject.tag != "Player" && co.gameObject.tag != "particle" && !collided){
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
