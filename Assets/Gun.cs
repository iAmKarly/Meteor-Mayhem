using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    
    public GameObject bulletPrefab;
    public float bulletSpeed = 10;
    public float movementSpeed = 5;

    void Update()
    {

        //Horizontal movements
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * horizontalInput * movementSpeed * Time.deltaTime);

        UpdateBulletSpawnPointPosition();

        // Aim based on mouse position
        Aim();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void Aim()
    {
        // Get the mouse position in screen coordinates
        Vector3 mousePosition = Input.mousePosition;

        // Convert the mouse position to a ray in world space
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        // Calculate the direction to aim (point from bulletSpawnPoint towards the ray)
        Vector3 aimDirection = ray.direction;

        // Update the rotation of the bulletSpawnPoint to match the aim direction
        bulletSpawnPoint.rotation = Quaternion.LookRotation(aimDirection);
    }

    void UpdateBulletSpawnPointPosition()
    {
        bulletSpawnPoint.position = transform.position;
    }

    void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * bulletSpeed;
    }
}

