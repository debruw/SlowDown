﻿using System.Collections;
using System.Collections.Generic;
using TapticPlugin;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    int currentLevel = 0;
    int maxLevelNumber = 19;
    public GameObject soundManager;
    public GameObject currentLevelObject;
    public bool isGameStarted, isGameOver;
    public int currentTargetScore;
    public int currentScore, highestScore;

    #region UIElements
    //public GameObject NextBttn;
    public Text LevelText, NextLevelText;
    //public GameObject VibrationButton;
    public GameObject GameWinPanel, StartPanel, InGamePanel, GameLosePanel;
    public Text currentScoreText;
    public Slider LevelSlider;
    #endregion

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        if (PlayerPrefs.HasKey("LevelId"))
        {
            currentLevel = PlayerPrefs.GetInt("LevelId");
        }
        else
        {
            PlayerPrefs.SetInt("LevelId", currentLevel);
        }
        if (PlayerPrefs.HasKey("HighScore"))
        {
            highestScore = PlayerPrefs.GetInt("HighScore");
        }
        else
        {
            PlayerPrefs.SetInt("HighScore", highestScore);
        }
        if (PlayerPrefs.GetInt("UseMenu").Equals(1) || !PlayerPrefs.HasKey("UseMenu"))
        {
            //MenuPanel.SetActive(true);
            PlayerPrefs.SetInt("UseMenu", 1);
        }
        else if (PlayerPrefs.GetInt("UseMenu").Equals(0))
        {
            //inGamePanel.GetComponent<Animator>().SetTrigger("ComeIn");
            //Ball.GetComponent<BallController>().canMove = true;
            //magnet.GetComponent<Animator>().enabled = false;
        }

        if (!PlayerPrefs.HasKey("VIBRATION"))
        {
            PlayerPrefs.SetInt("VIBRATION", 1);
            //VibrationButton.GetComponent<Image>().sprite = on;
        }
        if (SoundManager.Instance == null)
        {
            Instantiate(soundManager);
        }
        currentScoreText.text = currentScore.ToString();
        InitializeLevel();
    }

    public void InitializeLevel()
    {
        //TODO Test için konuldu kaldırılacak
        //currentLevel = 64;

        if (currentLevel > maxLevelNumber)
        {
            int rand = Random.Range(0, maxLevelNumber);
            if (rand == PlayerPrefs.GetInt("LastLevel"))
            {
                rand = Random.Range(0, maxLevelNumber);
            }
            PlayerPrefs.SetInt("LastLevel", rand);
            currentLevelObject = Instantiate(Resources.Load("Level" + rand), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        }
        else
        {
            Debug.Log("Level : " + currentLevel);
            currentLevelObject = Instantiate(Resources.Load("Level" + currentLevel), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        }
        ChangeColor(currentLevel);
        currentTargetScore = currentLevelObject.GetComponent<LevelObject>().targetScore;
        LevelSlider.maxValue = currentTargetScore;
        LevelText.text = (currentLevel + 1).ToString();
        NextLevelText.text = (currentLevel + 2).ToString();
    }


    public void GameWin()
    {
        currentScore = 0;
        Debug.Log("Game Win");
        isGameStarted = false;
        GameWinPanel.SetActive(true);
        Destroy(currentLevelObject);
        currentLevel++;
        PlayerPrefs.SetInt("LevelId", currentLevel);
    }

    public void GameLose()
    {
        currentScore = 0;
        Debug.Log("Game Lose");
        GameLosePanel.SetActive(true);
        isGameOver = true;
        isGameStarted = false;
        Destroy(currentLevelObject);
    }

    public void AddScore()
    {
        if (isGameStarted && !isGameOver)
        {
            Debug.Log("Add score : " + (currentLevel + 1));
            currentScore += (currentLevel + 1);
            currentScoreText.text = currentScore.ToString();
            LevelSlider.value = (float)currentScore;
            if (currentScore >= currentTargetScore)
            {
                GameWin();
            }
        }
    }

    private void OnApplicationPause(bool pause)
    {
        PlayerPrefs.SetInt("UseMenu", 1);
    }

    public void RetryButtonClick()
    {
        SceneManager.LoadScene("Scene_Game");
    }

    public void TapToStartButtonClick()
    {
        isGameStarted = true;
    }

    public void TapToNextButtonClick()
    {
        if (PlayerPrefs.GetInt("VIBRATION") == 1)
            TapticManager.Impact(ImpactFeedback.Light);
        PlayerPrefs.SetInt("UseMenu", 0);
        //SceneManager.LoadScene("Scene_Game");
        InitializeLevel();
        GameWinPanel.SetActive(false);
        isGameStarted = true; 
        isGameOver = false;
        ChangeColor(currentColor++);
    }

    public Sprite on, off;
    public void VibrateButtonClick()
    {
        if (PlayerPrefs.GetInt("VIBRATION").Equals(1))
        {//Vibration is on
            PlayerPrefs.SetInt("VIBRATION", 0);
            //VibrationButton.GetComponent<Image>().sprite = off;
        }
        else
        {//Vibration is off
            PlayerPrefs.SetInt("VIBRATION", 1);
            //VibrationButton.GetComponent<Image>().sprite = on;
        }

        if (PlayerPrefs.GetInt("VIBRATION") == 1)
            TapticManager.Impact(ImpactFeedback.Light);
    }

    public Material[] materials;
    public ColorData colorData;
    public int currentColor = 0;
    public void ChangeColor(int clr)
    {
        currentColor = clr;
        if (currentColor > 3)
        {
            currentColor = 0;
        }
        Debug.Log("current color : " + currentColor);
        StartCoroutine(WaitAndChange());
    }

    IEnumerator WaitAndChange()
    {
        for (int i = materials.Length - 1; i >= 0; i--)
        {
            materials[i].SetColor("_Color", colorData.colorGroups[currentColor].colors[i]);
            yield return new WaitForSeconds(.05f);
        }
    }
}