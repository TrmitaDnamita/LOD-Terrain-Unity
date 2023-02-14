using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MeshConstructor : MonoBehaviour
{
    Mesh mesh;
    Dictionary<float2,float> terrainMap = new();

    int widthSize = 0;
    public Vector2Int ChunkCoord { get; private set; }
    public Vector3 RealCoord { get; private set; }

    public void Init(Vector2Int fCoord, Vector3 rCoord, Material mat)
    {
        ChunkCoord = fCoord;
        RealCoord = rCoord;

        mesh = new() { indexFormat = UnityEngine.Rendering.IndexFormat.UInt32 };
        gameObject.GetComponent<MeshRenderer>().material = mat;
        gameObject.GetComponent<MeshFilter>().mesh = mesh;
    }

    public void CreateMesh(float _addition, WorldSettings Data)
    {
        List<Vector3> vertices;
        List<Vector3> normals;
        List<int> triangles;

        List<float3> fakeVertices = new();

        vertices = TerrainVerticesSet(_addition, Data, ref fakeVertices);
        triangles = TriangleSet();
        normals = NormalSet(_addition, Data, ref fakeVertices);
        
        MeshSet(vertices.ToArray(), triangles.ToArray(), normals.ToArray());
        
        fakeVertices.Clear();
        vertices.Clear();
        triangles.Clear();
        normals.Clear();
    }
    List<Vector3> TerrainVerticesSet(float _addition, WorldSettings Data, ref List<float3> fakeVertices)
    {
        float endValue = Data.Width / 2;
        float startValue = -endValue;

        endValue += _addition;

        List<Vector3> vertices = new();

        for (float z = startValue; z <= endValue; z += _addition)
            for (float x = startValue; x <= endValue; x += _addition)
            {
                float noiseHeight = Pnoise.Get2D(x + RealCoord.x, z + RealCoord.z);
                fakeVertices.Add(new(x, noiseHeight, z));

                if(z < endValue && x < endValue)
                {
                    terrainMap.Add(new(x, z), noiseHeight);
                    vertices.Add(new(x, noiseHeight, z));
                    if(z == startValue)
                        widthSize++;
                }
            }
        return vertices;
    }
    List<int> TriangleSet()
    {
        int idxVert = 0;
        List<int> triangles = new();

        for (int i = 0; i < widthSize - 1; i++)
        {
            for (int j = 0; j < widthSize - 1; j++)
            {
                triangles.Add(idxVert);
                triangles.Add(idxVert + widthSize);
                triangles.Add(idxVert + 1);
                triangles.Add(idxVert + 1);
                triangles.Add(idxVert + widthSize);
                triangles.Add(idxVert + widthSize + 1);

                idxVert++;
            }
            idxVert++;
        }
        return triangles;
    }

    List<Vector3> NormalSet(float _addition, WorldSettings Data, ref List<float3> fakeVertices)
    {
        float endValue = Data.Width / 2;
        float startValue = -endValue;

        endValue += _addition;

        int indxVert = 0;
        int newWidthSize = widthSize + 1;

        List<Vector3> normals = new();
        for (float z = startValue; z < endValue; z += _addition)
        {
            for (float x = startValue; x < endValue; x += _addition)
            {
                Vector3 pointA = fakeVertices[indxVert + newWidthSize];
                Vector3 pointB = fakeVertices[indxVert + newWidthSize + 1];
                Vector3 pointC = fakeVertices[indxVert + 1];

                Vector3 pointAB = pointB - pointA;
                Vector3 pointAC = pointC - pointA;

                normals.Add(Vector3.Cross(pointAB, pointAC).normalized);
                indxVert++;
            }
            indxVert++;
        }
        return normals;
    }
    void MeshSet(Vector3[] vertices, int[] triangles, Vector3[] normals)
    {
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
    }
    private void OnDestroy()
    {
        mesh.Clear();
        terrainMap.Clear();
    }
}