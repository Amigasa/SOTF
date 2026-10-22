using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class HudHP : Sounds
{
    [SerializeField] private GameObject HP;//Для хранения спрайта жизни
    [SerializeField] private GameObject HP2;//Для хранения спрайта жизни
    [SerializeField] private GameObject HP3;//Для хранения спрайта жизни
    [SerializeField] private GameObject Hp;//Для хранения спрайта жизни (погибший)
    [SerializeField] private GameObject Hp2;//Для хранения спрайта жизни (погибший)
    [SerializeField] private GameObject Hp3;//Для хранения спрайта жизни (погибший)
    [SerializeField] private GameObject LosePanel;//Для хранения панели проигрыша

    private bool isPaused = false;//Пауза игры
    bool AAA;//Переменная типа bool
    bool Un = false;//Переменная для неуязвимости
    int hp = 3;//хп игрока

    void UnSec()//Функция для отключения неуязвимости
    {
        Un = false;
    }

    // Вызывается из EnemyController (когда враг атакует)
    public void ApplyDamage(int dmg)
    {
        if (Un) return;//Если игрок неуязвим, пропускаем

        if (hp == 3)
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

            if (isPaused) Resume(); else Pause();
        }

        hp--;
        Un = true;
        Invoke("UnSec", 0.5f);
    }

    // Вызывается физически при столкновении (для урона от касания)
    private void OnCollisionEnter2D(Collision2D col)
    {
        // Ищем на объекте скрипт EnemyController
        EnemyController enemy = col.gameObject.GetComponent<EnemyController>();

        // Если это враг и он опасен (в погоне или атаке) — наносим урон
        if (enemy != null && enemy.IsDangerous)
        {
            ApplyDamage(enemy.Data.attackDamage);
        }
    }

    public void Resume()//Возобновление
    {
        LosePanel.SetActive(!AAA);
        AAA = !AAA;
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Pause()//Пауза (при проигрыше)
    {
        LosePanel.SetActive(!AAA);
        AAA = !AAA;
        Time.timeScale = 0f;
        isPaused = true;
    }
}