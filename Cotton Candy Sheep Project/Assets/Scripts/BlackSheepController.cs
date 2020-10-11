using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackSheepController : MonoBehaviour
{
    public float speed;
    public float wolfDistance;
    public Transform wolf;
    private Rigidbody rb;
    private Vector3 movement;
    public bool followingWolf = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        var distWolf = Vector3.Distance(transform.position, wolf.position);

        if (distWolf < wolfDistance)
            RunFromWolf();

        if (followingWolf)
            FollowWolf();
    }

    void FixedUpdate()
    {
        if (followingWolf)
            MoveSheep(movement);
    }

    private void MoveSheep(Vector3 direction)
    {
        rb.MovePosition(transform.position + (direction * speed * Time.deltaTime));
    }

    void FollowWolf()
    {
        Vector3 direction = wolf.position - transform.position;
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        rb.rotation = Quaternion.Euler(0, angle, 0);
        direction.Normalize();
        movement = direction;
    }

    void RunFromWolf()
    {
        Vector3 direction = transform.position - wolf.position;
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        rb.rotation = Quaternion.Euler(0, angle, 0);
        direction.Normalize();
        movement = direction;
    }
}
