using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotonPulsado : MonoBehaviour
{
    private Toggle toggle;
    private Image toggleImage;
    private RectTransform rectTransform;
    private Vector3 initialPosition; // Posici�n inicial del bot�n


    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
        toggleImage = GetComponent<Image>(); // Obtener el componente Image del ToggleButton
        rectTransform = GetComponent<RectTransform>(); // Obtener el componente RectTransform del ToggleButton
        initialPosition = rectTransform.localPosition; // Guardar la posici�n inicial del bot�n

        cambioBoton(toggle.isOn);
    }

    void OnToggleValueChanged(bool isOn)
    {
        cambioBoton(isOn);


    }

    public void cambioBoton(bool isOn) {
        if (isOn)
        {
            // Crear un nuevo color con la misma configuraci�n pero con la transparencia m�xima
            Color activatedColor = new Color(toggleImage.color.r, toggleImage.color.g, toggleImage.color.b, 1f);
            toggleImage.color = activatedColor;
            rectTransform.localPosition = new Vector3(initialPosition.x, -10f, initialPosition.z);

        }
        else
        {
            Color desactivatedColor = new Color(toggleImage.color.r, toggleImage.color.g, toggleImage.color.b, 0.4f);
            toggleImage.color = desactivatedColor;
            // Restaurar la posici�n inicial del bot�n
            rectTransform.localPosition = initialPosition;

        }

    }
}
