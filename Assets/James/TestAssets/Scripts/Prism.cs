using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prism : MonoBehaviour
{
    [SerializeField] private Transform vertRayHgt;
    [ReadOnly] public float rayHeight;

    [SerializeField] LayerMask _layerGround;

    void Update()
    {
        GetHeight();
    }

    void GetHeight()
    {
        RaycastHit hit;

        if (Physics.Raycast(vertRayHgt.transform.position, vertRayHgt.transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, _layerGround))
        {
            Debug.DrawRay(vertRayHgt.transform.position, vertRayHgt.transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
            //Debug.Log("Did Hit " + $"<{hit.transform.name}>");

            rayHeight = (float)System.Math.Round(hit.distance, 3);
        }
        else
        {
            Debug.DrawRay(vertRayHgt.transform.position, vertRayHgt.transform.TransformDirection(Vector3.down) * 1000, Color.white);
            //Debug.Log("Did not Hit");

            rayHeight = 0;
        }
    }
}
