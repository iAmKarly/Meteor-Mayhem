using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeteorBasic : MonoBehaviour
{
    public GameObject meteorHitVFX;
    public GameObject healthBarUI;
    public Slider slider;

    public float health = 100;
    public float maxHealth = 100;
    public float damage;
    
    void OnCollisionEnter (Collision co)
    {
        // If hit by a bullet, subtract health by the damage of the bullet and update healthbar
        if(co.gameObject.tag == "bullet")
        {
            BasicProjectile basicProjectile= co.gameObject.GetComponent<BasicProjectile>();
            health = health - basicProjectile.damage;

            slider.value = calculateHealth();
            
            // If health <= 0 create particles and destroy itself
            if(health <= 0)
            {
                // var impact = Instantiate(meteorHitVFX, gameObject.transform.localPosition, Quaternion.identity) as GameObject;
                var impact = Instantiate(meteorHitVFX, co.contacts[0].point, Quaternion.identity) as GameObject;
                Destroy(impact, 2);
                Destroy(gameObject);
            }
        }
    }
    float calculateHealth()
    {
        return health/maxHealth;
    }
}
