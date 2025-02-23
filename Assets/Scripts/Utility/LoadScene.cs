using System.Collections;
using UnityEngine;
public class LoadScene : MonoBehaviour
{
    public IEnumerator Load()
    {
        if (GameData<Main>.IsStartGame)
        {
            yield return new WaitForSeconds(1.5f);
            this.gameObject.SetActive(false);
            yield return null;
        }
        yield break;
    }
}
