using System.Collections.Generic;
using UnityEngine;

public class FireSkill : SkillChip
{
    //этот скилл уничтожает все фишки в радиусе своего действия
    //фишки, которые уничтожатся во время взрыва, записываются в список
    private List<GameObject> _chipsInRadius;

    private void Start()
    {
        _chipsInRadius = new List<GameObject>();
    }

    public override void UseSkillOnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == 3 && !_chipsInRadius.Contains(col.gameObject))
        {
            _chipsInRadius.Add(col.gameObject);
        }
        base.UseSkillOnTriggerEnter(col);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3 && _chipsInRadius.Contains(other.gameObject))        
            _chipsInRadius.Remove(other.gameObject);        
    }

    public override void UseSkillOnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer == 3 || col.gameObject.layer == 6) //взрыв при столкновении с фишкой или стеной
        {
            foreach (GameObject go in _chipsInRadius)
            {
                Chip goC = go.GetComponent<Chip>();
                ScoreController.S.RaiseScore(goC.СhipValue, go.transform.position, goC.СhipColor);
                goC.DestroyGO();
            }
            EffectsController.S.ShowExplosionEffect(transform.position);
            AudioManager.S.PlayExplosive();
            Destroy(gameObject);
        }
        base.UseSkillOnCollisionEnter(col);
    }
}
