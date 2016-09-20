using UnityEngine;
using System.Collections;

public class AIBulletBehavior : MonoBehaviour
{
    
    public float scale;
    public int bulletMaxDistance;
    public Vector3 direction, startingPos;
    Rigidbody RB;
    PlayerMovement playermovementScript;

    void Start()
    {
        transform.localScale = transform.localScale * scale;
        RB = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (direction == Vector3.zero)
        {
            Destroy(gameObject);
        }

        if (Input.anyKey)
        {
            transform.position = transform.position + direction;
        }

        if (bulletMaxDistance < Vector3.Distance(startingPos, transform.position))
        {
            Destroy(gameObject);
        }
    }

    void setPosition(Vector3 pos)
    {
        startingPos = pos;
        transform.position = pos;
    }

    void setDirection(Vector3 dir)
    {
        dir.y = 0;
        direction = dir;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            playermovementScript = (PlayerMovement) col.GetComponent(typeof(PlayerMovement));
            playermovementScript.youDead();
            Destroy(gameObject);
        }

        if (col.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}
