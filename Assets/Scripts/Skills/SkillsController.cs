using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillsController : MonoBehaviour
{
    public static SkillsController S;

    [SerializeField] private Image fireFillingImg;
    [SerializeField] private Image frostFillingImg;
    [SerializeField] private Image lightningFillingImg;
    [SerializeField] private Image cristalFillingImg;

    [SerializeField] private TextMeshProUGUI fireSkillCountText;
    [SerializeField] private TextMeshProUGUI frostSkillCountText;
    [SerializeField] private TextMeshProUGUI lightningSkillCountText;
    [SerializeField] private TextMeshProUGUI cristalSkillCountText;

    [SerializeField] private Button fireSkillBtn;
    [SerializeField] private Button frostSkillBtn;
    [SerializeField] private Button lightningSkillBtn;
    [SerializeField] private Button cristalSkillBtn;

    [SerializeField] private GameObject fireSkillEffect;
    [SerializeField] private GameObject frostSkillEffect;
    [SerializeField] private GameObject lightningSkillEffect;
    [SerializeField] private GameObject cristalSkillEffect;

    public int maxFillingSkills = 25;
    private float currentFillingFireSkill = 0;
    private float currentFillingFrostSkill = 0;
    private float currentFillingLightningSkill = 0;
    private float currentFillingCristalSkill = 0;

    private int currentFireSkillCount = 0;
    private int currentFrostSkillCount = 0;
    private int currentCristalSkillCount = 0;
    private int currentLightningSkillCount = 0;

    void Awake()
    {
        if (S == null)
            S = this;
    }

    public void RaiseSkillFilling(int value, eChipColors type)
    {
        switch (type)
        {
            case eChipColors.blue:
                currentFillingFrostSkill += value;
                if (currentFillingFrostSkill >= maxFillingSkills)
                {
                    currentFrostSkillCount++;

                    frostSkillCountText.text = currentFrostSkillCount.ToString();

                    currentFillingFrostSkill -= maxFillingSkills;
                    frostSkillBtn.interactable = true;
                    frostSkillEffect.SetActive(true);
                    Invoke("UnactiveFrostEffect", 1f);
                    AudioManager.S.PlayFilling();
                    MenuManager.S.lastSkillColor = type;
                }
                frostFillingImg.fillAmount = currentFillingFrostSkill / maxFillingSkills;
                break;
            case eChipColors.green:
                currentFillingCristalSkill += value;
                if (currentFillingCristalSkill >= maxFillingSkills)
                {
                    currentCristalSkillCount++;

                    cristalSkillCountText.text = currentCristalSkillCount.ToString();

                    currentFillingCristalSkill -= maxFillingSkills;
                    cristalSkillBtn.interactable = true;
                    cristalSkillEffect.SetActive(true);
                    Invoke("UnactiveCristalEffect", 1f);
                    AudioManager.S.PlayFilling();
                    MenuManager.S.lastSkillColor = type;
                }
                cristalFillingImg.fillAmount = currentFillingCristalSkill / maxFillingSkills;
                break;
            case eChipColors.red:
                currentFillingFireSkill += value;
                if (currentFillingFireSkill >= maxFillingSkills)
                {
                    currentFireSkillCount++;

                    fireSkillCountText.text = currentFireSkillCount.ToString();

                    currentFillingFireSkill -= maxFillingSkills;
                    fireSkillBtn.interactable = true;
                    fireSkillEffect.SetActive(true);
                    Invoke("UnactiveFireEffect", 1f);
                    AudioManager.S.PlayFilling();
                    MenuManager.S.lastSkillColor = type;
                }
                fireFillingImg.fillAmount = currentFillingFireSkill / maxFillingSkills;
                break;
            case eChipColors.purple:
                currentFillingLightningSkill += value;
                if (currentFillingLightningSkill >= maxFillingSkills)
                {
                    currentLightningSkillCount++;

                    lightningSkillCountText.text = currentLightningSkillCount.ToString();

                    currentFillingLightningSkill -= maxFillingSkills;
                    lightningSkillBtn.interactable = true;
                    lightningSkillEffect.SetActive(true);
                    Invoke("UnactiveLightningEffect", 1f);
                    AudioManager.S.PlayFilling();
                    MenuManager.S.lastSkillColor = type;
                }
                lightningFillingImg.fillAmount = currentFillingLightningSkill / maxFillingSkills;
                break;
        }
    }

    public void SetSkillFilling(int count, int value, eChipColors type)
    {
        switch (type)
        {
            case eChipColors.blue:
                currentFillingFrostSkill = value;
                if (count > 0)
                {
                    currentFrostSkillCount = count;
                    frostSkillBtn.interactable = true;
                }
                frostSkillCountText.text = currentFrostSkillCount.ToString();
                frostFillingImg.fillAmount = currentFillingFrostSkill / maxFillingSkills;
                break;
            case eChipColors.green:
                currentFillingCristalSkill = value;
                if (count > 0)
                {
                    currentCristalSkillCount = count;
                    cristalSkillBtn.interactable = true;
                }
                cristalSkillCountText.text = currentCristalSkillCount.ToString();
                cristalFillingImg.fillAmount = currentFillingCristalSkill / maxFillingSkills;
                break;
            case eChipColors.red:
                currentFillingFireSkill = value;
                if (count > 0)
                {
                    currentFireSkillCount = count;
                    fireSkillBtn.interactable = true;
                }
                fireSkillCountText.text = currentFireSkillCount.ToString();
                fireFillingImg.fillAmount = currentFillingFireSkill / maxFillingSkills;
                break;
            case eChipColors.purple:
                currentFillingLightningSkill = value;
                if (count > 0)
                {
                    currentLightningSkillCount = count;
                    lightningSkillBtn.interactable = true;
                }
                lightningSkillCountText.text = currentLightningSkillCount.ToString();
                lightningFillingImg.fillAmount = currentFillingLightningSkill / maxFillingSkills;
                break;
        }
    }

    public void UseFireSkill()
    {
        if (currentFireSkillCount > 0)
        {
            currentFireSkillCount -= 1;
            fireSkillCountText.text = currentFireSkillCount.ToString();
            GameManager.S.SetSkillChip(eChipColors.red);
            if (currentFireSkillCount == 0)
            {
                fireSkillBtn.interactable = false;
            }
        }
    }
    public void UseFrostSkill()
    {
        if (currentFrostSkillCount > 0)
        {
            currentFrostSkillCount -= 1;
            frostSkillCountText.text = currentFrostSkillCount.ToString();
            GameManager.S.SetSkillChip(eChipColors.blue);
            if (currentFrostSkillCount == 0)
            {
                frostSkillBtn.interactable = false;
            }
        }
    }
    public void UseLightningSkill()
    {
        if (currentLightningSkillCount > 0)
        {
            currentLightningSkillCount -= 1;
            lightningSkillCountText.text = currentLightningSkillCount.ToString();
            GameManager.S.SetSkillChip(eChipColors.purple);
            if (currentLightningSkillCount == 0)
            {
                lightningSkillBtn.interactable = false;
            }
        }
    }
    public void UseCristalSkill()
    {
        if (currentCristalSkillCount > 0)
        {
            currentCristalSkillCount -= 1;
            cristalSkillCountText.text = currentCristalSkillCount.ToString();
            GameManager.S.SetSkillChip(eChipColors.green);
            if (currentCristalSkillCount == 0)
            {
                cristalSkillBtn.interactable = false;
            }
        }
    }
    public float GetFilling(eChipColors color)
    {
        if (color == eChipColors.green)
            return currentFillingCristalSkill;
        else if (color == eChipColors.red)
            return currentFillingFireSkill;
        else if (color == eChipColors.blue)
            return currentFillingFrostSkill;
        else
            return currentFillingLightningSkill;
    }
    public int GetCount(eChipColors color)
    {
        if (color == eChipColors.green)
            return currentCristalSkillCount;
        else if (color == eChipColors.red)
            return currentFireSkillCount;
        else if (color == eChipColors.blue)
            return currentFrostSkillCount;
        else
            return currentLightningSkillCount;
    }
    public void UnactiveFireEffect()
    {
        fireSkillEffect.SetActive(false);
    }
    public void UnactiveFrostEffect()
    {
        frostSkillEffect.SetActive(false);
    }
    public void UnactiveCristalEffect()
    {
        cristalSkillEffect.SetActive(false);
    }
    public void UnactiveLightningEffect()
    {
        lightningSkillEffect.SetActive(false);
    }
}
