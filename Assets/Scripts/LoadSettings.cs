using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSettings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        //Cogemos el volumen que ya tenemos guardado de la musica y de los sonidos al iniciar
        PlayerPrefs.GetFloat("volumenMusica");
        PlayerPrefs.GetFloat("volumenSonidos");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
