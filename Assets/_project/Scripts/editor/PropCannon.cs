using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class PropCannon : EditorWindow
{
    const float TAU = 6.28318530718f;
    
    [MenuItem("Tools/Prop Cannon")]
    public static void OpenCannon() => GetWindow<PropCannon>();

    public float radius = 2f;
    public int spawnCount = 8;

    private SerializedObject so;
    private SerializedProperty propRadius;
    private SerializedProperty propSpawnCount;

    private Vector2[] randPoints;

    private void OnEnable()
    {
        so = new SerializedObject(this);
        
        propRadius = so.FindProperty("radius");
        propSpawnCount = so.FindProperty("spawnCount");
        GenerateRandomPoints();
        SceneView.duringSceneGui += DuringSceneGUI;
    }

    private void OnDisable() => SceneView.duringSceneGui -= DuringSceneGUI;

    void GenerateRandomPoints()
    {
        randPoints = new Vector2[spawnCount];
        for (int i = 0; i < spawnCount; i++)
        {
            randPoints[i] = Random.insideUnitCircle;
        }
    }

    private void OnGUI()
    {
        so.Update();
        EditorGUILayout.PropertyField(propRadius);
        propRadius.floatValue = propRadius.floatValue.AtLeast( 1 );
        EditorGUILayout.PropertyField(propSpawnCount);
        propSpawnCount.intValue = propSpawnCount.intValue.AtLeast( 1 );
        if (so.ApplyModifiedProperties())
        {
            GenerateRandomPoints();
            SceneView.RepaintAll();
        }
        
        // if you clicked left mouse button in the editor window
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            GUI.FocusControl(null);
            Repaint(); // repaint on the editor window GUI
        }
    }

    void DrawSphere(Vector3 pos) => Handles.SphereHandleCap( -1, pos, Quaternion.identity, 0.1f, EventType.Repaint);

    void DuringSceneGUI(SceneView sceneView)
    {
        Handles.zTest = CompareFunction.LessEqual;
        
        Transform camTransform = sceneView.camera.transform;

        // make sure it repaints on mouse move
        if (Event.current.type == EventType.MouseMove)
        {
            sceneView.Repaint();
        }

        bool holdingAlt = (Event.current.modifiers & EventModifiers.Alt) != 0;

        // change radius
        if (Event.current.type == EventType.ScrollWheel && !holdingAlt)
        {
            float scrollDir = Mathf.Sign(Event.current.delta.y);
            
            so.Update();
            propRadius.floatValue *= 1 + scrollDir * 0.05f;
            so.ApplyModifiedPropertiesWithoutUndo();
            Repaint(); // update editor window
            Event.current.Use(); // consume the event, don't let it fall through
        }

        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        // Ray ray = new Ray(camTransform.position, camTransform.forward);
        
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // setting up tangent space
            Vector3 hitNormal = hit.normal;
            Vector3 hitTangent = Vector3.Cross(hitNormal, camTransform.up).normalized;
            Vector3 hitBitangent = Vector3.Cross(hitNormal, hitTangent);

            Ray GetTangentRay(Vector2 tangentSpacePos)
            {
                Vector3 rayOrigin = hit.point + (hitTangent * tangentSpacePos.x + hitBitangent * tangentSpacePos.y) * radius;
                rayOrigin += hitNormal * 4; // offset margin thing
                Vector3 rayDirection = -hit.normal;
                
                return new Ray(rayOrigin, rayDirection);
            }
            
            // drawing the points
            foreach (Vector2 point in randPoints)
            {
                // create ray for this point
                Ray ptRay = GetTangentRay(point);
                // raycast to find point on surface
                if (Physics.Raycast( ptRay, out RaycastHit ptHit ))
                {
                    // draw sphere and normal on surface
                    DrawSphere(ptHit.point);
                    Handles.DrawAAPolyLine(ptHit.point, ptHit.point + ptHit.normal);
                }
            }
            Handles.color = Color.red;
            Handles.DrawAAPolyLine( 4,hit.point, hit.point + hitTangent );
            Handles.color = Color.green;
            Handles.DrawAAPolyLine( 4,hit.point, hit.point + hitBitangent );
            Handles.color = Color.blue;
            Handles.DrawAAPolyLine( 4,hit.point, hit.point + hitNormal );
            
            Handles.color = Color.white;

            // draw circle adapted to the terrain
            const int circleDetail = 128;
            Vector3[] ringPoints = new Vector3[circleDetail+1];
            for (int i = 0; i < circleDetail+1; i++)
            {
                float t = i / ((float)circleDetail-1); // go back to 0/1 position
                float angRad = t * TAU;
                Vector2 dir = new Vector2(Mathf.Cos(angRad), Mathf.Sin(angRad));
                Ray r = GetTangentRay(dir);
                if (Physics.Raycast(r, out RaycastHit cHit))
                {
                    ringPoints[i] = cHit.point + cHit.normal * 0.05f;
                }
                else
                {
                    ringPoints[i] = r.origin;
                }
            }
            
            Handles.DrawAAPolyLine(ringPoints);
            //Handles.DrawWireDisc(hit.point, hit.normal, radius);

            //Handles.DrawAAPolyLine( 4,hit.point, hit.point + hit.normal );
        }
    }
}
