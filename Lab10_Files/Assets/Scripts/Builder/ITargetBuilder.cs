using UnityEngine;

/*
 * Builder interface: the list of steps every target builder has to provide to put a target together
*/

public interface ITargetBuilder
{
    ITargetBuilder Reset();
    ITargetBuilder SetSprite(Sprite sprite);
    ITargetBuilder SetColor(Color color);
    ITargetBuilder SetSize(float size);
    ITargetBuilder SetSpeed(float speed);
    ITargetBuilder SetPointValue(int pointValue);
    ITargetBuilder SetPosition(Vector3 position);
    ITargetBuilder SetDirection(float direction);
    Target Build();
}