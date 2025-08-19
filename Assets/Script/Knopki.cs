using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Knopki : MonoBehaviour
{
    [SerializeField] private GameObject WinPanel;
    [SerializeField] private GameObject NaPanel;

    private bool isPaused = false;
    bool AAA;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Nastr()
    {
        WinPanel.SetActive(false);
        NaPanel.SetActive(true);
    }
    public void Ex()
    {
        NaPanel.SetActive(false);
        WinPanel.SetActive(true);
    }
    public void Play()
    {
        SceneManager.LoadScene(1);
        isPaused = false;
        Time.timeScale = 1f;
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
    public void Resume()
    {
        NaPanel.SetActive(!AAA);
        AAA = !AAA;
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Pause()
    {
        NaPanel.SetActive(!AAA);
        AAA = !AAA;
        Time.timeScale = 0f;
        isPaused = true;
    }
}
