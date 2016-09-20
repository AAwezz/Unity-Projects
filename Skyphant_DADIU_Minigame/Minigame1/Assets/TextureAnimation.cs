using UnityEngine;
using System.Collections;

public class TextureAnimation : MonoBehaviour {

    public Texture2D tex1, tex2;

    public float animationTime = 0;

    float randomTime;

	// Use this for initialization
	void Start () {

        randomTime = Random.Range(.5f, 2f);
	
	}
	
	// Update is called once per frame
	void Update () {

        randomTime -= Time.deltaTime;

        

      if (randomTime < 0f) {

            animationTime += Time.deltaTime;

      if (animationTime < .2f)
        {
            thunder();
        }
        else if(animationTime < 1f)
        {
            noThunder();
        }
        else if(animationTime < 1.2f)
        {
            thunder();
        }
            else if (animationTime < 3f)
            {
                noThunder();
                randomTime = Random.Range(2f, 4f);
                animationTime = 0f;
            }

        }
	
	}

    void thunder()
    {
        GetComponent<Renderer>().material.SetTexture("_MainTex", tex2);
    }

    void noThunder()
    {
        GetComponent<Renderer>().material.SetTexture("_MainTex", tex1);
    }
}


