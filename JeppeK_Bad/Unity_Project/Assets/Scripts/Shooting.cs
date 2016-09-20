using UnityEngine;

public class Shooting : MonoBehaviour
{
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.2f;
    public float BulletSpeed = 2f;
    public GameObject bullet,bulletCollider;
    public bool isShotgun=false;
    RaycastHit hitInfo;
    Vector3 flyToPos;
    //public GameObject AudioM;
    float timer;

    void Start()
    {

    }

    void Update()
    {
        timer += Time.deltaTime;

        if (Input.GetButtonDown("Fire1") && timer >= timeBetweenBullets)
        {
            //AudioM.GetComponent<AudioController>().
            Shoot();
        }

    }


    void Shoot()
    {
        Vector3 camPos = Camera.main.transform.position + new Vector3(0, -0.6f, 1f);
        Debug.Log(camPos);
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
        GameObject bulCol = Instantiate(bulletCollider, camPos, Quaternion.Euler(0, 0, 0)) as GameObject;
        bul.SendMessage("setPosition", transform.position);
        if (isShotgun)
        {
            Vector3 ran = new Vector3(Random.Range(0f, 1f) / 10f, Random.Range(0f, 1f) / 10f, Random.Range(0f, 1f) / 10f);
            bul.SendMessage("setDirection", (flyToPos - transform.position).normalized + ran  * BulletSpeed);
            bulCol.SendMessage("setDirection", (flyToPos- camPos ).normalized+ ran * BulletSpeed);
        }
        else
        {
            bul.SendMessage("setDirection", (flyToPos - transform.position).normalized * BulletSpeed);
            bulCol.SendMessage("setDirection", (flyToPos- camPos).normalized * BulletSpeed);
        }
        bulCol.SendMessage("setDmg", damagePerShot);
        bulCol.SendMessage("setBullet", bul);
    }
}
