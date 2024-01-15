using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class DataHandler : MonoBehaviour
{
    public bool IsInitializing = false;
    public bool IsPlayerDataLoading = false;
    [HideInInspector] public PlayerData CurrentPlayerData = null;
    public Action OnPlayerDataLoaded;

    public virtual void Initialize()
    {
        Debug.Log("Обработчик данных инициализирован");
    }
    public virtual void StartLoadingPlayerData()
    {
        Debug.Log("Начинаем загрузку данных игрока");
    }
    public virtual void SetPlayerData(string value)
    {
        Debug.Log("Данные игрока загружены");
    }
    public virtual void SavePlayerData()
    {
        Debug.Log("Начинаем сохранение данных игрока");
    }
    public virtual string GetPlayerLanguage()
    {
        return null;
    }
    public virtual void RateUs()
    {
        Debug.Log("Запуск оценки игры");
    }
    public virtual void SetOnLiderboard(int score)
    {
        Debug.Log("Новый счет отправлен в лидерборд");
    }
}
