using System;
using System.Collections.Generic;

/// <summary>
/// Stores Scene-agnostic decomposed Component data
/// </summary>
public class PersistentData
{
    // The storage mechanism for component data; Nested dictionary where the top level key is
    // a Guid, and the secondary key is an object type
    private static Dictionary<Guid, Dictionary<Type, object[]>> _savedData = new Dictionary<Guid, Dictionary<Type, object[]>>();

    /// <summary>
    /// Adds an entry to PersistentData under the given Guid for the given Type
    /// </summary>
    /// <remarks>Suggestion: Put is a safer way to insert data</remarks>
    /// <param name="key">The Guid to assign the data to</param>
    /// <param name="type">The Component type the data corresponds to</param>
    /// <param name="data">The object array of data to store</param>
    public static void Add(System.Guid key, Type type, object[] data)
    {
        if (!_savedData.ContainsKey(key)) _savedData[key] = new Dictionary<Type, object[]>();
        _savedData[key].Add(type, data);
    }

    /// <summary>
    /// Puts an entry to PersistentData under the given Guid for the given Type.  Will overwrite existing type data.
    /// </summary>
    /// <param name="key">The Guid to assign the data to</param>
    /// <param name="type">The Component type the data corresponds to</param>
    /// <param name="data">The object array of data to store</param>
    public static void Put(System.Guid key, Type type, object[] data)
    {
        if (!_savedData.ContainsKey(key)) _savedData[key] = new Dictionary<Type, object[]>();
        _savedData[key][type] = data;
    }

    /// <summary>
    /// Retrieves the object array data from PersistentData
    /// </summary>
    /// <param name="key">The Guid that corresponds to the Component</param>
    /// <param name="type">The Component type the data corresponds to</param>
    /// <returns>The Component's object array of decomposed data</returns>
    public static object[] Retrieve(System.Guid key, Type type)
    {
        return _savedData[key][type];
    }

    /// <summary>
    /// Does PersistentData contains the given Guid
    /// </summary>
    /// <param name="key">The Guid to check for</param>
    /// <returns>If PersistentData contains the given Guid</returns>
    public static bool Has(System.Guid key)
    {
        return _savedData.ContainsKey(key);
    }

    /// <summary>
    /// Does PersistentData contains the given Type under the given Guid?
    /// </summary>
    /// <param name="key">The Guid of the Component owner</param>
    /// <param name="type">The type to check for</param>
    /// <returns>If PersistentData contains the given Type for the Guid</returns>
    public static bool HasType(System.Guid key, Type type)
    {
        return _savedData.ContainsKey(key) && _savedData[key].ContainsKey(type);
    }

    /// <summary>
    /// Removes a Guid an its child Type data from PersistentData
    /// </summary>
    /// <param name="key">The Guid to remove</param>
    public static void Remove(System.Guid key)
    {
        _savedData[key].Clear();
        _savedData.Remove(key);
    }

    /// <summary>
    /// Removes the given Type data for the given Guid
    /// </summary>
    /// <param name="key">The Guid parent of the data</param>
    /// <param name="type">The type of data to remove</param>
    public static void RemoveType(System.Guid key, Type type)
    {
        _savedData[key].Remove(type);
    }

    /// <summary>
    /// Clears all data from PersistentData
    /// </summary>
    public static void Clear()
    {
        foreach (var entry in _savedData) entry.Value.Clear();
        _savedData.Clear();
    }
}