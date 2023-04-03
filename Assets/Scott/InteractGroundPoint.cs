using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractGroundPoint : MonoBehaviour
{
    [SerializeField] private GameObject tripod1;
    [SerializeField] private GameObject tripod2;
    [SerializeField] private GameObject tripod3;
    [SerializeField] private float maxInteractionDistance = 2;
    [SerializeField] private int tripod;
    public TextMeshProUGUI textSetupTripod;
    public EquipmentManager equipmentManager;
    public GameObject player;
    [SerializeField] float dist;
    public bool tripodSetup;
    


    // Start is called before the first frame update
    void Start()
    {
        tripod = 1;
        tripodSetup = false;
        textSetupTripod.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        dist = Vector3.Distance(this.transform.position, player.transform.position);

        if (dist <= maxInteractionDistance && tripodSetup == false)
        {
            textSetupTripod.enabled = true;
            if (Input.GetKeyDown(KeyCode.E) && equipmentManager.tripods < 3)
            {
                equipmentManager.tripods++;
                tripodSetup = true;
                //randomise which tripod is being selected
                tripod = Random.Range(1, 4);

                if (tripod == 1)
                {
                    tripod1.SetActive(true);
                }
                if (tripod == 2)
                {
                    tripod2.SetActive(true);
                }
                if (tripod == 3)
                {
                    tripod3.SetActive(true);
                }
                textSetupTripod.enabled = false;

            }
        }
        else
        {
            textSetupTripod.enabled = false;
        }
    }
}
