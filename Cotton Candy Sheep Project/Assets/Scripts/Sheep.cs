using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI; // AI declaration

public class Sheep : MonoBehaviour
{

    public GameObject wolf;
    public GameObject blackSheep;
    public PlayerController wolfScript;

    public float walkSpeed = 8;
    public float runSpeed = 12;

    public float detectionRange = 4;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        checkWolfOrBlackSheep();
    }

    // Checks if the wolf or black sheep is within range for a reaction from the sheep
    public void checkWolfOrBlackSheep()
    {
        Vector3 wolfOffset = transform.position - wolf.transform.position;
        Vector3 blackSheepOffset = transform.position - blackSheep.transform.position;

        float wolfDistance = wolfOffset.sqrMagnitude;
        float blackSheepDistance = blackSheepOffset.sqrMagnitude;

        if (wolfDistance <= detectionRange * detectionRange)
        {
            // If the wolf is the same color, move towards it
            if (wolfScript.matChange)// && wolf.GetComponent<Renderer>().material.color == GetComponent<Renderer>().material.color)
            {
                wolfOffset.y = 0;
                transform.LookAt(wolf.transform);

                transform.position += transform.forward * walkSpeed * Time.deltaTime;
                //transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            }

            // If the wolf is not disguised, run away
            else if (!wolfScript.matChange)
            {
                wolfOffset.y = 0;
                // Turns the sheep away from the wolf
                transform.rotation = Quaternion.LookRotation(wolfOffset.normalized);

                transform.position += transform.forward * runSpeed * Time.deltaTime;
                //transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            }
        }

        else if (blackSheepDistance <= detectionRange * detectionRange)
        {
            // Turns the sheep away from the black sheep
            blackSheepOffset.y = 0;
            transform.rotation = Quaternion.LookRotation(blackSheepOffset.normalized);

            transform.position += transform.forward * walkSpeed * Time.deltaTime;
            //transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }

        else
        {

        }
    }
}
