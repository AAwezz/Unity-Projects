using UnityEngine;
using System.Collections;

public class LightController : MonoBehaviour {

    GameObject[] _lightsources;
    float LightManipulationAmount = 200;

    private AudioControl _ac;
    //GameObject pointLight;
    //float timeForBlink;

    // Use this for initialization
    void Start () {
        _lightsources = GameObject.FindGameObjectsWithTag("LightSource");
        _ac = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioControl>();
	}

    public IEnumerator FlashAllLightForSecounds(float time)
    {
        float _startTime = Time.time;

        while (_startTime + time > Time.time)
        {          
            for (int i = 0; i < _lightsources.Length; i++)
            {
                if (_lightsources[i].GetComponent<StartLightSettings>())
                {
                    //Debug.Log("light found");
                    _lightsources[i].GetComponent<StartLightSettings>().AdjustRange(100);

                    if (Random.Range(-1, 1) < 0 && _lightsources[i].GetComponent<Light>().intensity > 0.3f)
                    {
                        _lightsources[i].GetComponent<StartLightSettings>().AdjustIntencity(100 - LightManipulationAmount * Time.deltaTime);
                        _lightsources[i].GetComponent<StartLightSettings>().AdjustRange(100 - LightManipulationAmount * Time.deltaTime);
                    }
                    else if (_lightsources[i].GetComponent<Light>().intensity < 1.3f)
                    {
                        _lightsources[i].GetComponent<StartLightSettings>().AdjustIntencity(100 + LightManipulationAmount * Time.deltaTime);
                        _lightsources[i].GetComponent<StartLightSettings>().AdjustRange(100 + LightManipulationAmount * Time.deltaTime);
                    }
                }
                else
                {
                    Debug.Log("StartLightSettings script is missing from " + _lightsources[i].name);
                }
            }
            yield return null;            
        }
        for (int i = 0; i < _lightsources.Length; i++)
        {
            if (_lightsources[i].GetComponent<StartLightSettings>())
            {
                _lightsources[i].GetComponent<StartLightSettings>().ResetIntencity();
                _lightsources[i].GetComponent<StartLightSettings>().ResetRange();
            }
        }
        _ac.StopFlickering();
        yield return null;
    }

    //used to blink with a single light, give it the point light gameobject and the time in which it should blink
    public IEnumerator FlashLightForSecounds(GameObject light, float time)
    {
        float _startTime = Time.time;

        StartLightSettings _individualLight = light.GetComponent<StartLightSettings>();

        if (!_individualLight)
        {
            Debug.Log("StartLightSettings script is missing from " + light.name);
            yield break;
        }

        while (_startTime + time > Time.time)
        {

            if (_individualLight)
            {
                _individualLight.AdjustRange(100);

                if (Random.Range(-1, 1) < 0 && light.GetComponent<Light>().intensity > 0.3f)
                {
                    _individualLight.AdjustIntencity(100 - LightManipulationAmount * Time.deltaTime);
                    _individualLight.AdjustRange(100 - LightManipulationAmount * Time.deltaTime);
                }
                else if (light.GetComponent<Light>().intensity < 1.3f)
                {
                    _individualLight.AdjustIntencity(100 + LightManipulationAmount * Time.deltaTime);
                    _individualLight.AdjustRange(100 + LightManipulationAmount * Time.deltaTime);
                }
            }

            yield return null;
        }
       
        _individualLight.ResetIntencity();
        _individualLight.ResetRange();
    
        _ac.StopFlickering();
        yield return null;
    }

    public void OneLampBlink(GameObject light, float time)
    {
        StartCoroutine(FlashLightForSecounds(light, time));
    }
}
