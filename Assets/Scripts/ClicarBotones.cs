using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClicarBotones : MonoBehaviour
{
    private Button button;

    void Start()
    {
        // Obtén el componente Button adjunto al botón
        button = GetComponent<Button>();

        // Desactiva el botón al inicio
        button.interactable = false;
    }

    // Método que se llama cuando se hace clic en el botón
    public void OnButtonClick()
    {
        // Activa o desactiva el botón según su estado actual
        button.interactable = true;
    }
}
