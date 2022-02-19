using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class VegetationGenerator:MonoBehaviour
{

    public List<Vector3> cubePositions = new List<Vector3>();


    public static Vector3 ObjectSpawnLocation(List<Vector3> vegetationArea)
    {
        // der Index holt sich einen zuf�lligen Cube auf dem gespawnt wird.
        int randomIndex = Random.Range(0, vegetationArea.Count);

        Vector3 newPos = new Vector3(
            vegetationArea[randomIndex].x,
            vegetationArea[randomIndex].y+0.5f,
            vegetationArea[randomIndex].z
        );
        //Debug.Log("ObjectSpawnPosition: " + "y: "+ newPos.y);
        //verhindert, dass 2 mal der selbe Position f�r spawn verwendet wird
        vegetationArea.RemoveAt(randomIndex);

        return newPos;
    }


    public static void SpawnGrass(List<Vector3> vertexPositions, int height)
    {
        List<Vector3> grassland = new List<Vector3>();
        for (int i = 0; i < vertexPositions.Count; i++)
        {
            if (vertexPositions[i].y < 0.5f * height && vertexPositions[i].y > 0.2f)
            {
                grassland.Add(vertexPositions[i]);
            }
        }
        
        for (int i = 0; i < grassland.Count; i++)
        {
            Instantiate(grass, ObjectSpawnLocation(grassland), Quaternion.identity);
        }
    }

    public static void SpawnTrees()
    {
        List<Vector3> woodland = new List<Vector3>();
        for (int i = 0; i < cubePositions.Count; i++)
        {
            if (cubePositions[i].y < 0.7f * noiseHeight && cubePositions[i].y > 0.3f)
            {
                woodland.Add(cubePositions[i]);
            }
        }

        for (int i = 0; i < numberOfTrees; i++)
        {
            Instantiate(trees, ObjectSpawnLocation(woodland), Quaternion.identity);
        }
    }
}
