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
        Debug.Log("���������� ������ ���������������");
    }
    public virtual void StartLoadingPlayerData()
    {
        Debug.Log("�������� �������� ������ ������");
    }
    public virtual void SetPlayerData(string value)
    {
        Debug.Log("������ ������ ���������");
    }
    public virtual void SavePlayerData()
    {
        Debug.Log("�������� ���������� ������ ������");
    }
    public virtual string GetPlayerLanguage()
    {
        return null;
    }
    public virtual void RateUs()
    {
        Debug.Log("������ ������ ����");
    }
    public virtual void SetOnLiderboard(int score)
    {
        Debug.Log("����� ���� ��������� � ���������");
    }
}
