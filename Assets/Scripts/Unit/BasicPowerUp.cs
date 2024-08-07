using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicPowerUp : MonoBehaviour
{
    
    [Tooltip("The effect of power up being hit.")]
    [SerializeField] private GameObject powerUpHitVFX;
    [Tooltip("The speed of the meteor.")]
    [SerializeField] private float speed = 5f;

    private Vector3 target;
    private GameObject body;

    void Start(){
        transform.LookAt(target);
        body = GameObject.FindGameObjectWithTag("Body").gameObject;
    }

    void Update(){
        // Move the power up towards the target
        Vector3 direction = (target - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Spins the power up
        Vector3 rotation = transform.localRotation.eulerAngles;
        rotation.y += 50 * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);

        if(Vector3.Distance(target, transform.position) < 5f){
            Destroy(gameObject);
        }

        if(Vector3.Distance(target, transform.position) > 30f){
            target.x = body.transform.localPosition.x;
        }

    }
    
    void OnCollisionEnter (Collision co)
    {
        if(co.gameObject.tag == "bullet")  
        {
            activatePowerUp ();
        }
    }

    /// <summary>
    /// Activate the power up based on the type of power up itself
    /// </summary>
    public void activatePowerUp ()
    {
        var impact = Instantiate(powerUpHitVFX, transform.position, Quaternion.identity) as GameObject;
        BulletPowerUp BPU = GetComponent<BulletPowerUp>(); 
        FastPowerUp FPU = GetComponent<FastPowerUp>();
        HeartPowerUp HPU = GetComponent<HeartPowerUp>();
        NukePowerUp NPU = GetComponent<NukePowerUp>();
        SlowPowerUp SPU = GetComponent<SlowPowerUp>();

        if(BPU != null){
            BPU.activatePowerUp();
        }
        if(FPU != null){
            FPU.activatePowerUp();
        }
        if(HPU != null){
            HPU.activatePowerUp();
        }
        if(NPU != null){
            NPU.activatePowerUp();
        }
        if(SPU != null){
            SPU.activatePowerUp();
        }

        Destroy(impact, 2);
        Destroy(gameObject);
    }

    /// <summary>
    /// Set the target where power up moves towards.
    /// </summary>
    /// <param name="target">Target where power up moves towards.</param>
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
