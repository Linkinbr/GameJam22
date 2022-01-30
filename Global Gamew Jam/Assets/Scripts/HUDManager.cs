using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager hUDManager;

    public GameObject pauseMenu;
    public GameObject diaryMenu;

    bool isPaused;
    bool isDiary;

    // Start is called before the first frame update
    void Start()
    {
        hUDManager = this;
        isPaused = false;
        isDiary = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Cancel") && !isDiary)
        {
            pauseGame();
        }
    }

    void pauseGame()
    {
        isPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void unpauseGame()
    {
        pauseMenu.SetActive(false);
        diaryMenu.SetActive(false);
        isPaused = false;
        isDiary = false;
        Time.timeScale = 1f;
    }

    public void openDiary()
    {
        if(!isPaused)
        {
            isDiary = true;
            diaryMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
