using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {
    public LevelType Type = LevelType.Infinite;
    public float TimeToBeat = 60f;

    float TimeElapsed = 0f;

    public Text TimerText;

    public Transform MissilLaucher;
        
    [HideInInspector]
    public bool LevelComplete = false;

    [HideInInspector]
    public bool LevelStarted = false;

    public int CollectiblesCollected = 0;

    [Header("MissilLauncher")]
    public GameObject MissilPrefab;
    public GameObject MissilTarget;
    public Text MissilsLeftText;

    public float Interval = 1f;
    public int NumberOfMissils = 1;
    public bool SpawnOnClick = true;
    public bool AutoSpawn = true;

    private int MissilsSpawned = 0;
    private float TimeAfterSpawn = 0;

    [Header("Instructions")]
    public Canvas InstructionCanvas;
    public TextMeshProUGUI BossLifeText;
    public TextMeshProUGUI MissilsText;
    public TextMeshProUGUI TimeText;
    public TextMeshProUGUI TimeLabelText;

    [Header("YouWin")]
    public Canvas YouWinCanvas;
    public List<Image> Collectibles = new List<Image>();
    public TextMeshProUGUI BeatTimeText;
    

    private void Awake()
    {
        MissilsLeftText.text = (NumberOfMissils - MissilsSpawned).ToString();

        var enemy = FindObjectOfType<Enemy>();

        BossLifeText.text = enemy.HP.ToString();
        TimeText.text = TimeToBeat.ToString("F1");
        MissilsText.text = NumberOfMissils.ToString();

        InstructionCanvas.enabled = true;

        if (Type == LevelType.Infinite)
        {
            TimeText.enabled = false;
            TimeLabelText.enabled = false;
        }

    }

    void Update () {
        if (LevelComplete)
        {
            CheckGoToNextLevel();
            return;
        }
        if (!LevelStarted)
        {
            CheckLevelStart();
            return;
        }

        TimeElapsed += Time.deltaTime;
        UpdateTimer();
        UpdateMissilLaucher();
	}

    void UpdateTimer()
    {
        switch (Type)
        {
            case LevelType.Infinite:
                TimerText.text = TimeElapsed.ToString("F1");
                break;
            case LevelType.Timed:
                float timer = (TimeToBeat - TimeElapsed);
                TimerText.text = (timer <= 0f ? 0f: timer).ToString("F1");
                if (timer <= 0f)
                    GameManager.Instance.GameOver();
                break;
            default:
                break;
        }
    }

    #region Missil Laucher
    void UpdateMissilLaucher()
    {
        if (LevelComplete)
            return;

        CheckOutOfMissils();
        if (MissilsSpawned == NumberOfMissils)
            return;

        if (SpawnOnClick && Input.GetMouseButtonDown(0))
            SpawnMissil();

        if (AutoSpawn)
        {
            TimeAfterSpawn += Time.deltaTime;
            if (TimeAfterSpawn >= Interval)
                SpawnMissil();
        }
    }

    void SpawnMissil()
    {
        var missil = Instantiate(MissilPrefab, MissilLaucher.position, MissilLaucher.rotation);

        missil.GetComponent<Missil>().Target = MissilTarget;

        MissilsSpawned++;
        TimeAfterSpawn = 0f;

        MissilsLeftText.text = (NumberOfMissils - MissilsSpawned).ToString();
    }

    void CheckOutOfMissils()
    {
        var missilsLeft = NumberOfMissils - MissilsSpawned;
        if (missilsLeft == 0)
        {

            var missilsOnScreen = FindObjectsOfType<Missil>().Length;
            Debug.Log(missilsOnScreen);
            if (missilsOnScreen == 0)
                GameManager.Instance.GameOver();
        }
    }
    #endregion

    void CheckLevelStart()
    {
        if (Input.GetMouseButtonDown(0))
        {
            InstructionCanvas.enabled = false;
            LevelStarted = true;
        }
    }

    public void CompleteLevel(float waitSeconds) {
        LevelComplete = true;
        BeatTimeText.text = TimerText.text;
        for(int i = 0; i < Collectibles.Count; i++)
        {
            if (i < CollectiblesCollected)
                Collectibles[i].enabled = true;
            else
                break;
        }

        StartCoroutine(ShowCompleteScreen(waitSeconds));
    }

    IEnumerator ShowCompleteScreen(float waitSeconds)
    {
        yield return new WaitForSeconds(waitSeconds);

        YouWinCanvas.enabled = true;
    }

    public void CheckGoToNextLevel()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}


public enum LevelType
{
    Infinite,
    Timed
}
