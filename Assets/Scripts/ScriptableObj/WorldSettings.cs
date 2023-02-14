using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class WorldSettings : ScriptableObject
{
    public byte Width;
    public byte RenderDistance;
    public int LOD_Distance;
    public int[] LOD;
    public Material MeshMaterial;
}
