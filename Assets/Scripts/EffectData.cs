using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class EffectData
{
    public EffectType Type;
    public int Power;

    public EffectData(EffectType type, int power)
    {
        Type = type;
        Power = power;
    }
}
[Serializable]
public class EffectsMaster
{
    [SerializeField] private List<EffectData> Effects;

    public EffectsMaster(List<EffectData> effects)
    {
        Effects = effects;
    }

    public EffectsMaster()
    {
        Effects = new List<EffectData>();
    }

    public void AddEffects(IEnumerable<EffectData> effects)
    {
        foreach (EffectData effect in effects) {
            AddEffect(effect);
        }
    }
    public void AddEffect(EffectData effect)
    {
        EffectData thisEffect = Effects.FirstOrDefault(x => x.Type == effect.Type);
        if (thisEffect != null) {
            thisEffect.Power += effect.Power;
        }
        else {
            Effects.Add(effect);
        }
    }

    public List<EffectData> GetAndClear()
    {
        var _Effects = Effects;
        Effects = new List<EffectData>();
        return _Effects;
    }
    public List<EffectData> Get()
    {
        return Effects;
    }
}
public enum EffectType
{
    BASIC = 0,
    Energy,
    Chaotic,
    Sagacity,
    Magic,
    Primary,

    ADDITIONAL = 100,
    Stability,
    Duration,
    Toxicity,

    TASTES = 200,
    Sweet,
    Bitter
}