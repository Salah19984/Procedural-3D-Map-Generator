using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    //Values for Noisemap Generation
    private float[,] noisemap;
    public int mapwidth, mapheight;
    public int seed;
    public int octaves;
    public float scale;
    public float lacunarity;
    public Vector2 offset;
    public float persistance;
    
    private float[,] falloffMap;


    // Start is called before the first frame update
    void Start()
    {
        //generate the noisemap
        noisemap = NoiseGenerator.GenerateNoiseMap(mapwidth,mapheight,seed,octaves,scale,lacunarity,offset,persistance);
        //Generate Falloffmap
        falloffMap = FalloffGenerator.GenerateFalloffMap(mapwidth,mapheight);
        //mix them together
        noisemap = GenerateIslandmap(mapheight,mapwidth,noisemap,falloffMap);
    }

    public float[,] GenerateIslandmap(int width, int height, float[,] noisemap,float[,] falloffMap)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                //substract the falloffmap from the noisemap values
                noisemap[x, y] = Mathf.Clamp01(noisemap[x, y] - falloffMap[x, y]);
            }
        }
        return noisemap;
    }
}
