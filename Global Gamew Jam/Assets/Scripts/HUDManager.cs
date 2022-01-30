using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager hUDManager;

    Personagem player;
    public GameObject termoObject;
    public Slider termoSlider;
    public Image termoFill;

    public Text totalClimb;
    public Text currentClimb;

    [Range(0,1)]
    public int vSyncValue = 1;

    public GameObject pauseMenu;
    public GameObject diaryMenu;
    public GameObject gameOverScreen;

    public float masterVolume;
    float distanceTravelled;

    public GameObject[] diaryEntries;
    public int currentEntry = 0;

    bool isPaused;
    bool isDiary;

    // Start is called before the first frame update
    void Awake()
    {
        hUDManager = this;
        isPaused = false;
        isDiary = false;
        Screen.fullScreen = true;
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Personagem>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Cancel") && !isDiary)
        {
            pauseGame();
        }

        if(player != null)
        {
            termoFill.fillAmount = (-player.Temperatura + 8) / 16;
            currentClimb.text = Mathf.RoundToInt(ControladorJogo.instance.alturaAtual).ToString() +"m";
        }
    }

    void pauseGame()
    {
        isPaused = true;
        termoObject.SetActive(false);
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void unpauseGame()
    {
        pauseMenu.SetActive(false);
        diaryMenu.SetActive(false);
        termoObject.SetActive(true);
        isPaused = false;
        isDiary = false;
        Time.timeScale = 1f;
    }

    public void openDiary()
    {
        if(!isPaused)
        {
            termoObject.SetActive(false);
            isDiary = true;
            diaryMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void previousEntry()
    {
        if(currentEntry > 0)
        {
            int aux = currentEntry;
            for (int i = 0; i < diaryEntries.Length; i++)
            {
                if(i == currentEntry-1)
                {
                    diaryEntries[i].SetActive(true);
                    aux = i;
                } else
                {
                    diaryEntries[i].SetActive(false);
                }
            }
            currentEntry = aux;
        }
    }

    public void nextEntry()
    {
        if(currentEntry+1 < diaryEntries.Length)
        {
            int aux = currentEntry;
            for (int i = 0; i < diaryEntries.Length; i++)
            {
                if(i == currentEntry+1)
                {
                    diaryEntries[i].SetActive(true);
                    aux = i;
                } else
                {
                    diaryEntries[i].SetActive(false);
                }
            }
            currentEntry = aux;
        }
    }

    public void fullScreenToggle()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void vsyncToggle(bool value)
    {
        if(value) QualitySettings.vSyncCount = 1;
        else QualitySettings.vSyncCount = 0;
    }

    public void gameOver()
    {
        gameOverScreen.SetActive(true);
        termoObject.SetActive(false);
        isPaused = true;
        Time.timeScale = 0f;
        totalClimb.text = Mathf.RoundToInt(ControladorJogo.instance.alturaAtual).ToString();
    }

    public void resetGame()
    {
        ControladorJogo.instance.recarregaCenas();
        Time.timeScale = 1f;
    }
}
