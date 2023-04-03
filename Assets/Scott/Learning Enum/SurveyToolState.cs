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
    //set height from 1.15 to 1.45 - to top of tripod, not including prism height
    [SerializeField] float height1;
    [SerializeField] float height2;
    [SerializeField] float height3;

    [SerializeField] int tripodSelect;



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
        if (tripod.active != true)
        {
            tripodSelect = Random.Range(1, 4);

            switch (tripodSelect)
            {
                case 1:
                    tripod.transform.position = new Vector3(tripod.transform.position.x, height1, tripod.transform.position.z);
                    break;
                case 2:
                    tripod.transform.position = new Vector3(tripod.transform.position.x, height2, tripod.transform.position.z);
                    break;
                case 3:
                    tripod.transform.position = new Vector3(tripod.transform.position.x, height3, tripod.transform.position.z);
                    break;

            }
        }

        tripod.SetActive(true);

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
