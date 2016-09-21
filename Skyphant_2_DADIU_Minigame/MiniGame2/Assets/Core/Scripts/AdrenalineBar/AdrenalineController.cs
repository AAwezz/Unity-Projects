using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AdrenalineController : MonoBehaviour {

    public Slider AdrenalineBar;

    private GameObject player;

    private Renderer[] rend;

    private Collider[] coll;

    public GameObject endSceneCanvas;

    private GameObject CameraHolder;

    private bool isDead = false;
	private AudioManager audioMngr;

	// Use this for initialization
	void Start () {
        AdrenalineBar.value = 75f;

	    player = GameObject.FindGameObjectWithTag("Player");

        rend = GameObject.FindGameObjectWithTag("Player").GetComponentsInChildren<Renderer>();
        coll = GameObject.FindGameObjectWithTag("Player").GetComponentsInChildren<Collider>();

	    //CameraHolder = Camera.main.GetComponentInParent<Transform>();
        CameraHolder = GameObject.Find("CameraHolder");

		audioMngr = GameObject.FindObjectOfType<AudioManager> ();
    }


    void FixedUpdate()
    {

        if (isDead && CameraHolder.transform.eulerAngles.x < 400)
        {
            //We rotate the camera

            Vector3 rot = CameraHolder.transform.rotation.eulerAngles;
            rot.x = rot.x - 50f*Time.deltaTime;
            CameraHolder.transform.eulerAngles = rot;
        }
    }

    public void DecreaseAdrenaline(float amount)
    {
        AdrenalineBar.value -= amount;
        if (AdrenalineBar.value < 1)
        {

            player.GetComponent<PlayerBoost>().playerIsDead = true;

            foreach (Collider c in coll)
            {

                c.enabled = false;

            }

            foreach (Renderer r in rend)
            {

                r.enabled = false;

            }

            var pauseButton = GameObject.FindGameObjectWithTag("PauseButton");
            if(pauseButton != null)
                pauseButton.SetActive(false);

            endSceneCanvas.SetActive(true);
            isDead = true;
           // CameraHolder.transform.parent = null;
            CameraHolder.GetComponent<SmoothFollow>().enabled = false;

			audioMngr.ThemeMusicStop();
			audioMngr.WindStop();
			audioMngr.FlappingStop();
			audioMngr.DeadSoundPlay();


            // Application.LoadLevel("gameOverSceneAW");
        }
    }

    public void IncreaseAdrenaline(float amount)
    {
        AdrenalineBar.value += amount;
    }
}
