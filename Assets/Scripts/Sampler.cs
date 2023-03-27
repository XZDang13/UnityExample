using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UIElements;

public class Sampler : MonoBehaviour
{
    public GameObject pointPrefab;

    public GameObject targetObjectA;
    public int numberOfPoints = 100;
    public Client client;
    private Mesh mesh;
    // Start is called before the first frame update
    void Start()
    {
        Action<string> generateSamplesOnTheSurfaceAction = GenerateSamplesOnTheSurfaceAPI;
        client.apiManager.Register("sample", generateSamplesOnTheSurfaceAction);
    }

    public void GenerateSamplesOnTheSurfaceAPI(string parameter)
    {
        int num = int.Parse(parameter);
        List<string> points = GenerateSamplesOnTheSurface(num);
        PointCloudMessage pointCloudMessage = new PointCloudMessage(points);
        ResponseMessage responseMessage = new ResponseMessage();
        responseMessage.value = pointCloudMessage.ToJson();

        client.Send(responseMessage);
    }

    public List<string> GenerateSamplesOnTheSurface(int num)
    {
        List<string> points = new List<string>();
        MeshFilter meshFilter = targetObjectA.GetComponent<MeshFilter>();

        if (meshFilter != null)
        {
            mesh = meshFilter.sharedMesh;
        }
        else
        {
            Debug.LogError("No MeshFilter component found on the target object.");
            return points;
        }

        for (int i = 0; i < num; i++)
        {
            PointMessage sample = GenerateRandomPointOnSurface(targetObjectA.transform, mesh);
            Instantiate(pointPrefab, sample.position, Quaternion.identity);
            points.Add(sample.ToJson());

        }

        return points;

    }

    private PointMessage GenerateRandomPointOnSurface(Transform objectTransform, Mesh mesh)
    {
        int randomTriangleIndex = UnityEngine.Random.Range(0, mesh.triangles.Length / 3);
        int[] triangles = mesh.triangles;
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;

        Vector3 p0 = vertices[triangles[randomTriangleIndex * 3]];
        Vector3 p1 = vertices[triangles[randomTriangleIndex * 3 + 1]];
        Vector3 p2 = vertices[triangles[randomTriangleIndex * 3 + 2]];

        Vector3 n0 = normals[triangles[randomTriangleIndex * 3]];
        Vector3 n1 = normals[triangles[randomTriangleIndex * 3 + 1]];
        Vector3 n2 = normals[triangles[randomTriangleIndex * 3 + 2]];

        float a = UnityEngine.Random.Range(0f, 1f);
        float b = UnityEngine.Random.Range(0f, 1f);

        if (a + b > 1f)
        {
            a = 1f - a;
            b = 1f - b;
        }

        Vector3 localPoint = p0 + a * (p1 - p0) + b * (p2 - p0);
        Vector3 worldPoint = objectTransform.TransformPoint(localPoint);

        
        Vector3 point2D = new Vector3(worldPoint.x, worldPoint.y, worldPoint.z);

        Vector3 normal = Vector3.Lerp(n0, n1, a) + Vector3.Lerp(n0, n2, b) - n0;
        normal = objectTransform.TransformDirection(normal).normalized;

        Debug.DrawLine(point2D, point2D + normal * 1, UnityEngine.Color.red, 20f);

        PointMessage sample = new PointMessage(point2D, normal);

        return sample;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
