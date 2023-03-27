using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointCloudMessage
{
    public List<string> point_json;

    public PointCloudMessage(List<string> point_json)
    {
        this.point_json = point_json;
    }

    public static PointCloudMessage FromJson(string json)
    {
        return JsonUtility.FromJson<PointCloudMessage>(json);
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
}
