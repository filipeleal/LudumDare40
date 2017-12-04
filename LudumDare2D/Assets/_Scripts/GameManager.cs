using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private static GameManager _instance;

    public static GameManager Instance
    {
        get { return _instance; }
    }

    void Awake () {
        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void GameOver(float waitSeconds)
    {
        StartCoroutine(GameOverCoroutine(waitSeconds));
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }
    IEnumerator GameOverCoroutine(float waitSeconds)
    {
        yield return new WaitForSeconds(waitSeconds);

        if (_instance != null)
            _instance.GameOver();
    }
}
