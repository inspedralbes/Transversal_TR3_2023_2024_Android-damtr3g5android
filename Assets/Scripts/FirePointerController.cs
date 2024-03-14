using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePointerController : MonoBehaviour
{
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AimAtMouse();
    }
    void AimAtMouse() {

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        Vector3 aimDirection = (mousePosition - player.position).normalized;

        float offset = 1f;
        transform.position = player.position + aimDirection * offset;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg -90f;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
