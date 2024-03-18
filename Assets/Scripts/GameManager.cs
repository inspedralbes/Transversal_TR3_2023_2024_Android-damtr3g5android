using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MessageData;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    public void UpdateGameState(GameState newState)
    {
        // Update the game state based on newState
        // Example: Update UI, spawn enemies, trigger events, etc.
    }
}
