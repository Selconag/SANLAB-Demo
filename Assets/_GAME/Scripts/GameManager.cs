using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("References")]
    [SerializeField] GameObject gameEndPanel;
    public void EndGame()
    {
        AudioManager.Instance.PlaySoundEffect(SoundEffects.LevelComplete);
        gameEndPanel.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
