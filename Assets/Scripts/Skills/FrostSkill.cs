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
                ScoreController.S.RaiseScore(col.gameObject.GetComponent<Chip>().ÑhipValue * 2, col.transform.position, col.gameObject.GetComponent<Chip>().ÑhipColor);
                SkillsController.S.RaiseSkillFilling(SkillsController.S.maxFillingSkills, col.gameObject.GetComponent<Chip>().ÑhipColor);
                EffectsController.S.ShowFrostEffect(transform.position);
                col.gameObject.GetComponent<Chip>().DestroyGO();
                Destroy(gameObject);
                AudioManager.S.PlayUnification();
            }
        }
    }
}
