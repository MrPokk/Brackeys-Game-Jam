using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class WhimOfFate : Catalyst
{
    private const int MaxDelta = 5;
    public override void Effect(List<EffectData> effects)
    {
        if (PeopleImplementation.Customer == null) return;
        List<EffectRange> effectRanges = PeopleImplementation.Customer.DataComponent.TypePoison.Recipe;
        List<EffectData> deltas = new List<EffectData>();

        foreach (EffectRange effectRange in effectRanges) {
            EffectData effectData = effects.FirstOrDefault(x => x.Type == effectRange.Type);
            if (effectData == null) {
                effectData = new EffectData(effectRange.Type, 0);
            }

            int delta = Math.Max(effectRange.Min - effectData.Power, 0) + Math.Min(effectRange.Max - effectData.Power, 0);
            if (delta != 0) deltas.Add(new EffectData(effectRange.Type, delta));
        }
        if (deltas.Count > 0) {
            deltas.Sort(new Comparison<EffectData>((x, y) => y.Power - x.Power));
            EffectsMaster effectsMaster = new EffectsMaster(effects);
            effectsMaster.AddEffect(new EffectData(deltas[0].Type, Math.Clamp(deltas[0].Power, -MaxDelta, MaxDelta)));
        }
    }
}
