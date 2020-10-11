using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CapturedSheep : MonoBehaviour
{
    public int sheepInGame;
    public int capturedSheepCount;
    List<GameObject> sheepList;
    List<GameObject> capturedSheepList;

    // Start is called before the first frame update
    void Start()
    {
        sheepList = new List<GameObject>();
        capturedSheepList = new List<GameObject>();
        foreach (GameObject sheep in GameObject.FindGameObjectsWithTag("Sheep"))
            sheepList.Add(sheep);
        sheepList.Add(GameObject.FindGameObjectWithTag("BlackSheep"));

        sheepInGame = sheepList.Count;

        Debug.Log("Amount of Sheep in Game is: " + sheepList.Count);
    }

    // Update is called once per frame
    void Update()
    {
        if(capturedSheepList.Capacity != sheepList.Capacity)
        {
            foreach (GameObject sheep in capturedSheepList)
            {
                if (sheep.CompareTag("BlackSheep") && capturedSheepCount != sheepList.Capacity - 1)
                    Debug.LogError("BLACK SHEEP ENTERED DEN BEFORE ALL OTHER SHEEP ARE CAPTURED - PLAYER LOST");
            }
        }
        else
        {
            bool win = true;
            foreach (GameObject sheep in sheepList)
            {
                
                if (!capturedSheepList.Contains(sheep))
                    win = false;

            }
           if(win)
                Debug.LogError("BLACK SHEEP ENTERED DEN, ALL OTHER SHEEP ARE CAPTURED - PLAYER WINS");
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name + " has Enter the Den.");
        if(other.gameObject.CompareTag("Sheep"))
        {
            other.gameObject.GetComponent<SheepController>().captured = true;
            capturedSheepList.Add(other.gameObject);
            capturedSheepCount++;
        }

        if (other.gameObject.CompareTag("BlackSheep"))
        {
            other.gameObject.GetComponent<BlackSheepController>().captured = true;
            capturedSheepList.Add(other.gameObject);
            capturedSheepCount++;
        }
        /*
        if (other.gameObject.CompareTag("BlackSheep") && capturedSheepCount == sheepList.Count)
        {
            other.gameObject.GetComponent<BlackSheepController>().captured = true;
            capturedSheepCount++;
        }
        */
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.gameObject.name + " has Exited the Den.");
        if (other.gameObject.CompareTag("Sheep"))
        {
            other.gameObject.GetComponent<SheepController>().captured = false;
            capturedSheepList.Remove(other.gameObject);
            capturedSheepCount--;
        }
    }
}
