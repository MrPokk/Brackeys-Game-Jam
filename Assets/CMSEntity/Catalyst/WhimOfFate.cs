using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WhimOfFate : Catalyst
{
    public override void Effect(List<EffectData> effects)
    {
        EffectsMaster effectsMaster = new EffectsMaster(effects);
        foreach (EffectType idEffect in Enum.GetValues(typeof(EffectType))) {
            if (idEffect > EffectType.ADDITIONAL && idEffect < EffectType.TASTES) {
                effectsMaster.AddEffect(new EffectData(idEffect, Random.Range(-5, 6)));
            }
        }
    }
}
