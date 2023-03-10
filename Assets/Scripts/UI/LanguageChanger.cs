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
        if (Application.systemLanguage == SystemLanguage.Russian || Application.systemLanguage == SystemLanguage.Ukrainian || Application.systemLanguage == SystemLanguage.Belarusian)
            SetRussian();
    }

    private void SetRussian()
    {
        _nextStageText.text = "?????????? ????????????!";
        _yourRewardText.text = "??????????????:";
        _noThanksText.text = "??????, ??????????????";
        _congratulationText.text = "??????????????????????!";
        _yourScoreText.text = "?????? ????????:";
        _yourBestScoreText.text = "?????? ???????????? ????????:";
        _newBestScoreText.text = "?????????? ????????????!";
        _ratingText.text = "??????????????";
        _tryAgainText.text = "?????? ??????";
        _menuText.text = "????????";
        _restartText.text = "????????????????????";
        _musicText.text = "????????????";
        _soundsText.text = "??????????";
        _wantToRestartText.text = "???????????? ???????????? ???????????????";
        _yesText.text = "????";
        _noText.text = "??????";
        _rulesPage1Text.text = "?????????????????????? ?????????? ?? ?????????????????????? ???????????? ?? ??????????????????, ?????????? ???????????????? ???????? ?? ???????????????? ?????????????????????????????? ??????????-??????????";
        _rulesPage2Text.text = "?????????????????????????????? ??????????-?????????? ???????????????????? ?????????????????? ?????????? ?? ?????????????????????? ???????????? ????????????????";
        _rulesPage3Text.text = "?????????????? ??????????-?????????? ???????????????????? ???????? ?????????? ???? ???????? ?? ?????????????????? ???????????????? ?????????????????????????????? ???? ??????????-??????????";
        _rulesPage4Text.text = "?????????????????????????? ??????????-?????????? ???????????????????? ?????????????????? ?????????? ???? ????????";
        _rulesPage5Text.text = "???????????????? ??????????-?????????? ???????????????????? ?????? ?????????? ?? ?????????????? ???????????? ????????????";
        _rulesPage6Text.text = "???? ???????????????? ???? ???????? ???????????????????? ??????????????, ?????????????????????? ????????";
        _praise1Text.text = "?????????????????? ?? ?????? ???? ????????!";
        _praise2Text.text = "?????????????? ??????????????????!";
        _praise3Text.text = "??????????????!";
        _praise4Text.text = "??????????????!";
    }
    private void SetEnglish()
    {

    }
}
