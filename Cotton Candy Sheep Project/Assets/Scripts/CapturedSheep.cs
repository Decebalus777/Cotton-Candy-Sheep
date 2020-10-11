using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapturedSheep : MonoBehaviour
{
    public int sheepInGame;
    public int capturedSheepCount;
    List<GameObject> sheepList;

    // Start is called before the first frame update
    void Start()
    {
        sheepList = new List<GameObject>();
        foreach (GameObject sheep in GameObject.FindGameObjectsWithTag("Sheep"))
            sheepList.Add(sheep);
        sheepList.Add(GameObject.FindGameObjectWithTag("BlackSheep"));

        sheepInGame = sheepList.Count;

        Debug.Log("Amount of Sheep in Game is: " + sheepList.Count);
    }

    // Update is called once per frame
    void Update()
    {
        if (capturedSheepCount == sheepList.Count)
            Debug.LogError("ALL SHEEP CAPTURED - PLAYER WINS");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name + " has Enter the Den.");
        if(other.gameObject.CompareTag("Sheep"))
        {
            other.gameObject.GetComponent<SheepController>().captured = true;
            capturedSheepCount++;
        }
        if(other.gameObject.CompareTag("BlackSheep") && capturedSheepCount!= sheepList.Count - 1)
        {
            Debug.LogError("BLACK SHEEP ENTERED DEN BEFORE ALL OTHER SHEEP ARE CAPTURED - PLAYER LOST");
        }
        if (other.gameObject.CompareTag("BlackSheep") && capturedSheepCount == sheepList.Count - 1)
        {
            capturedSheepCount++;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.gameObject.name + " has Exited the Den.");
        if (other.gameObject.CompareTag("Sheep"))
        {
            other.gameObject.GetComponent<SheepController>().captured = false;
            capturedSheepCount--;
        }
    }
}
