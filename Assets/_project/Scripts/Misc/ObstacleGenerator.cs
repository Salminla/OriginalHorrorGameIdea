using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ObstacleGenerator : MonoBehaviour
{
    [SerializeField] private Transform rockContainer;
    [SerializeField] private List<GameObject> rockPrefabs;
    [SerializeField] private Vector2 gridSize = new Vector2(100, 100);
    [SerializeField] private int minSeparation = 5;
    [SerializeField] private float scaleMin = 0.8f;
    [SerializeField] private float scaleMax = 2.0f;
    [SerializeField] private Vector3 maxOffset = Vector3.zero;
    [SerializeField] private float placementPropability = 1f;
    [SerializeField] private float placementYOffset = 0.5f;

    private List<GameObject> rocks = new List<GameObject>();
    
    [ContextMenu("Generate Rocks")]
    private void GenerateRocks()
    {
        if (rocks.Count > 0)
            RemoveRocks();
        rocks.Clear();
        
        for (int x = 0; x < gridSize.x; x += minSeparation)
        {
            for (int y = 0; y < gridSize.y; y += minSeparation)
            {
                if (!(Random.Range(0f, 1f) <= placementPropability)) continue;
                GameObject newRock = Instantiate(rockPrefabs[Random.Range(0, rockPrefabs.Count)], rockContainer);

                float posX = Random.Range(0, maxOffset.x);
                float posY = Random.Range(0, maxOffset.z);
                newRock.transform.localPosition = new Vector3(x + 0.5f + posX, 0.5f, y + 0.5f + posY);

                Quaternion rot = Random.rotation;
                newRock.transform.rotation = rot;
                
                float randomScale = Random.Range(scaleMin, scaleMax);
                newRock.transform.localScale = Vector3.one * randomScale;
                
                rocks.Add(newRock);
            }
        }
    }
    [ContextMenu("Align Rocks")]
    private void PlaceRocks()
    {
        foreach (var rock in rocks)
        {
            rock.transform.position += Vector3.up * 10f;
            int layerMask = 1 << 10;
            layerMask = ~layerMask;
            if (Physics.Raycast(rock.transform.position, -rock.transform.up, out var hit, Mathf.Infinity, layerMask))
                rock.transform.position += Vector3.down * (hit.distance + placementYOffset);
            else
                rock.transform.position = new Vector3(rock.transform.position.x, 0, rock.transform.position.z);
        }
    }
    [ContextMenu("Remove rocks")]
    private void RemoveRocks()
    {
        foreach (var rock in rocks)
        {
            DestroyImmediate(rock);
        }
        rocks.Clear();
    }
}
