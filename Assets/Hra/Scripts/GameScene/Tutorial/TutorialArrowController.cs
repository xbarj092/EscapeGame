using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialArrowController : MonoBehaviour
{
    [SerializeField] private RectTransform _arrowTransform;
    [SerializeField] private float _defaultOffset = 0;

    public void SetArrowOffset(float? offset = null)
    {
         _arrowTransform.anchoredPosition3D = new Vector3(_arrowTransform.anchoredPosition3D.x, 
                offset == null ? -_defaultOffset : -offset.Value, _arrowTransform.anchoredPosition3D.z);
    }

    public void SetArrowRotation(float zRotation)
    {
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, 
            transform.rotation.eulerAngles.y, zRotation));
    }
}
