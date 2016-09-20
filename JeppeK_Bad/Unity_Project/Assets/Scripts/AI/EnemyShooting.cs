using UnityEngine;
using System.Collections;

public class EnemyShooting : MonoBehaviour {

    public GameObject AIBullet;
    public float AIBulletSpeed;
    public float timeBetweenBullets;
    public int shootLength, knifeLength;
    GameObject player;
    private NavMeshAgent agent;
    private RaycastHit hitInfo;
    private Vector3 Pos, playerPos;

    float timer = 0;
    // Use this for initialization
    void Start () {
        agent = gameObject.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;
        if (!Input.anyKey)
        {
            return;
        }
        var playerPos = realPos(player.transform.position);

        Debug.DrawRay(Pos, playerPos - Pos);

        if (timer >= timeBetweenBullets)
        {
            Pos = transform.position;

            if (Vector3.Distance(Pos, playerPos) < shootLength && Vector3.Distance(Pos, playerPos) > knifeLength)
            {
                Physics.Raycast(Pos, playerPos - Pos, out hitInfo, shootLength);
                
                if (hitInfo.collider.gameObject.tag != "Player")
                {
                    return;
                }

                if (hitInfo.collider.gameObject.tag == "Player")
                {
                    Shoot();
                    timer = 0;
                }
                
            }
        }
    }

    IEnumerator ShootAtPlayer()
    {

        Shoot();
        yield return new WaitForSeconds(5.0f);
    }

    void Shoot()
    {
        GameObject AIBul = Instantiate(AIBullet, Pos, Quaternion.Euler(0, 0, 0)) as GameObject;
        AIBul.SendMessage("setPosition", Pos);
        AIBul.SendMessage("setDirection", agent.velocity.normalized * AIBulletSpeed);
    }
    private Vector3 realPos(Vector3 Pos)
    {
        return new Vector3(Pos.x, Pos.y - 1, Pos.z);
    }
}
