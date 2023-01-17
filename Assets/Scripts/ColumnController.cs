using UnityEngine;

public class ColumnController : MonoBehaviour
{
    private bool isMovingUp = false;
    private bool isMovingDown = false;
    public bool isUp = false;
    [SerializeField] private float moveSpeed = 0.01f;

    private void FixedUpdate()
    {
        if(isMovingDown)
        {
            if (transform.position.y > -1)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, -1, transform.position.z), moveSpeed);
            }
            else
            {
                isUp = false;
                isMovingDown = false;
            }
        }
        if (isMovingUp)
        {
            if (transform.position.y < 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 0, transform.position.z), moveSpeed);
            }
            else
            {
                isUp = true;
                isMovingUp = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isMovingUp)
        {
            if (other.gameObject.layer == 3)
            {
                EffectsController.S.ShowHitEffect(other.transform.position, other.GetComponent<ChipController>().chipColor);
                Destroy(other.gameObject);
            }
        }
    }

    public void MoveUp()
    {
        if(transform.position.y != 0)
            isMovingUp = true;
        if (isMovingDown) //мы сначала даем сигнал всем колоннам опускаться, так что эта проверка необходима
            isMovingDown = false;
        gameObject.layer = 6; //превращаем колонны в стенки
    }

    public void MoveDown()
    {
        if (transform.position.y != -1)
            isMovingDown = true;
        gameObject.layer = 0;
    }

    public void SetUp()
    {
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        gameObject.layer = 6;
        isUp = true;
    }
}
