using System.Collections.Generic;
using System.Linq;

public class FrogsLeg : Catalyst
{
    public override void Effect(List<EffectData> effects)
    {
        EffectData effect = effects.FirstOrDefault(x => x.Type == EffectType.Energy);
        if (effect != null) {
            effect.Power *= -1;
        }
    }
}