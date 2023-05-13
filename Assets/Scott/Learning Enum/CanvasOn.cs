using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CanvasOn : MonoBehaviour
{
    [SerializeField] private bool canvasIsOn;
    [SerializeField] private CinemachineVirtualCamera parentCam;
    [SerializeField] private Canvas[] canvases;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < canvases.Length; i++)
        {
            canvases[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (parentCam.Priority == 10 && !canvasIsOn)
        {
            for (int i = 0; i < canvases.Length; i++)
            {
                canvases[i].gameObject.SetActive(true);
            }
            canvasIsOn = true;
        }
        else if (parentCam.Priority != 10 && canvasIsOn)
        {
            for (int i = 0; i < canvases.Length; i++)
            {
                canvases[i].gameObject.SetActive(false);
            }
            canvasIsOn = false;
        }
    }
}
