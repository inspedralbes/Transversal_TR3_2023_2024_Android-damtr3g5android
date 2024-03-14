using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePointerController : MonoBehaviour
{
    public Transform player;
    public VariableJoystick joystick;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        AimAtJoystick();
    }

    void AimAtJoystick()
    {
        // Obtenemos la posición del joystick en un rango de -1 a 1 en ambos ejes
        Vector3 joystickPosition = new Vector3(joystick.Horizontal, joystick.Vertical, 0f);

        // Si el joystick no está siendo utilizado, no realizamos ninguna acción
        if (joystickPosition.magnitude < joystick.MoveThreshold)
            return;

        // Normalizamos la posición del joystick para obtener la dirección de apuntado
        Vector3 aimDirection = joystickPosition.normalized;

        // Aplicamos un offset para la posición de apuntado
        float offset = 1f;
        transform.position = player.position + aimDirection * offset;

        // Calculamos el ángulo de rotación para apuntar en la dirección adecuada
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;

        // Aplicamos la rotación al objeto que estamos controlando
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    /*void AimAtMouse() {

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        Vector3 aimDirection = (mousePosition - player.position).normalized;

        float offset = 1f;
        transform.position = player.position + aimDirection * offset;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg -90f;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }*/
}
