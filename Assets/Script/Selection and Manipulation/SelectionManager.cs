using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using xrc_assignments_project_g05.Scripts.Selection_and_Manipulation;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; private set; }

    [HideInInspector]
    public List<GameObject> allSelectedObject = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterSelectedObject(GameObject obj)
    {
        if (!allSelectedObject.Contains(obj))
        {
            allSelectedObject.Add(obj);
        }
    }

    public void DeselectAll()
    {
        for (int i = allSelectedObject.Count - 1; i >= 0; i--)
        {
            if (allSelectedObject[i] != null)
            {
                allSelectedObject[i].GetComponent<SelectionState>().isSelected = false;
            }
            allSelectedObject.RemoveAt(i);
        }
    }

    public void ClearSelectedObjects()
    {
        DeselectAll(); // Deselect before clearing the list.
        allSelectedObject.Clear();
    }
}
