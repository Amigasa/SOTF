using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class HudHP : Sounds
{
    [SerializeField] private GameObject HP;
    [SerializeField] private GameObject HP2;
    [SerializeField] private GameObject HP3;
    [SerializeField] private GameObject Hp;
    [SerializeField] private GameObject Hp2;
    [SerializeField] private GameObject Hp3;
    [SerializeField] private GameObject LosePanel;
    private bool isPaused = false;
    bool AAA;
    bool Un = false;
    int hp = 3;
    void UnSec()
    { 
        Un = false; 
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy" && Un == false)
        {
            if(hp == 3)
            {
                HP3.SetActive(false);
                Hp3.SetActive(true);
                PlaySound(sounds[0]);
            }
            else if (hp == 2)
            {
                HP2.SetActive(false);
                Hp2.SetActive(true);
                PlaySound(sounds[0]);
            }
            else if (hp == 1)
            {
                HP.SetActive(false);
                Hp.SetActive(true);
                PlaySound(sounds[0]);
                if (isPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
            hp--;
            Un = true;
            Invoke("UnSec", 0.5f);
        }
    }
    public void Resume()
    {
        LosePanel.SetActive(!AAA);
        AAA = !AAA;
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Pause()
    {
        LosePanel.SetActive(!AAA);
        AAA = !AAA;
        Time.timeScale = 0f;
        isPaused = true;
    }
}
