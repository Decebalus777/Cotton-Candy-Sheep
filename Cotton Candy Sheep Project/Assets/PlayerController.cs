using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int movementSpeed = 10;
    public int destroyTime = 30;

    //public for debugging purposes
    public GameObject currentCloak;
    public GameObject potentialCloak;
    public GameObject pickedUpCloak;
    public GameObject toUseCloak;
    public bool pickup = false;
    

    //placeholder materials
    Material baseMaterial;
    Material potentialMaterial;
    Material pickedupMaterial;
    // Start is called before the first frame update
    void Start()
    {
        baseMaterial = gameObject.GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
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
    }

    //Takes the picked up cloak and sets it to the current cloak of player
    void UseItem()
    {
        if(pickedupMaterial != null)
        {
            currentCloak = pickedUpCloak;
            Destroy(toUseCloak);
            pickedUpCloak = null;
            gameObject.GetComponent<MeshRenderer>().material = pickedupMaterial;
            pickedupMaterial = null;
        }

        StartCoroutine(WaitThenDie());
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
    //timer for when materials change
    IEnumerator WaitThenDie()
    {
        yield return new WaitForSeconds(destroyTime);
        gameObject.GetComponent<Renderer>().material = baseMaterial;
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
