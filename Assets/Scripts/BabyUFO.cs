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
    private float timer = 0f;
    private float zPos = 10f;
    private GameManager gameManager;
    private Vector3 fireDirection;
    private float score;

    void Start()
    {
        gameManager = GameManager.Instance;
        score = gameManager.score;
        StartCoroutine(Move());
        StartCoroutine(Fire());
    }

    IEnumerator Move()
    {
        while (true)
        {
            Vector3 newPos = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), zPos);
            transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * moveSpeed);
            yield return new WaitForSeconds(2f);
        }
    }

    IEnumerator Fire()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireRate);
            score = gameManager.score;

            if (score < 50)
                angleRange = 15f;
            else if (score >= 50 && score < 200)
                angleRange = 10f;
            else if (score >= 200 && score < 450)
                angleRange = 5f;
            else
                angleRange = 1f;

            fireDirection = transform.forward + new Vector3(Random.Range(-angleRange, angleRange), Random.Range(-angleRange, angleRange), 0);
            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            laser.GetComponent<Rigidbody>().velocity = fireDirection * moveSpeed * shotSpeed;
        }
    }
}
