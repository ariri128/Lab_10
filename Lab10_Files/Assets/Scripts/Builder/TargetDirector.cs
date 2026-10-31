using UnityEngine;

/*
 * Director: knows the order of builder steps needed to turn a premade TargetConfig into a target
*/

public class TargetDirector
{
    private readonly ITargetBuilder builder;

    // Sets up the director with the builder it will give instructions to
    public TargetDirector(ITargetBuilder builder)
    {
        this.builder = builder;
    }

    // Builds one target of the given type at a spawn position, moving in the given direction
    public Target Construct(TargetConfig config, Vector3 position, float direction)
    {
        if (builder == null || config == null)
        {
            return null;
        }

        return builder.Reset()
            .SetSprite(config.Sprite)
            .SetColor(config.Tint)
            .SetSize(config.Size)
            .SetSpeed(config.Speed)
            .SetPointValue(config.PointValue)
            .SetPosition(position)
            .SetDirection(direction)
            .Build();
    }
}