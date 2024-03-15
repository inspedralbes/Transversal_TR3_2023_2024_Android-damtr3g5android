using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClicarBotones : MonoBehaviour
{
    private Button button;

    void Start()
    {
        // Obt�n el componente Button adjunto al bot�n
        button = GetComponent<Button>();

        // Desactiva el bot�n al inicio
        button.interactable = false;
    }

    // M�todo que se llama cuando se hace clic en el bot�n
    public void OnButtonClick()
    {
        // Activa o desactiva el bot�n seg�n su estado actual
        button.interactable = true;
    }
}
