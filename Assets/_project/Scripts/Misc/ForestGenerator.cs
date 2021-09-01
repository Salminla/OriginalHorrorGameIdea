using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ForestGenerator : MonoBehaviour
{
    [SerializeField] private Terrain _terrain;

    [SerializeField] private int depth = 5;
    [SerializeField] private int width = 256;
    [SerializeField] private int height = 256;
    [SerializeField] private float scale = 20f;
    [SerializeField] private float offsetX = 100f;
    [SerializeField] private float offsetY = 100f;

    [SerializeField] private GameObject[] treeContainers;
    private List<GameObject> trees = new List<GameObject>();

    [SerializeField] private GameObject building;
    [SerializeField] private GameObject alignPoint;

    [ContextMenu("Generate level")]
    void GenerateLevel()
    {
        GenerateTerrain();
        GetTrees();
        PlaceTrees();
        PlaceBuilding();
    }
    
    [ContextMenu("Generate terrain")]
    void GenerateTerrain()
    {
        offsetX = Random.Range(0f, 9999f);
        offsetY = Random.Range(0f, 9999f);
        _terrain.terrainData = GenerateData();
    }

    TerrainData GenerateData()
    {
        _terrain.terrainData.heightmapResolution = width + 1;
        _terrain.terrainData.size = new Vector3(width, depth, height);
        _terrain.terrainData.SetHeights(0, 0, GenerateHeights());
        return _terrain.terrainData;
    }

    private float[,] GenerateHeights()
    {
        float[,] heights = new float[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                heights[x, y] = CalculateHeight(x, y);
            }
        }
        
        return heights;
    }

    private float CalculateHeight(int x, int y)
    {
        float xCoord = (float)x / width * scale + offsetX;
        float yCoord = (float)y / height * scale + offsetY;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }

    [ContextMenu("Get trees")]
    void GetTrees()
    {
        trees.Clear();
        foreach (GameObject treeContainer in treeContainers)
        {
            foreach (Transform treeTransform in treeContainer.transform)
            {
                trees.Add(treeTransform.gameObject);
            }
        }
    }
    [ContextMenu("Place trees")]
    void PlaceTrees()
    {
        AlignGOsToTerrain(trees, 0.1f);
    }

    [ContextMenu("Place building")]
    void PlaceBuilding()
    {
        AlignGOToTerrain(building, -0.1f, alignPoint.transform);
    }

    void AlignGOToTerrain(GameObject go, float yOffset, Transform alignTrans = null)
    {
        Transform tf = go.transform;
        if (alignTrans == null)
            alignTrans = go.transform;
        go.transform.position += Vector3.up * 10f;
        int layerMask = 1 << 10;
        layerMask = ~layerMask;
        if (Physics.Raycast(alignTrans.position, -alignTrans.up, out var hit, Mathf.Infinity, layerMask))
            go.transform.position += Vector3.down * (hit.distance + yOffset);
        else
            go.transform.position = new Vector3(go.transform.position.x, 0, go.transform.position.z);
    }
    void AlignGOsToTerrain(List<GameObject> gos, float yOffset)
    {
        if (gos.Count > 0)
        {
            foreach (GameObject go in gos)
            {
                AlignGOToTerrain(go, yOffset);
            }
        }
        else
            Debug.Log("No objects in the list!");
    }
}
