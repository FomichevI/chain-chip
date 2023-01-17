using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullSkill : MonoBehaviour
{
    public eChipColors color;

    void Start()
    {
        SkillsController.S.RaiseSkillFilling(SkillsController.S.maxFillingSkills, color);
    }
}
