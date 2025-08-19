using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Knopki : MonoBehaviour
{
    [SerializeField] private GameObject WinPanel;
    [SerializeField] private GameObject NaPanel;
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
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
}
