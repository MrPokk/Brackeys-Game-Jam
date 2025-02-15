using System;
using System.Collections.Generic;
using System.Reflection;


namespace Engin.Utility
{
 public static class ReflectionUtility
 {
  public static List<Type> FindAllImplement<T>()
  {
   Type type = typeof(T);

   List<Type> ListType = new List<Type>();

   Assembly Assembly = Assembly.GetAssembly(type);

   foreach (var elementType in Assembly.GetTypes())
   {
    if (elementType.IsSubclassOf(type) && !elementType.IsAbstract)
     ListType.Add(elementType);
   }
   return ListType;
  }
 }
}