using System.Collections.Generic;
using UnityEngine;

public class FireSkill : SkillChip
{
    private List<GameObject> chipsInRadius;

    private void Start()
    {
        chipsInRadius = new List<GameObject>();
    }

    public override void UseSkillOnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == 3 && !chipsInRadius.Contains(col.gameObject))
        {
            chipsInRadius.Add(col.gameObject);
        }

        base.UseSkillOnTriggerEnter(col);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3 && chipsInRadius.Contains(other.gameObject))
        {
            Debug.Log("при тригере");
            chipsInRadius.Remove(other.gameObject);
        }
    }

    public override void UseSkillOnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer == 3 || col.gameObject.layer == 6)
        {
            //Debug.Log("при колизии");
            foreach (GameObject go in chipsInRadius)
            {
                ChipController goCc = go.GetComponent<ChipController>();
                ScoreController.S.RaiseScore(goCc.chipValue, go.transform.position, goCc.chipColor);
                goCc.DestroyGO();
            }
            EffectsController.S.ShowExplosionEffect(transform.position);
            AudioManager.S.PlayExplosive();
            Destroy(gameObject);
        }

        base.UseSkillOnCollisionEnter(col);
    }
}
