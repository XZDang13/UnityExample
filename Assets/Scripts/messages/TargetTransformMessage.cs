using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTransformMessage
{
    public Vector3 position;
    public Vector3 euler;

    public TargetTransformMessage(Vector3 position, Vector3 euler)
    {
        this.position = position;
        this.euler = euler;
    }

    public static TargetTransformMessage FromJson(string json)
    {
        return JsonUtility.FromJson<TargetTransformMessage>(json);
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
}
