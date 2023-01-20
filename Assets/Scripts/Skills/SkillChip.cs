using UnityEngine;

public class SkillChip : MonoBehaviour
{
    public bool OnTable = false;
    public LayerMask ChipsLayer;
    public eChipColors Color;

    protected Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (rb.velocity.z < 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z * 0.93f); //замедляем, чтобы фишки не вылетали с поля слишком легко
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        UseSkillOnTriggerEnter(other);
    }
    private void OnCollisionEnter(Collision collision)
    {
        UseSkillOnCollisionEnter(collision);
    }


    public virtual void UseSkillOnCollisionEnter(Collision col)
    {
    }
    public virtual void UseSkillOnTriggerEnter(Collider col)
    {
    }
}
