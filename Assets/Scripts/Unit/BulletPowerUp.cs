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
    
    void OnCollisionEnter (Collision co)
    {
        if(co.gameObject.tag == "bullet")  
        {
            activatePowerUp();
        }
    }

    /// <summary>
    /// Activate the power up
    /// </summary>
    public void activatePowerUp()
    {
        unitManager.GetComponent<PowerUpSpawner>().StartCoroutine(activateBulletPowerUp(timeLimit));
    }

    /// <summary>
    /// Activates the bullet power up for a duration.
    /// </summary>
    /// <param name="seconds">The duration of the power up.</param>
    /// <returns>Returns IEnumerator.</returns>
    IEnumerator activateBulletPowerUp(int seconds)
    {
        float originalFireRateBasic = ShooterBasic.getFireRate();
        if (originalFireRateBasic < 6){
            ShooterBasic.setFireRate(originalFireRateBasic + 5);
            yield return new WaitForSeconds(seconds);
            ShooterBasic.setFireRate(originalFireRateBasic);
        }
        float originalFireRateRocket = ShooterRocket.getFireRate();
        if (originalFireRateRocket < 2){
            ShooterRocket.setFireRate(originalFireRateRocket + 1);
            yield return new WaitForSeconds(seconds);
            ShooterRocket.setFireRate(originalFireRateRocket);
        }
        float originalFireRateLaser = ShooterLaser.getFireRate();
        if (originalFireRateLaser < 21){
            ShooterLaser.setFireRate(originalFireRateLaser + 10);
            yield return new WaitForSeconds(seconds);
            ShooterLaser.setFireRate(originalFireRateLaser);
        }
    }
}
