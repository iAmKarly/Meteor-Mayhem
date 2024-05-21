using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : MonoBehaviour
{
    public GameObject basicImpactVFX;
    public float damage = 50;
    private bool collided;
    void OnCollisionEnter (Collision co)
    {
        // Destroy the bullet and particles after hitting a target or environment
        if(co.gameObject.tag != "bullet" && co.gameObject.tag != "Player" && co.gameObject.tag != "particle" && !collided)
        {
            collided = true;
            var impact = Instantiate(basicImpactVFX, co.contacts[0].point, Quaternion.identity) as GameObject;
            Destroy(impact, 2);
            Destroy(gameObject);
        }
    }
}
