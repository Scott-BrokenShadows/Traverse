using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class bubble : MonoBehaviour
{
    [SerializeField] private GameObject tribrach;
    [SerializeField] private float _zEuler;
    [SerializeField] private float _yEuler;
    [SerializeField] private float _zAngle;
    [SerializeField] private float _yAngle;
    [SerializeField] private float _zDist;
    [SerializeField] private float _yDist;


    public float bubbleMaxRange = 0.35f;

    void Update()
    {
        _zEuler = tribrach.transform.localEulerAngles.z;
        _yEuler = tribrach.transform.localEulerAngles.y;

        if (tribrach.transform.localEulerAngles.z >= 180)
        {
            _zAngle = (tribrach.transform.localEulerAngles.z - 360);

        }
        else
        {
            _zAngle = tribrach.transform.localEulerAngles.z;
        }

        if (tribrach.transform.localEulerAngles.y >= 180)
        {
            _yAngle = (tribrach.transform.localEulerAngles.y - 360);

        }
        else
        {
            _yAngle = tribrach.transform.localEulerAngles.y;
        }

        _zDist = (_zAngle / 1.05f) * bubbleMaxRange;
        _yDist = (_yAngle / 1.05f) * bubbleMaxRange;

        gameObject.transform.localPosition = new Vector3(_yDist, _zDist, gameObject.transform.localPosition.z);

        float distance = Vector3.Distance(gameObject.transform.localPosition, Vector3.zero);
        if (distance > bubbleMaxRange)
        {
            Vector3 clampedPos = Vector3.ClampMagnitude(gameObject.transform.localPosition, bubbleMaxRange);
            gameObject.transform.localPosition = clampedPos;
        }
    }


    
}
