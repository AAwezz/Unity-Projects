using UnityEngine;
using System.Collections;

public class changeWeapon : MonoBehaviour
{
    public string weapon;
    // Animations
    private float aniTimeElapsed;
    private float aniTime = 0.5f;
    private bool aniUp = true;
    public GameObject AudioM;
    // Use this for initialization
    void Start()
    {
        aniTimeElapsed = 0f;
    }

    // Update is called once per frame
    void Update()
    {
            //Animation of powerUp
            if ((aniTimeElapsed > aniTime))
            {
                aniUp = false;
            }
            if ((aniTimeElapsed < 0.0f))
            {
                aniUp = true;
            }

            transform.Rotate(new Vector3(0, 2, 0));

            if (aniUp)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime, transform.position.z);
                aniTimeElapsed += Time.deltaTime;
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - Time.deltaTime, transform.position.z);
                aniTimeElapsed -= Time.deltaTime;
            }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            AudioM.GetComponent<AudioController>().startNyt();
            col.gameObject.SendMessage("ChangeWeapon",weapon);
            Destroy(gameObject);
        }
    }
}