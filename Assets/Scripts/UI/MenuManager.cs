using System.Collections;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public static MenuManager S;

    [HideInInspector] public eChipColors LastSkillColor;
    [SerializeField] private GameObject _nextStagePanel;
    [SerializeField] private GameObject _newSkillPanel;
    [SerializeField] private GameObject _losePanel;
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _wantToRestartPanel;
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _rulesPanel;
    [SerializeField] private GameObject _logo;
    [SerializeField] private GameObject _noAdsBut;

    [Header("NewStagePanel Settings")]
    //при активации похвалы игрока рандомно выбирается один из текстов
    [SerializeField] private GameObject[] _praiseTexts;

    [Header("NewSkillPanel Settings")]
    //на панели получения нового скилла активируются необходимые значки, в зависимости от того, какой скилл получен
    //кнопка "No, thanks" появляется не сразу, а спустя несколько секунд    
    [SerializeField] private GameObject _noThanksBut;
    [SerializeField] private GameObject[] _fireSkillImgs;
    [SerializeField] private GameObject[] _frostSkillImgs;
    [SerializeField] private GameObject[] _lightningSkillImgs;
    [SerializeField] private GameObject[] _cristalSkillImgs;
    [HideInInspector] public bool IsLoadingNoThank = false; //переменная для проверки, находится ли кнопка "No, thanks" в стадии загрузки
    private bool _isActiveStageOrSkillPanel;

    [Header("SettingsPanel Settings")]
    //если музыка включена или отключена, активируем соответствующий чекбокс
    [SerializeField] private GameObject _musicCheckBoxImg;
    [SerializeField] private GameObject _soundsCheckBoxImg;

    [Header("LosePanel Settings")]
    //на панели проигрыша показываем текущий и лучший результаты игрока
    [SerializeField] private GameObject _newBestScoreObj;
    [SerializeField] private GameObject[] _lastScoreObjs;
    [SerializeField] private TextMeshProUGUI _currentScoreText;
    [SerializeField] private TextMeshProUGUI _lastScoreText;


    private void Awake()
    {
        if (S == null)
            S = this;
    }

    private void Start()
    {
        if (!XmlReader.S.NoAddsMode())
            _noAdsBut.SetActive(true);
        else
            _noAdsBut.SetActive(false);
    }

    public void ShowNextStagePanel()
    {
        if (!_isActiveStageOrSkillPanel)
        {
            _isActiveStageOrSkillPanel = true;
            _nextStagePanel.SetActive(true);
            //включить случайный текст с похвалой
            foreach (GameObject go in _praiseTexts)
                go.SetActive(false);
            _praiseTexts[Random.Range(0, _praiseTexts.Length)].SetActive(true);
        }
    }
    public void HideNextStagePanel()
    {
        _isActiveStageOrSkillPanel = false;
        _nextStagePanel.SetActive(false);

        if (!XmlReader.S.NoAddsMode())
        {
            //UnityAdsManager.S.ShowInterstitial();
            //AdsManager.S.ShowInterstitialAd();
        }
    }
    public void ShowNewSkillPanel()
    {
        if (!_isActiveStageOrSkillPanel && LastSkillColor != eChipColors.no)
        {
            _isActiveStageOrSkillPanel = true;
            _newSkillPanel.SetActive(true);
            StartCoroutine(ShowNoThanksBut());
            HideSkillsImgs();
            switch (LastSkillColor)
            {
                case eChipColors.green:
                    foreach (GameObject go in _cristalSkillImgs)
                        go.SetActive(true);
                    break;
                case eChipColors.blue:
                    foreach (GameObject go in _frostSkillImgs)
                        go.SetActive(true);
                    break;
                case eChipColors.red:
                    foreach (GameObject go in _fireSkillImgs)
                        go.SetActive(true);
                    break;
                case eChipColors.purple:
                    foreach (GameObject go in _lightningSkillImgs)
                        go.SetActive(true);
                    break;
            }
        }
        else
        {
            LastSkillColor = eChipColors.no;
        }
    }
    public void ShowAdd()
    {
        if (!XmlReader.S.NoAddsMode())
        {
            //UnityAdsManager.S.ShowRevarded();
            //AdsManager.S.ShowRevardedAd();
        }
        else
        {
            IsLoadingNoThank = false;
            SkillsController.S.IncreaseSkillFilling(SkillsController.S.MaxFillingSkills, LastSkillColor);
            LastSkillColor = eChipColors.no;
            HideAllPanels();
        }
    }
    IEnumerator ShowNoThanksBut()
    {
        IsLoadingNoThank = true;
        yield return new WaitForSeconds(2f);
        if (IsLoadingNoThank)
            _noThanksBut.SetActive(true);
    }
    public void ShowLosePanel()
    {
        HideAllPanels();
        _losePanel.SetActive(true);
        if (ScoreController.S.GetScore() > XmlReader.S.GetMaxScore())
        {
            foreach (GameObject go in _lastScoreObjs)
                go.SetActive(false);
            _newBestScoreObj.SetActive(true);
            XMLSaver.S.SetMaxScore(ScoreController.S.GetScore());
        }
        else
        {
            foreach (GameObject go in _lastScoreObjs)
                go.SetActive(true);
            _newBestScoreObj.SetActive(false);
            _lastScoreText.text = XmlReader.S.GetMaxScore().ToString();
        }
        _currentScoreText.text = ScoreController.S.GetScore().ToString();
        //if (ScoreController.S.GetScore() >= 250)
        //RManager.S.ShowRewie();
    }
    public void ShowSettingsPanel()
    {
        _settingsPanel.SetActive(true);
        _logo.SetActive(false);
    }
    public void HideSettingsPanel()
    {
        _settingsPanel.SetActive(false);
        if (_menuPanel.activeSelf)
            _logo.SetActive(true);
    }
    public void ShowWantToRestartPanel()
    {
        _wantToRestartPanel.SetActive(true);
    }
    public void HideWantToRestartPanel()
    {
        _wantToRestartPanel.SetActive(false);
    }
    public void RestartLevel()
    {
        HideAllPanels();
        XMLSaver.S.UnnullTable();
        GameManager.S.RestartLevel();
    }
    public void ShowMenuPanel()
    {
        HideAllPanels();
        _menuPanel.SetActive(true);
        _logo.SetActive(true);
    }
    public void ShowRulesPanel()
    {
        _rulesPanel.SetActive(true);
        _rulesPanel.GetComponent<ManualController>().SetStartPage();
    }
    public void HideAllPanels()
    {
        _nextStagePanel.SetActive(false);
        _newSkillPanel.SetActive(false);
        _losePanel.SetActive(false);
        _settingsPanel.SetActive(false);
        _wantToRestartPanel.SetActive(false);
        _menuPanel.SetActive(false);
        _noThanksBut.SetActive(false);
        IsLoadingNoThank = false;
        _isActiveStageOrSkillPanel = false;
        LastSkillColor = eChipColors.no;
        _logo.SetActive(false);
    }

    private void HideSkillsImgs()
    {
        foreach (GameObject go in _fireSkillImgs)
            go.SetActive(false);
        foreach (GameObject go in _frostSkillImgs)
            go.SetActive(false);
        foreach (GameObject go in _lightningSkillImgs)
            go.SetActive(false);
        foreach (GameObject go in _cristalSkillImgs)
            go.SetActive(false);
    }
    public void ShowRecords()
    {
        //Records.S.ShowLeaderBoard();
    }
    public void SwitchOffAdds()
    {
        if (!XmlReader.S.NoAddsMode())
        {
            XMLSaver.S.SwitchOffAds();
        }
    }

    public void SwitchMusic()
    {
        if (XmlReader.S.GetMusicOn())
        {
            XMLSaver.S.SetMusic(0);
        }
        else
        {
            XMLSaver.S.SetMusic(1);
        }
        SetSoundsSettings();
    }
    public void SwitchSounds()
    {
        if (XmlReader.S.GetSoundsOn())
        {
            XMLSaver.S.SetSounds(0);
        }
        else
        {
            XMLSaver.S.SetSounds(1);
        }
        SetSoundsSettings();
    }

    public void SetSoundsSettings()
    {
        if (XmlReader.S.GetMusicOn())
        {
            _musicCheckBoxImg.SetActive(true);
            AudioManager.S.SetMusicVolume(1);
        }
        else
        {
            _musicCheckBoxImg.SetActive(false);
            AudioManager.S.SetMusicVolume(0);
        }
        if (XmlReader.S.GetSoundsOn())
        {
            _soundsCheckBoxImg.SetActive(true);
            AudioManager.S.SetSoundsVolume(1);
        }
        else
        {
            _soundsCheckBoxImg.SetActive(false);
            AudioManager.S.SetSoundsVolume(0);
        }
    }
    public void Play()
    {
        HideAllPanels();
        if (!XmlReader.S.HasSave() && XmlReader.S.GetMaxScore() == 0)
        {
            ShowRulesPanel();
        }
    }
}
