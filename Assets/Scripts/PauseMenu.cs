using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool IsGamePaused = false;
    public AudioSource audioPauseSource;
    public AudioSource audioResumedSource;
    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(IsGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        IsGamePaused = false;
        audioResumedSource.Play();
        audioPauseSource.Stop();
    }

    void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        IsGamePaused = true;
        audioResumedSource.Stop();
        audioPauseSource.Play();
    }

    public void QuitGame()
    {
        Time.timeScale = 0f;
        SceneManager.LoadScene(0);
        Destroy(GameObject.Find("UI"));
        Destroy(GameObject.Find("GameManager"));
        Destroy(GameObject.Find("Player"));
    }
}
