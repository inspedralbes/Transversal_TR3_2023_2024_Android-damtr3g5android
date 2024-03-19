using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    public void UpdatePlayerPosition(Vector3 newPosition)
    {
        // Update the position of the player GameObject based on newPosition
        // Example: PlayerGameObject.transform.position = newPosition;
    }
}
