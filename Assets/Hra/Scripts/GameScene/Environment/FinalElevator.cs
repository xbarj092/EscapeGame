using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FinalElevator : MonoBehaviour
{
    [SerializeField] private Light2D _light;
    [SerializeField] private float _moveDuration = 5f;
    [SerializeField] private float _lightIntensityIncrease = 2f;

    private void Start()
    {
        _light.intensity = 0f;
    }

    public IEnumerator GoUp()
    {
        float elapsedTime = 0f;
        float targetIntensity = _lightIntensityIncrease * _moveDuration;

        while (elapsedTime < _moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float fraction = Mathf.Clamp01(elapsedTime / _moveDuration);

            _light.intensity = Mathf.Lerp(0, targetIntensity, fraction);

            yield return null;
        }

        _light.intensity = targetIntensity;
    }
}
