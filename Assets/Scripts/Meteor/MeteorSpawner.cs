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
            createMeteor(x, y, z);
        }
    }

    /// <summary>
    /// Spawns a meteor at a location.
    /// </summary>
    /// <param name="x">The X coordinate where the meteor will spawn.</param>
    /// <param name="y">The Y coordinate where the meteor will spawn.</param>
    /// <param name="z">The Z coordinate where the meteor will spawn.</param>
    /// <returns>Returns the meteor created./returns>
    GameObject createMeteor(float x, float y, float z){
        GameObject meteor = Instantiate(meteorPrefab[Random.Range(0, meteorPrefab.Length)], new Vector3(x, y , z), Random.rotation) as GameObject;
        Vector3 targetVector = target.transform.position;
        meteor.GetComponent<MeteorBasic>().setTarget(targetVector);
        return meteor;
    }
}
