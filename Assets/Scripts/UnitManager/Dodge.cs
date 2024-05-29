using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

public class Dodge : MonoBehaviour
{
    [Tooltip("Source of hand data.")]
    [SerializeField] private HandTracker handTracker;

    private GameObject body;
    private int position = 0;
    private bool isDodging;
    private float distanceTraveled = 0;
    private float timeToDodge = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        body = GameObject.FindGameObjectWithTag("Body").gameObject;
    }

    // Update is called once per frame
    void Update()
    {   
        if(!isDodging && Time.time >= timeToDodge){
            if(handTracker.checkHandRecognized()){
                if(String.Equals(handTracker.dodging(), "Right") && position < 2 ){
                    timeToDodge = Time.time + 1;
                    position += 1;
                    StartCoroutine(dodgeRight());
                }
                if(String.Equals(handTracker.dodging(), "Left") && position > -2){
                    timeToDodge = Time.time + 1;
                    position -= 1;
                    StartCoroutine(dodgeLeft());
                } 
            }
            else{
                if(Input.GetKeyDown(KeyCode.D) && position < 2 ){
                    timeToDodge = Time.time + 1;
                    position += 1;
                    StartCoroutine(dodgeRight());
                }
                if(Input.GetKeyDown(KeyCode.A ) && position > -2){
                    timeToDodge = Time.time + 1;
                    position -= 1;
                    StartCoroutine(dodgeLeft());
                } 
            }
        }
    }

    /// <summary>
    /// Moves the player to the left
    /// </summary>
    /// <returns>Returns IEnumerator.</returns>
    IEnumerator dodgeLeft(){
        isDodging = true;
        distanceTraveled = 0;
        while (distanceTraveled < 4)
        {
            body.GetComponent<Rigidbody>().velocity = new Vector3(30, 0, 0);
            distanceTraveled += Mathf.Abs(body.GetComponent<Rigidbody>().velocity.x) * Time.deltaTime;
            yield return null;
        }
        body.GetComponent<Rigidbody>().velocity = Vector3.zero;
        isDodging = false;
    }

    /// <summary>
    /// Moves the player to the left
    /// </summary>
    /// <returns>Returns IEnumerator.</returns>
    IEnumerator dodgeRight(){
        isDodging = true;
        distanceTraveled = 0;
        while (distanceTraveled < 5)
        {
            body.GetComponent<Rigidbody>().velocity = new Vector3(-30, 0, 0);
            distanceTraveled += Mathf.Abs(body.GetComponent<Rigidbody>().velocity.x) * Time.deltaTime;
            yield return null;
        }
        body.GetComponent<Rigidbody>().velocity = Vector3.zero;
        isDodging = false;
    }
}
