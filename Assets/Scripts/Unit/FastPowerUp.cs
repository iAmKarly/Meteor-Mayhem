using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FastPowerUp : MonoBehaviour
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
            unitManager.GetComponent<PowerUpSpawner>().StartCoroutine(activateFastPowerUp(timeLimit));
        }
    }


    /// <summary>
    /// Activates the fast power up for a duration.
    /// </summary>
    /// <param name="seconds">The duration of the power up.</param>
    /// <returns>Returns IEnumerator.</returns>
    IEnumerator activateFastPowerUp(int seconds)
    {
        float originalSpeed = BasicProjectile.getSpeed();
        if (originalSpeed < 500){
            BasicProjectile.setSpeed(originalSpeed + 200);
            yield return new WaitForSeconds(seconds);
            BasicProjectile.setSpeed(originalSpeed);
        }
    }
}
