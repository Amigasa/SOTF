using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class Nastroyki : MonoBehaviour
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
}