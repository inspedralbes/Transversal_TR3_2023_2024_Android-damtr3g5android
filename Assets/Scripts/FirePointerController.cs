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
        // Obtenemos la posici�n del joystick en un rango de -1 a 1 en ambos ejes
        Vector3 joystickPosition = new Vector3(joystick.Horizontal, joystick.Vertical, 0f);

        // Si el joystick no est� siendo utilizado, no realizamos ninguna acci�n
        if (joystickPosition.magnitude < joystick.MoveThreshold)
            return;

        // Normalizamos la posici�n del joystick para obtener la direcci�n de apuntado
        Vector3 aimDirection = joystickPosition.normalized;

        // Aplicamos un offset para la posici�n de apuntado
        float offset = 1f;
        transform.position = player.position + aimDirection * offset;

        // Calculamos el �ngulo de rotaci�n para apuntar en la direcci�n adecuada
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;

        // Aplicamos la rotaci�n al objeto que estamos controlando
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
