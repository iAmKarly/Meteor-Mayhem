using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NukePowerUp : MonoBehaviour
{
    [Tooltip("How long the power up delay.")]
    [SerializeField] private float delay = 1f;
    private GameObject bigExplosion;
    private GameObject unitManager;
    private ScoreHandler scoreHandler;
    void Start(){
        unitManager = GameObject.FindGameObjectWithTag("UnitManager").gameObject;
        scoreHandler = GameObject.FindGameObjectWithTag("ScoreHandler").GetComponent<ScoreHandler>();
        bigExplosion = Resources.Load<GameObject>("Prefabs/Explosion/TinyExplosion");
        bigExplosion.transform.localScale = new Vector3(200,200,200);
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
        var impact = Instantiate(bigExplosion, transform.position, Quaternion.identity) as GameObject;
        Destroy(impact, 1);
        unitManager.GetComponent<PowerUpSpawner>().StartCoroutine(activateNukePowerUp(delay));
    }

    /// <summary>
    /// Activates the nuke power up after a duration.
    /// </summary>
    /// <param name="seconds">The duration of the delay.</param>
    /// <returns>Returns IEnumerator.</returns>
    IEnumerator activateNukePowerUp(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        foreach (var meteors in GameObject.FindGameObjectsWithTag("Meteor")){
            meteors.GetComponent<MeteorBasic>().destroyMeteor();
            scoreHandler.addScore(1);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
