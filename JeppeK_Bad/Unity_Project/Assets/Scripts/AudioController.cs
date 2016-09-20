using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour {

    public AudioSource Intro, Loop, Ahaha, Ahaha2, Stiv, Coke, Mener, Mener2, Ligemeget, Nyt, Shot, Boevser, Kvaeler, cain1, cain2, ak47, ShutgunShot;
    private int count = 0;
    private float timer = 0;
    public float timeRepeat = 11;
	// Use this for initialization
	void Start () {
        startStiv();
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (Intro.isPlaying == false && count == 0)
        {
            Intro.Stop();
            startLoop();
            count = 1;
        }
        if (timer >= timeRepeat)
        {
            int rand = Random.Range(1,7);
            switch(rand)
            {
                case 1:
                    startAhaha();
                    break;
                case 2:
                    startAhaha2();
                    break;
                case 3:
                    startLigemeget();
                    break;
                case 4:
                    startMener();
                    break;
                case 5:
                    startMener2();
                    break;
                case 6:
                    Boevser.Play();
                    break;
                case 7:
                    Kvaeler.Play();
                    break;
                default :
                    break;
            }
            timer = 0;
        }
	}

    public void startLoop()
    {
        Loop.Play();
    }

    public void startNyt()
    {
        Nyt.Play();
    }

    public void startAhaha()
    {
        Ahaha.Play();
    }

    public void startStiv()
    {
        Stiv.Play();
    }

    public void startCoke()
    {
        Coke.Play();
    }

    public void startAhaha2()
    {
        Ahaha2.Play();
    }

    public void startMener()
    {
        Mener.Play();
    }

    public void startLigemeget()
    {
        Ligemeget.Play();
    }

    public void startMener2()
    {
        Mener2.Play();
    }
}
