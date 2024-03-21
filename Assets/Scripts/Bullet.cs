using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject hitEffect;
    public LayerMask destroyLayers;

    void OnCollisionEnter2D(Collision2D collision) {

        if (destroyLayers == (destroyLayers | (1 << collision.gameObject.layer)))
        {
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 0.2f); 
            Destroy(gameObject); 
        }
    }
}
