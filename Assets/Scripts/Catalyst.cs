using System.Collections.Generic;

public abstract class Catalyst : Ingredient
{
    public abstract void Effect(List<EffectData> effects);
}