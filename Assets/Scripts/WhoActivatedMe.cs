using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Networking;

public class WhoActivatedMe : MonoBehaviour
{
    [GreyOut]
    [SerializeField]
    private string whoActivatedMe;

    public void SetActivator(string ID) {
        whoActivatedMe = ID;
    }
    public string GetActivator()
    { return whoActivatedMe; }
}
