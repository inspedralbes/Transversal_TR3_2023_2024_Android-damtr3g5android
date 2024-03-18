using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;

public class MessageData : MonoBehaviour
{
    public MessageType type;
    public Vector3 position;
    public GameState gameState;
    // Start is called before the first frame update
    public enum MessageType
    {
        PlayerPosition,
        GameState
    }

    // Update is called once per frame
    public class GameState
    {
        // Define properties of the game state data
    }
}
