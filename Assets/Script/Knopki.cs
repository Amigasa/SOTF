using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Knopki : MonoBehaviour
{
    [SerializeField] private GameObject WinPanel;//Панель главного меню
    [SerializeField] private GameObject NaPanel;//Панель настроек

    private bool isPaused = false;//Пауза игры
    bool AAA;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))//При нажатии на кнопку Escape
        {
            if (isPaused)//Если значение true тогда
            {
                Resume();//Используется функция восстановления после паузы
            }
            else//Иначе
            {
                Pause();//Используется функция остановки времени игры
            }
        }
    }
    public void Nastr()//Публичная функция для кнопки Настройки
    {
        WinPanel.SetActive(false);//Отключается панель главного меню
        NaPanel.SetActive(true);//Включаются настройки
    }
    public void Ex()//Кнопка выхода в настройках
    {
        NaPanel.SetActive(false);//Отключается панель настройки
        WinPanel.SetActive(true);//Включается главное меню
    }
    public void Play()//Для кнопки Играть и Перезагрузить
    {
        SceneManager.LoadScene(1);//Запускается сцена игры
        isPaused = false;//Пауза отключается, сделано для того чтобы если игрок вышел в главное меню и назад пауза не была включена
        Time.timeScale = 1f;//Время переключается в значение 1, что значит игра продолжается
    }
    public void Exit()//Кнопка выхода в главном меню
    {
        Application.Quit();//Полностью выход из игры
    }
    public void Menu()//Для кнопки выхода при проигрыше и в настройках в сцене игры
    {
        SceneManager.LoadScene(0);//Переходит на сцену главного меню
    }
    public void Resume()//Функция отключения паузы
    {
        NaPanel.SetActive(!AAA);//Панель настройки включается/выключается по нажатию кнопки
        AAA = !AAA;//Заменяет значение на противоположный после вкл/выкл настроек
        Time.timeScale = 1f;//Время идет
        isPaused = false;//Отключает паузу
    }

    void Pause()//
    {
        NaPanel.SetActive(!AAA);//Панель настройки включается/выключается по нажатию кнопки
        AAA = !AAA;//Заменяет значение на противоположный после вкл/выкл настроек
        Time.timeScale = 0f;//Время останавливается, игра ставится на паузу
        isPaused = true;//Включает паузу
    }
}
