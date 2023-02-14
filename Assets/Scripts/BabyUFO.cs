using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyUFO : MonoBehaviour
{
    public GameObject laserPrefab;

    public float moveSpeed = 5f;
    public float shotSpeed = 10f;
    public float fireRate = 0.5f;
    public float angleRange = 15f;

    [SerializeField] private float changeDirectionTime = 2.0f; // Time to change direction

    private GameManager gameManager;

    private Vector3 fireDirection;
    private float score;
    private float changeDirectionTimer = 0.0f; // Timer for changing direction
    private float zPos = 10f;



    private Vector3 movement = Vector3.zero;

    void Start()
    {
        gameManager = GameManager.Instance;
        score = gameManager.score;
        StartCoroutine(Fire());
        movement = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0);
    }

    private void Update()
    {
        BabyUfoShip();
    }

    public void BabyUfoShip()
    {
        // Move the ship along the x and y axis
        transform.position += movement * Time.deltaTime * moveSpeed;

        // Update the timer for changing direction
        changeDirectionTimer += Time.deltaTime;

        // Change the movement direction after a certain amount of time
        if (changeDirectionTimer >= changeDirectionTime)
        {
            changeDirectionTimer = 0;
            movement = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0);
        }
    }

    IEnumerator Fire()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireRate);
            score = gameManager.score;

            if (score < 200)
                angleRange = 15f;
            else if (score >= 200 && score < 400)
                angleRange = 10f;
            else if (score >= 400 && score < 950)
                angleRange = 5f;
            else
                angleRange = 1f;

            fireDirection = transform.forward + new Vector3(Random.Range(-angleRange, angleRange), Random.Range(-angleRange, angleRange), 0);
            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            laser.GetComponent<Rigidbody>().velocity = fireDirection * moveSpeed * shotSpeed;
        }
    }
}
