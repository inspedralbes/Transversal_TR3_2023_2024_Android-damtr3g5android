using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Networking;
using WebSocketSharp;

[RequireComponent(typeof(NetworkIdentity))]
public class NetworkAnimator : MonoBehaviour
{
    [SerializeField]
    [GreyOut]
    private Vector3 oldPosition;
    private NetworkIdentity networkIdentity;
    private Player player;

    public Animator animator;

    private float stillCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        networkIdentity = GetComponent<NetworkIdentity>();
        oldPosition = transform.position;
        player = new Player();
        player.animator = new AnimatorData();

        if (!networkIdentity.IsControlling())
        {
            enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (networkIdentity.IsControlling())
        {
            if (oldPosition != transform.position)
            {
                oldPosition = transform.position;
                stillCounter = 0;
                sendData();
            }
            else
            {
                stillCounter += Time.deltaTime;
                if (stillCounter >= 1)
                {
                    stillCounter = 0;
                    sendData();
                }
            }
        }
    }
    private void sendData()
    {
        player.id = networkIdentity.GetId();
        player.animator.speed = animator.GetFloat("speed");
        player.animator.vertical = animator.GetFloat("vertical");
        player.animator.horizontal = animator.GetFloat("horizontal");
        player.animator.lastVertical = animator.GetFloat("lastVertical");
        player.animator.lastHorizontal = animator.GetFloat("lastHorizontal");

        string playerJson = JsonUtility.ToJson(player);
        JObject data = new JObject
        {
            ["event"] = "updateAnimation",
            ["data"] = JObject.Parse(playerJson)
        };
        string message = data.ToString(Newtonsoft.Json.Formatting.None);
        networkIdentity.GetSocket().Send(message);
    }
}
