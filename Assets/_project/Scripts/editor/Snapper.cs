using UnityEditor;
using UnityEngine;

// CLASS CREATED FOR A TOOL DEVELOPMENT COURSE
public static class Snapper
{
    private const string UNDO_STR_SNAP = "Snap objects";
    
    [MenuItem("Edit/Snap Selected Objects")]
    public static void SnapTheThings()
    {
        foreach (GameObject gameObject in Selection.gameObjects)
        {
            Undo.RecordObject( gameObject.transform, UNDO_STR_SNAP );
            gameObject.transform.position = gameObject.transform.position.Round();
        }
    }
}
