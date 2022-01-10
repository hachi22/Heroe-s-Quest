using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{

    public void NewGame()
    {
        SceneManager.LoadScene("Intro");
    }
    public void Load ()
    {
        SceneManager.LoadScene("Pueblo");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOverScreen");
    }
}
