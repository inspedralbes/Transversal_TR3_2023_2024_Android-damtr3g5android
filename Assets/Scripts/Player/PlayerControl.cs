using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    public class PlayerControl : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField]
        public float speed =3;

        public float speedJoystick=3;

        [Header("Class Refereneces")]
        [SerializeField]
        private NetworkIdentity networkIdentity;

        private Animator animator;
        float horizontalMove = 0;
        float verticallMove = 0;
        private Rigidbody2D rigidBody;
        public Joystick joystick;



        Vector2 movement;
        Vector2 movementJoystick;



        private void Start()
        {
            animator = GetComponent<Animator>();
            rigidBody = GetComponent<Rigidbody2D>();
        }


        private void Update()
        {
            if (networkIdentity.IsControlling())
            {
                checkMovement();
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
                horizontalMove = joystick.Horizontal * speed;
                verticallMove = joystick.Vertical * speed;


                if (horizontalMove < 0)
                {

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
            
        }
        private void FixedUpdate()
        {
            if (movement != Vector2.zero)
            {
                rigidBody.MovePosition(rigidBody.position + movement * speed * Time.fixedDeltaTime);
            }
            else
            {
                rigidBody.MovePosition(rigidBody.position + movementJoystick * Time.fixedDeltaTime);
            }
        }

        /*void UpdateDirection(Vector2 movement)
        {
            if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
            {
                // Horizontal movement
                animator.SetInteger("Direction", movement.x > 0 ? 1 : 3); // Right = 1, Left = 3
            }
            else
            {
                // Vertical movement
                animator.SetInteger("Direction", movement.y > 0 ? 0 : 2); // Up = 0, Down = 2
            }
        }*/





    }
}
