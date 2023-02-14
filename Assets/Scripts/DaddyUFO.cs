using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaddyUFO : MonoBehaviour
{
    public GameObject laserPrefab; // Prefab of the laser

    GameObject laser;

    [SerializeField] private float fireRate = 0.5f; // Time between shots
    [SerializeField] private float speed = 10.0f; // Speed of the enemy ship
    [SerializeField] private float changeDirectionTime = 2.0f; // Time to change direction
    private float nextFireTime = 0.0f; // Next time the laser can fire
    private float changeDirectionTimer = 0.0f; // Timer for changing direction
    private Vector3 movement = Vector3.zero;


    void Start()
    {
        // Set the initial movement direction
        movement = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0);
    }

    void Update()
    {
        DaddyUFOShip();
    }

    public void timeToDestroy()
    {
        Destroy(laser);
    }

    public void DaddyUFOShip()
    {
        // Move the ship along the x and y axis
        transform.position += movement * Time.deltaTime * speed;

        // Update the timer for changing direction
        changeDirectionTimer += Time.deltaTime;

        // Change the movement direction after a certain amount of time
        if (changeDirectionTimer >= changeDirectionTime)
        {
            changeDirectionTimer = 0;
            movement = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0);
        }

        // Shoot a laser in a random direction
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;

            Vector3 laserDirection = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0);
            laserDirection = laserDirection.normalized;
            laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            laser.GetComponent<Rigidbody>().velocity = laserDirection * 10;
            Invoke("timeToDestroy", 2f);
            
        }
    }



}

