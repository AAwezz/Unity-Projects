using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using Assets.Core;
using Assets.Core.Scripts;
using UnityEngine.UI;

public class cubeDeath : MonoBehaviour {

    public bool enableControl = true;
	public AudioManager AudioMngr;
    public GameObject heartController;

    public static bool lifeTimeHit = false;
    public static float espeed;
    public static int loseLife;
    public GameObject YourScoreText;
    public GameObject HighScoreText;

    public GameObject elephantModel;
    private Renderer[] elephantMesh;
    public GameObject elecSprite;

    // Use this for initialization
    void Start () {
        ScoreSystem.points = 0;
        loseLife = 3;
		AudioMngr = GameObject.FindGameObjectWithTag ("AudioManager").GetComponent<AudioManager>();
        elephantMesh = elephantModel.GetComponentsInChildren<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (enableControl) {
            if (Input.GetKey("left"))
            {
                transform.Translate(Vector3.left * Time.deltaTime * espeed);
            }
            if (Input.GetKey("right"))
            {
                transform.Translate(Vector3.right * Time.deltaTime * espeed);
            }
        }
    }

    //Restart the level when you hit an object
    void OnTriggerEnter(Collider coll)
    {
		Camera.main.GetComponent<PerlinShake> ().PlayShake ();
		AudioMngr.ThunderPlay();
		AudioMngr.ElectrifiedPlay();
        loseLife -= 1;
        heartController.GetComponent<HeartController>().loseLife(loseLife);
        ScoreSystem.comboCount = 0;
        ScoreSystem.comboTimeReset();
        coll.gameObject.GetComponentInChildren<Collider>().enabled = false;
        //coll.gameObject.GetComponentInChildren<Renderer>().enabled = false;

        if (loseLife < 1)
        {
            if (ScoreSystem.points > PlayerPrefs.GetInt("Best score"))
            {
                PlayerPrefs.SetInt(("Best score"), ScoreSystem.points);
                ScoreSystem.comboCount = 0;
                ScoreSystem.comboTimeReset();

            }
            YourScoreText.SetActive(true);
            YourScoreText.GetComponent<Text>().text = LanguageManager.Instance.Get("Phrases/Score") + ScoreSystem.points;
            HighScoreText.SetActive(true);
           
            GameObject.FindGameObjectWithTag(Constants.Tags.GameMaster).GetComponent<GameOverMaster>().GameOver();
        }
        StartCoroutine(Electricute());
        //lifeTimeHit = true;
    }

    IEnumerator Electricute()
    {
        Time.timeScale = 0.1f;

        foreach(Renderer ren in elephantMesh)
        {

            ren.enabled = false;

        }

        elecSprite.SetActive(true);

        yield return new WaitForSeconds(0.01f);

        foreach (Renderer ren in elephantMesh)
        {

            ren.enabled = true;

        }

        elecSprite.SetActive(false);

        yield return new WaitForSeconds(0.01f);

        foreach (Renderer ren in elephantMesh)
        {

            ren.enabled = false;

        }

        elecSprite.SetActive(true);

        yield return new WaitForSeconds(0.01f);

        foreach (Renderer ren in elephantMesh)
        {

            ren.enabled = true;

        }

        elecSprite.SetActive(false);


        Time.timeScale = 1f;
    }
}
