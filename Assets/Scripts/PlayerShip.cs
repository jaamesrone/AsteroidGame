using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShip : MonoBehaviour
{
    public Camera cam;

    

    public InputAction playerControls;
    public InputAction turnInput;
    public InputAction HyperSpace;
    public GameObject ballPrefab;
    public Transform spawnPoint;

    GameObject ball;

    public float shootSpeed;
    public float moveSpeed;
    public float rotationSpeed;
    public float axisValue;

    private Rigidbody rb;
    private bool moving;
    private bool turning;
    private bool spaceMode;

    private float screenRight;
    private float screenTop;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>(); //RB variable gets attached to my Rigidbody.
    }

    void Start()
    {
        playerControls.Enable();
        turnInput.Enable();
        HyperSpace.Enable();
        screenRight = cam.orthographicSize * Screen.width / Screen.height;
        screenTop = cam.orthographicSize;
    }

    private void Update()
    {
        CheckKeybinds();
        OnMove();
        if (turning)
        {
            OnRotate();
        }
        else if (spaceMode)
        {
            Teleport();
        }
        
    }

    private void CheckKeybinds()
    {
        moving = playerControls.IsPressed();
        turning = turnInput.IsPressed();
        spaceMode = HyperSpace.IsPressed();

    }


    public void OnMove()
    {
        if (moving)
        {
            rb.AddForce(transform.up * moveSpeed);
        }
    }


    public void OnRotate()
    {
        axisValue = turnInput.ReadValue<float>();

        if (axisValue > 0)
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
        else if (axisValue < 0)
        {
            transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
        }
    }

    public void Teleport()
    {
        float x = Random.Range(-screenRight, screenRight);
        float y = Random.Range(-screenRight, screenTop);
        Vector3 newPos = new Vector3(x, y, 10);
        transform.position = newPos;
        
    }

    public void OnFire()
    {
        Debug.Log("Shooting?");
        // Instantiate a new ball at the spawnPoint
        ball = Instantiate(ballPrefab, spawnPoint.position, spawnPoint.rotation);

        // Get the Ball component and set its velocity
        BallScript ballComponent = ball.GetComponent<BallScript>();
        ballComponent.velocity = transform.up * shootSpeed;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Large"))
        {
            Debug.Log("detected??");
            GameManager.Instance.PlayerHurt();                      
        }
        else if (other.CompareTag("Medium"))
        {
            Debug.Log("detected??");
            GameManager.Instance.PlayerHurt();
        }
        else if (other.CompareTag("Small"))
        {
            Debug.Log("detected??");
            GameManager.Instance.PlayerHurt();
        }
        else if (other.CompareTag("UFOLaser"))
        {
            GameManager.Instance.PlayerHurt();
        }
    }
}

