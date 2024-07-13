using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Watchlight : MonoBehaviour
{
    [SerializeField] private Volume _volume;
    [SerializeField] private Light2D _light;
    [SerializeField] private float _detectionAngle;
    [SerializeField] private float _detectionDistance;
    [SerializeField] private float _raycastInterval;
    [SerializeField] private float _targetLostTime;

    private bool _shouldBlind = false;
    private bool _shouldCheck = true;
    private float _noTargetTime = 0f;
    private float _blindTime = 0f;
    private float _maxBlindTime = 1f;

    private ChromaticAberration _chromaticAberration;

    public event Action<Transform> OnPlayerSpotted;
    public event Action OnPlayerTargetLost;

    private void FixedUpdate()
    {
        int numberOfRaycasts = Mathf.CeilToInt(_detectionAngle / _raycastInterval);
        bool targetDetected = false;

        for (int i = -numberOfRaycasts / 2; i <= numberOfRaycasts / 2; i++)
        {
            float currentAngle = (i * _raycastInterval) + 90;
            Vector3 direction = Quaternion.Euler(0, 0, currentAngle) * transform.right;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, _detectionDistance);
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag(GlobalConstants.Tags.Player.ToString()))
                {
                    if (_shouldCheck)
                    {
                        PlayerSpotted(hit.collider.transform);
                    }

                    _noTargetTime = 0f;
                    targetDetected = true;
                    break;
                }
            }
        }

        if (!_shouldCheck && !targetDetected)
        {
            _noTargetTime += Time.deltaTime;
            if (_noTargetTime >= _targetLostTime)
            {
                PlayerLost();
            }
        }

        if (_shouldBlind)
        {
            Blind(targetDetected);
        }
    }

    private void Blind(bool blind)
    {
        if (_blindTime >= _maxBlindTime)
        {
            PlayerEvents.OnPlayerDeathInvoke();
        }

        if (_chromaticAberration == null)
        {
            _volume.profile.TryGet(out _chromaticAberration);
        }

        if (_chromaticAberration != null)
        {
            if (_chromaticAberration.intensity.value == 0f)
            {
                _blindTime = 0f;
            }
            else if (_chromaticAberration.intensity.value == 1)
            {
                _blindTime += Time.deltaTime;
            }

            _chromaticAberration.intensity.value += blind ? 0.02f : -0.02f;
        }
    }

    private void PlayerSpotted(Transform transform)
    {
        StartCoroutine(ChangeLightIntensity(true));
        OnPlayerSpotted?.Invoke(transform);
        _shouldCheck = false;
    }

    private void PlayerLost()
    {
        StartCoroutine(ChangeLightIntensity(false));
        OnPlayerTargetLost?.Invoke();
        _shouldCheck = true;
        _shouldBlind = false;
        _noTargetTime = 0;
    }

    private IEnumerator ChangeLightIntensity(bool aggro)
    {
        float startIntensity = _light.intensity;
        float targetIntensity = aggro ? 15 : 1;
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            _light.intensity = Mathf.Lerp(startIntensity, targetIntensity, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        _shouldBlind = true;
        _light.intensity = targetIntensity;
    }

    public void SetAlert(bool shouldCheck)
    {
        _shouldCheck = shouldCheck;
    }
}
