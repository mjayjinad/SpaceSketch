using UnityEngine;

public class ChangeColorCommand : ICommand
{
    private string uniqueObjectName;
    private Color originalColor;
    private Color newColor;

    public ChangeColorCommand(string uniqueObjectName, Color originalColor, Color newColor)
    {
        this.uniqueObjectName = uniqueObjectName;
        this.originalColor = originalColor;
        this.newColor = newColor;
    }

    private GameObject FindObjectByUniqueName()
    {
        return GameObject.Find(uniqueObjectName);
    }

    public void Execute()
    {
        GameObject objectToChangeColor = FindObjectByUniqueName();
        if (objectToChangeColor != null)
        {
            SetColor(objectToChangeColor, newColor);
        }
    }

    public void Undo()
    {
        GameObject objectToChangeColor = FindObjectByUniqueName();
        if (objectToChangeColor != null)
        {
            SetColor(objectToChangeColor, originalColor);
        }
    }

    private void SetColor(GameObject obj, Color color)
    {
        MeshRenderer[] meshRenderers = obj.GetComponentsInChildren<MeshRenderer>();
        foreach (var renderer in meshRenderers)
        {
            if (renderer.material != null)
            {
                renderer.material.color = color;
            }
        }
    }
}
