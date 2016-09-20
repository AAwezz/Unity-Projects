using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    Vector3 direction;

    void Update()
    {
        if (Input.anyKey)
        {
            transform.position = transform.position + direction;
        }
    }

    void setPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    void setDirection(Vector3 dir)
    {
        direction = dir;
    }
}
