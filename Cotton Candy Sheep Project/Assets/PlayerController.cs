using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int movementSpeed = 10;
    public GameObject currentCloak;
    public GameObject potentialCloak;
    public GameObject pickedUpCloak;
    public bool pickup = false;

    Material baseMaterial;
    Material potentialMaterial;
    Material currentMaterial;
    Material pickedupMaterial;
    // Start is called before the first frame update
    void Start()
    {
        baseMaterial = gameObject.GetComponent<MeshRenderer>().material;
        currentMaterial = baseMaterial;
        gameObject.GetComponent<MeshRenderer>().material = currentMaterial;
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
    void UseItem()
    {
        if(pickedupMaterial != null)
        {
            currentCloak = pickedUpCloak;
            pickedUpCloak = null;
            gameObject.GetComponent<MeshRenderer>().material = pickedupMaterial;
            pickedupMaterial = null;
        }
    }

    void Pickup()
    {
        if(pickup && potentialCloak != null)
        {
            pickedUpCloak = potentialCloak;
            pickedupMaterial = potentialMaterial;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Pickup")
        {
            pickup = true;
            potentialCloak = other.gameObject;
            potentialMaterial = other.gameObject.GetComponent<MeshRenderer>().material;
        }   
    }

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
