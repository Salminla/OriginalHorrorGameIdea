using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

// CLASS CREATED FOR A TOOL DEVELOPMENT COURSE
public class SnapperTool : EditorWindow
{
    public enum GridType
    {
        Cartesian,
        Polar
    }
    
    [MenuItem("Tools/Snapper")]
    public static void OpenTheThing() => GetWindow<SnapperTool>("Snapper");
    
    const float TAU = 6.28318530718f;
    
    public GridType gridType = GridType.Cartesian;
    public float gridScale = 1f;
    public int gridSize = 100;
    public int angularDivisions = 24;
    public float increment = 0.25f;

    private SerializedObject so;
    private SerializedProperty propGridSize;
    private SerializedProperty propGridScale;
    private SerializedProperty propGridType;
    private SerializedProperty propAngularDivisions;

    private void OnEnable()
    {
        so = new SerializedObject(this);
        propGridSize = so.FindProperty("gridSize");
        propGridScale = so.FindProperty("gridScale");
        propGridType = so.FindProperty("gridType");
        propAngularDivisions = so.FindProperty("angularDivisions");
        
        // load saved configuration
        gridType = (GridType)EditorPrefs.GetInt("SNAPPER_TOOL_gridType", 0);
        gridScale = EditorPrefs.GetFloat("SNAPPER_TOOL_gridScale", 1f);
        angularDivisions = EditorPrefs.GetInt("SNAPPER_TOOL_angularDivisions", 24);

        Selection.selectionChanged += Repaint;
        SceneView.duringSceneGui += DuringSceneGUI;
    }

    private void OnDisable()
    {
        // save configuration
        EditorPrefs.SetInt("SNAPPER_TOOL_gridType", (int)gridType);
        EditorPrefs.SetFloat("SNAPPER_TOOL_gridScale", gridScale);
        EditorPrefs.SetInt("SNAPPER_TOOL_angularDivisions", angularDivisions);
        
        Selection.selectionChanged -= Repaint;
        SceneView.duringSceneGui -= DuringSceneGUI;
    }

    void DuringSceneGUI(SceneView sceneView)
    {
        if (Selection.gameObjects.Length > 0)
        {
            if (Event.current.type == EventType.Repaint)
            {
                Handles.zTest = CompareFunction.LessEqual;
                
                const float gridDrawExtent = 16;

                if (gridType == GridType.Cartesian)
                    DrawCartesianGrid(gridDrawExtent);
                else 
                    DrawPolarGrid(gridDrawExtent);
            }
        }
    }

    private void OnGUI()
    {
        so.Update();
        EditorGUILayout.PropertyField(propGridType);
        EditorGUILayout.PropertyField(propGridScale);
        if (gridType == GridType.Polar)
        {
            EditorGUILayout.PropertyField(propAngularDivisions);
            propAngularDivisions.intValue = Mathf.Max(4, propAngularDivisions.intValue);
        }
        so.ApplyModifiedProperties();
        gridScale = EditorGUILayout.Slider( "Grid scale",gridScale, 0.25f, 10f);
        gridSize = EditorGUILayout.IntSlider( "Grid size",gridSize, 0, 100);
        increment = EditorGUILayout.Slider( "Angle increment",increment, 0.1f, 0.5f);
        using (new EditorGUI.DisabledScope(Selection.gameObjects.Length == 0))
        {
            if (GUILayout.Button("Snap Selection"))
            {
                SnapSelection();
            }
        }
    }

    private void DrawCartesianGrid(float extent)
    {
        
        int lineCount = Mathf.RoundToInt(extent * 2 / gridScale);
        if (lineCount % 2 == 0) 
            lineCount++;
        
        int halfLineCount = lineCount / 2;

        for (int i = 0; i < lineCount; i++)
        {
            int intOffset = i - halfLineCount;
            float xCoord = intOffset * gridScale;
            float zCoord0 = halfLineCount * gridScale;
            float zCoord1 = -halfLineCount * gridScale;
            Vector3 p0 = new Vector3(xCoord, 0f, zCoord0);
            Vector3 p1 = new Vector3(xCoord, 0f, zCoord1);
            Handles.DrawAAPolyLine(p0, p1);
            p0 = new Vector3(zCoord0, 0f, xCoord);
            p1 = new Vector3(zCoord1, 0f, xCoord);
            Handles.DrawAAPolyLine(p0, p1);
        }

        /* My old way
        Mathf.Clamp(gridScale, 0.25f, 100f);
        for (float x = 0; x < gridSize; x+=gridScale)
        {
            float xOffset = x - gridSize / 2;
            float zOffset = -gridSize / 2;
            Vector3 p0 = new Vector3(xOffset, 0f, zOffset);
            Vector3 p1 = new Vector3(xOffset, 0f, -zOffset);
            Handles.DrawLine(p0, p1);
            p0 = new Vector3(zOffset, 0f, xOffset);
            p1 = new Vector3(-zOffset, 0f, xOffset);
            Handles.DrawLine(p0, p1);
        }
        */
    }
    
    private void DrawPolarGrid(float extent)
    {
        int ringCount = Mathf.RoundToInt(extent / gridScale);

        float radiusOuter = (ringCount-1) * gridScale;
        // radial grid (rings)
        for (int i = 1; i < ringCount; i++)
        {
            Handles.DrawWireDisc( Vector3.zero, Vector3.up, i * gridScale );
        }

        
        
        // angluar grid (lines)
        for (int i = 0; i < angularDivisions; i++)
        {
            float t = i / (float)angularDivisions;
            float angRad = t * TAU;     // turns to radians
            float x = Mathf.Cos(angRad);
            float y = Mathf.Sin(angRad);
            Vector3 dir = new Vector3(x, 0f, y);
            Handles.DrawAAPolyLine( Vector3.zero, dir * radiusOuter);
        }
        
        /* My old way
        for (float i = 0; i < gridSize; i+=gridScale)
        {
            Handles.DrawWireDisc(Vector3.zero, Vector3.up, i);
        }
        for (float i = 0; i < 2*Mathf.PI; i+=increment)
        {
            float angle = i * Mathf.PI;
            float posX = Mathf.Sin(angle) * gridSize;
            float posZ = Mathf.Cos(angle) * gridSize;
                
            Vector3 pos0 = Vector3.zero;
            Vector3 pos1 = new Vector3(posX, 0f, posZ);
            Handles.DrawAAPolyLine(pos0, pos1);
            //Handles.DrawLine(Vector3.zero, new Vector3(Mathf.Sin(i*Mathf.PI) * gridSize,0,Mathf.Cos(i*Mathf.PI) * gridSize));
        }
        */
    }
    
    private void SnapSelection()
    {
        foreach (GameObject gameObject in Selection.gameObjects)
        {
            Undo.RecordObject(gameObject.transform, "UNDO_STR_SNAP");
            gameObject.transform.position = GetSnappedPosition(gameObject.transform.position);
        }
    }

    Vector3 GetSnappedPosition( Vector3 posOriginal )
    {
        if (gridType == GridType.Cartesian)
        {
            return posOriginal.Round(gridScale);
        }

        if (gridType == GridType.Polar)
        {
            Vector2 vec = new Vector2(posOriginal.x, posOriginal.z);
            float dist = vec.magnitude;
            float distSnapped = dist.Round(gridScale);

            float angRad = Mathf.Atan2(vec.y, vec.x); // 0 to TAU
            float angTurns = angRad / TAU; // 0 to 1
            //float angTurnsSnapped = Mathf.Round(angTurns * angularDivisions) / angularDivisions;
            float angTurnsSnapped = angTurns.Round(1f / angularDivisions);
            float angRadSnapped = angTurnsSnapped * TAU;

            Vector2 dirSnapped = new Vector2(Mathf.Cos(angRadSnapped), Mathf.Sin(angRadSnapped));
            Vector2 snappedVec = dirSnapped * distSnapped;

            return new Vector3(snappedVec.x, posOriginal.y, snappedVec.y);
        }

        return default;
    }
}
