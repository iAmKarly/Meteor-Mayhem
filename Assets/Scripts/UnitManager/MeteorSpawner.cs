using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Meteor : MonoBehaviour
{
    [Tooltip("The prefabs of the meteors.")]
    [SerializeField] private GameObject[] meteorPrefab;
    [Tooltip("The target where the meteor moves towards.")]
    [SerializeField] private GameObject target;
    [Tooltip("The x and y range where meteor may spawn.")]
    [SerializeField] private int range;
    [Tooltip("The spawnrate of the meteor")]
    [SerializeField] private float spawnRate = 2;

    private float timeToSpawn = 0;
    
    // Start is called before the first frame update
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
            y = Random.Range(-range/2, range/2);
            z = -100;
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