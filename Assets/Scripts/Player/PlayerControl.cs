using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using Newtonsoft.Json.Linq;
using Networking;

namespace Cainos.PixelArtTopDown_Basic
{
    public class PlayerControl : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField]
        public float speed = 3;

        public float speedJoystick = 3;
        public Transform firePoint;

        [Header("Class Refereneces")]
        [SerializeField]
        private NetworkIdentity networkIdentity;

        private Animator animator;
        float horizontalMove = 0;
        float verticallMove = 0;
        private Rigidbody2D rigidBody;
        public Joystick movingJoystick;
        public VariableJoystick fireJoystick;



        Vector2 movement;
        Vector2 movementJoystick;

        //Shooting
        private Cooldown shootingCooldown;
        private BulletData bulletData;



        private void Start()
        {
            shootingCooldown = new Cooldown(1);
            bulletData = new BulletData();
            bulletData.position = new Position();
            bulletData.direction = new Position();
            animator = GetComponent<Animator>();
            rigidBody = GetComponent<Rigidbody2D>();
        }


        private void FixedUpdate()
        {
            if (networkIdentity.IsControlling())
            {
                checkMovement();
                checkShooting();
            }

        }
        public void checkMovement()
        {
            movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            animator.SetFloat("horizontal", movement.x);
            animator.SetFloat("vertical", movement.y);
            animator.SetFloat("speed", movement.sqrMagnitude);



            if (movement != Vector2.zero)
            {
                //UpdateDirection(movement);
                animator.SetFloat("lastHorizontal", movement.x);
                animator.SetFloat("lastVertical", movement.y);
            }
            else
            {
                horizontalMove = fireJoystick.Horizontal * speed;
                verticallMove = fireJoystick.Vertical * speed;


                if (horizontalMove < 0)
                {
                    Debug.Log("A la izquierda");
                    animator.SetFloat("horizontal", horizontalMove);

                }
                else if (horizontalMove > 0)
                {

                    animator.SetFloat("horizontal", horizontalMove);

                }

                if (verticallMove > 0 && Mathf.Abs(verticallMove) > Mathf.Abs(horizontalMove))
                {

                    animator.SetFloat("vertical", verticallMove);
                }
                else if (verticallMove < 0 && Mathf.Abs(verticallMove) > Mathf.Abs(horizontalMove))
                {

                    animator.SetFloat("vertical", verticallMove);
                }

                movementJoystick = new Vector2(horizontalMove, verticallMove).normalized * speedJoystick;
                animator.SetFloat("speed", movementJoystick.sqrMagnitude);
            }
            if (movement != Vector2.zero)
            {
                rigidBody.MovePosition(rigidBody.position + movement * speed * Time.fixedDeltaTime);
            }
            else
            {
                rigidBody.MovePosition(rigidBody.position + movementJoystick * Time.fixedDeltaTime);
            }

        }
        public void checkShooting()
        {

            shootingCooldown.CooldownUpdate();
            Vector3 joystickPosition = new Vector3(fireJoystick.Horizontal, fireJoystick.Vertical, 0f);

            // Verificar si el joystick está en el centro
            /*
            if (fireJoystick.Horizontal == 0f && fireJoystick.Vertical == 0f)
            {
                return;
            }
            */

            // Verificar si ha pasado suficiente tiempo desde el último disparo
            if (!shootingCooldown.IsOnCooldown() && Input.GetKeyDown(KeyCode.R))
            {
                // Disparar y actualizar el tiempo del último disparo
                
                shootingCooldown.StartCooldown();
                Shoot();
            }
            void Shoot()
            {
                bulletData.position.x = firePoint.position.x.TwoDecimals();
                bulletData.position.y = firePoint.position.y.TwoDecimals();
                bulletData.direction.x = firePoint.up.x;
                bulletData.direction.x = firePoint.up.y;
                string bulletJson = JsonUtility.ToJson(bulletData);
                JObject data = new JObject
                {
                    ["event"] = "fireBullet",
                    ["playerID"] = networkIdentity.GetId(),
                    ["bulletdata"] = JObject.Parse(bulletJson)
                };
                string message = data.ToString(Newtonsoft.Json.Formatting.None);

                networkIdentity.GetSocket().Send(message);
                /* Quaternion bulletRotation = Quaternion.Euler(firePoint.eulerAngles.x, firePoint.eulerAngles.y, firePoint.eulerAngles.z + 90);
             GameObject bullet = Instantiate(bulletPrefab, firePoint.position, bulletRotation);
             Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
             Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
             rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);*/
            }

        }
    }
}
