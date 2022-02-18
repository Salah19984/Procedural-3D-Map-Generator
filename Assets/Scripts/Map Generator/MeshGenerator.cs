using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{

    Mesh mesh;

    public int mapwidth = 10;
    public int mapheight = 10;

    //total size of needed vertices is (mapwidth + 1) * (mapheight + 1) because we always need 2 vertices for 1 quad
    Vector3[] vertices;
    int[] triangles;

    public int seed;
    public int octaves;
    public float scale;
    public float lacunarity;
    public Vector2 offset;
    public float persistance;
    public float heightmultiplier;
    private float[,] noiseMap;
    private float[,] fallofmap;
    private float[,] heightmap;
    // Start is called before the first frame update
    void Start()
    {
        
        mesh = new Mesh();
        //meshfilter stores the mesh iself which contains all infos about the shape of our object
        GetComponent<MeshFilter>().mesh = mesh;
        //the meshrenderer is responsible for taking this data and render it so we can actually see it!

        noiseMap = new float[mapwidth, mapheight];
        fallofmap = new float[mapwidth, mapheight];
        heightmap = new float[mapwidth, mapheight];
        noiseMap = NoiseGenerator.GenerateNoiseMap(mapwidth, mapheight, seed, octaves, scale, lacunarity, offset, persistance);
        fallofmap = FalloffGenerator.GenerateFalloffMap(mapwidth, mapheight);


        heightmap = GenerateIslandmap(noiseMap, fallofmap);
        CreateMesh(heightmap, heightmultiplier);
        UpdateMesh();
    }

    //turns the heightmap into an island shape
    float [,] GenerateIslandmap(float[,] noiseMap, float[,] fallofmap)
    {
        for (int z = 0; z < mapheight; z++)
        {
            for (int x = 0; x < mapwidth; x++)
            {
                heightmap[x, z] = Mathf.Clamp01(noiseMap[x, z] - fallofmap[x, z]);
            }
        }
        return heightmap;
    }

    //filling the arrays with data
    private void CreateMesh(float[,] heightmap,float heightmultiplier)
    {
        vertices = new Vector3[(mapwidth+1) * (mapheight+1)];
        triangles = new int[mapwidth * mapheight * 6];
        //index to iterate over the vertices array
        int vertexIndex = 0;
        
        //from row (xaxis) to column (zAxis)
        for (int z = 0; z < mapheight; z++)
        {
            for (int x = 0; x < mapwidth; x++)
            {
                float height = (float)heightmap[x, z] * heightmultiplier;

                //float y = Mathf.PerlinNoise(x*.3f, z*.3f)*2f;
                //Y_height = GenerateIslandmap(height);
                vertices[vertexIndex] = new Vector3(x, height, z);
                vertexIndex++;
            }
        }

        int verts = 0; // always shifts the triangles 1 to the right
        int tris = 0;
        for(int z  = 0; z< mapheight; z++)
        {
            for (int x = 0; x < mapwidth; x++)
            {
                triangles[tris + 0] = verts + 0;
                triangles[tris + 1] = verts + mapwidth + 1;
                triangles[tris + 2] = verts + 1;
                triangles[tris + 3] = verts + 1;
                triangles[tris + 4] = verts + mapwidth + 1;
                triangles[tris + 5] = verts + mapwidth + 2;
                verts++;
                tris += 6;
            }
            verts++;
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

  /*  private void OnDrawGizmos()
    {
        if(vertices == null)
            return;

        for(int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], .1f);
        }
    }
  */
}
