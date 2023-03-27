using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointMessage
{
    public Vector3 position;
    public Vector3 normal;

    public PointMessage(Vector3 position, Vector3 normal)
    {
        this.position = position;
        this.normal = normal;
    }

    public static PointMessage FromJson(string json)
    {
        return JsonUtility.FromJson<PointMessage>(json);
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
}
