using UnityEngine;
using System.Collections;

public class CloudController : MonoBehaviour {

    public GameObject[] clouds;

    public Texture2D[] textures;

    public GameObject[] spawns;
 



    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SpawnCloud(GameObject cloud)
    {
        //Set speed, size, background/foreground, 

        cloud.transform.localScale = new Vector3(0.35f, 1f, 0.2f);

        int rand = Random.Range(0, spawns.Length);

        cloud.GetComponent<Renderer>().material.SetTexture("_MainTex", textures[Random.Range(0, textures.Length)]);

       

        if(cloud.GetComponent<CloudMovement>().isInForeground)
        {
            cloud.transform.position = new Vector3(Random.Range(-3f, 3f), spawns[0].transform.position.y, spawns[0].transform.position.z + cloud.GetComponent<CloudMovement>().zOffset);
            cloud.transform.localScale = cloud.transform.localScale * Random.Range(0.5f, 0.8f);
            //  cloud.GetComponent<CloudMovement>().speed = Random.Range(2f, 3f);
            // cloud.GetComponent<CloudMovement>().waitTime = Random.Range(1f, 5f);
            cloud.GetComponent<CloudMovement>().speed = 3f;
            cloud.GetComponent<CloudMovement>().waitTime = Random.Range(1f, 5f);

        }
        else if(cloud.GetComponent<CloudMovement>().isInForeground == false)
        {
            cloud.transform.position = new Vector3(Random.Range(-3f, 3f), spawns[1].transform.position.y, spawns[1].transform.position.z + cloud.GetComponent<CloudMovement>().zOffset);
            cloud.transform.localScale = cloud.transform.localScale * Random.Range(2.5f, 2.8f);
            //   cloud.GetComponent<CloudMovement>().speed = Random.Range(.2f, .3f);
            //   cloud.GetComponent<CloudMovement>().waitTime = Random.Range(4f, 10f);
            cloud.GetComponent<CloudMovement>().speed = .5f;
            cloud.GetComponent<CloudMovement>().waitTime = Random.Range(1f, 5f);


        }


    }


}
