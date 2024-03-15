using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class WS_Client : MonoBehaviour
{
    WebSocket ws;
    // Start is called before the first frame update
    void Start()
    {
        ws = new WebSocket("http://localhost:8080");
        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("Message received from " + ((WebSocket)sender).Url + ", Data : " + e.Data);
        };
        ws.Connect();
    }

    // Update is called once per frame
    void Update()
    {
        if (ws == null )
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Hola");
            ws.Send("Hello");
        }
    }
}
