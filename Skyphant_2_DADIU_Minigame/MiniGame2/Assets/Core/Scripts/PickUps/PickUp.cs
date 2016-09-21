using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour {
    public enum Type
    {
        Coin,
        PickUp,
    };

    public Type type;

    public int PointsWorth = 100;

    private ScoreControl scoreControl;

    public bool EnableRoatation = true;

    public float RotationSpeed = 1f;

    private Renderer[] renderers;
	private AudioManager audioMngr;
    // Use this for initialization
    void Start()
    {

        renderers = GetComponentsInChildren<Renderer>();
		audioMngr = GameObject.FindObjectOfType<AudioManager> ();

        scoreControl = GameObject.FindGameObjectWithTag("UI").GetComponent<ScoreControl>();

        if (type == Type.Coin)
        {

                renderers[1].material.SetColor("_Color", Color.yellow);
     
    
        }
        else if (type == Type.PickUp)
        {

                renderers[1].material.SetColor("_Color", Color.blue);
  
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (EnableRoatation)
        {
            GetComponentInChildren<Transform>().Rotate(new Vector3(0, 1, 0), RotationSpeed * Time.deltaTime);
        }

    }

    private void OnTriggerEnter(Collider col)
    {

       // Debug.Log("Hit it!!");
        if (col.tag == "Player")
        {
            if (type == Type.Coin)
            {
				audioMngr.CoinsPlay();
                GetComponent<ParticleSystem>().Play();

                scoreControl.AddPoints(PointsWorth);

            }


            renderers[1].enabled = false;
            gameObject.SetActive(false);
        }

    }




}
