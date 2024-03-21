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
    private AnimatorData oldAnimatorData;
    private Player player;

    public Animator animator;

    private const float updateInterval = 1.0f; // Interval in seconds to force an update
    private float timeSinceLastUpdate = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        networkIdentity = GetComponent<NetworkIdentity>();
        animator = GetComponent<Animator>();

        oldPosition = transform.position;
        player = new Player();
        player.animator = new AnimatorData();

        oldAnimatorData = new AnimatorData();
        UpdateAnimatorData(ref oldAnimatorData);

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
            timeSinceLastUpdate += Time.deltaTime;

            bool hasSignificantMovement = Vector3.Distance(transform.position, oldPosition) > 0.05f;
            bool hasAnimationChanged = CheckAnimatorDataChanged();

            if (hasSignificantMovement || hasAnimationChanged || timeSinceLastUpdate >= updateInterval)
            {
                SendData();
                oldPosition = transform.position;
                UpdateAnimatorData(ref oldAnimatorData); // Update oldAnimatorData with the latest values
                timeSinceLastUpdate = 0.0f;
            }
        }
    }
    private void UpdateAnimatorData(ref AnimatorData animatorData)
    {
        animatorData.speed = animator.GetFloat("speed");
        animatorData.vertical = animator.GetFloat("vertical");
        animatorData.horizontal = animator.GetFloat("horizontal");
        animatorData.lastVertical = animator.GetFloat("lastVertical");
        animatorData.lastHorizontal = animator.GetFloat("lastHorizontal");
    }
    private bool CheckAnimatorDataChanged()
    {
        // This method checks if there has been a significant change in the animation parameters
        return Mathf.Abs(oldAnimatorData.speed - animator.GetFloat("speed")) > 0.1f ||
               Mathf.Abs(oldAnimatorData.vertical - animator.GetFloat("vertical")) > 0.1f ||
               Mathf.Abs(oldAnimatorData.horizontal - animator.GetFloat("horizontal")) > 0.1f ||
               Mathf.Abs(oldAnimatorData.lastVertical - animator.GetFloat("lastVertical")) > 0.1f ||
               Mathf.Abs(oldAnimatorData.lastHorizontal - animator.GetFloat("lastHorizontal")) > 0.1f;
    }
    private void SendData()
    {
        AnimatorData currentData = new AnimatorData();
        UpdateAnimatorData(ref currentData);

        JObject data = new JObject
        {
            ["event"] = "updateAnimation",
            ["data"] = new JObject
            {
                ["id"] = networkIdentity.GetId(),
                ["animator"] = JObject.FromObject(currentData)
            }
        };

        string message = data.ToString(Newtonsoft.Json.Formatting.None);
        networkIdentity.GetSocket().Send(message);
    }
}
