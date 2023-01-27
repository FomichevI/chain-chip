using UnityEngine;

public enum eChipColors { green, red, blue, purple, no }

public class Chip : MonoBehaviour
{
    public bool IsOnTable = false;

    private Rigidbody _rb;
    private eChipColors _chipColor = eChipColors.green; public eChipColors СhipColor { get { return _chipColor; } }
    private int _chipValue = 1; public int СhipValue { get { return _chipValue; } }
    private float _timeUntilGameOver = 1f; //время, которая фишка должна провести за пределами поля для окончания игры
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
            float multiplay = 0.87f + transform.position.z / 100; //чем ближе к границе, тем сильнее замедление
            _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y, _rb.velocity.z * multiplay); // замедляем, чтобы фишки не вылетали с поля слишком легко
        }
    }

    private static T GetRandomEnum<T>() //возвращает случайный член перечеслителя Enum
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

    private void OnTriggerStay(Collider other) //обрабатываем выход фишки за пределы игрового поля
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
        if (coll.gameObject.layer == 3) //обрабатываем столкновения с другими фишками
        {
            if (!coll.gameObject.GetComponent<Chip>()) //на случай столкновения с фишкой скилла
                return;
            Chip targetC = coll.gameObject.GetComponent<Chip>();
            if (targetC.СhipColor == СhipColor && targetC.СhipValue == СhipValue)
            {
                if (_rb.velocity.magnitude >= targetC._rb.velocity.magnitude) //проверка, чтобы фишки отлетали всегда в нужную сторону
                {
                    if (!_isUsed)
                    {
                        _isUsed = true;
                        if (СhipValue == 6)
                        {
                            targetC.DestroyGO();
                            DestroyGO();
                        }
                        else
                        {
                            targetC.RaiseValue();
                        }
                        SkillsController.S.IncreaseSkillFilling(СhipValue, СhipColor);
                        ScoreController.S.RaiseScore(СhipValue, transform.position, СhipColor);
                        DestroyGO();
                        AudioManager.S.PlayUnification();
                    }
                }
            }
        }
    }

    public void DestroyGO()
    {        
        EffectsController.S.ShowHitEffect(transform.position, СhipColor);
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        GameManager.S.ChipsOnTable.Remove(gameObject);
    }
}
