using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    private void Start()
    {
        Destroy(GameObject.Find("UI"));
        Destroy(GameObject.Find("Player"));
        Destroy(GameObject.Find("GameManager"));
    }
    public void Restart()
    {
        SceneManager.LoadScene(0);
        
    }
}
