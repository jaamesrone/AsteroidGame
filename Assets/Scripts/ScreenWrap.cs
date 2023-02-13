using UnityEngine;

public class ScreenWrap : MonoBehaviour
{
    // Reference to the camera component
    public Camera cam;

    // Screen bounds
    private float screenRight;
    private float screenTop;

    private void Start()
    {
        //this calculates the screen bounds based on the camera size and screen aspect ratio
        screenRight = cam.orthographicSize * Screen.width / Screen.height;
        screenTop = cam.orthographicSize;
    }

    void Update() //checks everyframe if any object this script is attacthed to, has touched any corner of my screen to change its position to the opposite side.
    {
        updateObjects();
    }

    public void updateObjects()
    {
        if (transform.position.x > screenRight)
        {
            transform.position = new Vector3(-screenRight, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -screenRight)
        {
            transform.position = new Vector3(screenRight, transform.position.y, transform.position.z);
        }
        if (transform.position.y > screenTop)
        {
            transform.position = new Vector3(transform.position.x, -screenTop, transform.position.z);
        }
        else if (transform.position.y < -screenTop)
        {
            transform.position = new Vector3(transform.position.x, screenTop, transform.position.z);
        }
    }

}
