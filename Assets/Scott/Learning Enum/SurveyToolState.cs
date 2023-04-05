using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveyToolState : MonoBehaviour
{
    public SurveyState surveyState;

    [SerializeField] GameObject groundPoint;
    [SerializeField] GameObject tripod;
    [SerializeField] GameObject prism;
    [SerializeField] GameObject totalStn;
    [Header("Random 1.15 - 1.45")]
    [SerializeField] float height;
    
    // Start is called before the first frame update
    void Start()
    {
        surveyState = SurveyState.GP;
    }

    // Update is called once per frame
    void Update()
    {
        switch (surveyState)
        {
            case SurveyState.GP:
                GroundPoint();
                break;
            case SurveyState.Tripod:
                Tripod();
                break;
            case SurveyState.TotalStn:
                TotalStation();
                break;
            case SurveyState.Prism:
                Prism();
                break;

        }
    }

    void GroundPoint()
    {
        if (tripod.active == true)
        {
            tripod.SetActive(false);
        }
    }

    void Tripod()
    {
        if (tripod.active == false)
        {
            height = Random.Range(1.15f, 1.45f);

            tripod.transform.position = new Vector3(tripod.transform.position.x, height, tripod.transform.position.z);

            tripod.SetActive(true);
        }

        

        if (prism.active == true)
        {
            prism.SetActive(false);
        }
        if (totalStn.active == true)
        {
            totalStn.SetActive(false);
        }

    }

    void TotalStation()
    {
        totalStn.SetActive(true);
    }

    void Prism()
    {
        prism.SetActive(true);
    }
}
