using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LanguageChanger : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nextStageText;
    [SerializeField] private TextMeshProUGUI yourRewardText;
    [SerializeField] private TextMeshProUGUI noThanksText;
    [SerializeField] private TextMeshProUGUI congratulationText;
    [SerializeField] private TextMeshProUGUI yourScoreText;
    [SerializeField] private TextMeshProUGUI yourBestScoreText;
    [SerializeField] private TextMeshProUGUI newBestScoreText;
    [SerializeField] private TextMeshProUGUI ratingText;
    [SerializeField] private TextMeshProUGUI tryAgainText;
    [SerializeField] private TextMeshProUGUI menuText;
    [SerializeField] private TextMeshProUGUI restartText;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private TextMeshProUGUI soundsText;
    [SerializeField] private TextMeshProUGUI wantToRestartText;
    [SerializeField] private TextMeshProUGUI yesText;
    [SerializeField] private TextMeshProUGUI noText;
    [SerializeField] private TextMeshProUGUI rulesPage1Text;
    [SerializeField] private TextMeshProUGUI rulesPage2Text;
    [SerializeField] private TextMeshProUGUI rulesPage3Text;
    [SerializeField] private TextMeshProUGUI rulesPage4Text;
    [SerializeField] private TextMeshProUGUI rulesPage5Text;
    [SerializeField] private TextMeshProUGUI rulesPage6Text;

    [SerializeField] private TextMeshProUGUI praise1Text;
    [SerializeField] private TextMeshProUGUI praise2Text;
    [SerializeField] private TextMeshProUGUI praise3Text;
    [SerializeField] private TextMeshProUGUI praise4Text;

    void Awake()
    {
        if (Application.systemLanguage == SystemLanguage.Russian || Application.systemLanguage == SystemLanguage.Ukrainian || Application.systemLanguage == SystemLanguage.Belarusian)
            SetRussian();
    }

    private void SetRussian()
    {
        nextStageText.text = "Новая стадия!";
        yourRewardText.text = "Награда:";
        noThanksText.text = "Нет, спасибо";
        congratulationText.text = "Поздравляем!";
        yourScoreText.text = "Ваш счет:";
        yourBestScoreText.text = "Ваш лучший счет:";
        newBestScoreText.text = "Новый рекорд!";
        ratingText.text = "Рейтинг";
        tryAgainText.text = "Еще раз";
        menuText.text = "Меню";
        restartText.text = "Переиграть";
        musicText.text = "Музыка";
        soundsText.text = "Звуки";
        wantToRestartText.text = "Хотите начать сначала?";
        yesText.text = "Да";
        noText.text = "Нет";
        rulesPage1Text.text = "Объединяйте фишки с одинаковыми цветом и значением, чтобы получать очки и заряжать соответствующие супер-фишки";
        rulesPage2Text.text = "Кристаллическая супер-фишка уничтожает несколько фишек в направлении своего движения";
        rulesPage3Text.text = "Ледяная супер-фишка уничтожает одну фишку на поле и полностью заряжает соответствующую ей супер-фишку";
        rulesPage4Text.text = "Электрическая супер-фишка уничтожает несколько фишек по цепи";
        rulesPage5Text.text = "Огненная супер-фишка уничтожает все фишки в области своего взрыва";
        rulesPage6Text.text = "Со временем на поле появляются колонны, усложняющие игру";
        praise1Text.text = "Продолжай в том же духе!";
        praise2Text.text = "Хороший результат!";
        praise3Text.text = "Отлично!";
        praise4Text.text = "Молодец!";
    }
    private void SetEnglish()
    {

    }
}
