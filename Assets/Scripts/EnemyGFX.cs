using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyGFX : MonoBehaviour
{
    public AIPath aiPath;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Reset speed, horizontal, and vertical parameters
        anim.SetFloat("speed", 0);
        anim.SetFloat("horizontal", 0);
        anim.SetFloat("vertical", 0);

        // Check horizontal movement
        if (Mathf.Abs(aiPath.desiredVelocity.x) >= Mathf.Abs(aiPath.desiredVelocity.y))
        {
            if (aiPath.desiredVelocity.x > 0)
            {
                anim.SetFloat("horizontal", 1);
            }
            else if (aiPath.desiredVelocity.x < 0)
            {
                anim.SetFloat("horizontal", -1);
            }
        }
        else // Vertical movement
        {
            if (aiPath.desiredVelocity.y > 0)
            {
                anim.SetFloat("vertical", 1);
            }
            else if (aiPath.desiredVelocity.y < 0)
            {
                anim.SetFloat("vertical", -1);
            }
        }

        // Set speed parameter
        float movementSpeed = aiPath.desiredVelocity.magnitude;
        anim.SetFloat("speed", movementSpeed);
    }
}
