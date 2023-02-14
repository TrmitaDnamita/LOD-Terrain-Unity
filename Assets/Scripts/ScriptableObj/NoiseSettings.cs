using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class NoiseSettings : ScriptableObject
{
    public string Seed;
    public float Height;
    
    public float Scale;
    [Range(1,4)]  public float Lacunarity;
    [Range(0,1)]  public float Persistance;
    [Range(1, 8)] public int Octaves;
}
