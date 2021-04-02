using UnityEngine;
using UnityEngine.EventSystems;

public class TestDragging : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private float _speed = 10;
    [SerializeField] private float _force = 10;

    [SerializeField] private Rigidbody _rb;
    [SerializeField] private SphereCollider _throwingBallCollider;
    [SerializeField] private SphereCollider _sphereCollider;
    [SerializeField] private LayerMask SphereMask;
    [SerializeField] private TestTrajectory _trajectory;

    private Vector3 _startPos;
    private Plane _plane;
    private TestPowerBar _powerBar;
    private float _sphereRadius = 3;

    private void Awake()
    {
        _powerBar = FindObjectOfType<TestPowerBar>();
        if (_powerBar == null)
        {
            Debug.LogError("Can't find TestPowerBar!");
        }
    }
    private void Start()
    {
        _startPos = transform.position;
        _plane = new Plane(Camera.main.transform.forward * -1, _startPos);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _throwingBallCollider.enabled = false;
        _sphereCollider.enabled = true;
        _powerBar.StartFill();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 50f, SphereMask))
        {
            var pos = Vector3.Lerp(transform.position, hit.point, Time.deltaTime * _speed);
            transform.position = pos;
        }
        else if (_plane.Raycast(ray, out float distance))
        {
            var hitOnPlane = ray.GetPoint(distance);
            var dir2D = (hitOnPlane - _startPos).normalized;
            var edgePoint = _startPos + dir2D * _sphereRadius;

            var pos = Vector3.Lerp(transform.position, edgePoint, Time.deltaTime * _speed);
            transform.position = pos;
        }

        // FIXME - Trajectory drawing is not correct
        // _trajectory.DrawTrajectory(transform.position, (_startPos - transform.position).normalized, _force * _powerBar.FillAmount);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _throwingBallCollider.enabled = true;
        _sphereCollider.enabled = false;

        _powerBar.StopFill();
        // _trajectory.ResetTrajectory();

        var throwDir = (_startPos - transform.position).normalized;

        var rb = gameObject.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.AddForce(throwDir * _force * _powerBar.FillAmount);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Floor")
        {
            Reset();
        }
    }

    private void Reset()
    {
        _powerBar.ResetFill();
        _rb.useGravity = false;
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        transform.position = _startPos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, Vector3.one);
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, _startPos);
    }
}