using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RansomSpawner : MonoBehaviour
{
    public GameObject asteroidPrefab; // Reference to the asteroid prefab
    public float spawnInterval = 1.0f; // Time interval between spawns
    public float moveSpeed = 1.0f; // Speed at which the asteroids move

    private float timer; // Timer to track spawning interval
    private List<GameObject> asteroids = new List<GameObject>(); // List to store spawned asteroids

    void Start()
    {
        timer = 0f; // Initialize timer
    }

    void Update()
    {
        // Increment the timer
        timer += Time.deltaTime;

        // Check if it's time to spawn an asteroid
        if (timer >= spawnInterval)
        {
            // Reset the timer
            timer = 0f;

            // Spawn an asteroid
            Vector3 randomSpawnPosition = new Vector3(Random.Range(-5, 6), 5, Random.Range(-5, 6));
            GameObject asteroid = Instantiate(asteroidPrefab, randomSpawnPosition, Quaternion.identity);
            asteroids.Add(asteroid); // Add the asteroid to the list

            // Calculate the direction towards the perspective scene
            Vector3 direction = (Camera.main.transform.position - asteroid.transform.position).normalized;

            // Add a Rigidbody component if not already present
            Rigidbody rb = asteroid.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = asteroid.AddComponent<Rigidbody>();
                rb.useGravity = false; // Disable gravity
            }

            // Apply velocity directly towards the perspective scene
            rb.velocity = direction * moveSpeed;
        }

        // Check each asteroid's position
        foreach (GameObject asteroid in asteroids)
        {
            if (asteroid != null) // Check if the asteroid is not destroyed yet
            {
                Vector3 asteroidViewportPos = Camera.main.WorldToViewportPoint(asteroid.transform.position);
                Debug.Log("Asteroid Viewport Position: " + asteroidViewportPos);

                if (asteroidViewportPos.z < 0) // Check if the asteroid is behind the camera
                {
                    Debug.Log("Asteroid is behind the camera.");
                    // Call the LoseHeart method from GameManager.
                    GameObject gameManagerObject = GameObject.Find("GameManager");
                    if (gameManagerObject != null)
                    {
                        GameManager gameManager = gameManagerObject.GetComponent<GameManager>();
                        if (gameManager != null)
                        {
                            gameManager.LoseHeart();
                        }
                        else
                        {
                            Debug.LogError("GameManager component not found on GameManager GameObject.");
                        }
                    }
                    else
                    {
                        Debug.LogError("GameManager GameObject not found.");
                    }

                    // Remove the asteroid from the list
                    asteroids.Remove(asteroid);
                    Destroy(asteroid);
                    break; // Exit the loop since the list is modified
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Destroyable"))
        {
            Destroy(collision.gameObject); // Destroy the collided object
        }
    }
}
