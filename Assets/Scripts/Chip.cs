using UnityEngine;

public enum eChipColors { green, red, blue, purple, no }

public class Chip : MonoBehaviour
{
    public bool IsOnTable = false;

    private Rigidbody _rb;
    private eChipColors _chipColor = eChipColors.green; public eChipColors �hipColor { get { return _chipColor; } }
    private int _chipValue = 1; public int �hipValue { get { return _chipValue; } }
    private float _timeUntilGameOver = 1f; //�����, ������� ����� ������ �������� �� ��������� ���� ��� ��������� ����
    private bool _isUsed = false;


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void SetColorAndValue()
    {
        _chipColor = GetRandomEnum<eChipColors>();
        switch (_chipColor)
        {
            case eChipColors.green:
                GetComponent<MeshRenderer>().material = Resources.Load<Material>("3DModels/Chips/Materials/ChipsMat_Green");
                break;
            case eChipColors.blue:
                GetComponent<MeshRenderer>().material = Resources.Load<Material>("3DModels/Chips/Materials/ChipsMat_Blue");
                break;
            case eChipColors.red:
                GetComponent<MeshRenderer>().material = Resources.Load<Material>("3DModels/Chips/Materials/ChipsMat_Red");
                break;
            case eChipColors.purple:
                GetComponent<MeshRenderer>().material = Resources.Load<Material>("3DModels/Chips/Materials/ChipsMat_Purple");
                break;
        }
        SetValue(Random.Range(1, 3));
    }

    public void SetColorAndValue(int value, eChipColors color)
    {
        _chipColor = color;
        _chipValue = value;
        switch (_chipColor)
        {
            case eChipColors.green:
                GetComponent<MeshRenderer>().material = Resources.Load<Material>("3DModels/Chips/Materials/ChipsMat_Green");
                break;
            case eChipColors.blue:
                GetComponent<MeshRenderer>().material = Resources.Load<Material>("3DModels/Chips/Materials/ChipsMat_Blue");
                break;
            case eChipColors.red:
                GetComponent<MeshRenderer>().material = Resources.Load<Material>("3DModels/Chips/Materials/ChipsMat_Red");
                break;
            case eChipColors.purple:
                GetComponent<MeshRenderer>().material = Resources.Load<Material>("3DModels/Chips/Materials/ChipsMat_Purple");
                break;
        }
        GetComponent<MeshFilter>().mesh = Resources.Load<Mesh>("3DModels/Chips/Chip_" + _chipValue);
    }

    private void FixedUpdate()
    {
        if (_rb.velocity.z < 0)
        {
            float multiplay = 0.87f + transform.position.z / 100; //��� ����� � �������, ��� ������� ����������
            _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y, _rb.velocity.z * multiplay); // ���������, ����� ����� �� �������� � ���� ������� �����
        }
    }

    private static T GetRandomEnum<T>() //���������� ��������� ���� ������������� Enum
    {
        System.Array A = System.Enum.GetValues(typeof(T));
        T V = (T)A.GetValue(Random.Range(0, A.Length - 1));
        return V;
    }

    private void SetValue(int num)
    {
        _chipValue = num;
        GetComponent<MeshFilter>().mesh = Resources.Load<Mesh>("3DModels/Chips/Chip_" + num);
    }

    public void RaiseValue()
    {
        SetValue(_chipValue + 1);
    }

    private void OnTriggerStay(Collider other) //������������ ����� ����� �� ������� �������� ����
    {
        if (IsOnTable && other.gameObject.tag == "EndZone" && _timeUntilGameOver > 0)
            _timeUntilGameOver -= Time.deltaTime;
        if (_timeUntilGameOver <= 0 && !GameManager.S.IsFailing)
        {
            MenuManager.S.ShowLosePanel();
            GameManager.S.IsFailing = true;
            XMLSaver.S.UnnullTable();
        }
    }

    private void OnCollisionStay(Collision collision) { TouchWithCollision(collision); }
    private void OnCollisionEnter(Collision collision) { TouchWithCollision(collision); }
    private void TouchWithCollision(Collision coll)
    {
        if (coll.gameObject.layer == 3) //������������ ������������ � ������� �������
        {
            Chip targetCC = coll.gameObject.GetComponent<Chip>();
            if (targetCC.�hipColor == �hipColor && targetCC.�hipValue == �hipValue)
            {
                if (_rb.velocity.magnitude >= targetCC._rb.velocity.magnitude) //��������, ����� ����� �������� ������ � ������ �������
                {
                    if (!_isUsed)
                    {
                        _isUsed = true;
                        if (�hipValue == 6)
                        {
                            targetCC.DestroyGO();
                            DestroyGO();
                        }
                        else
                        {
                            targetCC.RaiseValue();
                        }
                        SkillsController.S.RaiseSkillFilling(�hipValue, �hipColor);
                        ScoreController.S.RaiseScore(�hipValue, transform.position, �hipColor);
                        DestroyGO();
                        AudioManager.S.PlayUnification();
                    }
                }
            }
        }
    }

    public void DestroyGO()
    {
        EffectsController.S.ShowHitEffect(transform.position, �hipColor);
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        GameManager.S.ChipsOnTable.Remove(gameObject);
    }
}
