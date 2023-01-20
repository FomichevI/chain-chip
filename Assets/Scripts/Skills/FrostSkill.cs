using UnityEngine;

public class FrostSkill : SkillChip
{
    public override void UseSkillOnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer == 3)
        {
            ScoreController.S.RaiseScore(col.gameObject.GetComponent<Chip>().СhipValue * 2, col.transform.position, col.gameObject.GetComponent<Chip>().СhipColor);
            SkillsController.S.IncreaseSkillFilling(SkillsController.S.MaxFillingSkills, col.gameObject.GetComponent<Chip>().СhipColor);
            EffectsController.S.ShowFrostEffect(transform.position);
            col.gameObject.GetComponent<Chip>().DestroyGO();
            Destroy(gameObject);
            AudioManager.S.PlayUnification();
        }
    }
}
