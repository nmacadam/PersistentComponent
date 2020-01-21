using System;
using UnityEngine;

/// <summary>
/// Stores a Transform component in PersistentData
/// </summary>
public class PersistentTransform : PersistentComponent<Transform>
{
    private void Start()
    {
        // If the Guid has associated Transform data, reassign it
        if (IsStored())
        {
            Debug.Log("Assigning preexisting data");
            Read();
        }
    }

    /// <summary>
    /// Decomposes the Transform component into an array of 'parts'
    /// </summary>
    protected override object[] Decompose(Transform component)
    {
        // Note the order of the array; Order will be important for reassignment
        return new object[] {component.position, component.rotation, component.localScale};
    }

    /// <summary>
    /// Composes the Transform component with the given array of objects.
    /// </summary>
    protected override void Compose(Transform component, object[] values)
    {
        // Simply reassign the object array elements to their corresponding component fields
        transform.position = (Vector3)values[0];
        transform.rotation = (Quaternion)values[1];
        transform.localScale = (Vector3)values[2];
    }
}