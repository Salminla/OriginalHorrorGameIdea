using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class InteractionWindow : EditorWindow
{
    private static InteractionWindow window;

    private static int interactableCount;
    private static IEnumerable<GameObject> interactables;
    
    private static List<Type> interactableTypes = new List<Type>();

    [MenuItem("Window/Custom Things/Interaction Window")]
    private static void Init()
    {
        window = (InteractionWindow)GetWindow(typeof(InteractionWindow));
        window.Show();

        UpdateCount();
        GetTypes();
    }

    private void OnHierarchyChange()
    {
        UpdateCount();
        GetTypes();
        Repaint();
    }

    private void OnSelectionChange()
    {
        Repaint();
    }

    private void OnGUI()
    {
        GUILayout.Label($"Number of interactables: {interactableCount}", EditorStyles.boldLabel);

        if (GUILayout.Button("Select all interactables"))
            Selection.objects = interactables.ToArray();

        GUILayout.Space(10);
        
        GUILayout.Label($"Select interactables with a specific script", EditorStyles.boldLabel);
        
        foreach (var t in interactableTypes)
        {
            if (GUILayout.Button("Select " + t.Name))
                SelectInteractablesOfType(t);
        }

        if (!EditorApplication.isPlaying) return;
        GUILayout.Label($"Interact with selected:", EditorStyles.boldLabel);
        if (Selection.gameObjects.Count(o => o.GetComponent<IInteractable>() != null) > 0)
        {
            if (GUILayout.Button("Interact"))
            {
                Selection.gameObjects.ToList().ForEach(o => o.GetComponent<IInteractable>().Interact());
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Nothing selected", MessageType.Warning);
        }
    }

    private static void GetTypes()
    {
        interactableTypes.Clear();
        foreach (var interactable in interactables.ToList())
        {
            if (!interactableTypes.Contains(interactable.GetComponent<IInteractable>().GetType()))
            {
                interactableTypes.Add(interactable.GetComponent<IInteractable>().GetType());
            }
        }
    }

    private static void UpdateCount()
    {
        interactables = GetInteractables();
        interactableCount = interactables.Count();
    }
    private static IEnumerable<GameObject> GetInteractables()
    {
        return FindObjectsOfType<GameObject>().Where(o => o.GetComponent<IInteractable>() != null);
    }
    private static void SelectInteractablesOfType<T>()
    {
        Selection.objects = interactables.Where(o => o.GetComponent<IInteractable>().GetType() == typeof(T)).ToArray();
    }
    private static void SelectInteractablesOfType(Type t)
    {
        Selection.objects = interactables.Where(o => o.GetComponent<IInteractable>().GetType() == t).ToArray();
    }
}
