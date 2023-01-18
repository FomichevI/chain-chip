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
                ScoreController.S.RaiseScore(col.gameObject.GetComponent<Chip>().�hipValue * 2, col.transform.position, col.gameObject.GetComponent<Chip>().�hipColor);
                SkillsController.S.RaiseSkillFilling(SkillsController.S.maxFillingSkills, col.gameObject.GetComponent<Chip>().�hipColor);
                EffectsController.S.ShowFrostEffect(transform.position);
                col.gameObject.GetComponent<Chip>().DestroyGO();
                Destroy(gameObject);
                AudioManager.S.PlayUnification();
            }
        }
    }
}
