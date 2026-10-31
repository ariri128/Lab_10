using UnityEngine;
using System;

/*
 * Base class for saveable scene objects
 * Gives each object a permanent unique SaveID so the right object can be found again when loading
*/

public abstract class SaveableBehaviour : MonoBehaviour, ISaveable, ISerializationCallbackReceiver
{
    [SerializeField] private string saveID;

    public string SaveID => saveID;

    public abstract string SavedData { get; }

    public abstract void LoadFromData(string data);

    // Creates a new GUID the first time this component is serialized, so the ID is stored with the scene
    public void OnBeforeSerialize()
    {
        if (string.IsNullOrEmpty(saveID))
        {
            saveID = Guid.NewGuid().ToString();
        }
    }

    // Required by ISerializationCallbackReceiver - nothing needs to happen after loading the ID
    public void OnAfterDeserialize()
    {
    }
}