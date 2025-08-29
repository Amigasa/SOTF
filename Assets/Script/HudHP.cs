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
    [SerializeField] private GameObject Hp;//Для хранения спрайта жизни
    [SerializeField] private GameObject Hp2;//Для хранения спрайта жизни
    [SerializeField] private GameObject Hp3;//Для хранения спрайта жизни
    [SerializeField] private GameObject LosePanel;//Для хранения панели проигрыша
    private bool isPaused = false;//Пауза игры
    bool AAA;//Переменная типа bool
    bool Un = false;//переменная
    int hp = 3;//хп игрока
    void UnSec()//Функция для неуязвимости после получения урона
    { 
        Un = false;//равна false
    }
    private void OnCollisionEnter2D(Collision2D col)//Функция для определения столкновения объектов с помощью колизии
    {
        if (col.gameObject.tag == "Enemy" && Un == false)//Если тэг врага и неуязвимость отключена
        {
            if(hp == 3)//Если хп равно 3 то есть полное
            {
                HP3.SetActive(false);//Отключается отображение живого гриба
                Hp3.SetActive(true);//Включается отображение погибшего гриба
                PlaySound(sounds[0]);//Звук получения урона
            }
            else if (hp == 2)//Если хп равно 2
            {
                HP2.SetActive(false);//Отключается отображение живого гриба
                Hp2.SetActive(true);//Включается отображение погибшего гриба
                PlaySound(sounds[0]);//Звук получения урона
            }
            else if (hp == 1)//Если хп равно 1
            {
                HP.SetActive(false);//Отключается отображение живого гриба
                Hp.SetActive(true);//Включается отображение погибшего гриба
                PlaySound(sounds[0]);//Звук получения урона
                if (isPaused)//Если пауза true
                {
                    Resume();//Игра продолжается
                }
                else//Иначе
                {
                    Pause();//Пауза
                }
            }
            hp--;//Хп отнимается для понимания сколько хп у игрока
            Un = true;//Включается неуязвимость
            Invoke("UnSec", 0.5f);//Длительность действия
        }
    }
    public void Resume()//Возобновление
    {
        LosePanel.SetActive(!AAA);
        AAA = !AAA;
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Pause()//Пауза
    {
        LosePanel.SetActive(!AAA);
        AAA = !AAA;
        Time.timeScale = 0f;
        isPaused = true;
    }
}
