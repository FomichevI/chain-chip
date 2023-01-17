using UnityEngine;

public class FrostSkill : SkillChip
{
    private bool isUsed = false;

    public override void UseSkillOnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer == 3)
        {
            if (!isUsed)
            {
                isUsed = true;
                ScoreController.S.RaiseScore(col.gameObject.GetComponent<ChipController>().chipValue * 2, col.transform.position, col.gameObject.GetComponent<ChipController>().chipColor);
                SkillsController.S.RaiseSkillFilling(SkillsController.S.maxFillingSkills, col.gameObject.GetComponent<ChipController>().chipColor);
                EffectsController.S.ShowFrostEffect(transform.position);
                col.gameObject.GetComponent<ChipController>().DestroyGO();
                Destroy(gameObject);
                AudioManager.S.PlayUnification();
            }
        }
    }
}
