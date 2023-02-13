using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallScript : MonoBehaviour
{
    public Vector3 velocity;
    public GameObject mediumAsteroidPrefab;
    public GameObject smallAsteroidPrefab;


    private void Start()
    {
        // Apply the velocity to the ball
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = velocity;
    }

    private void Awake()
    {
        Invoke("timeToDestroy", 3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Destroy the ball when it collides with an asteroid
        if (other.gameObject.tag == "Large")
        {
            Destroy(this.gameObject);
            Destroy(other.gameObject);
            MediumAsteroidSpawner();
            GameManager.Instance.AddLargeScore();
        }
        else if (other.gameObject.tag == "Medium")
        {
            Destroy(this.gameObject);
            Destroy(other.gameObject);
            SmallAsteroidSpawner();
            GameManager.Instance.AddMediumScore();
        }
        else if (other.gameObject.tag == "Small")
        {
            Destroy(this.gameObject);
            Destroy(other.gameObject);
            GameManager.Instance.AddSmallScore();
        }
        else if(other.gameObject.tag == "DaddyUFO")
        {
            Destroy(this.gameObject);
            Destroy(other.gameObject);
            GameManager.Instance.AddDaddyUFOScore();
        }
        else if(other.gameObject.tag == "BabyUFO")
        {
            Destroy(this.gameObject);
            Destroy(other.gameObject);
            GameManager.Instance.AddBabySaucerScore();
        }

    }

    public void timeToDestroy()
    {
        Destroy(this.gameObject);
    }
    public void MediumAsteroidSpawner()
    {
        Vector3 MedAsteroid1 = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 10f) * 4;
        Vector3 MedAsteroid2 = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 10f) * 4;
        Instantiate(mediumAsteroidPrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody>().velocity = MedAsteroid1;
        Instantiate(mediumAsteroidPrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody>().velocity = MedAsteroid2;
    }

    public void SmallAsteroidSpawner()
    {
        Vector3 smallAsteroid = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 10f) * 4;
        Instantiate(smallAsteroidPrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody>().velocity = smallAsteroid;
    }
}