using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBehavior : MonoBehaviour

{
    [SerializeField]private int life =20;
    [SerializeField] private Slider barraVida;


    [SerializeField] private float attackRate = 0.5f; // Tiempo entre ataques en segundos
    [SerializeField] private float timeSinceLastAttack = 0f; // Tiempo transcurrido desde el último ataque


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
            
            //Debug.Log(life);

        }
        

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Verificar si ha pasado suficiente tiempo desde el último disparo
            if (Time.time - timeSinceLastAttack >= attackRate)
            {
                collision.gameObject.GetComponent<PlayerDamage>().RestarVida();
                timeSinceLastAttack = Time.time;
            }
            

        }
    }
}
