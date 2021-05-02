using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RandomizeTrees : MonoBehaviour
{
    [SerializeField] private GameObject treeContainer;
    private List<GameObject> trees;

    private void GetTrees()
    {
        for (int i = 0; i < treeContainer.transform.childCount; i++)
        {
            trees.Add(treeContainer.transform.GetChild(i).gameObject);
        }
    }
    [ContextMenu("Randomize")]
    public void RandomizeTreeParams()
    {
        GetTrees();
        foreach (var tree in trees)
        {
            tree.transform.Rotate(Vector3.up, Random.Range(0, 180));
            float randomScale = Random.Range(0.8f, 1.2f);
            tree.transform.localScale = Vector3.one * randomScale;
        }
    }
}
