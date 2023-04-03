using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Tribrach : MonoBehaviour
{
    [SerializeField] private float maxInteractionDistance = 1;
    public bool tripodSetup;
    public TextMeshProUGUI textTribrach;
    public EquipmentManager equipmentManager;
    public GameObject player;
    [SerializeField] float dist;
    public bool prismSetup;
    public bool totalStnSetup;
    public InteractGroundPoint groundPoint;

    // Start is called before the first frame update
    void Start()
    {
        tripodSetup = false;
        prismSetup = false;
        totalStnSetup = false;
        textTribrach.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.activeInHierarchy == true)
        {
            tripodSetup = true;
        }
        else
        {
            tripodSetup = false;
        }

        if (tripodSetup == true)
        {
            dist = Vector3.Distance(this.transform.position, player.transform.position);

            if (dist <= maxInteractionDistance)
            {
                textTribrach.enabled = true;
                if (Input.GetKeyDown(KeyCode.R))
                {
                    equipmentManager.tripods--;
                    textTribrach.enabled = false;
                    groundPoint.tripodSetup = false;
                    gameObject.SetActive(false);
                }
            }
            else
            {
                textTribrach.enabled = false;
            }
        }
    }
}
