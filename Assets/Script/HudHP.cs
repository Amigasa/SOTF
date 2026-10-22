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
        if (Un) return;// Если игрок неуязвим, пропускаем

        if (hp == 3) // Если хп равно 3 то есть полное
        {
            HP3.SetActive(false); // Отключается отображение живого гриба
            Hp3.SetActive(true); // Включается отображение погибшего гриба
            PlaySound(sounds[0]); // Звук получения урона
        }
        else if (hp == 2) // Если хп равно 2
        {
            HP2.SetActive(false); // Отключается отображение живого гриба
            Hp2.SetActive(true); // Включается отображение погибшего гриба
            PlaySound(sounds[0]); // Звук получения урона
        }
        else if (hp == 1) // Если хп равно 1
        {
            HP.SetActive(false); // Отключается отображение живого гриба
            Hp.SetActive(true); // Включается отображение погибшего гриба
            PlaySound(sounds[0]); // Звук получения урона

            if (isPaused) Resume(); else Pause(); // Если пауза true игра продолжается, иначе пауза
        }

        hp--; // Хп отнимается для понимания сколько хп у игрока
        Un = true; // Включается неуязвимость
        Invoke("UnSec", 0.5f); // Выключаем неуязвимость через 0.5 секунды
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

    public void Resume() // Возобновление
    {
        LosePanel.SetActive(!AAA); // Включает/выключает панель проигрыша
        AAA = !AAA; // Меняет значение на противоположное
        Time.timeScale = 1f; // Время идет
        isPaused = false; // Отключает паузу
    }

    void Pause() //Пауза (вызывается при проигрыше)
    {
        LosePanel.SetActive(!AAA); // Включает/выключает панель проигрыша
        AAA = !AAA; // Меняет значение на противоположное
        Time.timeScale = 0f; // Время останавливается, игра ставится на паузу
        isPaused = true; // Включает паузу
    }
}