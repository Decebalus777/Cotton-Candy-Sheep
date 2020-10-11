using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepController : MonoBehaviour
{
    public float speed;
    public float wolfDistance;
    public float blackSheepDistance;
    public Transform wolf;
    public Transform blackSheep;
    private Rigidbody rb;
    private Vector3 movement;
    public bool followingWolf = false;
    public bool blackSheepNearby = false;
    public bool captured = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        var distWolf = Vector3.Distance(transform.position, wolf.position);

        if (distWolf < wolfDistance && !captured)
            RunFromWolf();

        if (followingWolf && !captured)
            FollowWolf();

        var distSheep = Vector3.Distance(transform.position, blackSheep.position);
        if (distSheep < blackSheepDistance && followingWolf && !captured)
        {
            followingWolf = false;
            blackSheepNearby = true;
        }
        else if (distSheep == blackSheepDistance && !captured)
        {
            followingWolf = false;
        }
        else
            blackSheepNearby = false;

        if (blackSheepNearby && !captured)
            RunFromBlackSheep();         
    }

    //for physics stuff
    void FixedUpdate()
    {
        if (followingWolf && !captured)
            MoveSheep(movement);       
    }
    
    //helper method to move sheep 
    private void MoveSheep(Vector3 direction)
    {
        rb.MovePosition(transform.position + (direction * speed * Time.deltaTime));
    }

    //if there is a cloak, follow
    void FollowWolf()
    {
        Vector3 direction = wolf.position - transform.position;
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        rb.rotation = Quaternion.Euler(0,angle,0);
        direction.Normalize();
        movement = direction;
    }

    //if no cloak, run from wolf
    void RunFromWolf()
    {
        Vector3 direction = transform.position - wolf.position;
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        rb.rotation = Quaternion.Euler(0, angle, 0);
        direction.Normalize();
        movement = direction;
    }

    //If black sheep is near by, runaway
    void RunFromBlackSheep()
    {
        Vector3 direction = transform.position - blackSheep.position;
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        rb.rotation = Quaternion.Euler(0, angle, 0);
        direction.Normalize();
        movement = direction;
    }
}
