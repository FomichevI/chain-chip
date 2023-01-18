using UnityEngine;

public class Column : MonoBehaviour
{
    public bool IsUp = false;
    [SerializeField] private float _moveSpeed = 0.05f;
    private bool _isMovingUp = false;
    private bool _isMovingDown = false;

    private void FixedUpdate()
    {
        if (_isMovingDown)
        {
            if (transform.position.y > -1)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, -1, transform.position.z), _moveSpeed);
            }
            else
            {
                IsUp = false;
                _isMovingDown = false;
            }
        }
        if (_isMovingUp)
        {
            if (transform.position.y < 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 0, transform.position.z), _moveSpeed);
            }
            else
            {
                IsUp = true;
                _isMovingUp = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_isMovingUp)
        {
            if (other.gameObject.layer == 3)
            {
                EffectsController.S.ShowHitEffect(other.transform.position, other.GetComponent<Chip>().�hipColor);
                Destroy(other.gameObject);
            }
        }
    }

    public void MoveUp()
    {
        if (transform.position.y != 0)
            _isMovingUp = true;
        if (_isMovingDown) //�� ������� ���� ������ ���� �������� ����������, ��� ��� ��� �������� ����������
            _isMovingDown = false;
        gameObject.layer = 6; //���������� ������� � ������
    }

    public void MoveDown()
    {
        if (transform.position.y != -1)
            _isMovingDown = true;
        gameObject.layer = 0; //���������� ������� � ������� ������
    }

    public void SetUp() //���������� ����� � �������� ���������
    {
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        gameObject.layer = 6;
        IsUp = true;
    }
}
