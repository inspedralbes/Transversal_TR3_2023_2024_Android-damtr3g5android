using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetObjects : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Chest"))
        {
            
            Destroy(collision.gameObject);            

        }
        else if (collision.gameObject.CompareTag("Life"))
        {

            this.gameObject.GetComponent<PlayerDamage>().SumarVida();
            Destroy(collision.gameObject);

        }


    }
}
