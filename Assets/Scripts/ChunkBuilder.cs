using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ChunkBuilder : MonoBehaviour
{
    Dictionary<Vector2Int, MeshConstructor> ChunkS = new Dictionary<Vector2Int, MeshConstructor>();
    Transform Player;

    public WorldSettings Data;
    [HideInInspector] public bool DataState;
    public NoiseSettings Noise;
    [HideInInspector] public bool NoiseState;

    private void Start()
    {
        Pnoise.Init(ref Noise);
        Player = Instantiate(Resources.Load<Transform>("Timmy"),transform);

        CreateChunks();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
    void CreateChunks()
    {
        float lod;
        int end = Data.RenderDistance + 1;
        int start = -Data.RenderDistance;

        Vector2Int _coord = Vector2Int.zero;

        for (int i = start; i < end; i++)
            for (int j = start; j < end; j++)
            {
                _coord.Set(i, j);

                lod = GetLOD(new(_coord.x * Data.Width, 0, _coord.y * Data.Width));
                
                if(lod != -1)
                    CreateChunk(_coord, lod);
            }
    }

    float GetLOD(Vector3 coord)
    {
        float distance = math.distance(coord, Player.position);
        int lodValue = Mathf.RoundToInt(distance / Data.LOD_Distance);

        if (lodValue < Data.LOD.Length)
            return (Data.Width + 0f) / (Data.LOD[lodValue] + 0f);
        else
            return -1;
    }

    void CreateChunk(Vector2Int _coord, float lod)
    {
        GameObject chunk = new(string.Format("Chunk: ({0},{1})", _coord.x, _coord.y), typeof(MeshRenderer), typeof(MeshFilter));
        chunk.transform.parent = transform;
        chunk.transform.position = new(_coord.x * Data.Width, 0, _coord.y * Data.Width);

        MeshConstructor meshConstructor = chunk.AddComponent<MeshConstructor>();
        ChunkS.Add(_coord, meshConstructor);

        meshConstructor.Init(_coord, chunk.transform.position, Data.MeshMaterial);
        meshConstructor.CreateMesh(lod, Data);
    }

    private void OnDestroy()
    {
        foreach(var chunk in ChunkS.Values)
            Destroy(chunk.gameObject);
        ChunkS.Clear();
    }
}
