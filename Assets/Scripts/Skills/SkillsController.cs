using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillsController : MonoBehaviour
{
    public static SkillsController S;

    public int MaxFillingSkills = 25;
    //image для отображения заполнения скиллов
    [SerializeField] private Image _fireFillingImg;
    [SerializeField] private Image _frostFillingImg;
    [SerializeField] private Image _lightningFillingImg;
    [SerializeField] private Image _cristalFillingImg;
    [SerializeField] private TextMeshProUGUI _fireSkillCountText;
    [SerializeField] private TextMeshProUGUI _frostSkillCountText;
    [SerializeField] private TextMeshProUGUI _lightningSkillCountText;
    [SerializeField] private TextMeshProUGUI _cristalSkillCountText;
    [SerializeField] private Button _fireSkillBtn;
    [SerializeField] private Button _frostSkillBtn;
    [SerializeField] private Button _lightningSkillBtn;
    [SerializeField] private Button _cristalSkillBtn;
    //эффекты, которые включаются при полном заполнении скилла
    [SerializeField] private GameObject _fireSkillEffect;
    [SerializeField] private GameObject _frostSkillEffect;
    [SerializeField] private GameObject _lightningSkillEffect;
    [SerializeField] private GameObject _cristalSkillEffect;

    private float _currentFillingFireSkill = 0;
    private float _currentFillingFrostSkill = 0;
    private float _currentFillingLightningSkill = 0;
    private float _currentFillingCristalSkill = 0;
    private int _currentFireSkillCount = 0;
    private int _currentFrostSkillCount = 0;
    private int _currentCristalSkillCount = 0;
    private int _currentLightningSkillCount = 0;

    void Awake()
    {
        if (S == null)
            S = this;
    }

    public void IncreaseSkillFilling(int value, eChipColors type)
    {
        ref float currentFillingSkill = ref _currentFillingFrostSkill;
        ref int currentSkillCount = ref _currentFrostSkillCount;
        ref Button skillBtn = ref _frostSkillBtn;
        ref GameObject skillEffect = ref _frostSkillEffect;
        ref Image fillingImg = ref _frostFillingImg;
        ref TextMeshProUGUI skillCountText = ref _frostSkillCountText;
        switch (type)
        {
            case eChipColors.green:
                currentFillingSkill = ref _currentFillingCristalSkill;
                currentSkillCount =ref  _currentCristalSkillCount;
                skillBtn =ref  _cristalSkillBtn;
                skillEffect =ref  _cristalSkillEffect;
                fillingImg =ref _cristalFillingImg;
                skillCountText = ref _cristalSkillCountText;
                break;
            case eChipColors.red:
                currentFillingSkill = ref _currentFillingFireSkill;
                currentSkillCount = ref _currentFireSkillCount;
                skillBtn = ref _fireSkillBtn;
                skillEffect = ref _fireSkillEffect;
                fillingImg = ref _fireFillingImg;
                skillCountText = ref _fireSkillCountText;
                break;
            case eChipColors.purple:
                currentFillingSkill = ref _currentFillingLightningSkill;
                currentSkillCount = ref _currentLightningSkillCount;
                skillBtn = ref _lightningSkillBtn;
                skillEffect = ref _lightningSkillEffect;
                fillingImg = ref _lightningFillingImg;
                skillCountText = ref _lightningSkillCountText;
                break;
        }

        currentFillingSkill += value;
        if (currentFillingSkill >= MaxFillingSkills)
        {
            currentSkillCount++;

            skillCountText.text = currentSkillCount.ToString();

            currentFillingSkill -= MaxFillingSkills;
            skillBtn.interactable = true;
            skillEffect.SetActive(true);
            Invoke("UnactiveFrostEffect", 1f);
            AudioManager.S.PlayFilling();
            MenuManager.S.LastSkillColor = type;
        }
        fillingImg.fillAmount = currentFillingSkill / MaxFillingSkills;
    }

    public void SetSkillFilling(int count, int value, eChipColors type)
    {
        ref float currentFillingSkill = ref _currentFillingFrostSkill;
        ref int currentSkillCount = ref _currentFrostSkillCount;
        ref Button skillBtn = ref _frostSkillBtn;
        ref Image fillingImg = ref _frostFillingImg;
        ref TextMeshProUGUI skillCountText = ref _frostSkillCountText;
        switch (type)
        {
            case eChipColors.green:
                currentFillingSkill =ref _currentFillingCristalSkill;
                currentSkillCount = ref _currentCristalSkillCount;
                skillBtn = ref _cristalSkillBtn;
                fillingImg = ref _cristalFillingImg;
                skillCountText = ref _cristalSkillCountText;
                break;
            case eChipColors.red:
                currentFillingSkill = ref _currentFillingFireSkill;
                currentSkillCount = ref _currentFireSkillCount;
                skillBtn = ref _fireSkillBtn;
                fillingImg = ref _fireFillingImg;
                skillCountText = ref _fireSkillCountText;
                break;
            case eChipColors.purple:
                currentFillingSkill = ref _currentFillingLightningSkill;
                currentSkillCount = ref _currentLightningSkillCount;
                skillBtn = ref _lightningSkillBtn;
                fillingImg = ref _lightningFillingImg;
                skillCountText = ref _lightningSkillCountText;
                break;
        }

        currentFillingSkill = value;
        if (count > 0)
        {
            currentSkillCount = count;
            skillBtn.interactable = true;
        }
        skillCountText.text = currentSkillCount.ToString();
        fillingImg.fillAmount = currentFillingSkill / MaxFillingSkills;
    }
    public void UseFireSkill()
    {
        if (_currentFireSkillCount > 0)
        {
            _currentFireSkillCount -= 1;
            _fireSkillCountText.text = _currentFireSkillCount.ToString();
            GameManager.S.SetSkillChip(eChipColors.red);
            if (_currentFireSkillCount == 0)
            {
                _fireSkillBtn.interactable = false;
            }
        }
    }
    public void UseFrostSkill()
    {
        if (_currentFrostSkillCount > 0)
        {
            _currentFrostSkillCount -= 1;
            _frostSkillCountText.text = _currentFrostSkillCount.ToString();
            GameManager.S.SetSkillChip(eChipColors.blue);
            if (_currentFrostSkillCount == 0)
            {
                _frostSkillBtn.interactable = false;
            }
        }
    }
    public void UseLightningSkill()
    {
        if (_currentLightningSkillCount > 0)
        {
            _currentLightningSkillCount -= 1;
            _lightningSkillCountText.text = _currentLightningSkillCount.ToString();
            GameManager.S.SetSkillChip(eChipColors.purple);
            if (_currentLightningSkillCount == 0)
            {
                _lightningSkillBtn.interactable = false;
            }
        }
    }
    public void UseCristalSkill()
    {
        if (_currentCristalSkillCount > 0)
        {
            _currentCristalSkillCount -= 1;
            _cristalSkillCountText.text = _currentCristalSkillCount.ToString();
            GameManager.S.SetSkillChip(eChipColors.green);
            if (_currentCristalSkillCount == 0)
            {
                _cristalSkillBtn.interactable = false;
            }
        }
    }
    public float GetFilling(eChipColors color)
    {
        if (color == eChipColors.green)
            return _currentFillingCristalSkill;
        else if (color == eChipColors.red)
            return _currentFillingFireSkill;
        else if (color == eChipColors.blue)
            return _currentFillingFrostSkill;
        else
            return _currentFillingLightningSkill;
    }
    public int GetCount(eChipColors color)
    {
        if (color == eChipColors.green)
            return _currentCristalSkillCount;
        else if (color == eChipColors.red)
            return _currentFireSkillCount;
        else if (color == eChipColors.blue)
            return _currentFrostSkillCount;
        else
            return _currentLightningSkillCount;
    }
}
