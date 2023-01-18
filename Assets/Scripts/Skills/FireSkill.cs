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
            Debug.Log("ïðè òðèãåðå");
            chipsInRadius.Remove(other.gameObject);
        }
    }

    public override void UseSkillOnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer == 3 || col.gameObject.layer == 6)
        {
            //Debug.Log("ïðè êîëèçèè");
            foreach (GameObject go in chipsInRadius)
            {
                Chip goCc = go.GetComponent<Chip>();
                ScoreController.S.RaiseScore(goCc.ÑhipValue, go.transform.position, goCc.ÑhipColor);
                goCc.DestroyGO();
            }
            EffectsController.S.ShowExplosionEffect(transform.position);
            AudioManager.S.PlayExplosive();
            Destroy(gameObject);
        }

        base.UseSkillOnCollisionEnter(col);
    }
}
