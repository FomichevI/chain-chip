using System.Collections;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public static MenuManager S;

    private eChipColors _lastSkillColor = eChipColors.no; //переменная, определяющая, какой скилл заполнился после прошлого хода (или никакой)
    [SerializeField] private GameObject _nextStagePanel;
    [SerializeField] private GameObject _rateUsButton;
    [SerializeField] private GameObject _newSkillPanel;
    [SerializeField] private GameObject _losePanel;
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _wantToRestartPanel;
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _rulesPanel;
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

    private bool isRated = false;

    private void Awake()
    {
        if (S == null)
            S = this;
    }

    private void OnEnable()
    {
        EventAggregator.Lose.AddListener(ShowLosePanel);
        EventAggregator.NewStage.AddListener(ShowNextStagePanel);
        EventAggregator.AddNewSkill.AddListener(ShowNewSkillPanel);
        EventAggregator.SkillFilled.AddListener(SkillFilled);
    }
    private void OnDisable()
    {
        EventAggregator.Lose.RemoveListener(ShowLosePanel);
        EventAggregator.NewStage.RemoveListener(ShowNextStagePanel);
        EventAggregator.AddNewSkill.RemoveListener(ShowNewSkillPanel);
        EventAggregator.SkillFilled.RemoveListener(SkillFilled);
    }
    public void InitSounds()
    {
        SetSoundsSettings();
    }
    public void TryShowRules()
    {
        if (!XmlReader.S.HasSave())
        {
            if (XmlReader.S.GetMaxScore() == 0) //если нет сохраненного уровня и не найдено никакого результата в сохранениях, то начинаем игру с показа подсказки
                ShowRulesPanel();
            //ShowMenuPanel();
        }
    }
    private void SkillFilled(eChipColors color)
    {
        _lastSkillColor = color;
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
            //включить кнопку с оценкой игры
            if(!isRated && XmlReader.S.GetCurrentScore() > 100)
                _rateUsButton.SetActive(true);
            else
                _rateUsButton.SetActive(false);
        }
    }
    public void HideNextStagePanel()
    {
        _isActiveStageOrSkillPanel = false;
        _nextStagePanel.SetActive(false);

        if (!XmlReader.S.NoAddsMode())
        {
            AdvertismentManager.S.ShowInterstitial();
        }
    }
    public void OnRateUsClick()
    {
        DataManager.S.CurrentDataHandler.RateUs();
        _isActiveStageOrSkillPanel = false;
        _nextStagePanel.SetActive(false);
        isRated = true;
    }
    public void ShowNewSkillPanel()
    {
        if (!_isActiveStageOrSkillPanel && _lastSkillColor != eChipColors.no)
        {
            _isActiveStageOrSkillPanel = true;
            _newSkillPanel.SetActive(true);
            StartCoroutine(ShowNoThanksBut());
            HideSkillsImgs();
            switch (_lastSkillColor)
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
            _lastSkillColor = eChipColors.no;
        }
    }
    public void ShowAdd()
    {
        if (!XmlReader.S.NoAddsMode())
        {
            AdvertismentManager.S.ShowRevarded(() =>
            {
                IsLoadingNoThank = false;
                if (_lastSkillColor != eChipColors.no)
                    EventAggregator.FullSkill.Invoke(_lastSkillColor);
                _lastSkillColor = eChipColors.no;
                HideAllPanels();
            }
            );
        }
        else
        {
            IsLoadingNoThank = false;
            if (_lastSkillColor != eChipColors.no)
                EventAggregator.FullSkill.Invoke(_lastSkillColor);
            _lastSkillColor = eChipColors.no;
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
        if (XmlReader.S.GetCurrentScore() > XmlReader.S.GetMaxScore())
        {
            foreach (GameObject go in _lastScoreObjs)
                go.SetActive(false);
            _newBestScoreObj.SetActive(true);
            XMLSaver.S.SetMaxScore(XmlReader.S.GetCurrentScore());
        }
        else
        {
            foreach (GameObject go in _lastScoreObjs)
                go.SetActive(true);
            _newBestScoreObj.SetActive(false);
            _lastScoreText.text = XmlReader.S.GetMaxScore().ToString();
        }
        _currentScoreText.text = XmlReader.S.GetCurrentScore().ToString();
        //if (ScoreController.S.GetScore() >= 250)
        //RManager.S.ShowRewie();
    }
    public void ShowSettingsPanel()
    {
        _settingsPanel.SetActive(true);
        //_logo.SetActive(false);
    }
    public void HideSettingsPanel()
    {
        _settingsPanel.SetActive(false);
        //if (_menuPanel.activeSelf)
        //    _logo.SetActive(true);
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
    public void RestartLevelWithSaveScore()
    {
        if (XmlReader.S.GetCurrentScore() > XmlReader.S.GetMaxScore())        
            XMLSaver.S.SetMaxScore(XmlReader.S.GetCurrentScore());        
        HideAllPanels();
        XMLSaver.S.UnnullTable();
        GameManager.S.RestartLevel();
    }
    public void ShowMenuPanel()
    {
        HideAllPanels();
        _menuPanel.SetActive(true);
    }
    public void ShowRulesPanel()
    {
        _rulesPanel.SetActive(true);
        _rulesPanel.GetComponent<ManualController>().StartPage();
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
        _lastSkillColor = eChipColors.no;
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
            EventAggregator.SetMusicVolume.Invoke(1);
        }
        else
        {
            _musicCheckBoxImg.SetActive(false);
            EventAggregator.SetMusicVolume.Invoke(0);
        }
        if (XmlReader.S.GetSoundsOn())
        {
            _soundsCheckBoxImg.SetActive(true);
            EventAggregator.SetSoundsVolume.Invoke(1);
        }
        else
        {
            _soundsCheckBoxImg.SetActive(false);
            EventAggregator.SetSoundsVolume.Invoke(0);
        }
    }
    public void Play()
    {
        HideAllPanels();
        TryShowRules();
    }
}
