using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Networking;
using Newtonsoft.Json.Linq;


public class CollisionDestroyer : MonoBehaviour
{
    public GameObject hitEffect;

    [SerializeField]
    private NetworkIdentity networkIdentity;
    [SerializeField]
    private WhoActivatedMe whoActivatedMe;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        NetworkIdentity ni = collision.gameObject.GetComponent<NetworkIdentity>();
        if (ni == null || ni.GetId() != whoActivatedMe.GetActivator())
        {
            var iddata = new IDData();
            iddata.id = networkIdentity.GetId();
            string idData = JsonUtility.ToJson(iddata);
            JObject data = new JObject
            {
                ["event"] = "collisionDestroy",
                ["data"] = JObject.Parse(idData)
            };
            string message = data.ToString(Newtonsoft.Json.Formatting.None);
            networkIdentity.GetSocket().Send(message);
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 0.2f);
            Destroy(gameObject);
        }
        
    }


}

