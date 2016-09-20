using UnityEngine;
using System.Collections;

public class StartLightSettings : MonoBehaviour {

    float _startRange = 0.0f;
    float _startIntencity = 0.0f;

    public Light Spotlight;
    Light _pointlight;

    float _startSpotRange = 0.0f;
    float _startSpotIntencity = 0.0f;

    // Use this for initialization
    void Start () {
        _pointlight = this.gameObject.GetComponent<Light>();
        _startRange = _pointlight.range;
        _startIntencity = _pointlight.intensity;

        if (Spotlight)
        {
            _startSpotRange = Spotlight.range;
            _startSpotIntencity = Spotlight.intensity;
        }
    }

    public void ResetIntencity()
    {
        _pointlight.intensity = _startIntencity;
        if (Spotlight)
        {
            Spotlight.intensity = _startSpotIntencity;
        }
    }

    public void AdjustIntencity(float percent)
    {
        _pointlight.intensity = _pointlight.intensity * percent / 100;
        if (Spotlight)
        {
            Spotlight.intensity = Spotlight.intensity * percent / 100;
        }
    }

    public void ResetRange()
    {
        _pointlight.range = _startRange;
        if (Spotlight)
        {
            Spotlight.range = _startSpotRange;
        }
    }

    public void AdjustRange(float percent)
    {
        _pointlight.range = _pointlight.range * percent / 100;
        if (Spotlight)
        {
            Spotlight.range = Spotlight.range * percent / 100;
        }
    }
}
