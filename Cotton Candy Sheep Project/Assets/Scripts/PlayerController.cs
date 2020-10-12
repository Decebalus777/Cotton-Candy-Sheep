using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //instances to change based off of custom preference
    public int movementSpeed = 10;
    public float duration = 20;
    public GameObject cloak;
    public GameObject costume;
    //public GameObject instr;
    public Material[] cloaks;
    public List<GameObject> followingSheep;

    // Needed public for Sheep AI
    public bool matChange = false;

    #region Non-Public Variables
    GameObject potentialCloak;
    GameObject pickedUpCloak;
    GameObject toUseCloak;

    bool pickup = false;

    Color startColor;
    Color endColor;

    //placeholder materials
     Material baseMaterial; //Do not change
     public Material potentialMaterial;
     public Material pickedupMaterial;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        baseMaterial = costume.GetComponent<Renderer>().material;
        endColor = baseMaterial.color;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Color of Wolf is: " + GetComponent<Renderer>().material.name);
        Movement();
        MaterialCountDown();
        /*
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            Destroy(instr);
        }*/
    }

    void FixedUpdate()
    {
        LureSheep();
    }

    public void Movement()
    {

        if(Input.GetKey("w"))
        {
            
            transform.position += Vector3.forward * movementSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward.normalized), 0.2f);
        }
        if (Input.GetKey("a"))
        {
            transform.position += Vector3.left * movementSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left.normalized), 0.2f);
        }
        if (Input.GetKey("s"))
        {
            transform.position += Vector3.back * movementSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back.normalized), 0.2f);
        }
        if (Input.GetKey("d"))
        {
            transform.position += Vector3.right * movementSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.2f);
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
            ResetWolf();
        }
    }

    void ResetWolf()
    {
        //GetComponent<Renderer>().material = baseMaterial;
        cloak.SetActive(false);
        matChange = false;
        duration = 20;
        if (followingSheep.Capacity != 0)
        {
            foreach (GameObject sheep in followingSheep)
            {
                sheep.GetComponent<SheepController>().followingWolf = false;
            }
        }
        GameObject.FindGameObjectWithTag("BlackSheep").GetComponent<BlackSheepController>().followingWolf = false;
    }
    //Takes the picked up cloak and sets it to the current cloak of player
    void UseItem()
    {
        if (!cloak.activeSelf)
            cloak.SetActive(true);
        if (pickedupMaterial != null)
        {
            duration = 20;
            Destroy(toUseCloak);
            pickedUpCloak = null;
            costume.GetComponent<Renderer>().material = pickedupMaterial;
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

    //slowly returns color back to base
    void MaterialCountDown()
    {
        if(matChange)
        {
            if (duration > 0)
                duration -= Time.deltaTime;

             StartFlashingMaterial();
            
            if(duration <= 0)
            {
                cloak.SetActive(false);
                costume.GetComponent<Renderer>().material = baseMaterial;
                matChange = false;
                duration = 20;
                if(followingSheep.Capacity != 0)
                    foreach(GameObject sheep in followingSheep)
                    {
                        sheep.GetComponent<SheepController>().followingWolf = false;
                    }
                GameObject.FindGameObjectWithTag("BlackSheep").GetComponent<BlackSheepController>().followingWolf = false;
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
            cloak.GetComponentInChildren<Renderer>().material.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time, 1.5f));
        }

        if (duration < 10.0f && duration > 5.0f)
        {
            cloak.GetComponentInChildren<Renderer>().material.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time, 1f));
        }
        if (duration < 5.0f && duration > 0.0f)
        {

            cloak.GetComponentInChildren<Renderer>().material.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time, 0.5f));
        }
    }

    void LureSheep()
    {
        if(cloak.activeSelf)
        {
            foreach (GameObject sheep in GameObject.FindGameObjectsWithTag("Sheep"))
            {
                //Debug.Log("Sheep Color is: " + sheep.GetComponent<Renderer>().materials[1].name);
                if (costume.GetComponent<Renderer>().material.name.Contains(sheep.GetComponent<Renderer>().materials[1].name))
                {
                    followingSheep.Add(sheep);
                    GameObject.FindGameObjectWithTag("BlackSheep").GetComponent<BlackSheepController>().followingWolf = true;
                    sheep.GetComponent<SheepController>().followingWolf = true;
                } 
                else
                {
                    //GameObject.FindGameObjectWithTag("BlackSheep").GetComponent<BlackSheepController>().followingWolf = false;
                    sheep.GetComponent<SheepController>().followingWolf = false;
                }
                    
            }
        }
    }
    //Checks for any Cloaks player advances to
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Pickup")
        {
            pickup = true;
            for(int i = 0; i < cloaks.Length; i++)
            {
                //to check if the cloak is in the list of cloaks that can be picked up
                if(other.gameObject.GetComponent<Renderer>().material.name.Contains(cloaks[i].name))
                {
                    potentialCloak = other.gameObject;
                    potentialMaterial = cloaks[i];
                }
            }
            
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
