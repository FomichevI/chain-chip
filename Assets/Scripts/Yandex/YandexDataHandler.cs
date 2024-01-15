using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class YandexDataHandler : DataHandler
{
    [DllImport("__Internal")]
    private static extern void InitPlayer();
    [DllImport("__Internal")]
    private static extern void InitYandex();
    [DllImport("__Internal")]
    private static extern void SaveExtern(string data);
    [DllImport("__Internal")]
    private static extern void LoadExtern();
    [DllImport("__Internal")]
    private static extern void Rate();
    [DllImport("__Internal")]
    private static extern void SetNewScore(int value);
    [DllImport("__Internal")]
    private static extern string GetLanguage();

    private bool _isSdkInit = false;

    public override void Initialize()
    {
        base.Initialize();
        StartCoroutine(Init());
    }
    private IEnumerator Init()
    {
        InitYandex();
        while (!_isSdkInit)
        {
            yield return new WaitForSeconds(.1f);
        }
        InitPlayer();
    }
    public void OnPlayerInitializing()
    {
        Debug.Log("���������� �� ������ ��������");
        IsInitializing = true;
    }
    public void OnYandexSdkInitializing()
    {
        Debug.Log("SDK ��������������� � �����");
        _isSdkInit = true;
    }
    public override void StartLoadingPlayerData()
    {
        base.StartLoadingPlayerData();
        LoadExtern();
    }
    public override string GetPlayerLanguage()
    {
        string lang = GetLanguage();
        Debug.Log("���� ������ �������: " + lang);
        return lang;
    }
    public override void SetPlayerData(string value)
    {
        base.SetPlayerData(value);
        Debug.Log(value);
        if (value != null)
        {
            Debug.Log("������ ������ ������� �� �������");
            CurrentPlayerData = JsonUtility.FromJson<PlayerData>(value);
            Debug.Log("��������: " + CurrentPlayerData.ChipDatas.Count);
        }
        else
        {
            Debug.Log("������ ������ �� ���� ������� �� �������. ������ ����� ��������� ������");
            CurrentPlayerData = new PlayerData();
        }
        IsPlayerDataLoading = true;
    }
    public override void SavePlayerData()
    {
        base.SavePlayerData();
        SaveExtern(JsonUtility.ToJson(CurrentPlayerData));
        //Debug.Log(JsonUtility.ToJson(CurrentPlayerData));
        if (CurrentPlayerData.CurrentScore > CurrentPlayerData.MaxScore)
            SetOnLiderboard(CurrentPlayerData.CurrentScore);
    }
    public override void RateUs()
    {
        base.RateUs();
        Rate();
    }
    public override void SetOnLiderboard(int score)
    {
        base.SetOnLiderboard(score);
        SetNewScore(score);
    }
}
