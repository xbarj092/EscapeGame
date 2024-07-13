using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watchlight : MonoBehaviour
{
    [SerializeField] private float _detectionAngle;
    [SerializeField] private float _detectionDistance;
    [SerializeField] private float _raycastInterval;

    private void Update()
    {
        int numberOfRaycasts = Mathf.CeilToInt(_detectionAngle / _raycastInterval);
        for (int i = -numberOfRaycasts / 2; i <= numberOfRaycasts / 2; i++)
        {
            float currentAngle = (i * _raycastInterval) + 90;
            Vector3 direction = Quaternion.Euler(0, 0, currentAngle) * transform.right;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, _detectionDistance);
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag(GlobalConstants.Tags.Player.ToString()))
                {
                    DoSomething();
                    break;
                }
            }
        }
    }

    private void DoSomething()
    {
        Debug.Log("Player detected!");
    }
}
