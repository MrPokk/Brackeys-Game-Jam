
    using UnityEngine;
    using UnityEngine.SceneManagement;
    public class ButtonRestart : CustomButton
    {
        public override void Click()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            GameData<Main>.Boot.StartGame();
        }
    }

