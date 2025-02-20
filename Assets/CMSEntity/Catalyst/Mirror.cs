using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : Catalyst
{
    public override void Effect(List<EffectData> effects)
    {
        foreach (EffectData effect in effects)
        {
            effect.Power *= -1;
        }
    }
}
