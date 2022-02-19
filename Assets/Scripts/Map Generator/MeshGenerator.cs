using UnityEngine;

public class MeshGenerator : MonoBehaviour
{

    Mesh mesh;

    public int mapwidth = 10;
    public int mapheight = 10;

    //total size of needed vertices is (mapwidth + 1) * (mapheight + 1) because we always need 2 vertices for 1 quad
    private Vector3[] vertices;
    int[] triangles;
    //for setting the Colors
    Color[] colors;
    public Gradient gradient;
    private float minTerrainHeight;
    private float maxTerrainHeight;


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

    void Start()
    {
        mesh = new Mesh();
        //meshfilter stores the mesh iself which contains all infos about the shape of our object
        GetComponent<MeshFilter>().mesh = mesh;
        //the meshrenderer is responsible for taking this data and render it so we can actually see it!

        //inizializing the arrays for the map
        noiseMap = new float[mapwidth, mapheight];
        fallofmap = new float[mapwidth, mapheight];
        heightmap = new float[mapwidth, mapheight];
        noiseMap = NoiseGenerator.GenerateNoiseMap(mapwidth, mapheight, seed, octaves, scale, lacunarity, offset, persistance);
        fallofmap = FalloffGenerator.GenerateFalloffMap(mapwidth, mapheight);

        heightmap = GenerateIslandmap(noiseMap, fallofmap);
        CreateMesh();
        UpdateMesh();
        //setting the collider to the generated mesh
        GetComponent<MeshCollider>().sharedMesh = mesh;
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
    private void CreateMesh()
    {
        vertices = new Vector3[(mapwidth+1) * (mapheight+1)];
        triangles = new int[mapwidth * mapheight * 6];
        colors = new Color[vertices.Length];

        //index to iterate over the vertices array
        int vertexIndex = 0;
        //from row (xaxis) to column (zAxis)
        for (int z = 0; z < mapheight; z++)
        {
            for (int x = 0; x < mapwidth; x++)
            {
                float mapHeight = heightmap[x, z] * heightmultiplier;
                vertices[vertexIndex] = new Vector3(x, mapHeight, z);
                //updating the max and min terrainheight for the color gradient
                if (mapHeight > maxTerrainHeight)
                {
                    maxTerrainHeight = mapHeight;
                }
                if(mapHeight < minTerrainHeight)
                {
                    minTerrainHeight = mapHeight;
                }
                vertexIndex++;
            }
        }
        int verts = 0; // always shifts the triangles 1 to the right
        int tris = 0;  // counter for triangles
        //form triangles from the vertices always clockwise to not mess up the uvs
        for(int z  = 0; z < mapheight-1; z++)
        {
            for (int x = 0; x < mapwidth-1; x++)
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
        for (int i=0 , z = 0; z < mapheight; z++)
        {
            for (int x = 0; x < mapwidth; x++)
            {
                float height = Mathf.InverseLerp(minTerrainHeight,maxTerrainHeight, vertices[i].y);
                colors[i] = gradient.Evaluate(height);
                i++;
            }
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;
        mesh.RecalculateNormals();
    }

}
