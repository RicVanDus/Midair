using UnityEngine;

public class LightFlash : MonoBehaviour
{
    public float flashTime = 0.5f;
    public float flashIntensity = 1f;
    
    private Light _light;

    private float _timer;
    private float _timePeak;
    private float _timeMult;
    private bool _bGoBack = false;
    
    void Start()
    {
        _light = GetComponent<Light>();
        _timePeak = flashTime / 2;
        _timeMult = 1 / flashTime;
        _light.intensity = 0f;
    }

    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > _timePeak)
            _bGoBack = true;

        if (_bGoBack)
        {
            _light.intensity = (flashTime - _timer) * flashIntensity;
        }
        else
        {
            _light.intensity = _timer * flashIntensity;
        }

    }
}
