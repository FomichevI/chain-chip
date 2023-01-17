using System.Collections;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public static MenuManager S;

    [SerializeField] private GameObject nextStagePanel;
    [SerializeField] private GameObject newSkillPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject wantToRestartPanel;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject rulesPanel;
    [SerializeField] private GameObject logo;

    [Header("NewStagePanel Settings")]
    [SerializeField] private GameObject[] praiseTexts;

    [Header("NewSkillPanel Settings")]
    [SerializeField] private GameObject noThanksSkillPanelBut;
    [SerializeField] private GameObject[] fireSkillImgs;
    [SerializeField] private GameObject[] frostSkillImgs;
    [SerializeField] private GameObject[] lightningSkillImgs;
    [SerializeField] private GameObject[] cristalSkillImgs;
    [SerializeField] private GameObject noAdsBut;
    private bool isActiveStageOrSkillPanel;

    [Header("SettingsPanel Settings")]
    [SerializeField] private GameObject musicCheckBoxImg;
    [SerializeField] private GameObject soundsCheckBoxImg;

    [Header("LosePanel Settings")]
    [SerializeField] private GameObject newBestScoreObj;
    [SerializeField] private GameObject[] lastScoreObjs;
    [SerializeField] private TextMeshProUGUI currentScoreText;
    [SerializeField] private TextMeshProUGUI lastScoreText;

    public bool isWorkShowNoThanksCor = false;
    public eChipColors lastSkillColor;


    private void Awake()
    {
        if (S == null)
            S = this;
    }

    private void Start()
    {
        if (!XmlReader.S.NoAddsMode())
            noAdsBut.SetActive(true);
        else
            noAdsBut.SetActive(false);
    }

    public void ShowNextStagePanel()
    {
        if (!isActiveStageOrSkillPanel)
        {
            isActiveStageOrSkillPanel = true;
            nextStagePanel.SetActive(true);
            //включить случайный текст с похвалой
            foreach (GameObject go in praiseTexts)
                go.SetActive(false);
            praiseTexts[Random.Range(0, praiseTexts.Length)].SetActive(true);
        }
    }
    public void HideNextStagePanel()
    {
        isActiveStageOrSkillPanel = false;
        nextStagePanel.SetActive(false);

        if (!XmlReader.S.NoAddsMode())
        {
            UnityAdsManager.S.ShowInterstitial();
            //AdsManager.S.ShowInterstitialAd();
        }
    }
    public void ShowNewSkillPanel()
    {
        if (!isActiveStageOrSkillPanel && lastSkillColor != eChipColors.no)
        {
            isActiveStageOrSkillPanel = true;
            newSkillPanel.SetActive(true);
            StartCoroutine(ShowNoThanksBut());
            HideSkillsImgs();
            switch (lastSkillColor)
            {
                case eChipColors.green:
                    foreach (GameObject go in cristalSkillImgs)
                        go.SetActive(true);
                    break;
                case eChipColors.blue:
                    foreach (GameObject go in frostSkillImgs)
                        go.SetActive(true);
                    break;
                case eChipColors.red:
                    foreach (GameObject go in fireSkillImgs)
                        go.SetActive(true);
                    break;
                case eChipColors.purple:
                    foreach (GameObject go in lightningSkillImgs)
                        go.SetActive(true);
                    break;
            }
        }
        else
        {
            lastSkillColor = eChipColors.no;
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
            isWorkShowNoThanksCor = false;
            SkillsController.S.RaiseSkillFilling(SkillsController.S.maxFillingSkills, MenuManager.S.lastSkillColor);
            lastSkillColor = eChipColors.no;
            HideAllPanels();
        }
    }
    IEnumerator ShowNoThanksBut()
    {
        isWorkShowNoThanksCor = true;
        yield return new WaitForSeconds(2f);
        if (isWorkShowNoThanksCor)
            noThanksSkillPanelBut.SetActive(true);
    }
    public void ShowLosePanel()
    {
        HideAllPanels();
        losePanel.SetActive(true);
        if (ScoreController.S.GetScore() > XmlReader.S.GetMaxScore())
        {
            foreach (GameObject go in lastScoreObjs)
                go.SetActive(false);
            newBestScoreObj.SetActive(true);
            XMLSaver.S.SetMaxScore(ScoreController.S.GetScore());
        }
        else
        {
            foreach (GameObject go in lastScoreObjs)
                go.SetActive(true);
            newBestScoreObj.SetActive(false);
            lastScoreText.text = XmlReader.S.GetMaxScore().ToString();
        }
        currentScoreText.text = ScoreController.S.GetScore().ToString();
        //if (ScoreController.S.GetScore() >= 250)
            //RManager.S.ShowRewie();
    }
    public void ShowSettingsPanel()
    {
        settingsPanel.SetActive(true);
        logo.SetActive(false);
    }
    public void HideSettingsPanel()
    {
        settingsPanel.SetActive(false);
        if (menuPanel.activeSelf)
            logo.SetActive(true);
    }
    public void ShowWantToRestartPanel()
    {
        wantToRestartPanel.SetActive(true);
    }
    public void HideWantToRestartPanel()
    {
        wantToRestartPanel.SetActive(false);
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
        menuPanel.SetActive(true);
        logo.SetActive(true);
    }
    public void ShowRulesPanel()
    {
        rulesPanel.SetActive(true);
        rulesPanel.GetComponent<ManualController>().SetStartPage();
    }
    public void HideAllPanels()
    {
        nextStagePanel.SetActive(false);
        newSkillPanel.SetActive(false);
        losePanel.SetActive(false);
        settingsPanel.SetActive(false);
        wantToRestartPanel.SetActive(false);
        menuPanel.SetActive(false);
        noThanksSkillPanelBut.SetActive(false);
        isWorkShowNoThanksCor = false;
        isActiveStageOrSkillPanel = false;
        lastSkillColor = eChipColors.no;
        logo.SetActive(false);
    }

    private void HideSkillsImgs()
    {
        foreach (GameObject go in fireSkillImgs)
            go.SetActive(false);
        foreach (GameObject go in frostSkillImgs)
            go.SetActive(false);
        foreach (GameObject go in lightningSkillImgs)
            go.SetActive(false);
        foreach (GameObject go in cristalSkillImgs)
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
            musicCheckBoxImg.SetActive(true);
            AudioManager.S.SetMusicVolume(1);
        }
        else
        {
            musicCheckBoxImg.SetActive(false);
            AudioManager.S.SetMusicVolume(0);
        }
        if (XmlReader.S.GetSoundsOn())
        {
            soundsCheckBoxImg.SetActive(true);
            AudioManager.S.SetSoundsVolume(1);
        }
        else
        {
            soundsCheckBoxImg.SetActive(false);
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
