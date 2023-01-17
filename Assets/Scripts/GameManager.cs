using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager S;
    public List<GameObject> chipsOnTable;

    private GameObject currentChip;
    private GameObject chipOnStartPosition;
    private Vector3 startPos;
    private float maxDistance = 1.5f;
    [SerializeField] private float force = 2;

    [SerializeField] private GameObject chipPrefab;
    [SerializeField] private GameObject fireChipPrefab;
    [SerializeField] private GameObject frostChipPrefab;
    [SerializeField] private GameObject lightningChipPrefab;
    [SerializeField] private GameObject cristalChipPrefab;

    [SerializeField] LayerMask chipLayers;
    [SerializeField] LayerMask uiLayer;
    [SerializeField] private float maxAngle = 60;
    [SerializeField] private float minForceMultiplier = 0.5f;

    [SerializeField] private ColumnController[] columnControllers;
    public bool isFailing = false;
    private bool isWorkSpawnNewChipCor = false;

    private void Awake()
    {
        if (S == null)
            S = this;
    }

    private void Start()
    {
        startPos = new Vector3(0, 0.5f, 0);
        MenuManager.S.SetSoundsSettings();

        if (XmlReader.S.HasSave())
        {
            LoadLevel();
        }
        else
        {
            if (XmlReader.S.GetMaxScore() == 0)
                MenuManager.S.ShowMenuPanel();
            GameObject chipGO = Instantiate(chipPrefab, startPos, Quaternion.Euler(Vector3.zero));
            chipGO.GetComponent<ChipController>().SetColorAndValue();
            chipOnStartPosition = chipGO;
            GenerateStartLevel();
        }
    }

    private void GenerateStartLevel()
    {
        AddChip(new Vector3(-1.9f, 0.5f, 9f), 1, eChipColors.red);
        AddChip(new Vector3(-0.7f, 0.5f, 6.5f), 1, eChipColors.purple);
        AddChip(new Vector3(0.7f, 0.5f, 9f), 1, eChipColors.green);
        AddChip(new Vector3(1.9f, 0.5f, 6.5f), 1, eChipColors.blue);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 50, chipLayers) && hit.transform != null)
            {
                if (EventSystem.current.IsPointerOverGameObject())
                    return;
                if (Input.touchCount > 0)
                    if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                    return;
                if (hit.transform.GetComponent<ChipController>())
                {
                    if (!hit.transform.GetComponent<ChipController>().onTable)
                        currentChip = hit.transform.gameObject;
                }

                else if (hit.transform.GetComponent<SkillChip>())
                    if (!hit.transform.GetComponent<SkillChip>().onTable)
                        currentChip = hit.transform.gameObject;
            }

            if (currentChip != null)
                AimLine.S.SetTarget(currentChip.transform);
        }

        if (Input.GetMouseButton(0) && currentChip != null)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y));
            mousePosition.y = currentChip.transform.position.y;

            if (mousePosition.z > 0)
                mousePosition.z = 0;



            //ограничение по углу поворота фишки
            float angle = Vector3.Angle((mousePosition - startPos), new Vector3(0, 0.6f, -1.5f));
            if (angle >= maxAngle)
            {
                if (mousePosition.x < 0)
                    mousePosition.x = mousePosition.z * Mathf.Tan(Mathf.Deg2Rad * maxAngle);
                else
                    mousePosition.x = mousePosition.z * Mathf.Tan(Mathf.Deg2Rad * (-maxAngle));
            }

            float currentDistance = (mousePosition - startPos).magnitude;

            //ограничение по дальности натяжения фишки
            if (currentDistance >= maxDistance)
            {
                mousePosition.x = mousePosition.x * maxDistance / currentDistance;
                mousePosition.z = mousePosition.z * maxDistance / currentDistance;
            }

            currentChip.GetComponent<Rigidbody>().MovePosition(Vector3.MoveTowards(currentChip.transform.position, mousePosition, 0.3f)); //огранечение скорости перемещения фишки
            AimLine.S.ShowLine(startPos);
        }

        if (Input.GetMouseButtonUp(0) && currentChip != null)
        {
            //назначаем мультипликатор, чтобы фишки не вылетали слишком медленно
            float forceMultiplier = (currentChip.transform.position - startPos).magnitude;
            if (forceMultiplier < minForceMultiplier)
                forceMultiplier = minForceMultiplier;

            if (currentChip.transform.position != startPos)
                currentChip.GetComponent<Rigidbody>().AddForce((startPos - currentChip.transform.position).normalized * forceMultiplier * force);
            else
                currentChip.GetComponent<Rigidbody>().AddForce(currentChip.transform.forward.normalized * forceMultiplier * force);

            if (currentChip.transform.GetComponent<ChipController>())
            {
                currentChip.GetComponent<ChipController>().onTable = true;
                chipsOnTable.Add(currentChip); //добавляем фишку в список
            }
            else if (currentChip.transform.GetComponent<SkillChip>())
                currentChip.GetComponent<SkillChip>().onTable = true;

            currentChip = null;
            StartCoroutine(SpawnNewChip());

            AimLine.S.HideLine();
            AudioManager.S.PlayFly();
        }        
    }

    IEnumerator SpawnNewChip()
    {
        isWorkSpawnNewChipCor = true;
        yield return new WaitForSeconds(0.5f);
        if (!isFailing && isWorkSpawnNewChipCor)
        {
            SetStartChip();
            XMLSaver.S.SaveTable(chipsOnTable, chipOnStartPosition, new bool[3] { columnControllers[0].isUp, columnControllers[1].isUp, columnControllers[2].isUp });
            //Records.S.AddInRecords(ScoreController.S.GetScore());
            MenuManager.S.ShowNewSkillPanel();
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SetStartChip()
    {
        GameObject chipGO = Instantiate(chipPrefab, startPos, Quaternion.Euler(Vector3.zero));
        chipGO.GetComponent<ChipController>().SetColorAndValue();
        chipOnStartPosition = chipGO;
    }
    public void SetStartChip(int value, eChipColors color)
    {
        GameObject chipGO = Instantiate(chipPrefab, startPos, Quaternion.Euler(Vector3.zero));
        chipGO.GetComponent<ChipController>().SetColorAndValue(value, color);
        chipOnStartPosition = chipGO;
    }
    public void SetSkillChip(eChipColors type)
    {
        isWorkSpawnNewChipCor = false;
        if (chipOnStartPosition != null)
            Destroy(chipOnStartPosition.gameObject);
        currentChip = null;

        GameObject skillChipGO = null;
        switch (type)
        {
            case eChipColors.green:
                skillChipGO = Instantiate(cristalChipPrefab, startPos, Quaternion.Euler(Vector3.zero));
                break;
            case eChipColors.blue:
                skillChipGO = Instantiate(frostChipPrefab, startPos, Quaternion.Euler(Vector3.zero));
                break;
            case eChipColors.red:
                skillChipGO = Instantiate(fireChipPrefab, startPos, Quaternion.Euler(Vector3.zero));
                break;
            case eChipColors.purple:
                skillChipGO = Instantiate(lightningChipPrefab, startPos, Quaternion.Euler(Vector3.zero));
                break;
        }
        chipOnStartPosition = skillChipGO;
        XMLSaver.S.SaveTable(chipsOnTable, chipOnStartPosition, new bool[3] { columnControllers[0].isUp, columnControllers[1].isUp, columnControllers[2].isUp });
    }

    public void RiseColumn(int count)
    {
        MenuManager.S.ShowNextStagePanel();
        DropAllColumns();

        if (count == 1)
        {
            int rand = Random.Range(0, 3);
            columnControllers[rand].MoveUp();
        }
        else if (count == 2)
        {
            int rand = Random.Range(0, 3);
            for (int i = 0; i < 3; i++)
            {
                if (i != rand)
                {
                    columnControllers[i].MoveUp();
                }
            }
        }
        else if (count == 3)
        {
            foreach (ColumnController c in columnControllers)
            {
                c.MoveUp();
            }
        }
    }

    private void DropAllColumns()
    {
        for (int i = 0; i < columnControllers.Length; i++)
        {
            columnControllers[i].MoveDown();
        }
    }

    public void LoadLevel()
    {
        XmlReader.S.LoadLevel(ref columnControllers);
    }
    public void AddChip(Vector3 pos, int value, eChipColors color)
    {
        GameObject chipGO = Instantiate(chipPrefab, pos, Quaternion.Euler(Vector3.zero));
        chipGO.GetComponent<ChipController>().SetColorAndValue(value, color);
        chipGO.GetComponent<ChipController>().onTable = true;
        chipsOnTable.Add(chipGO);
    }
}

