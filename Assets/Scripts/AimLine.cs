using UnityEngine;

public class AimLine : MonoBehaviour
{
    public static AimLine S;
    private LineRenderer line;

    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private Transform target;
    [SerializeField] LayerMask bourdersLayer;

    private int count;
    private Vector3 center;

    private void Start()
    {
        if (S == null)
            S = this;
        line = GetComponent<LineRenderer>();
        center = Vector3.zero;
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 linePos = new Vector3(target.position.x, 0.3f, target.position.z);
            transform.position = linePos;
            //transform.position = target.position;


            line.positionCount = 2;
            //устанавливаем первую точку
            line.SetPosition(0, new Vector3(0, 0.3f, 0)); //для нормального отображения линии

            //устанавливаем вторую точку
            Ray ray;
            if (center != transform.position)
                ray = new Ray(transform.position, (center - transform.position));
            else
                ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxDistance, bourdersLayer))
                line.SetPosition(1, hit.point - new Vector3(transform.position.x, 0, transform.position.z)); //проблема в этой точке во время приближения
        }
    }

    public void ShowLine(Vector3 center)
    {
        this.center = new Vector3 (center.x, 0.3f,center.z); //для нормального отображения линии
        line.enabled = true;
    }


    public void SetTarget(Transform targ)    {        target = targ;    }

    public void HideLine() { line.enabled = false; }
}
