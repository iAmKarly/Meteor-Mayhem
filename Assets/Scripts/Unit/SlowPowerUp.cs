using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlowPowerUp : MonoBehaviour
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
        unitManager.GetComponent<PowerUpSpawner>().StartCoroutine(activateSlowPowerUp(timeLimit));
    }

    /// <summary>
    /// Activates the slow power up for a duration.
    /// </summary>
    /// <param name="seconds">The duration of the power up.</param>
    /// <returns>Returns IEnumerator.</returns>
    IEnumerator activateSlowPowerUp(int seconds)
    {
        float originalSpeed = MeteorBasic.getSpeed();
        if (originalSpeed > 1){
            MeteorBasic.setSpeed(originalSpeed - 3);
            yield return new WaitForSeconds(seconds);
            MeteorBasic.setSpeed(originalSpeed);
        }
    }
}
