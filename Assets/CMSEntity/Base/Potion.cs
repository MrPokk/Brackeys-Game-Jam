using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour, IRaise
{
    public List<EffectData> effects = new List<EffectData>();
}