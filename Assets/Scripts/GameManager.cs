using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager S;
    [HideInInspector] public List<GameObject> ChipsOnTable;
    [HideInInspector] public bool IsFailing = false;

    [SerializeField] private AimLine _aimLine;
    [SerializeField] private float _shootForce = 500; //сила запуска фишки
    [SerializeField] private GameObject _chipPrefab;
    [SerializeField] private GameObject _fireChipPrefab;
    [SerializeField] private GameObject _frostChipPrefab;
    [SerializeField] private GameObject _lightningChipPrefab;
    [SerializeField] private GameObject _cristalChipPrefab;

    [SerializeField] private LayerMask _chipsLayer;
    [SerializeField] private LayerMask _uiLayer;
    [SerializeField] private float _maxShootAngle = 60;
    [SerializeField] private float _minForceMultiplier = 0.5f;
    [SerializeField] private Column[] _columnControllers;

    private GameObject _currentChip;
    private GameObject _chipOnStartPosition;
    private Vector3 _startPos;
    private float _maxShootDistance = 1.5f; //максимальное расстояние, на которое можем оттянуть фишку
    private bool _isSpawningNewChip = false;

    private void Awake()
    {
        if (S == null)
            S = this;

        _startPos = new Vector3(0, 0.5f, 0);        
    }
    private void Start()
    {
        if (DataManager.S.IsGameFirstLoaded)
            StartCoroutine(DataManager.S.Starting());
    }
    private void OnEnable()
    {
        EventAggregator.Init.AddListener(InitLevel);
    }
    private void OnDisable()
    {
        EventAggregator.Init.RemoveListener(InitLevel);
    }

    private void InitLevel()
    {
        if (XmlReader.S.HasSave())
        {
            Debug.Log("3");
            LoadLevel();
        }
        else
        {
            Debug.Log("2");
            GameObject chipGO = Instantiate(_chipPrefab, _startPos, Quaternion.Euler(Vector3.zero));
            chipGO.GetComponent<Chip>().SetColorAndValue();
            _chipOnStartPosition = chipGO;
            GenerateStartLevel();
        }
        ScoreController.S.Init();
        MenuManager.S.InitSounds();
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
        if (Input.GetMouseButtonDown(0)) //если мы только что нажали, то захватываем фишку под курсором
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 50, _chipsLayer) && hit.transform != null)
            {
                //две проверки, необходимые для того, чтобы избежать захвата фишки через элементы UI
                if (EventSystem.current.IsPointerOverGameObject())
                    return;
                if (Input.touchCount > 0)
                    if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                        return;

                if (hit.transform.GetComponent<Chip>())
                {
                    if (!hit.transform.GetComponent<Chip>().IsOnTable)
                        _currentChip = hit.transform.gameObject;
                }
                else if (hit.transform.GetComponent<SkillChip>())
                {
                    if (!hit.transform.GetComponent<SkillChip>().OnTable)
                        _currentChip = hit.transform.gameObject;
                }
            }
            if (_currentChip != null)
                _aimLine.SetTarget(_currentChip.transform);
        }

        if (Input.GetMouseButton(0) && _currentChip != null) //если мы уже удерживаем фишку, то перемещаем ее вслед за курсором
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y));
            mousePosition.y = _currentChip.transform.position.y;
            //ограничение перемещения по Z
            if (mousePosition.z > 0)
                mousePosition.z = 0;
            //ограничение перемещения по углу поворота фишки
            float angle = Vector3.Angle((mousePosition - _startPos), new Vector3(0, 0.6f, -1.5f));
            if (angle >= _maxShootAngle)
            {
                if (mousePosition.x < 0)
                    mousePosition.x = mousePosition.z * Mathf.Tan(Mathf.Deg2Rad * _maxShootAngle);
                else
                    mousePosition.x = mousePosition.z * Mathf.Tan(Mathf.Deg2Rad * (-_maxShootAngle));
            }
            //ограничение по дальности натяжения фишки
            float currentDistance = (mousePosition - _startPos).magnitude;
            if (currentDistance >= _maxShootDistance)
            {
                mousePosition.x = mousePosition.x * _maxShootDistance / currentDistance;
                mousePosition.z = mousePosition.z * _maxShootDistance / currentDistance;
            }
            //огранечение скорости перемещения фишки
            _currentChip.GetComponent<Rigidbody>().MovePosition(Vector3.MoveTowards(_currentChip.transform.position, mousePosition, 0.3f));
            _aimLine.ShowLine(_startPos); //обновляем линию прицеливания
        }

        if (Input.GetMouseButtonUp(0) && _currentChip != null) //если отпустили кнопку, то запускаем фишку на игровое поле
        {
            //назначаем мультипликатор, чтобы фишки не вылетали слишком медленно
            //мультипликатор зависит от расстояния между начальной точкой спавна фишки и ее текущим положением
            float forceMultiplier = (_currentChip.transform.position - _startPos).magnitude;
            if (forceMultiplier < _minForceMultiplier)
                forceMultiplier = _minForceMultiplier;
            //определяем направление запуска фишки
            if (_currentChip.transform.position != _startPos)
                _currentChip.GetComponent<Rigidbody>().AddForce((_startPos - _currentChip.transform.position).normalized * forceMultiplier * _shootForce);
            else
                _currentChip.GetComponent<Rigidbody>().AddForce(_currentChip.transform.forward.normalized * forceMultiplier * _shootForce);
            //выполняем дествия, которые зависят от того, запустили мы обычную фишку, или скилл
            if (_currentChip.transform.GetComponent<Chip>())
            {
                _currentChip.GetComponent<Chip>().IsOnTable = true;
                ChipsOnTable.Add(_currentChip); //добавляем фишку в список
            }
            else if (_currentChip.transform.GetComponent<SkillChip>())
            {
                _currentChip.GetComponent<SkillChip>().OnTable = true;
            }

            _currentChip = null;
            StartCoroutine(SpawnNewChip());
            _aimLine.HideLine();
            EventAggregator.ThrowChip.Invoke();
        }
    }

    IEnumerator SpawnNewChip()
    {
        _isSpawningNewChip = true;
        yield return new WaitForSeconds(0.5f);
        if (!IsFailing && _isSpawningNewChip)
        {
            SetStartChip();
            XMLSaver.S.SaveTable(ChipsOnTable, _chipOnStartPosition, new bool[3] { _columnControllers[0].IsUp, _columnControllers[1].IsUp, _columnControllers[2].IsUp });
            //Records.S.AddInRecords(ScoreController.S.GetScore());
            EventAggregator.AddNewSkill.Invoke();
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SetStartChip()
    {
        GameObject chipGO = Instantiate(_chipPrefab, _startPos, Quaternion.Euler(Vector3.zero));
        chipGO.GetComponent<Chip>().SetColorAndValue();
        _chipOnStartPosition = chipGO;
    }
    public void SetStartChip(int value, eChipColors color)
    {
        GameObject chipGO = Instantiate(_chipPrefab, _startPos, Quaternion.Euler(Vector3.zero));
        chipGO.GetComponent<Chip>().SetColorAndValue(value, color);
        _chipOnStartPosition = chipGO;
    }
    public void SetSkillChip(eChipColors type)
    {
        _isSpawningNewChip = false;
        if (_chipOnStartPosition != null)
            Destroy(_chipOnStartPosition.gameObject);
        _currentChip = null;

        GameObject skillChipGO = null;
        switch (type)
        {
            case eChipColors.green:
                skillChipGO = Instantiate(_cristalChipPrefab, _startPos, Quaternion.Euler(Vector3.zero));
                break;
            case eChipColors.blue:
                skillChipGO = Instantiate(_frostChipPrefab, _startPos, Quaternion.Euler(Vector3.zero));
                break;
            case eChipColors.red:
                skillChipGO = Instantiate(_fireChipPrefab, _startPos, Quaternion.Euler(Vector3.zero));
                break;
            case eChipColors.purple:
                skillChipGO = Instantiate(_lightningChipPrefab, _startPos, Quaternion.Euler(Vector3.zero));
                break;
        }
        _chipOnStartPosition = skillChipGO;
        XMLSaver.S.SaveTable(ChipsOnTable, _chipOnStartPosition, new bool[3] { _columnControllers[0].IsUp, _columnControllers[1].IsUp, _columnControllers[2].IsUp });
    }

    public void StartNewStage(int stage)
    {
        if (stage % 4 == 1)        
            RiseColumn(1);        
        else if (stage % 4 == 2)        
            RiseColumn(2);        
        else if (stage % 4 == 3)        
            RiseColumn(3);        
        else if (stage % 4 == 0)        
            RiseColumn(0);
        EventAggregator.NewStage.Invoke();
    }

    private void RiseColumn(int count)
    {
        DropAllColumns();

        if (count == 1)
        {
            int rand = Random.Range(0, 3);
            _columnControllers[rand].MoveUp();
        }
        else if (count == 2)
        {
            int rand = Random.Range(0, 3);
            for (int i = 0; i < 3; i++)
                if (i != rand)
                    _columnControllers[i].MoveUp();
        }
        else if (count == 3)
        {
            foreach (Column c in _columnControllers)
                c.MoveUp();
        }
    }

    private void DropAllColumns()
    {
        for (int i = 0; i < _columnControllers.Length; i++)        
            _columnControllers[i].MoveDown();        
    }

    public void LoadLevel()
    {
        XmlReader.S.LoadLevel(ref _columnControllers);
    }
    public void AddChip(Vector3 pos, int value, eChipColors color)
    {
        GameObject chipGO = Instantiate(_chipPrefab, pos, Quaternion.Euler(Vector3.zero));
        chipGO.GetComponent<Chip>().SetColorAndValue(value, color);
        chipGO.GetComponent<Chip>().IsOnTable = true;
        ChipsOnTable.Add(chipGO);
    }
}

