using UnityEngine;

/*
 * Gives the visible half-width and half-height of the main orthographic camera so scripts can clamp and despawn without hardcoded edges
*/

public static class ScreenBounds
{
    public static float HalfHeight => Camera.main != null ? Camera.main.orthographicSize : 0f;

    public static float HalfWidth => Camera.main != null ? Camera.main.orthographicSize * Camera.main.aspect : 0f;
}