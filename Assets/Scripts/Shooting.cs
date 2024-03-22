using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Networking;


public class Shooting : MonoBehaviour
{
    [Header("Class Refereneces")]
    [SerializeField]
    private NetworkIdentity networkIdentity;

    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletForce = 20f;
    public VariableJoystick joystick;   

    public float fireRate = 0.5f; // Tiempo entre disparos en segundos
    private float timeSinceLastShot = 0f; // Tiempo transcurrido desde el último disparo


    // Update is called once per frame
    void Update()
    {
        Vector3 joystickPosition = new Vector3(joystick.Horizontal, joystick.Vertical, 0f);
       
        // Verificar si el joystick está en el centro
        if (joystick.Horizontal == 0f && joystick.Vertical == 0f)
        {
            // El joystick está en el centro, detener el disparo
            return;
        }

        // Verificar si ha pasado suficiente tiempo desde el último disparo
        if (Time.time - timeSinceLastShot >= fireRate)
        {
            // Disparar y actualizar el tiempo del último disparo
            Shoot();
            timeSinceLastShot = Time.time;
        }
    }
    void Shoot()
    {
        Quaternion bulletRotation = Quaternion.Euler(firePoint.eulerAngles.x, firePoint.eulerAngles.y, firePoint.eulerAngles.z + 90);
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, bulletRotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    }
}
