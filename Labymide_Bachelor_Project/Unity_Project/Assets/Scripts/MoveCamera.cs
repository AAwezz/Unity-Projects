
using UnityEngine;
using System.Collections;

/*
 * This is the script to be able to pan or orbit with the camera over the board game.
 */

public class MoveCamera : MonoBehaviour
{

    public Transform target;
    public static bool OrbitOrPan = false;
    public float distance;
    public float zoomStep = 1.0f;
    public float minZoom = 25.0f;
    public float maxZoom = 7.0f;
    public float minX = -10.0f;
    public float maxX = 10.0f;
    public float minZ = -10.0f;
    public float maxZ = 10.0f;
    public float xSpeed = 1f;
    public float ySpeed = 1f;
    public float zSpeed = 1f;
    public float xSpeedBuf;
    public float ySpeedBuf;
    public float xSpeedTouch = 1f;
    public float ySpeedTouch = 1f;
    public float zSpeedTouch = 0.05f;
    public float clickingBuffer = 0.1f;
    float camX = 0.0f;
    float camY = 0.0f;
    float camZ = 0.0f;
    Vector3 distanceVector;
    public float perspectiveZoomSpeed = 0.5f;

    void Start()
    {
        distance = this.transform.position.y;
        distanceVector = new Vector3(0.0f, 0.0f, -distance);

        Vector3 angles = this.transform.localEulerAngles;
        camX = angles.y;
        camY = angles.x;
        camZ = angles.z;
        if (OrbitOrPan)
        {
            this.Orbit(camX, camY);
        }
        else
        {
            this.Pan(camX, camZ);
        }
    }

    /*
     * This allows the user to change from panning to orbiting around in the game,
     * and allow you to go back to the lobby when the game has ended.
     */
    void OnGUI()
    {
        var svMat = NetworkManager.ScalingScreen();
        if (GUI.Button(new Rect(450, 320, 140, 25), "Change Camera View"))
        {
            OrbitOrPan = !OrbitOrPan;
            transform.position = new Vector3(0.0f, 25.0f, 0.0f);
            transform.eulerAngles = new Vector3(90.0f, 0.0f, 0.0f);
            camX = 0.0f;
            camY = 90.0f;
            if (OrbitOrPan)
            {
                xSpeedBuf = xSpeed;
                ySpeedBuf = ySpeed;
                xSpeed = 1f;
                ySpeed = 1f;
            }
            else
            {
                xSpeed = xSpeedBuf;
                ySpeed = ySpeedBuf;
            }
        }
        GUI.matrix = svMat;
    }

    /* 
     * Moving the camera or zoom depending on the input of the player.
     */
    void Update()
    {
        if (target)
        {
            this.MoveControls();
            this.Zoom();
        }
    }

    /* 
     * Depending on which platform is used, this method is used to track
     * camera movement and/or zooming
     */
    void MoveControls()
    {
        if (Input.touchCount == 1) // If you use only one finger you want to move the camera
        {
            if (OrbitOrPan)
            {
                if (camY - Input.GetTouch(0).deltaPosition.y * ySpeedTouch >= 10 && camY - Input.GetTouch(0).deltaPosition.y * ySpeedTouch < 90)
                {
                    camY += -Input.GetTouch(0).deltaPosition.y * ySpeedTouch;
                    camX += Input.GetTouch(0).deltaPosition.x * xSpeedTouch;
                    this.Orbit(camX, camY);
                }
                else
                {
                    camX += Input.GetTouch(0).deltaPosition.x * xSpeedTouch;
                    this.Orbit(camX, camY);
                }
            }
            else
            {
                if (minX <= camX + -Input.GetTouch(0).deltaPosition.x * zSpeedTouch && maxX >= camX + -Input.GetTouch(0).deltaPosition.x * zSpeedTouch)
                    camX += -Input.GetTouch(0).deltaPosition.x * zSpeedTouch;

                if (minZ <= camZ + -Input.GetTouch(0).deltaPosition.y * zSpeedTouch && maxZ >= camZ + -Input.GetTouch(0).deltaPosition.y * zSpeedTouch)
                    camZ += -Input.GetTouch(0).deltaPosition.y * zSpeedTouch;

                this.Pan(camX, camZ);
            }
        }
        else if (Input.touchCount == 2) // if you use two fingers you want to zoom the camera
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
            GetComponent<Camera>().fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;
            GetComponent<Camera>().fieldOfView = Mathf.Clamp(GetComponent<Camera>().fieldOfView, 10.0f, 60.0f);
            if (GetComponent<Camera>().fieldOfView < 30.0f)
            {
                xSpeedTouch = 1f;
                ySpeedTouch = 1f;
                zSpeedTouch = 0.02f;
            }
            else
            {
                xSpeedTouch = 0.5f;
                ySpeedTouch = 0.5f;
                zSpeedTouch = 0.05f;
            }
        }
        else if (Input.GetButton("Fire1") && Input.touchCount <= 0) // and if you use a mouse click you also want to move the camera
        {
            if (OrbitOrPan)
            {
                if (camY - Input.GetAxis("Mouse Y") * ySpeed >= 10 && camY - Input.GetAxis("Mouse Y") * ySpeed < 90)
                {
                    if (Mathf.Abs(Input.GetAxis("Mouse X")) * xSpeed > clickingBuffer && Mathf.Abs(Input.GetAxis("Mouse Y")) * ySpeed > clickingBuffer)
                    {
                    camX += Input.GetAxis("Mouse X") * xSpeed;
                    camY += -Input.GetAxis("Mouse Y") * ySpeed;
                    this.Orbit(camX, camY);
                }
                }
                else
                {
                    if (Mathf.Abs(Input.GetAxis("Mouse X")) > clickingBuffer)
                    {
                        camX += Input.GetAxis("Mouse X") * xSpeed;
                        this.Orbit(camX, camY);
                    }
                }
            }
            else
            {
                if (minX <= (camX + -Input.GetAxis("Mouse X") * xSpeed) && maxX >= camX + -Input.GetAxis("Mouse X") * xSpeed &&
                    Mathf.Abs(Input.GetAxis("Mouse X")) * xSpeed > clickingBuffer)
                    camX += -Input.GetAxis("Mouse X") * xSpeed;

                if (minZ <= (camZ + -Input.GetAxis("Mouse Y") * zSpeed) && maxZ >= camZ + -Input.GetAxis("Mouse Y") * zSpeed &&
                    Mathf.Abs(Input.GetAxis("Mouse Y")) * zSpeed > clickingBuffer)
                    camZ += -Input.GetAxis("Mouse Y") * ySpeed;
                this.Pan(camX, camZ);
            }
        }
    }

    /* 
     * This method is used to pan around the game
     */
    void Pan(float x, float z)
    {
        Vector3 position = new Vector3(x, -distanceVector.z, z);
        transform.position = position;
    }

    /* 
     * This method is used to orbit around the game
     */
    void Orbit(float x, float y)
    {
        if (y >= 10)
        {
            Quaternion rotation = Quaternion.Euler(y, x, 0.0f);
            Vector3 position = rotation * distanceVector + target.position;
            transform.rotation = rotation;
            transform.position = position;
        }
        else
        {
            Quaternion rotation = Quaternion.Euler(11, x, 0.0f);
            Vector3 position = rotation * distanceVector + target.position;
            transform.rotation = rotation;
            transform.position = position;
        }
    }

    /* 
     * Zoom or zoom out depending on the input of the mouse wheel.
     */
    void Zoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0.0f)
        {
            this.ZoomOut();
            if (xSpeed + 0.1f <= 1.0f)
                xSpeed += 0.1f;
            if (ySpeed + 0.1f <= 1.0f)
                ySpeed += 0.1f;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0.0f)
        {
            this.ZoomIn();
            if ((xSpeed - 0.1f) >= 0.1f)
                xSpeed += -0.1f;
            if ((ySpeed - 0.1f) >= 0.1f)
                ySpeed += -0.1f;
        }

    }

    /* 
     * This is the method to zoom
     */
    void ZoomIn()
    {
        if (distance > maxZoom)
            distance -= zoomStep;
        distanceVector = new Vector3(0.0f, 0.0f, -distance);
        if (OrbitOrPan)
        {
            this.Orbit(camX, camY);
        }
        else
        {
            this.Pan(camX, camZ);
        }
    }

    /* 
     * This is the method to zoom out
     */
    void ZoomOut()
    {
        if (distance < minZoom)
            distance += zoomStep;
        distanceVector = new Vector3(0.0f, 0.0f, -distance);
        if (OrbitOrPan)
        {
            this.Orbit(camX, camY);
        }
        else
        {
            this.Pan(camX, camZ);
        }
    }
}
