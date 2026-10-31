using UnityEngine;

/* 
 * A premade target type holding the attributes the builder uses
*/

[CreateAssetMenu(fileName = "NewTargetConfig", menuName = "Carnival/Target Config")]
public class TargetConfig : ScriptableObject
{
    [SerializeField] private string displayName = "Target";
    [SerializeField] private Sprite sprite;
    [SerializeField] private Color tint = Color.white;
    [SerializeField, Min(0.1f)] private float size = 1f;
    [SerializeField, Min(0f)] private float speed = 2f;
    [SerializeField, Min(0)] private int pointValue = 10;

    public string DisplayName => displayName;
    public Sprite Sprite => sprite;
    public Color Tint => tint;
    public float Size => size;
    public float Speed => speed;
    public int PointValue => pointValue;
}