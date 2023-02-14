using UnityEngine;
using Unity.Collections;
using NoiseTest;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Jobs;

public static class Pnoise
{
    static NoiseSettings NoiseData;
    static OpenSimplexNoise noise;
    public static void Init(ref NoiseSettings _noise)
    {
        NoiseData = _noise;
        noise = new OpenSimplexNoise(NoiseData.Seed.GetHashCode());
    }
    public static float Get2D(float x, float z)
    {
        float amplitude = 1f;
        float frecuency = 1f;
        float noiseHeight = 0f;

        for(int o = 0; o < NoiseData.Octaves; o++)
        {
            double sampleX = x / NoiseData.Scale * frecuency;
            double sampleZ = z / NoiseData.Scale * frecuency;
            double Noise = noise.Evaluate(sampleX, sampleZ);
            
            noiseHeight += (float)(Noise * amplitude);

            amplitude *= NoiseData.Persistance;
            frecuency *= NoiseData.Lacunarity;
        }
        
        return NoiseData.Height * ( (noiseHeight + 1f) / 2f);
    }
}
