using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] public Slider volumenMusica;
    [SerializeField] public Slider volumenSonidos;
    [SerializeField] public GameObject[] sonidos;
    [SerializeField] public GameObject[] musica;
    private float ultimoVolumenMusica;
    private float ultimoVolumenSonidos;


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
        foreach(GameObject music in musica){
            music.GetComponent<AudioSource>().volume = volumenMusica.value;
        }

        foreach (GameObject sonido in sonidos)
        {
            sonido.GetComponent<AudioSource>().volume = volumenSonidos.value;
        }
    }

    public void GuardarVolumenMusica(){
        
            //Seteamos el volumen de la musica segun su valor en las opciones
            PlayerPrefs.SetFloat("volumenMusica", volumenMusica.value);
       
        
    }

    public void GuardarVolumenSonidos()
    {
        
            //Seteamos el volumen de los sonidos segun su valor en las opciones
            PlayerPrefs.SetFloat("volumenSonidos", volumenSonidos.value);
        
        
    }

    public void MuteVolumenMusica(bool musica)
    {
        ultimoVolumenMusica = PlayerPrefs.GetFloat("volumenMusica");
        if (musica) {
            PlayerPrefs.SetFloat("volumenMusica", 0);
        }
        else
            PlayerPrefs.SetFloat("volumenMusica", ultimoVolumenMusica);
    }

    public void MuteVolumenSonidos(bool sonido)
    {
        ultimoVolumenSonidos = PlayerPrefs.GetFloat("volumenSonidos");
        if (sonido)
        {
            PlayerPrefs.SetFloat("volumenSonidos", 0);
        }
        else
            PlayerPrefs.SetFloat("volumenSonidos", ultimoVolumenSonidos);
    }


}
