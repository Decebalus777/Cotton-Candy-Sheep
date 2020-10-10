using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //instances to change based off of custom preference
    public int movementSpeed = 10;
    public float duration = 20;

    #region Non-Public Variables
    GameObject potentialCloak;
    GameObject pickedUpCloak;
    GameObject toUseCloak;

    bool pickup = false;
    bool matChange = false;

    Color startColor;
    Color endColor;

    //placeholder materials
    Material baseMaterial;
    Material potentialMaterial;
    Material pickedupMaterial;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        baseMaterial = GetComponent<Renderer>().material;
        endColor = baseMaterial.color;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        MaterialCountDown();
    }

    public void Movement()
    {

        if(Input.GetKey("w"))
        {
            transform.position += Vector3.forward * movementSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a"))
        {
            transform.position += Vector3.left * movementSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s"))
        {
            transform.position += Vector3.back * movementSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d"))
        {
            transform.position += Vector3.right * movementSpeed * Time.deltaTime;
        }
        if(Input.GetKeyDown("j"))
        {
            Pickup();
        }
        if(Input.GetKeyDown("k"))
        {
            UseItem();
        }
        if(Input.GetKey("l"))
        {
            matChange = false;
            duration = 20;
            GetComponent<Renderer>().material.color = endColor;
        }
    }

    //Takes the picked up cloak and sets it to the current cloak of player
    void UseItem()
    {
        if(pickedupMaterial != null)
        {
            Destroy(toUseCloak);
            pickedUpCloak = null;
            GetComponent<Renderer>().material = pickedupMaterial;
            startColor = pickedupMaterial.color;
            pickedupMaterial = null;
            matChange = true;
        }      
    }

    //Grabs which ever cloak player is hovering over
    void Pickup()
    {
        if(pickup)
        {
            //Destroys the cloak that the player interacts with and sets the new cloak to use
            if (potentialCloak != null)
            {
                pickedUpCloak = potentialCloak;
                Destroy(potentialCloak);
                potentialCloak = null;
                pickedupMaterial = potentialMaterial;
            }

            //checks if there is no next material to change into creates a new instance of that material
            if (pickedUpCloak != null && toUseCloak == null)
            {
                toUseCloak = Instantiate(pickedUpCloak, transform.position + Vector3.up, Quaternion.identity);
                toUseCloak.transform.parent = gameObject.transform;
            }
            //if player already has a cloak they can use, destroys that and sets it as the new one
            else if (toUseCloak != null)
            {
                Destroy(toUseCloak);
                toUseCloak = null;
                toUseCloak = Instantiate(pickedUpCloak, transform.position + Vector3.up, Quaternion.identity);
                toUseCloak.transform.parent = gameObject.transform;
            }
        }
        pickup = false;
    }


    void MaterialCountDown()
    {
        if(matChange)
        {
            if (duration > 0)
                duration -= Time.deltaTime;

             StartFlashingMaterial();
            
            if(duration <= 0)
            {
                GetComponent<Renderer>().material.color = endColor;
                matChange = false;
                duration = 20;
            }
                
        }
    }

    /*
     * Slowly increase the speed at which the color flashes indicating the cloak is about to run out
     */
    void StartFlashingMaterial()
    {

        if (duration < 15.0f && duration > 10.0f)
        {
            GetComponent<Renderer>().material.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time, 1.5f));
        }

        if (duration < 10.0f && duration > 5.0f)
        {
            GetComponent<Renderer>().material.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time, 1f));
        }
        if (duration < 5.0f && duration > 0.0f)
        {

            GetComponent<Renderer>().material.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time, 0.5f));
        }
    }

    //Checks for any Cloaks player advances to
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Pickup")
        {
            pickup = true;
            potentialCloak = other.gameObject;
            potentialMaterial = other.gameObject.GetComponent<MeshRenderer>().material;
        }   
    }

    //resets options for picking up a new cloak
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Pickup")
        {
            pickup = false;
            potentialCloak = null;
            potentialMaterial = null;
        }   
    } 
}
