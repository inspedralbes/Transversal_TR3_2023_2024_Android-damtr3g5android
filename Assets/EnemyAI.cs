using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{

    public Transform target;
    public Animator enemyGFX;

    public float speed = 200f;
    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, .5f);
        
    }
    void UpdatePath()
    {
        if (seeker.IsDone()) {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
        
    }

    void OnPathComplete(Path p) {
        if (!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        if (path == null)
            return;
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        { 
            reachedEndOfPath= false;
        }
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed* Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance) 
        {
            currentWaypoint++;
        }
        // Reset speed, horizontal, and vertical parameters
        enemyGFX.SetFloat("speed", 0);
        enemyGFX.SetFloat("horizontal", 0);
        enemyGFX.SetFloat("vertical", 0);

        // Check horizontal movement
        if (Mathf.Abs(force.x) >= Mathf.Abs(force.y))
        {
            if (force.x > 0)
            {
                enemyGFX.SetFloat("horizontal", 1);
            }
            else if (force.x < 0)
            {
                enemyGFX.SetFloat("horizontal", -1);
            }
        }
        else // Vertical movement
        {
            if (force.y > 0)
            {
                enemyGFX.SetFloat("vertical", 1);
            }
            else if (force.y < 0)
            {
                enemyGFX.SetFloat("vertical", -1);
            }
        }

        // Set speed parameter
        float movementSpeed = force.magnitude;
        enemyGFX.SetFloat("speed", movementSpeed);
    }

}
