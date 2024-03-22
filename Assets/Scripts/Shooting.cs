using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    [SerializeField] private AudioSource shotSound;
    [SerializeField] private AudioSource shotGunSound;
    public GameObject bulletPrefab;
    public float bulletForce = 20f;
    public VariableJoystick joystick;
    [SerializeField] private Slider barraAmmo;
    private int bulletDamage = 1;

    public float fireRate = 0.5f; // Tiempo entre disparos en segundos
    private float timeSinceLastShot = 0f; // Tiempo transcurrido desde el último disparo
    private int ammoGun = 100;
    private int ammoMachineGun = 0;
    private int ammoShotGun = 0;
    private bool canShoot = true;
    private string weaponSelected= "";
    

    private void Start()
    {
        barraAmmo.maxValue = ammoGun;
        SetAmmogun();
       
    }


    // Update is called once per frame
    void Update()
    {
        comprobarAmmo();
        Vector3 joystickPosition = new Vector3(joystick.Horizontal, joystick.Vertical, 0f);
       
        // Verificar si el joystick está en el centro
        if (joystick.Horizontal == 0f && joystick.Vertical == 0f)
        {
            // El joystick está en el centro, detener el disparo
            return;
        }       

        // Verificar si ha pasado suficiente tiempo desde el último disparo
        if (Time.time - timeSinceLastShot >= fireRate && canShoot)
        {
            // Disparar y actualizar el tiempo del último disparo
            Shoot();
            setAmmo();           
            timeSinceLastShot = Time.time;
        }
        
    }
       

    // Método público para cambiar el tipo arma
    public void ChangeWeapon(string newWeapon)
    {
        if (newWeapon.Equals("Gun"))
        {
            
            fireRate = 0.5f;
            weaponSelected = newWeapon;
            SetAmmogun();
        }
        else if (newWeapon.Equals("Machinegun")) {
            
            fireRate = 0.1f;
            if (ammoMachineGun == 0)
            {
                canShoot = false;
            }
            else {
                canShoot = true;
            }
            SetAmmoMachinegun();
            weaponSelected = newWeapon;

        }
        else if (newWeapon.Equals("Shotgun"))
        {
            
            fireRate = 0.7f;
            if (ammoShotGun == 0)
            {
                canShoot = false;
            }
            else
            {
                canShoot = true;
            }
            SetAmmoShotgun();
            weaponSelected = newWeapon;

        }

    }

    public void GetAmmo(int numero)
    {
        if (numero == 1)
        {
            ammoMachineGun += 20;
            if (weaponSelected.Equals("Machinegun"))
            {
                SetAmmoMachinegun();
            }
            Debug.Log("Municion Machinegun +20");


        }
        else if (numero == 2)
        {
            ammoShotGun += 10;
            if (weaponSelected.Equals("Shotgun")) { 
                SetAmmoShotgun();
            }
            Debug.Log("Municion Shotgun +10");            

        }

    }

    void setAmmo()
    {
        if (weaponSelected.Equals("Machinegun")&& ammoMachineGun>0) {
            ammoMachineGun--;
            SetAmmoMachinegun();
            Debug.Log("Municion MachineGun: "+ammoMachineGun);
           
        }
        else if (weaponSelected.Equals("Shotgun")&& ammoShotGun>0)
        {
            ammoShotGun--;
            SetAmmoShotgun();
            Debug.Log("Municion Shotgun: "+ammoShotGun);
            
        }
    }

    void comprobarAmmo()
    {
        if (weaponSelected.Equals("Machinegun") && ammoMachineGun == 0)
        {
            canShoot = false;
        }
        else if (weaponSelected.Equals("Shotgun") && ammoShotGun == 0)
        {
            canShoot = false;
        }
        else if (weaponSelected.Equals("Gun"))
        {
            canShoot = true;
        }
        else {
            canShoot = true;
        }
    
    }

    void Shoot()
    {
        Quaternion bulletRotation = Quaternion.Euler(firePoint.eulerAngles.x, firePoint.eulerAngles.y, firePoint.eulerAngles.z + 90);
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, bulletRotation);
        bullet.GetComponent<Bullet>().damage = bulletDamage;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);

        if (weaponSelected.Equals("Shotgun"))
        {
            // Reproducir el sonido de disparo de escopeta
            shotGunSound.Play();
        }
        else
        {
            shotSound.Play();
        }


    }

    void SetAmmogun()
    {
        barraAmmo.value = ammoGun;
        barraAmmo.GetComponentInChildren<Text>().text = "Infinite";
        bulletDamage = 1;

    }

    void SetAmmoMachinegun() {
        barraAmmo.value = ammoMachineGun;
        barraAmmo.GetComponentInChildren<Text>().text = ammoMachineGun.ToString();
        bulletDamage = 1;

    }

    void SetAmmoShotgun()
    {
        barraAmmo.value = ammoShotGun;
        barraAmmo.GetComponentInChildren<Text>().text = ammoShotGun.ToString();
        bulletDamage = 5;
    }

}
