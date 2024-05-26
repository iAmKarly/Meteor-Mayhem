using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeteorBasic : MonoBehaviour
{
    public GameObject meteorHitVFX;
    public Slider slider;

    public float health = 100;
    public float maxHealth = 100;
    public float damage;
    public float speed = 5f;

    private Vector3 target;
    private Vector3 rotation;

    void Start(){
        rotation= new Vector3(0,0,0);
        transform.LookAt(target);
    }

    void Update(){
        // Move the meteor towards the target
        Vector3 direction = (target - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Rotate the meteor
        rotation.x += Random.Range(-5f, 5f);
        rotation.y += Random.Range(-5f, 5f);
        rotation.z += Random.Range(-5f, 5f);
        MeteorBasic meteorObj = GetComponentInChildren<MeteorBasic>();
        GameObject meteorAsset = transform.GetChild(0).gameObject;
        meteorAsset.transform.Rotate(rotation * Time.deltaTime);

        if(Vector3.Distance(target, transform.position) < 5f)
        {
            Destroy(gameObject);
        }
    }
    
    void OnCollisionEnter (Collision co)
    {
        // If hit by a bullet, subtract health by the damage of the bullet and update healthbar
        if(co.gameObject.tag == "bullet")  
        {
            BasicProjectile basicProjectile= co.gameObject.GetComponent<BasicProjectile>();
            health = health - basicProjectile.damage;
            slider.value = health/maxHealth; 
        }
        if(health <= 0)
        {
            var impact = Instantiate(meteorHitVFX, transform.position, Quaternion.identity) as GameObject;
            Destroy(impact, 2);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Set the target where meteor moves towards.
    /// </summary>
    /// <param name="target">Target where meteor moves towards.</param>
    /// <returns>Returns True if target is set.</returns>
    public bool setTarget(Vector3 target){
        bool success = false;
        this.target = target;
        if (this.target != null){
            success = true;
        }
        return success;
    }
}
