using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletPowerUp : MonoBehaviour
{
    [Tooltip("How long the power up last.")]
    [SerializeField] private int timeLimit = 10;
    private GameObject unitManager;
    void Start(){
        unitManager = GameObject.FindGameObjectWithTag("UnitManager").gameObject;
    }

    void Update(){

    }
    
    void OnCollisionEnter (Collision co)
    {
        if(co.gameObject.tag == "bullet")  
        {
            unitManager.GetComponent<PowerUpSpawner>().StartCoroutine(activateBulletPowerUp(timeLimit));
        }
    }


    /// <summary>
    /// Activates the bullet power up for a duration.
    /// </summary>
    /// <param name="seconds">The duration of the power up.</param>
    /// <returns>Returns IEnumerator.</returns>
    IEnumerator activateBulletPowerUp(int seconds)
    {
        float originalFireRate = ShooterBasic.getFireRate();
        if (originalFireRate < 15){
            ShooterBasic.setFireRate(originalFireRate + 5);
            yield return new WaitForSeconds(seconds);
            ShooterBasic.setFireRate(originalFireRate);
        }
    }
}
