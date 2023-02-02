using UnityEngine;

public class AimLine : MonoBehaviour
{
    [SerializeField] private float _maxDistance = 20f;
    [SerializeField] LayerMask _bourdersLayer;
    private LineRenderer _line;
    private Transform _target;
    private Vector3 _center; //вторая точка для определения направления линии

    private void Start()
    {
        _line = GetComponent<LineRenderer>();
        _center = Vector3.zero;
    }

    private void FixedUpdate()
    {
        if (_target != null)
        {
            Vector3 linePos = new Vector3(_target.position.x, 0.3f, _target.position.z);
            transform.position = linePos;
            //transform.position = target.position;


            _line.positionCount = 2;
            //устанавливаем первую точку
            _line.SetPosition(0, new Vector3(0, 0.3f, 0)); //для нормального отображения линии

            //устанавливаем вторую точку
            Ray ray;
            if (_center != transform.position)
                ray = new Ray(transform.position, (_center - transform.position));
            else
                ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, _maxDistance, _bourdersLayer))
                _line.SetPosition(1, hit.point - new Vector3(transform.position.x, 0, transform.position.z)); //проблема в этой точке во время приближения
        }
    }

    public void ShowLine(Vector3 center)
    {
        _center = new Vector3(center.x, 0.3f, center.z); //для нормального отображения линии
        _line.enabled = true;
    }

    public void SetTarget(Transform targ) { _target = targ; }
    public void HideLine() { _line.enabled = false; }
}
