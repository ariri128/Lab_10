using UnityEngine;
using System;

/*
 * The local position, rotation and scale of a transform, in a form JsonUtility can write to JSON
*/

[Serializable]
public class TransformData
{
    public Vector3 localPosition;
    public Quaternion localRotation;
    public Vector3 localScale;
}