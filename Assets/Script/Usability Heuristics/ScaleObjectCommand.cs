using UnityEngine;

public class ScaleObjectCommand : ICommand
{
    private string uniqueObjectName;
    private Vector3 initialScale;
    private Vector3 finalScale;

    public ScaleObjectCommand(string uniqueObjectName, Vector3 initialScale, Vector3 finalScale)
    {
        this.uniqueObjectName = uniqueObjectName;
        this.initialScale = initialScale;
        this.finalScale = finalScale;
    }

    private GameObject FindObjectByUniqueName()
    {
        // Find the GameObject by its unique name (including the GUID)
        return GameObject.Find(uniqueObjectName);
    }

    public void Execute()
    {
        GameObject objectToScale = FindObjectByUniqueName();
        if (objectToScale != null)
        {
            objectToScale.transform.localScale = finalScale;
        }
    }

    public void Undo()
    {
        GameObject objectToScale = FindObjectByUniqueName();
        if (objectToScale != null)
        {
            objectToScale.transform.localScale = initialScale;
        }
    }
}
