using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.LightningBolt;

public class LightningSkill : SkillChip
{
    [SerializeField] private int _targetsCount = 5;
    [SerializeField] private LightningBoltScript _lightningScript;
    [SerializeField] private GameObject _lightningEffect;
    private List<Chip> _targets;
    private MeshRenderer _meshRenderer;
    private bool _isUsed = false;
    private int _collTimes = 0;

    private void Start()
    {
        _targets = new List<Chip>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public override void UseSkillOnCollisionEnter(Collision col)
    {
        if (!_isUsed)
        {
            if (col.gameObject.layer == 3) //при столкновении с фишкой запускаем цепную молнию
            {
                _isUsed = true;
                Vector3 currentChipPos = transform.position;
                int currentTargetCount = 1;

                while (currentChipPos != Vector3.zero && currentTargetCount < _targetsCount)
                {
                    currentChipPos = FindNextChip(currentChipPos);
                    currentTargetCount++;
                }
                DestroyTargets();
            }
            else if (col.gameObject.layer == 6) //при столкновении со стеной уничтожаем
            {
                Destroy(gameObject);
            }
        }
    }

    private Vector3 FindNextChip(Vector3 currentChipPos) //находим ближайшую фишку в пределах радиуса
    {
        float maxDistance = 3;
        int nearestChip = -1;
        List<GameObject> chipsOnTable = GameManager.S.ChipsOnTable;
        for (int i = 0; i < chipsOnTable.Count; i++)
        {
            if ((currentChipPos - chipsOnTable[i].transform.position).magnitude < maxDistance)
            {
                if (!_targets.Contains(chipsOnTable[i].GetComponent<Chip>()))
                {
                    maxDistance = (currentChipPos - chipsOnTable[i].transform.position).magnitude;
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
            _targets.Add(chipsOnTable[nearestChip].GetComponent<Chip>());
            return chipsOnTable[nearestChip].transform.position;
        }
    }


    private void DestroyTargets() //уничтожаем всю цепочку найденых фишек
    {
        _lightningScript.StartPosition = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);
        if (_targets.Count > 0)
            _lightningScript.EndPosition = new Vector3(_targets[0].gameObject.transform.position.x, _targets[0].gameObject.transform.position.y + 0.25f, _targets[0].gameObject.transform.position.z);
        StartCoroutine(DestroyAllTargest());
        EventAggregator.UseLightning.Invoke();
    }

    IEnumerator DestroyAllTargest()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            _collTimes += 1;
            if (_collTimes > 6)
                Destroy(gameObject); // на случай непредвиденного бага

            if (_targets.Count > 0)
            {
                if (_meshRenderer.enabled == true)
                {
                    _meshRenderer.enabled = false;
                    _lightningEffect.SetActive(false);

                    if (_targets[0] != null)                    
                        _lightningScript.StartPosition = _targets[0].gameObject.transform.position;                    
                    else                    
                        Destroy(gameObject);
                    
                    if (_targets.Count > 1)
                    {
                        if (_targets[1] == null)
                            Destroy(gameObject);
                        _lightningScript.EndPosition = _targets[1].gameObject.transform.position;
                    }                     
                }
                else
                {
                    if (_targets[0] != null) //одна из целей может удалиться раньше, чем до нее дойдет цепная молния, в этом случае нужна проверка
                    {
                        EventAggregator.ChipUnification.Invoke(_targets[0].СhipValue, _targets[0].transform.position, _targets[0].СhipColor);
                        _targets[0].DestroyGO();
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                    _targets.Remove(_targets[0]);

                    if (_targets.Count != 0)
                    {
                        _lightningScript.StartPosition = new Vector3(_targets[0].gameObject.transform.position.x, _targets[0].gameObject.transform.position.y + 0.25f, _targets[0].gameObject.transform.position.z);
                        if (_targets.Count > 1)
                        {
                            _lightningScript.EndPosition = new Vector3(_targets[1].gameObject.transform.position.x, _targets[1].gameObject.transform.position.y + 0.25f, _targets[1].gameObject.transform.position.z);
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
