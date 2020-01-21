using UnityEngine;

/// <summary>
/// Makes a Component able to become persistent between scenes.  Requires a GuidComponent.
/// </summary>
/// <typeparam name="T">The type of component to make persistent</typeparam>
[RequireComponent(typeof(GuidComponent))]
public abstract class PersistentComponent<T> : MonoBehaviour where T : Component
{
    public T Component;
    private GuidComponent _guid;

    private void Awake()
    {
        // Cache the GuidComponent
        _guid = GetComponent<GuidComponent>();
    }

    /// <summary>
    /// Decomposes the component into an array of objects.  Implementation determines which data to
    /// store in the object array.
    /// </summary>
    /// <param name="component">The component to decompose</param>
    /// <returns>An object array of the component's data</returns>
    protected abstract object[] Decompose(T component);

    /// <summary>
    /// Composes an component from an array of objects.  Implementation determines which data to
    /// to assign to which component field.  Note that since we cannot create a new instance of a Component, an existing
    /// Component to modify is passed as an argument.
    /// </summary>
    /// <param name="component">The component to compose</param>
    /// <param name="values">The object array of data to reassign to the component</param>
    protected abstract void Compose(T component, object[] values);
    
    /// <summary>
    /// Writes the Component to PersistentData using the Decompose method to create an array of objects
    /// </summary>
    public void Write()
    {
        PersistentData.Put(_guid.GetGuid(), typeof(T), Decompose(Component));
    }

    /// <summary>
    /// Writes the Component to PersistentData using the given object array instead of the Decompose method
    /// </summary>
    /// <remarks>
    /// Note that if the data types in the object array are incompatible with the Compose method,
    /// data reassignment will generate an error.
    /// </remarks>
    public void Write(object[] values)
    {
        PersistentData.Put(_guid.GetGuid(), typeof(T), values);
    }

    /// <summary>
    /// Reads the Component data from PersistentData and reassigns it using the Compose method
    /// </summary>
    public void Read()
    {
        Compose(Component, PersistentData.Retrieve(_guid.GetGuid(), typeof(Transform)));
    }

    /// <summary>
    /// Tries to cast the data at the given index as the given type
    /// </summary>
    /// <typeparam name="TData">The type to cast to</typeparam>
    /// <param name="values">The Component's object data array</param>
    /// <param name="index">The index of the relevant data</param>
    /// <returns>The type-casted output</returns>
    public TData TryResolve<TData>(object[] values, int index)
    {
        // Check that index is valid
        if (values.Length < index - 1 || index < 0)
        {
            Debug.LogError($"Value index of '{index}' invalid, returning default");
            return default;
        }

        // If the cast is good, return the casted data
        if (values[index] is TData t)
        {
            return t;
        }

        // Otherwise return default
        Debug.LogError("Bad cast, returning default");
        return default;
    }

    /// <summary>
    /// Does PersistentData contain an entry for this data type for this Guid?
    /// </summary>
    /// <returns>Whether PersistentData contains an entry for this data type for this Guid</returns>
    public bool IsStored()
    {
        return PersistentData.HasType(_guid.GetGuid(), typeof(T));
    }

    /// <summary>
    /// Removes entries in PersistentData for this data type under this Guid
    /// </summary>
    public void Clear()
    {
        PersistentData.RemoveType(_guid.GetGuid(), typeof(T));
    }
}