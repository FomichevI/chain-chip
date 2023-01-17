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
        nextStageText.text = "����� ������!";
        yourRewardText.text = "�������:";
        noThanksText.text = "���, �������";
        congratulationText.text = "�����������!";
        yourScoreText.text = "��� ����:";
        yourBestScoreText.text = "��� ������ ����:";
        newBestScoreText.text = "����� ������!";
        ratingText.text = "�������";
        tryAgainText.text = "��� ���";
        menuText.text = "����";
        restartText.text = "����������";
        musicText.text = "������";
        soundsText.text = "�����";
        wantToRestartText.text = "������ ������ �������?";
        yesText.text = "��";
        noText.text = "���";
        rulesPage1Text.text = "����������� ����� � ����������� ������ � ���������, ����� �������� ���� � �������� ��������������� �����-�����";
        rulesPage2Text.text = "��������������� �����-����� ���������� ��������� ����� � ����������� ������ ��������";
        rulesPage3Text.text = "������� �����-����� ���������� ���� ����� �� ���� � ��������� �������� ��������������� �� �����-�����";
        rulesPage4Text.text = "������������� �����-����� ���������� ��������� ����� �� ����";
        rulesPage5Text.text = "�������� �����-����� ���������� ��� ����� � ������� ������ ������";
        rulesPage6Text.text = "�� �������� �� ���� ���������� �������, ����������� ����";
        praise1Text.text = "��������� � ��� �� ����!";
        praise2Text.text = "������� ���������!";
        praise3Text.text = "�������!";
        praise4Text.text = "�������!";
    }
    private void SetEnglish()
    {

    }
}
