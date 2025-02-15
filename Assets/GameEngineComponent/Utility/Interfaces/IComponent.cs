using System;


namespace Engin.Utility
{
 public interface IComponent
 {
  public virtual Type ID { get => GetType(); }
 }
}