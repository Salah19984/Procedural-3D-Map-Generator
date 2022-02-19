using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetationGenerator:MonoBehaviour
{
    public static Vector3 ObjectSpawnLocation(List<Vector3> vegetationArea)
    {
        // der Index holt sich einen zuf�lligen Cube auf dem gespawnt wird.
        int randomIndex = Random.Range(0, vegetationArea.Count);

        Vector3 newPos = new Vector3(
            vegetationArea[randomIndex].x,
            vegetationArea[randomIndex].y+0.01f,
            vegetationArea[randomIndex].z
        );
        //Debug.Log("ObjectSpawnPosition: " + "y: "+ newPos.y);
        //verhindert, dass 2 mal der selbe Position f�r spawn verwendet wird
        vegetationArea.RemoveAt(randomIndex);

        return newPos;
    }

    public static void SpawnPlants(List<Vector3> vertexPositions, float height, int numberOfTrees, int numberOfgrass, GameObject grass, GameObject trees)
    {
        List<Vector3> greenland = new List<Vector3>();
        for (int i = 0; i < vertexPositions.Count; i++)
        {
            if (vertexPositions[i].y < 0.42f * height && vertexPositions[i].y > 0.2f * height)
            {
                greenland.Add(vertexPositions[i]);
            }
        }
        //trees spawning
        for (int i = 0; i < numberOfTrees; i++)
        {
            Instantiate(trees, ObjectSpawnLocation(greenland), Quaternion.identity);
        }
        //grass spawning
        for (int i = 0; i < numberOfgrass; i++)
        {
            Instantiate(grass, ObjectSpawnLocation(greenland), Quaternion.identity);
        }
    }

}
