using UnityEngine;

public enum eChipColors { green, red, blue, purple, no}

public class ChipController : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private eChipColors _chipColor = eChipColors.green;
    public eChipColors chipColor { get { return _chipColor; } }

    [SerializeField] private int _chipValue = 1;
    public int chipValue { get { return _chipValue; } }


    public bool onTable = false;
    private float timeUntilGameOver = 1f;
    private bool isUsed = false;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
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
        if (rb.velocity.z < 0)
        {
            float multiplay = 0.87f + transform.position.z/100; //чем ближе к границе, тем сильнее замедление
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z * multiplay); // замедляем, чтобы фишки не вылетали с поля слишком легко
        }

    }

    static T GetRandomEnum<T>()
    {
        System.Array A = System.Enum.GetValues(typeof(T));
        T V = (T)A.GetValue(Random.Range(0, A.Length-1));
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


    private void OnTriggerStay(Collider other)
    {
        if (onTable && other.gameObject.tag == "EndZone" && timeUntilGameOver > 0)
            timeUntilGameOver -= Time.deltaTime;
        if (timeUntilGameOver <= 0 && !GameManager.S.isFailing)
        {
            MenuManager.S.ShowLosePanel();
            GameManager.S.isFailing = true;
            XMLSaver.S.UnnullTable();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == 3) //обрабатываем столкновения с другими фишками
        {
            ChipController targetCC = collision.gameObject.GetComponent<ChipController>();
            if (targetCC.chipColor == chipColor && targetCC.chipValue == chipValue)
            {
                //if (GameManager.S.chipsOnTable.IndexOf(gameObject) > GameManager.S.chipsOnTable.IndexOf(targetCC.gameObject))
                if (rb.velocity.magnitude >= targetCC.rb.velocity.magnitude) //проверка, чтобы фишки отлетали всегда в нужную сторону
                {
                    if (!isUsed)
                    {
                        isUsed = true;
                        if (chipValue == 6)
                        {
                            targetCC.DestroyGO();
                            DestroyGO();
                        }
                        else
                        {
                            //targetCC.GetComponent<Rigidbody>().AddForce((targetCC.gameObject.transform.position - transform.position).normalized * forceOnUnification);
                            targetCC.RaiseValue();
                        }
                        SkillsController.S.RaiseSkillFilling(chipValue, chipColor);
                        ScoreController.S.RaiseScore(chipValue, transform.position, chipColor);
                        DestroyGO();
                        AudioManager.S.PlayUnification();
                    }
                }
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 3) //обрабатываем столкновения с другими фишками
        {
            ChipController targetCC = collision.gameObject.GetComponent<ChipController>();
            if (targetCC.chipColor == chipColor && targetCC.chipValue == chipValue)
            {
                //if (GameManager.S.chipsOnTable.IndexOf(gameObject) > GameManager.S.chipsOnTable.IndexOf(targetCC.gameObject))
                if (rb.velocity.magnitude >= targetCC.rb.velocity.magnitude) //проверка, чтобы фишки отлетали всегда в нужную сторону
                {
                    if (!isUsed)
                    {
                        isUsed = true;
                        if (chipValue == 6)
                        {
                            targetCC.DestroyGO();
                            DestroyGO();
                        }
                        else
                        {
                            //targetCC.GetComponent<Rigidbody>().AddForce((targetCC.gameObject.transform.position - transform.position).normalized * forceOnUnification);
                            targetCC.RaiseValue();
                        }
                        SkillsController.S.RaiseSkillFilling(chipValue, chipColor);
                        ScoreController.S.RaiseScore(chipValue, transform.position, chipColor);
                        DestroyGO();
                        
                        AudioManager.S.PlayUnification();
                    }
                }
            }
        }
    }

    public void DestroyGO()
    {
        EffectsController.S.ShowHitEffect(transform.position, chipColor);
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        GameManager.S.chipsOnTable.Remove(gameObject);
    }
}
