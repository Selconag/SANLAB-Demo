using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [Header("References")]
    [SerializeField] GameObject gameEndPanel;
    public void EndGame()
    {
        gameEndPanel.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
