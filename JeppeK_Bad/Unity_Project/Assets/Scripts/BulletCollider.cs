using UnityEngine;
using System.Collections;

public class BulletCollider : MonoBehaviour {

    public int damagePerShot, bulletDistance;
    GameObject bullet;
    Vector3 direction, startingPos;
    Rigidbody RB;

    void Start()
    {
        RB = gameObject.GetComponent<Rigidbody>();
        damagePerShot = GameMasterPublicVariables.dmg;
    }

    void Update()
    {
        if (Input.anyKey)
        {
            RB.velocity = direction;
        }
        if (bulletDistance < Vector3.Distance(startingPos, transform.position))
        {
            Destroy(bullet);
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
        direction = dir;
    }

    void setBullet(GameObject bul)
    {
        bullet = bul;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "AI")
        {
            EnemyHealth enemyHealth = col.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damagePerShot);
                Destroy(bullet);
                Destroy(gameObject);
            }
        }
        else if (col.gameObject.tag == "Wall")
        {
            Destroy(bullet);
            Destroy(gameObject);
        }
    }
}
