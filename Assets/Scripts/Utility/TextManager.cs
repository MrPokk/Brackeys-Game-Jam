using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TextManager : MonoBehaviour
{
 [SerializeField] public List<TMP_Text> AllText = new List<TMP_Text>();
 public TMP_Text Get(string NameGameObject)
 {
  return AllText.FirstOrDefault(x => x.name == NameGameObject);
 }
}
 