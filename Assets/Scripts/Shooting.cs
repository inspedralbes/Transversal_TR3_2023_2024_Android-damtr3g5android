using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletForce = 20f;
    public VariableJoystick joystick;
    [SerializeField] private Slider barraAmmo;

    public float fireRate = 0.5f; // Tiempo entre disparos en segundos
    private float timeSinceLastShot = 0f; // Tiempo transcurrido desde el �ltimo disparo
    private int ammoGun = 100;
    private int ammoMachineGun = 0;
    private int ammoShotGun = 0;

    private void Start()
    {
        barraAmmo.maxValue = ammoGun;
        barraAmmo.value = ammoGun;
    }


    // Update is called once per frame
    void Update()
    {
        Vector3 joystickPosition = new Vector3(joystick.Horizontal, joystick.Vertical, 0f);
       
        // Verificar si el joystick est� en el centro
        if (joystick.Horizontal == 0f && joystick.Vertical == 0f)
        {
            // El joystick est� en el centro, detener el disparo
            return;
        }

        // Verificar si ha pasado suficiente tiempo desde el �ltimo disparo
        if (Time.time - timeSinceLastShot >= fireRate)
        {
            // Disparar y actualizar el tiempo del �ltimo disparo
            Shoot();
            timeSinceLastShot = Time.time;
        }
    }

    // M�todo p�blico para cambiar el fireRate
    public void ChangeFireRate(float newFireRate)
    {
        fireRate = newFireRate;
    }

    // M�todo p�blico para cambiar el tipo municion
    public void ChangeWeapon(string newWeapon)
    {
        if (newWeapon.Equals("Gun"))
        {
            barraAmmo.value = ammoGun;
            fireRate = 0.5f;

        }
        else if (newWeapon.Equals("Machinegun")) {
            barraAmmo.value = ammoMachineGun;
            fireRate = 0.1f;

        }
        else if (newWeapon.Equals("Shotgun"))
        {
            barraAmmo.value = ammoShotGun;
            fireRate = 0.7f;

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
