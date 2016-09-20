using UnityEngine;

public class Shooting : MonoBehaviour
{
    public float timeBetweenBullets = 0.2f;
    public float BulletSpeed = 2f;
    public GameObject bullet,bulletCollider,colliderSpawn;
    public bool isShotgun=false, isAk = false;
    RaycastHit hitInfo;
    Vector3 flyToPos;
    public GameObject AudioM;
    float timer;

    void Start()
    {

    }

    void Update()
    {
        timer += Time.deltaTime;
        if (Input.GetButton("Fire1") && timer >= timeBetweenBullets && isAk == true)
        {
            AudioM.GetComponent<AudioController>().Shot.loop = true;
            AudioM.GetComponent<AudioController>().Shot.Play();
            Shoot();
            return;
        }
        AudioM.GetComponent<AudioController>().Shot.loop = false;
        if (Input.GetButtonDown("Fire1") && timer >= timeBetweenBullets)
        {
            if (!isShotgun) 
            { 
                AudioM.GetComponent<AudioController>().Shot.Play();
            }
            else
            {
                AudioM.GetComponent<AudioController>().ShutgunShot.Play();
            }
            Shoot();
        }

    }


    void Shoot()
    {
        //Vector3 camPos = Camera.main.transform.position + new Vector3(0, -0.6f, 1f);
        //Debug.Log(camPos);
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, 100))
        {
            flyToPos = hitInfo.point;
        }
        else
        {
            flyToPos = Camera.main.transform.position + Camera.main.transform.forward * 10;
        }
        timer = 0f;
        GameObject bul = Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
        GameObject bulCol = Instantiate(bulletCollider, colliderSpawn.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
        bul.SendMessage("setPosition", transform.position);
        if (isShotgun)
        {
            Vector3 ran = new Vector3(Random.Range(0f, 1f) / 10f, Random.Range(0f, 1f) / 10f, Random.Range(0f, 1f) / 10f);
            bul.SendMessage("setDirection", (flyToPos - transform.position).normalized + ran  * BulletSpeed);
            bulCol.SendMessage("setDirection", (flyToPos- colliderSpawn.transform.position).normalized+ ran * BulletSpeed);
        }
        else
        {
            bul.SendMessage("setDirection", (flyToPos - transform.position).normalized * BulletSpeed);
            bulCol.SendMessage("setDirection", (flyToPos- colliderSpawn.transform.position).normalized * BulletSpeed);
        }
        bulCol.SendMessage("setBullet", bul);
    }
}
