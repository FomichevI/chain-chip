using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing.MiniJSON;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public static DataManager S;
    [SerializeField] public DataHandler CurrentDataHandler;
    public bool IsGameFirstLoaded = false;

    private void Awake()
    {
        if (S == null)
            S = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject);
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        CurrentDataHandler = null;
        SplashScreen.S.Hide(() => MenuManager.S.TryShowRules());
#else
        StartCoroutine(Starting());
#endif
    }
    public IEnumerator Starting()
    {
        yield return new WaitForSeconds(.05f);
        if (CurrentDataHandler != null)
        {
            if (!IsGameFirstLoaded)
            {
                CurrentDataHandler.Initialize();
                while (!CurrentDataHandler.IsInitializing)
                {
                    yield return new WaitForSeconds(.1f);
                }
                CurrentDataHandler.StartLoadingPlayerData();
                while (!CurrentDataHandler.IsPlayerDataLoading)
                {
                    yield return new WaitForSeconds(.1f);
                }
                IsGameFirstLoaded = true;
            }
            string lang = CurrentDataHandler.GetPlayerLanguage();
            yield return new WaitForSeconds(.5f);
            if (lang != "en")
                EventAggregator.SetRussianLanguage.Invoke();
            SplashScreen.S.Hide(() => MenuManager.S.TryShowRules());
            EventAggregator.Init.Invoke();
        }
    }
    void OnApplicationFocus(bool hasFocus)
    {
        if (CurrentDataHandler.IsPlayerDataLoading)
            SaveData();
    }
    void OnApplicationPause(bool isPaused)
    {
        if (CurrentDataHandler.IsPlayerDataLoading)
            SaveData();
    }

    #region Saves
    public void SaveData()
    {
        if (CurrentDataHandler != null && CurrentDataHandler.CurrentPlayerData != null)
        {
            CurrentDataHandler.SavePlayerData();
        }
    }
    public void SetMaxScore(int score)
    {
        if (CurrentDataHandler != null && CurrentDataHandler.CurrentPlayerData != null)
        {
            CurrentDataHandler.CurrentPlayerData.MaxScore = score;
        }
    }
    public void SetCurrentScore(int score)
    {
        if (CurrentDataHandler != null && CurrentDataHandler.CurrentPlayerData != null)
        {
            CurrentDataHandler.CurrentPlayerData.CurrentScore = score;
        }
    }
    public void SetVolumeOn(bool isOn)
    {
        if (CurrentDataHandler != null && CurrentDataHandler.CurrentPlayerData != null)
        {
            CurrentDataHandler.CurrentPlayerData.VolumeOn = isOn;
            CurrentDataHandler.SavePlayerData();
        }
    }
    public void SetMusicOn(bool isOn)
    {
        if (CurrentDataHandler != null && CurrentDataHandler.CurrentPlayerData != null)
        {
            CurrentDataHandler.CurrentPlayerData.MusicOn = isOn;
            CurrentDataHandler.SavePlayerData();
        }
    }
    public void SetChipsDatas(List<ChipData> chipsDatas)
    {
        if (CurrentDataHandler != null && CurrentDataHandler.CurrentPlayerData != null)
        {
            CurrentDataHandler.CurrentPlayerData.ChipDatas = chipsDatas;
        }
    }
    public void SetStartChipData(ChipData chipsData, bool isSkill)
    {
        if (CurrentDataHandler != null && CurrentDataHandler.CurrentPlayerData != null)
        {
            CurrentDataHandler.CurrentPlayerData.StartChipData = chipsData;
            CurrentDataHandler.CurrentPlayerData.StartChipIsSkill = isSkill;
        }
    }
    public void SetSkillAndFilling(eChipColors color, int count, float filling)
    {
        if (CurrentDataHandler != null && CurrentDataHandler.CurrentPlayerData != null)
        {
            switch (color)
            {
                case eChipColors.green:
                    CurrentDataHandler.CurrentPlayerData.GreenSkillCount = count;
                    CurrentDataHandler.CurrentPlayerData.GreenSkillFilling = filling;
                    break;
                case eChipColors.red:
                    CurrentDataHandler.CurrentPlayerData.RedSkillCount = count;
                    CurrentDataHandler.CurrentPlayerData.RedSkillFilling = filling;
                    break;
                case eChipColors.purple:
                    CurrentDataHandler.CurrentPlayerData.PurpleSkillCount = count;
                    CurrentDataHandler.CurrentPlayerData.PurpleSkillFilling = filling;
                    break;
                case eChipColors.blue:
                    CurrentDataHandler.CurrentPlayerData.BlueSkillCount = count;
                    CurrentDataHandler.CurrentPlayerData.BlueSkillFilling = filling;
                    break;
            }
        }
    }
    public void SetColumns(int[] columns)
    {
        if (CurrentDataHandler != null && CurrentDataHandler.CurrentPlayerData != null)
        {
            CurrentDataHandler.CurrentPlayerData.Columns = columns;
        }
    }
    #endregion
}
