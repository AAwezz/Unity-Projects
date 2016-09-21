using UnityEngine;
using System.Collections;

public class EndScene : MonoBehaviour
{

    GameObject CameraHolder = null;
    public float cameraSpeedEnd = 5;
    Vector3 endPos;
    private bool smoothEnd = false;
    private bool lookUp = false;
    float t;
    Vector3 startPos;
    public float rotationAmount = 30.0f;
    public GameObject endSceneCanvas;
    public GameObject Fireworks1;
    public GameObject Fireworks2;
    public GameObject Fireworks3, Fireworks4, Fireworks5, Fireworks6, Fireworks7, Fireworks8, Fireworks9, Fireworks10, Fireworks11, Fireworks12;
	private AudioManager audioMngr;

    void Start()
    {
        CameraHolder = GameObject.Find("CameraHolder");
        endPos = transform.GetChild(0).position;
		audioMngr = GameObject.FindObjectOfType<AudioManager> ();
    }

    void FixedUpdate()
    {
        if (smoothEnd)
        {
            //We move the camera slowly after we reach the goal
            t += Time.deltaTime / cameraSpeedEnd;
            CameraHolder.transform.position = Vector3.Lerp(startPos, endPos, t);
            if (Vector3.Distance(CameraHolder.transform.position, endPos) < 1)
            {
                smoothEnd = false;
                lookUp = true;
                t = 0;
                startPos = CameraHolder.transform.position;
            }
        }

        if (lookUp)
        {
            //We rotate the camera
            Vector3 rot = CameraHolder.transform.rotation.eulerAngles;
            rot.x = rot.x - rotationAmount * Time.deltaTime;
            CameraHolder.transform.eulerAngles = rot;

            //We move the camera up a bit
            Vector3 end = new Vector3(CameraHolder.transform.position.x, CameraHolder.transform.position.y + .5f, CameraHolder.transform.position.z);
            t += Time.deltaTime / cameraSpeedEnd;
            CameraHolder.transform.position = Vector3.Lerp(startPos, end, t);

            if (CameraHolder.transform.eulerAngles.x == 270)
            {
                var pauseButton = GameObject.FindGameObjectWithTag("PauseButton");
                if(pauseButton != null)
                    pauseButton.SetActive(false);

                endSceneCanvas.SetActive(true);
            }


        }

    }



    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            CameraHolder.transform.parent = null;
            CameraHolder.GetComponent<SmoothFollow>().enabled = false;
            startPos = CameraHolder.transform.position;
            t = 0;
            smoothEnd = true;
            StartCoroutine(Fireworks());
        }
    }

    IEnumerator Fireworks()
    {
		audioMngr.FireWorksPlay();
        yield return new WaitForSeconds(2.2f);

        Fireworks1.GetComponent<ParticleSystem>().Play();
        Fireworks2.GetComponent<ParticleSystem>().Play();
        Fireworks3.GetComponent<ParticleSystem>().Play();

        yield return new WaitForSeconds(.8f);

        Fireworks4.GetComponent<ParticleSystem>().Play();
        Fireworks5.GetComponent<ParticleSystem>().Play();
        Fireworks6.GetComponent<ParticleSystem>().Play();

        yield return new WaitForSeconds(.8f);

        Fireworks7.GetComponent<ParticleSystem>().Play();
        Fireworks8.GetComponent<ParticleSystem>().Play();
        Fireworks9.GetComponent<ParticleSystem>().Play();

        yield return new WaitForSeconds(.4f);

        Fireworks10.GetComponent<ParticleSystem>().Play();
        Fireworks11.GetComponent<ParticleSystem>().Play();
        Fireworks12.GetComponent<ParticleSystem>().Play();
        audioMngr.FireWorksStop();
    }

}
