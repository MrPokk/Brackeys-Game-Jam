using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enhancement : Catalyst
{
    public override void Effect(List<EffectData> effects)
    {
        foreach (EffectData effect in effects)
        {
            if (effect.Power > 0) effect.Power = (int)(effect.Power * 1.5f + 0.5f);
        }
    }
}
