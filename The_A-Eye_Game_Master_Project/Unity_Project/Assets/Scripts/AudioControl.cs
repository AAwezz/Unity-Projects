using UnityEngine;
using System.Collections;

public class AudioControl : MonoBehaviour {

    public AudioSource StartMusic;
    public AudioSource ScaryMusic;
    public AudioSource Flickering;
    public AudioSource MetalDoor;
    public AudioSource Dragging;
    public AudioSource ScareSound;
    public AudioSource PianoTune;
    public AudioSource ScaryVoice;
    public AudioSource Screamer;
    public AudioSource StaticTV;
    public AudioSource ToiletFlush;
    public AudioSource Clock;
    public AudioSource GhoulGrowl, GhoulBreath;
    public AudioSource BabyLaugh;

    public void BabyStop()
    {
        BabyLaugh.Stop();
    }

    public void BabyStart()
    {
        BabyLaugh.Play();
    }

    void Start()
    {
        Clock.Play();
    }

    public void PlayScaryVoice()
    {
        ScaryVoice.Play();
    }

    public void ScreamerStop()
    {
        Screamer.Stop();
    }

    public void ScreamerStart()
    {
        Screamer.Play();
    }

    public void StopTV()
    {
        StaticTV.Stop();
    }

    public void StartTV()
    {
        StaticTV.Play();
    }

    public void StartFlush()
    {
        ToiletFlush.Play();
    }

    public void StartPiano()
    {
        PianoTune.Play();
    }

    public void StartScare()
    {
        ScareSound.Play();
    }

    public void StopDragging()
    {
        Dragging.Stop();
    }

    public void StartDragging()
    {
        Dragging.Play();
    }

    public void StopStartMusic()
    {
        StartMusic.Stop();
    }

    public void StartScaryMusic()
    {
        ScaryMusic.Play();
    }

    public void PlayFlickering()
    {
        Flickering.Play();
    }

    public void StopFlickering()
    {
        Flickering.Stop();
    }

    public void StartMetalDoor()
    {
        MetalDoor.Play();
    }

    public void PlayGhoulGrowl()
    {
        GhoulGrowl.Play();
    }
    public void StopGhoulGrowl()
    {
        GhoulGrowl.Stop();
    }

    public void PlayGhoulBreath()
    {
        GhoulBreath.Play();
    }

    public void StopGhoulBreath()
    {
        GhoulBreath.Stop();
    }

}
