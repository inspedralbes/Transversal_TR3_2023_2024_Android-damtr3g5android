using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] public Slider volumenMusica;
    [SerializeField] public Slider volumenSonidos;
    [SerializeField] public Toggle muteMusica;
    [SerializeField] public Toggle muteSonidos;
    [SerializeField] public GameObject[] sonidos;
    [SerializeField] public GameObject[] musica;
   


    private void Start()
    {
        //Inicializamos los objetos de musica y sonido que contengan los tags
        musica = GameObject.FindGameObjectsWithTag("Musica");
        sonidos = GameObject.FindGameObjectsWithTag("Sonidos");

        //Cogemos el volumen que ya tenemos guardado de la musica y de los sonidos al iniciar
        volumenMusica.value = PlayerPrefs.GetFloat("volumenMusica");
        volumenSonidos.value = PlayerPrefs.GetFloat("volumenSonidos");

    }

    private void Update()
    {
        foreach (GameObject music in musica)
        {
            if (!muteMusica.isOn)
            {
                // Si el toggle de muteo de la música está desactivado, establecer el volumen a 0
                music.GetComponent<AudioSource>().volume = 0;
                PlayerPrefs.SetFloat("volumenMusica", 0);
            }
            else
            {
                // De lo contrario, establecer el volumen según el valor del slider
                music.GetComponent<AudioSource>().volume = volumenMusica.value;
            }
        }

        foreach (GameObject sonido in sonidos)
        {
            if (!muteSonidos.isOn)
            {
                // Si el toggle de muteo de los sonidos está desactivado, establecer el volumen a 0
                sonido.GetComponent<AudioSource>().volume = 0;
                PlayerPrefs.SetFloat("volumenSonidos", 0);
            }
            else
            {
                // De lo contrario, establecer el volumen según el valor del slider
                sonido.GetComponent<AudioSource>().volume = volumenSonidos.value;
            }
        }
    }

    public void GuardarVolumenMusica(){

        // Guardar el volumen de la música según su valor en las opciones
        PlayerPrefs.SetFloat("volumenMusica", volumenMusica.value);
        if (!muteMusica.isOn)
        {
            // Si la música está muteada, desactivar el toggle de muteo
            muteMusica.isOn = false;
        }

    }

    public void GuardarVolumenSonidos()
    {
        // Guardar el volumen de los sonidos según su valor en las opciones
        PlayerPrefs.SetFloat("volumenSonidos", volumenSonidos.value);
        if (!muteSonidos.isOn)
        {
            // Si los sonidos están muteados, desactivar el toggle de muteo
            muteSonidos.isOn = false;
        }


    }



}
