using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RocketExplosion : MonoBehaviour
{   
    
    [Tooltip("How much damage a explosion does.")]
    [SerializeField] private int damage = 50;
    [Tooltip("Radius of the explosion.")]
    [SerializeField] private float radius = 20.0f;

    void Start(){
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var hitCollider in hitColliders)
        {
            if(hitCollider.tag == "Meteor"){
                hitCollider.GetComponent<MeteorBasic>().dealDamage(damage);
            }
            if(hitCollider.tag == "powerup"){
                hitCollider.GetComponent<BasicPowerUp>().activatePowerUp();
            }
        }
    }
}
