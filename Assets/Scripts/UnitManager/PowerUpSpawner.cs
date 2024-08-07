using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpSpawner : MonoBehaviour
{
    [Tooltip("The prefabs of the power ups.")]
    [SerializeField] private GameObject[] powerUpPrefab;
    [Tooltip("The target where the power ups moves towards.")]
    [SerializeField] private GameObject target;
    [Tooltip("The x and y range where power ups may spawn.")]
    [SerializeField] private int range;
    [Tooltip("The spawnrate of the power ups")]
    [SerializeField] private float spawnRate = 0.05f;

    private float timeToSpawn = 0;
    
    // Update is called once per frame
    void Update()
    {
        // Calculate how often the power up spawns based on spawn rate
        // Then summons a power up
        if(Time.time >= timeToSpawn)
        {
            timeToSpawn = Time.time + 1 / (spawnRate * Random.Range(1, 10)) ;
            float x, y ,z;
            x = 6.5f + Random.Range(-range, range);
            y = Random.Range(-range/2, range/2);
            z = -100;
            createPowerUp(x, y, z);
        }
    }

    /// <summary>
    /// Spawns a power up at a location.
    /// </summary>
    /// <param name="x">The X coordinate where the power up will spawn.</param>
    /// <param name="y">The Y coordinate where the power up will spawn.</param>
    /// <param name="z">The Z coordinate where the power up will spawn.</param>
    /// <returns>Returns the power up created./returns>
    GameObject createPowerUp(float x, float y, float z){
        int powerUpType = Random.Range(0, powerUpPrefab.Length);
        GameObject powerUp = Instantiate(powerUpPrefab[powerUpType], new Vector3(x, y , z), Random.rotation) as GameObject;
        Vector3 targetVector = target.transform.position;
        powerUp.GetComponent<BasicPowerUp>().setTarget(targetVector);
        switch(powerUpType){
            case 0:
                powerUp.AddComponent<HeartPowerUp>();
                break;
            case 1:
                powerUp.AddComponent<SlowPowerUp>();
                break;
            case 2:
                powerUp.AddComponent<FastPowerUp>();
                break;
            case 3:
                powerUp.AddComponent<BulletPowerUp>();
                break;
            case 4:
                powerUp.AddComponent<NukePowerUp>();
                break;
        }
        return powerUp;
    }
}
