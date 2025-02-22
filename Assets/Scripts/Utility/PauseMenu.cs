using SmallHedge.SoundManager;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    public static bool Paused { get; private set; } = false;
    [SerializeField] private GameObject Instance;
    [SerializeField] private AudioMixer Mixer;
    [SerializeField] private Slider SliderGame;
    [SerializeField] private Slider SliderMusic;

    [SerializeField] private string NameMixerGame = "MixerGame";
    [SerializeField] private string NameMixerMusic = "MixerMusic";
    private void Start()
    {
        SliderGame.value = PlayerPrefs.GetFloat(NameMixerGame);
        SliderMusic.value = PlayerPrefs.GetFloat(NameMixerMusic);
        Mixer.SetFloat(NameMixerGame, PlayerPrefs.GetFloat(NameMixerGame));
        Mixer.SetFloat(NameMixerMusic, PlayerPrefs.GetFloat(NameMixerMusic));
    }
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

    public void UpdatrMixerGame() => UpdatrMixer("MixerGame", SliderGame.value);
    public void UpdatrMixerMusic() => UpdatrMixer("MixerMusic", SliderMusic.value);

    public void UpdatrMixer(string NameMixer, float value)
    {
        PlayerPrefs.SetFloat(NameMixer, value);
        Mixer.SetFloat(NameMixer, value);
        SoundManager.PlaySound(SoundType.OpenPopup);
    }
}
