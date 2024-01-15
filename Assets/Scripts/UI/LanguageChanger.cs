using UnityEngine;
using TMPro;

public class LanguageChanger : MonoBehaviour
{
    [Header("Next Stage Panel")]
    [SerializeField] private TextMeshProUGUI _nextStageText;
    [SerializeField] private TextMeshProUGUI _praise1Text;
    [SerializeField] private TextMeshProUGUI _praise2Text;
    [SerializeField] private TextMeshProUGUI _praise3Text;
    [SerializeField] private TextMeshProUGUI _praise4Text;
    [SerializeField] private TextMeshProUGUI _rateUsText;
    [Header("New Skill Panel")]
    [SerializeField] private TextMeshProUGUI _yourRewardText;
    [SerializeField] private TextMeshProUGUI _noThanksText;
    [Header("Lose Panel")]
    [SerializeField] private TextMeshProUGUI _congratulationText;
    [SerializeField] private TextMeshProUGUI _yourScoreText;
    [SerializeField] private TextMeshProUGUI _yourBestScoreText;
    [SerializeField] private TextMeshProUGUI _newBestScoreText;
    [SerializeField] private TextMeshProUGUI _ratingText;
    [SerializeField] private TextMeshProUGUI _tryAgainText;
    [Header("Settings Panel")]
    [SerializeField] private TextMeshProUGUI _menuText;
    [SerializeField] private TextMeshProUGUI _restartText;
    [SerializeField] private TextMeshProUGUI _musicText;
    [SerializeField] private TextMeshProUGUI _soundsText;
    [Header("Restart Panel")]
    [SerializeField] private TextMeshProUGUI _wantToRestartText;
    [SerializeField] private TextMeshProUGUI _yesText;
    [SerializeField] private TextMeshProUGUI _noText;
    [Header("Rules Panel")]
    [SerializeField] private TextMeshProUGUI _rulesPage1Text;
    [SerializeField] private TextMeshProUGUI _rulesPage2Text;
    [SerializeField] private TextMeshProUGUI _rulesPage3Text;
    [SerializeField] private TextMeshProUGUI _rulesPage4Text;
    [SerializeField] private TextMeshProUGUI _rulesPage5Text;
    [SerializeField] private TextMeshProUGUI _rulesPage6Text;

    void Awake()
    {
        //if (Application.systemLanguage == SystemLanguage.Russian || Application.systemLanguage == SystemLanguage.Ukrainian || Application.systemLanguage == SystemLanguage.Belarusian)
        //    SetRussian();
        EventAggregator.SetRussianLanguage.AddListener(SetRussian);
    }

    private void OnDisable()
    {
        EventAggregator.SetRussianLanguage.RemoveListener(SetRussian);
    }

    private void SetRussian()
    {
        Debug.Log("Устанавливаем русский язык");

        _nextStageText.text = "Новая стадия!";
        _yourRewardText.text = "Награда:";
        _noThanksText.text = "Нет, спасибо";
        _congratulationText.text = "Поздравляем!";
        _yourScoreText.text = "Ваш счет:";
        _yourBestScoreText.text = "Ваш лучший счет:";
        _newBestScoreText.text = "Новый рекорд!";
        _ratingText.text = "Рейтинг";
        _tryAgainText.text = "Еще раз";
        _menuText.text = "Меню";
        _restartText.text = "Переиграть";
        _musicText.text = "Музыка";
        _soundsText.text = "Звуки";
        _wantToRestartText.text = "Хотите начать сначала?";
        _yesText.text = "Да";
        _noText.text = "Нет";
        _rulesPage1Text.text = "Объединяйте фишки с одинаковыми цветом и значением, чтобы получать очки и заряжать соответствующие умения";
        _rulesPage2Text.text = "Кристаллическое умение уничтожает несколько фишек в направлении своего движения";
        _rulesPage3Text.text = "Ледяное умение уничтожает одну фишку на поле, давая в 3 раза больше очков, и полностью заряжает соответствующее ей умение";
        _rulesPage4Text.text = "Электрическое умение уничтожает несколько фишек по цепи";
        _rulesPage5Text.text = "Огненное умение уничтожает все фишки в области своего взрыва";
        _rulesPage6Text.text = "Со временем на поле появляются колонны, усложняющие игру";
        _praise1Text.text = "Продолжай в том же духе!";
        _praise2Text.text = "Хороший результат!";
        _praise3Text.text = "Отлично!";
        _praise4Text.text = "Молодец!";
        _rateUsText.text = "Оценить";
    }
    private void SetEnglish()
    {

    }
}
