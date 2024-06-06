using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartPowerUp : MonoBehaviour
{
    private HeartHandler heartHandler;

    void Start(){
        heartHandler = GameObject.FindGameObjectWithTag("HeartHandler").GetComponent<HeartHandler>();
    }

    void Update(){

    }
    
    void OnCollisionEnter (Collision co)
    {
        if(co.gameObject.tag == "bullet")  
        {
            heartHandler.increaseHearts();
        }
    }
}
