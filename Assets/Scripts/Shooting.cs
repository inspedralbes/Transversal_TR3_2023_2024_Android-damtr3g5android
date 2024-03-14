using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletForce = 20f;
    public VariableJoystick joystick;
    private bool isJoystickMoving = false;


    // Update is called once per frame
    void Update()
    {
        Vector3 joystickPosition = new Vector3(joystick.Horizontal, joystick.Vertical, 0f);
        // Verificar si el joystick está en movimiento
        if (joystickPosition.magnitude < joystick.MoveThreshold)
        {
             isJoystickMoving = false;
            Debug.Log("Joystick is not moving");
            
        }
        else
        {
            isJoystickMoving = true;
            Debug.Log("Joystick is moving");
           
        }

        // Si el joystick está en movimiento, disparar continuamente
        if (isJoystickMoving)
        {
            Shoot();
            Debug.Log("Shoot");
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
