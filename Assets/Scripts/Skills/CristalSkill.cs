using UnityEngine;

public class CristalSkill : SkillChip
{
    private int maxTargets = 5;
    private int currentTargets = 0;
    private SphereCollider sphereTrigger;
    private bool isCenterChanged = false;
    private Vector3 permanentVelocity;

    private void Start()
    {
        SphereCollider[] spheres = GetComponents<SphereCollider>();
        foreach (SphereCollider sc in spheres)
            if (sc.isTrigger == true)
                sphereTrigger = sc;
    }

    private void Update()
    {
        if (!isCenterChanged && onTable == true && rb.velocity != Vector3.zero)
        {
            Vector3 direction = rb.velocity.normalized;
            permanentVelocity = rb.velocity * 1;
            sphereTrigger.center = new Vector3(direction.x * 0.5f, sphereTrigger.center.y, direction.z * 0.5f);
            isCenterChanged = true;
        }
        if (isCenterChanged)
            rb.velocity = permanentVelocity;
    }

    public override void UseSkillOnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == 3)
        {
            if (currentTargets < maxTargets)
            {
                ScoreController.S.RaiseScore(col.gameObject.GetComponent<Chip>().ÑhipValue, col.transform.position, col.gameObject.GetComponent<Chip>().ÑhipColor);
                col.gameObject.GetComponent<Chip>().DestroyGO();
                AudioManager.S.PlayUnification();
                currentTargets++;
            }
            else
            {
                EffectsController.S.ShowCristalHitEffect(transform.position);
                AudioManager.S.PlayCristalBreak();
                Destroy(gameObject);
            }
        }
    }

    public override void UseSkillOnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer == 6)
        {
            EffectsController.S.ShowCristalHitEffect(transform.position);
            AudioManager.S.PlayCristalBreak();
            Destroy(gameObject);
        }
    }
}
