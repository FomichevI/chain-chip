using UnityEngine;

public class CristalSkill : SkillChip
{
    private int _maxTargets = 5;
    private int _currentTargets = 0;
    private SphereCollider _sphereTrigger;
    private bool _isCenterChanged = false;
    private Vector3 _permanentVelocity;

    private void Start()
    {
        SphereCollider[] spheres = GetComponents<SphereCollider>();
        foreach (SphereCollider sc in spheres)
            if (sc.isTrigger == true)
                _sphereTrigger = sc;
    }

    private void Update()
    {
        //фишка этого скилла движется с постоянной скоростью прямолинейно
        if (!_isCenterChanged && OnTable == true && rb.velocity != Vector3.zero) //когда мы только что запустили фишку
        {
            Vector3 direction = rb.velocity.normalized;
            _permanentVelocity = rb.velocity * 1;
            //передвигаем триггер немного вперед по направлению движения фишки, чтобы фишка не тормозила от столкновений
            _sphereTrigger.center = new Vector3(direction.x * 0.5f, _sphereTrigger.center.y, direction.z * 0.5f); 
            _isCenterChanged = true;
        }
        if (_isCenterChanged)
            rb.velocity = _permanentVelocity;
    }

    public override void UseSkillOnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == 3) //если происходит столкновение с фишкой
        {
            if (_currentTargets < _maxTargets)
            {
                EventAggregator.ChipUnification.Invoke(col.gameObject.GetComponent<Chip>().СhipValue, col.transform.position, col.gameObject.GetComponent<Chip>().СhipColor);
                EventAggregator.UnificationSound.Invoke();
                col.gameObject.GetComponent<Chip>().DestroyGO();
                _currentTargets++;
            }
            else
            {
                EventAggregator.DestroyCristal.Invoke(transform.position);
                Destroy(gameObject);
            }
        }
    }

    public override void UseSkillOnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer == 6) //если происходит столкновение со стенкой
        {
            EventAggregator.DestroyCristal.Invoke(transform.position);
            Destroy(gameObject);
        }
    }
}
