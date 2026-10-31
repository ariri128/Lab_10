using UnityEngine;

/*
 * Saves and loads the position, rotation and scale of the object it is attached to - its static helpers are also used to save the ducks
*/

public class TransformSaver : SaveableBehaviour
{
    public override string SavedData => JsonUtility.ToJson(Capture(transform));

    // Restores this object's transform from its saved JSON
    public override void LoadFromData(string data)
    {
        if (string.IsNullOrEmpty(data))
        {
            return;
        }

        Apply(transform, JsonUtility.FromJson<TransformData>(data));
    }

    // Copies a transform's local position, rotation and scale into a TransformData
    public static TransformData Capture(Transform target)
    {
        if (target == null)
        {
            return null;
        }

        return new TransformData
        {
            localPosition = target.localPosition,
            localRotation = target.localRotation,
            localScale = target.localScale
        };
    }

    // Puts saved position, rotation and scale values back onto a transform
    public static void Apply(Transform target, TransformData data)
    {
        if (target == null || data == null)
        {
            return;
        }

        target.localPosition = data.localPosition;
        target.localRotation = data.localRotation;
        target.localScale = data.localScale;
    }
}