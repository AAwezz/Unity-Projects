using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public float speed = 2.0f;
    public GameObject ak, shutgun, pistol, resBut;

    private Vector3 Pos;

    private Rigidbody RB;

    private bool collided;

    //Needed for double speed powerup
    bool boolDoubleSpeed = false;
    private float doubleSpeed;
    private float time = 5.0f;
    private float timeElapsed = 0;

    // Use this for initialization
    void Start () {
        RB = GetComponent<Rigidbody>();
        doubleSpeed = speed * 2;
    }
	
	// Update is called once per frame


	void Update () {

        //RB.velocity = Vector3.zero;

        Vector3 moveDir = Vector3.zero;

        if (boolDoubleSpeed)
        {
            speed = doubleMovementSpeed(speed);
        }

        transform.rotation = Camera.main.transform.rotation;

        if (Input.GetKey("w"))
        {
            //resetRB();
            moveDir = moveDir + transform.forward;
        }

        if (Input.GetKey("s"))
        {
            //resetRB();
            moveDir = moveDir - transform.forward;
        }

        if (Input.GetKey("d"))
        {
            //resetRB();
            moveDir = moveDir + transform.right;
        }

        if (Input.GetKey("a"))
        {
            //resetRB();
            moveDir = moveDir - transform.right;
        }

        if (Input.anyKey == false)
        {
            resetRB();
        }

        //Move the player according to the appropriate force
        //moveDir.y = 2;
        RB.AddForce(moveDir * speed * Time.deltaTime);

        // Restricting movement velocity.
        Vector3 clampedVelocity = RB.velocity;

        clampedVelocity.x = Mathf.Clamp(clampedVelocity.x, -5.0f, 5.0f);
        clampedVelocity.z = Mathf.Clamp(clampedVelocity.z, -5.0f, 5.0f);
        clampedVelocity.y = 0;
        RB.velocity = clampedVelocity;

        //make sure the player dosent start flying because of physics
        //Pos = transform.localPosition;
        //Pos.y = 2;
        //transform.localPosition = Pos;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "AI")
        {
            resBut.SetActive(true);
        }
    }

    void ChangeWeapon(string weapon)
    {
        switch (weapon)
        {
            case ("Ak47"):
                ak.SetActive(true);
                shutgun.SetActive(false);
                pistol.SetActive(false);
                break;
            case ("Shutgun"):
                ak.SetActive(false);
                shutgun.SetActive(true);
                pistol.SetActive(false);
                break;
            case ("Pistol"):
                ak.SetActive(false);
                shutgun.SetActive(false);
                pistol.SetActive(true);
                break;
            default:
                break;
        }
    }

    private float doubleMovementSpeed(float speed)
    {
        if (timeElapsed < time)
        {
            speed = doubleSpeed;
            timeElapsed += Time.deltaTime;
        }
        else
        {
            speed = speed / 2;
            boolDoubleSpeed = false;
            timeElapsed = 0;
        }
        return speed;
    }

    private void resetRB()
    {
        RB.velocity = Vector3.zero;
        RB.angularVelocity = Vector3.zero;
    }
}
