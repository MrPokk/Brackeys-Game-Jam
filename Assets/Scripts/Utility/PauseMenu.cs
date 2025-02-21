using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    public static bool Paused { get; private set; } = false;
    [SerializeField] private GameObject Instance;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Paused)
                Resume();
            else
                Pause();
        }
    }
    private void Pause()
    {
        Instance.SetActive(true);
        Time.timeScale = 0;
        Paused = true;
    }
    private void Resume()
    {
        Instance.SetActive(false);
        Time.timeScale = 1;
        Paused = false;
    }
}
