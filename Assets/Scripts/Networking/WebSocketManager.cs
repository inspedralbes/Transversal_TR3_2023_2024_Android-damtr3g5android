using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using UnityEngine;
using WebSocketSharp;
using Newtonsoft.Json.Linq;
namespace Networking
{
    public class SocketManager : MonoBehaviour
    {
        [Header("Network Client")]
        [SerializeField]
        private Transform networkContainer;

        [SerializeField]
        private GameObject playerPrefab;

        public static string ClientID { get; private set; }

        private Dictionary<string, NetworkIdentity> serverObjects;

        WebSocket socket;
        public GameObject player;
        public PlayerData playerData;

        public float updateRate = 10000f;
        private float nextUpdateTime = 0f;

        private ConcurrentQueue<Action> actionsToExecuteOnMainThread = new ConcurrentQueue<Action>();

        //Package URL for Newtonsoft JSON utilities
        string PackageURL = "https://github.com/jilleJr/Newtonsoft.Json-for-Unity.git#upm";

        public delegate void SocketEvent(JObject data);
        public event SocketEvent OnOpen;
        public event SocketEvent OnRegister;
        public event SocketEvent OnSpawn;
        public event SocketEvent OnMessage;
        public event SocketEvent OnClose;
        public event SocketEvent OnDisconnected;
        public event SocketEvent OnUpdatePosition;
        public event SocketEvent OnUpdateAnimation;

        // Start is called before the first frame update
        void Start()
        {

            socket = new WebSocket("ws://localhost:3450");
            socket.Connect();
            Initialize();
            SetUpEvents();
            //WebSocket onMessage function
            socket.OnMessage += (sender, e) =>
            {

                //If received data is type text...
                if (e.IsText)
                {
                    JObject message = JObject.Parse(e.Data);
                    string eventType = message["event"].ToString();
                    JObject data = (JObject)message["data"];

                    switch (eventType)
                    {
                        case "open":
                            OnOpen?.Invoke(data);
                            break;
                        case "register":
                            OnRegister?.Invoke(data);
                            break;
                        case "spawn":
                            OnSpawn?.Invoke(data);
                            break;
                        case "disconnect":
                            OnDisconnected?.Invoke(data);
                            break;
                        case "updatePosition":
                            OnUpdatePosition?.Invoke(data);
                            break;
                        case "updateAnimation":
                            OnUpdateAnimation?.Invoke(data);
                            break;
                        // Add more cases as needed for different event types
                        default:
                            Debug.LogWarning($"Unhandled event type: {eventType}");
                            break;
                    }

                }

            };

            //If server connection closes (not client originated)
            socket.OnClose += (sender, e) =>
            {
                Debug.Log(e.Code);
                Debug.Log(e.Reason);
                Debug.Log("Connection Closed!");
            };
        }
        void Initialize()
        {
            serverObjects = new Dictionary<string, NetworkIdentity>();
        }
        void SetUpEvents()
        {
            OnOpen += (data) =>
            {
                Debug.Log("Conexión establecida con el servidor");
            };

            OnRegister += (data) =>
            {
                ClientID = data["id"].ToString().RemoveQuotes();
                Debug.Log("Registro existoso");
            };

            OnSpawn += (data) =>
            {
                string id = data["id"].ToString().RemoveQuotes();
                actionsToExecuteOnMainThread.Enqueue(() =>
                {
                    GameObject go = Instantiate(playerPrefab, networkContainer);
                    go.name = string.Format("Player({0})", id);
                    NetworkIdentity ni = go.GetComponent<NetworkIdentity>();
                    ni.SetControllerId(id);
                    ni.SetSocketReference(this);
                    Player player = new Player();
                    player.id = id;
                    player.position = new Position();
                    player.animator = new AnimatorData();
                    serverObjects.Add(id, ni);
                });

            };
            OnDisconnected += (data) =>
            {
                string id = data["id"].ToString().RemoveQuotes();

                GameObject go = serverObjects[id].gameObject; ;
                Destroy(go);
                serverObjects.Remove(ClientID);
            };
            OnUpdatePosition += (data) =>
            {
                string id = data["id"].ToString().RemoveQuotes();
                float x = data["position"]["x"].Value<float>();
                float y = data["position"]["y"].Value<float>();
                actionsToExecuteOnMainThread.Enqueue(() => {
                    if (serverObjects.TryGetValue(id, out NetworkIdentity ni))
                    {
                        ni.transform.position = new Vector3(x, y, 0);

                    }
                });
            };
            OnUpdateAnimation += (data) =>
            {
                string id = data["id"].ToString().RemoveQuotes();
                float speed = data["animator"]["speed"].Value<float>();
                float vertical = data["animator"]["vertical"].Value<float>();
                float horizontal = data["animator"]["horizontal"].Value<float>();
                float lastVertical = data["animator"]["lastVertical"].Value<float>();
                float lastHorizontal = data["animator"]["lastHorizontal"].Value<float>();

                actionsToExecuteOnMainThread.Enqueue(() =>
                {
                    if (serverObjects.TryGetValue(id, out NetworkIdentity ni))
                    {
                        // Update animator parameters for the player
                        // Assuming you have a script attached to the player prefab that manages the animator
                        Animator animator = ni.GetComponent<Animator>();
                        if (animator != null)
                        {
                            animator.SetFloat("speed", speed);
                            animator.SetFloat("vertical", vertical);
                            animator.SetFloat("horizontal", horizontal);
                            animator.SetFloat("lastVertical", lastVertical);
                            animator.SetFloat("lastHorizontal", lastHorizontal);
                        }
                    }
                });
            };

        }
        public void Send(string message)
        {
            if (socket != null && socket.ReadyState == WebSocketState.Open)
            {
                socket.Send(message);
            }
            else
            {
                Debug.LogWarning("WebSocket connection is not open or socket is null.");
            }
        }

        // Update is called once per frame
        void Update()
        {
            
            while (actionsToExecuteOnMainThread.TryDequeue(out var action))
            {
                action.Invoke();
            }
            /*
            if (socket == null || Time.time < nextUpdateTime)
            {
                return;
            }
            nextUpdateTime = Time.time + updateRate;

            //If player is correctly configured, begin sending player data to server
            if (player != null && playerData.id != "")
            {
                //Grab player current position and rotation data
                playerData.xPos = player.transform.position.x;
                playerData.yPos = player.transform.position.y;

                System.DateTime epochStart = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
                double timestamp = (System.DateTime.UtcNow - epochStart).TotalSeconds;
                //Debug.Log(timestamp);
                playerData.timestamp = timestamp;

                string playerDataJSON = JsonUtility.ToJson(playerData);
                socket.Send(playerDataJSON);
            }
            else
            {
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                string messageJSON = "{\"message\": \"Some Message From Client\"}";
                socket.Send(messageJSON);
            }
            */
        }

        private void OnDestroy()
        {
            //Close socket when exiting application
            socket.Close();
        }



    }
    public static class StringExtensions
    {
        public static string RemoveQuotes(this string value)
        {
            return value.Replace("\"", "");
        }
    }
    [Serializable]
    public class Player {
        public string id;
        public Position position;
        public AnimatorData animator;
    }
    [Serializable]
    public class Position {
        public float x;
        public float y;
    }
    [Serializable]
    public class AnimatorData
    {
        public float speed;
        public float vertical;
        public float horizontal;
        public float lastVertical;
        public float lastHorizontal;
    }
}