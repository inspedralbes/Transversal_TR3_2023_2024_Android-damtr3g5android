using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Networking; // Make sure this namespace matches where your WebSocketManager is located

namespace Networking {
    public class NetworkIdentity : MonoBehaviour
    {
        [Header("Helpful Values")]
        [SerializeField][GreyOut] private string id;
        [SerializeField][GreyOut] private bool isControlling;

        // Removed the SocketIOComponent reference
        // Assume WebSocketManager is a singleton for this example
        private SocketManager socket;

        void Awake()
        {
            isControlling = false;

            // Find the WebSocketManager in the scene
            socket = FindObjectOfType<SocketManager>();

            if (socket == null)
            {
                Debug.LogError("SocketManager instance not found in the scene!");
                return;
            }

            // Optionally, listen to WebSocketManager events if needed
            //socket.OnRegister += HandleRegisterEvent;
        }

        // Example handler for the OnRegister event from WebSocketManager
        private void HandleRegisterEvent(JObject data)
        {
            string newId = data["id"].ToString();
            SetControllerId(newId);
        }

        public void SetControllerId(string ID)
        {
            id = ID;
            isControlling = (SocketManager.ClientID == ID) ? true : false;
        }
        public void SetSocketReference(SocketManager socket)
        {
            socket = socket;
        }
        public string GetId()
        {
            return id;
        }
        public bool IsControlling()
        {
            return isControlling;
        }
        public SocketManager GetSocket()
        {
            return socket;
        }
    }
}
