using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class EffectData
{
    public EffectType Type;
    public int Power;
}
[Serializable]
public class EffectsMaster
{
    [SerializeField] private List<EffectData> Effects;
    public void AddEffects(IEnumerable<EffectData> effects)
    {
        foreach (EffectData effect in effects) {
            EffectData thisEffect = Effects.FirstOrDefault(x => x.Type == effect.Type);
            if (thisEffect != null) {
                thisEffect.Power += effect.Power;
            }
            else {
                Effects.Add(effect);
            }
        }
    }

    public List<EffectData> GetAndClear()
    {
        var _Effects = Effects;
        Effects = new List<EffectData>();
        return _Effects;
    }
}
public enum EffectType
{
    healing,
    dizziness,
    bitter
}