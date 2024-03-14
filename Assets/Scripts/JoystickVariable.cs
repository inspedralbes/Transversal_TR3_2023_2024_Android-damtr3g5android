using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickVariable : Joystick
{
    public float MoveThreshold { get { return moveThreshold; } set { moveThreshold = Mathf.Abs(value); } }

    [SerializeField] private float moveThreshold = 1;
    [SerializeField] private TypeJoystick TypeJoystick = TypeJoystick.Dynamic;

    private Vector2 fixedPosition = Vector2.zero;

    public void SetMode(TypeJoystick TypeJoystick)
    {
        this.TypeJoystick = TypeJoystick;
        if(TypeJoystick == TypeJoystick.Fixed)
        {
            background.anchoredPosition = fixedPosition;
            background.gameObject.SetActive(true);
        }
        else
            background.gameObject.SetActive(false);
    }

    protected override void Start()
    {
        base.Start();
        fixedPosition = background.anchoredPosition;
        SetMode(TypeJoystick);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if(TypeJoystick != TypeJoystick.Fixed)
        {
            background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            background.gameObject.SetActive(true);
        }
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        SetMode(TypeJoystick.Fixed);
        if(TypeJoystick != TypeJoystick.Fixed)
            background.gameObject.SetActive(false);
           
        base.OnPointerUp(eventData);
    }

    protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        if (TypeJoystick == TypeJoystick.Dynamic && magnitude > moveThreshold)
        {
            Vector2 difference = normalised * (magnitude - moveThreshold) * radius;
            background.anchoredPosition += difference;
        }
        base.HandleInput(magnitude, normalised, radius, cam);
    }
}
public enum TypeJoystick { Fixed, Floating, Dynamic }