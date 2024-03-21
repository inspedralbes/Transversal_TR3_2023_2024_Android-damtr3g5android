using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Networking;
using WebSocketSharp;

[RequireComponent(typeof(NetworkIdentity))]
public class NetworkTransform : MonoBehaviour
{
    [SerializeField]
    [GreyOut]
    private Vector3 oldPosition;
    private NetworkIdentity networkIdentity;
    private Player player;

    private float stillCounter = 0;
    //Updat tunes
    private const float minimumMovementThreshold = 0.01f;
    private const float forceUpdateTime = 0.05f;
    private float timeSinceLastUpdate = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        networkIdentity = GetComponent<NetworkIdentity>();
        oldPosition = transform.position;
        player = new Player();
        player = new Player
        {
            position = new Position
            {
                x = transform.position.x,
                y = transform.position.y
            }
        };

        if (!networkIdentity.IsControlling()) {
            enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (networkIdentity.IsControlling())
        {
            timeSinceLastUpdate += Time.deltaTime;
            float distanceMoved = Vector3.Distance(transform.position, oldPosition);

            if (distanceMoved > minimumMovementThreshold || timeSinceLastUpdate >= forceUpdateTime)
            {
                sendData();
                oldPosition = transform.position;
                timeSinceLastUpdate = 0.0f;
            }
        }
    }

    private void sendData() {
        
        player.id = networkIdentity.GetId();
        player.position.x = Mathf.Round(transform.position.x * 1000.0f) / 1000.0f;
        player.position.y = Mathf.Round(transform.position.y * 1000.0f) / 1000.0f;
        string playerJson = JsonUtility.ToJson(player);
        JObject data = new JObject
        {
            ["event"] = "updatePosition",
            ["data"] = JObject.Parse(playerJson)
        };
        string message = data.ToString(Newtonsoft.Json.Formatting.None);
        networkIdentity.GetSocket().Send(message);
    }
}
