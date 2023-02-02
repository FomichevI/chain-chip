using UnityEngine;

public class FrostSkill : SkillChip
{
    public override void UseSkillOnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer == 3)
        {
            EventAggregator.ChipUnification.Invoke(col.gameObject.GetComponent<Chip>().СhipValue * 2, col.transform.position, col.gameObject.GetComponent<Chip>().СhipColor);
            EventAggregator.UnificationSound.Invoke();
            EventAggregator.FullSkill.Invoke(col.gameObject.GetComponent<Chip>().СhipColor);
            EventAggregator.DestroyFrost.Invoke(transform.position);
            col.gameObject.GetComponent<Chip>().DestroyGO();
            Destroy(gameObject);
        }
    }
}
