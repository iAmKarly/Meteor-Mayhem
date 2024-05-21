using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Meteor : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] meteorPrefab;
    public GameObject target;
    public int range;
    public float speed = 1f;
    public float spawnRate = 2;

    private float timeToSpawn = 0;
    
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate how often the meteor spawns based on spawn rate
        // Then summons a meteor
        if(Time.time >= timeToSpawn)
        {
            timeToSpawn = Time.time + 1/spawnRate;
            float x, y ,z;
            x = 6.5f + Random.Range(-range, range);
            y = transform.position.y + Random.Range(0, range/2);
            z = -50;
            GameObject meteor = Instantiate(meteorPrefab[Random.Range(0, meteorPrefab.Length)], new Vector3(x, y , z), Random.rotation) as GameObject;
            meteor.transform.LookAt(target.transform);
            StartCoroutine(SendHoming(meteor));
        }
    }

    public IEnumerator SendHoming(GameObject meteor)
    {
        // Define the final meteor rotation
        // Update the meteor's position and rotation while it is not destroyed until it reaches targer position
        MeteorBasic meteorObj = meteor.GetComponentInChildren<MeteorBasic>();
        GameObject meteorAsset = meteor.transform.GetChild(0).gameObject;
        
        Quaternion rotation = Random.rotation;
        while (meteorObj.health > 0 && Vector3.Distance(target.transform.position, meteor.transform.position) > 5f)
        {
            meteor.transform.position += (target.transform.position - meteor.transform.position).normalized * speed * Time.deltaTime;
            meteorAsset.transform.rotation = Quaternion.Slerp(meteorAsset.transform.rotation, rotation, Time.deltaTime);

            yield return null;
        }
        // If it hits the player, will destroy itself
        hitPlayer(ref meteor);
    }
    void hitPlayer(ref GameObject meteor)
    {
        Destroy(meteor); 
    }
}
