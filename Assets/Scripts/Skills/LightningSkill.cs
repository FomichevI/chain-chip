using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.LightningBolt;

public class LightningSkill : SkillChip
{
    private List<ChipController> targets;
    [SerializeField] private int targetsCount = 4;
    [SerializeField] private LightningBoltScript lightningScript;
    private MeshRenderer meshRenderer;
    private bool isUsed = false;
    private int collTimes = 0;

    [SerializeField] private GameObject lightningEffect;

    private void Start()
    {
        targets = new List<ChipController>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public override void UseSkillOnCollisionEnter(Collision col)
    {
        if (!isUsed)
        {
            if (col.gameObject.layer == 3)
            {
                isUsed = true;

                Vector3 currentChipPos = transform.position;
                int currentTargetCount = 1;

                while (currentChipPos != Vector3.zero && currentTargetCount < targetsCount)
                {
                    currentChipPos = FindNextChip(currentChipPos);
                    currentTargetCount++;
                }
                //Debug.Log(targets.Count);
                DestroyTargets();
            }
            else if (col.gameObject.layer == 6)
            {
                Destroy(gameObject);
            }
        }
    }

    private Vector3 FindNextChip(Vector3 currentChipPos) //находим ближайшую фишку в пределах радиуса
    {
        float maxDistance = 3;
        int nearestChip = -1;

        for(int i = 0; i < GameManager.S.chipsOnTable.Count; i++)
        {
            if ((currentChipPos - GameManager.S.chipsOnTable[i].transform.position).magnitude < maxDistance)
            {
                if (!targets.Contains(GameManager.S.chipsOnTable[i].GetComponent<ChipController>()))
                {
                    maxDistance = (currentChipPos - GameManager.S.chipsOnTable[i].transform.position).magnitude;
                    nearestChip = i;
                }
            }
        }

        if (nearestChip == -1)
        {
            return Vector3.zero;
        }
        else
        {
            targets.Add(GameManager.S.chipsOnTable[nearestChip].GetComponent<ChipController>());
            return GameManager.S.chipsOnTable[nearestChip].transform.position;
        }
    }


    private void DestroyTargets() //уничтожаем всю цепочку найденых фишек
    {
        lightningScript.StartPosition = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);
        if (targets.Count > 0)
            lightningScript.EndPosition = new Vector3(targets[0].gameObject.transform.position.x, targets[0].gameObject.transform.position.y + 0.25f, targets[0].gameObject.transform.position.z);
        StartCoroutine(DestroyAllTargest());
        AudioManager.S.PlayLightningHit();
    }

    IEnumerator DestroyAllTargest()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            collTimes += 1;
            if (collTimes > 6)
                Destroy(gameObject); // на случай непредвиденного бага

            if (targets.Count > 0)
            {
                if (meshRenderer.enabled == true)
                {
                    meshRenderer.enabled = false;
                    lightningEffect.SetActive(false);

                    if (targets[0] != null)                    
                        lightningScript.StartPosition = targets[0].gameObject.transform.position;                    
                    else                    
                        Destroy(gameObject);
                    
                    if (targets.Count > 1)
                    {
                        if (targets[1] == null)
                            Destroy(gameObject);
                        lightningScript.EndPosition = targets[1].gameObject.transform.position;
                    }                     
                }
                else
                {
                    if (targets[0] != null) //одна из целей может удалиться раньше, чем до нее дойдет цепная молния, в этом случае нужна проверка
                    {
                        ScoreController.S.RaiseScore(targets[0].chipValue, targets[0].transform.position, targets[0].chipColor);
                        targets[0].DestroyGO();
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                    targets.Remove(targets[0]);

                    if (targets.Count != 0)
                    {
                        lightningScript.StartPosition = new Vector3(targets[0].gameObject.transform.position.x, targets[0].gameObject.transform.position.y + 0.25f, targets[0].gameObject.transform.position.z);
                        if (targets.Count > 1)
                        {
                            lightningScript.EndPosition = new Vector3(targets[1].gameObject.transform.position.x, targets[1].gameObject.transform.position.y + 0.25f, targets[1].gameObject.transform.position.z);
                        }
                    }
                }
            }
            else
            {
                StopCoroutine(DestroyAllTargest());
                Destroy(gameObject);
            }
        }        
    }
    private void OnDestroy()
    {
        StopCoroutine(DestroyAllTargest());
    }

}
