
using System;
using System.Collections.Generic;
using Engin.Utility;


public class Interaction
{
 public List<BaseInteraction> InteractionList = new();

 public void Init()
 {
  FindAllBaseInteraction();
 }
 private void FindAllBaseInteraction()
 {
  var AllElement = ReflectionUtility.FindAllImplement<BaseInteraction>();
  foreach (var Element in AllElement)
  {
   InteractionList.Add(Activator.CreateInstance(Element) as BaseInteraction);
  }
 }

 public List<T> FindAll<T>()
 {
  return InteractionCache<T>.FindAll(this);
 }

}
public static class InteractionCache<T>
{
 public static List<T> AllInteraction;
 const int COUNT_INTERACTION = 64;

 public static List<T> FindAll(Interaction Interact)
 {
  if (AllInteraction != null) return AllInteraction;
  AllInteraction = new List<T>(COUNT_INTERACTION);
  foreach (var Element in Interact.InteractionList)
  {
   if (Element is T activator)
    AllInteraction.Add(activator);

   AllInteraction.Sort((x, y) => (int)(x as BaseInteraction).PriorityInteraction -
   (int)(y as BaseInteraction).PriorityInteraction);
  }
  return AllInteraction;
 }
}

