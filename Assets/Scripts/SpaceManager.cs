using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceManager : MonoBehaviour
{
    public GameObject targetObject;
    public Client client;
    // Start is called before the first frame update
    void Start()
    {
        Action<string> updateTargetObjectTransformAction = UpdateTargetObjectTransformAPI;
        client.apiManager.Register("update", updateTargetObjectTransformAction);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateTargetObjectTransformAPI(string patrameter)
    {
        TargetTransformMessage message = TargetTransformMessage.FromJson(patrameter);
        UpdateTargetObjectTransform(message.position, message.euler);
    }

    public void UpdateTargetObjectTransform(Vector3 position, Vector3 euler)
    {
        targetObject.transform.position = position;
        targetObject.transform.rotation = Quaternion.Euler(euler);
    }
}
