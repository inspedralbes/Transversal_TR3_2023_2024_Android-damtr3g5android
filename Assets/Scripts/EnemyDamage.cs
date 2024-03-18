using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamage : MonoBehaviour

{
    [SerializeField]private int life =20;
    [SerializeField]private Slider barraVida;


    private void Start()
    {
        barraVida.maxValue = life;
        barraVida.value = life;
    }

    private void Update()
    {
        if (life==0) {
            Destroy(this.gameObject);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            life--;
            barraVida.value = life;
            //Destroy(this.gameObject);
            Debug.Log(life);

        }

    }
}
