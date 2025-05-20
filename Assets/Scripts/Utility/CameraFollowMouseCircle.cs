using UnityEngine;
using UnityEngine.Serialization;

public class CameraFollowMouseCircle : MonoBehaviour
{

    [Header("Основные настройки")]
    [SerializeField] private float _followSpeed = 5f;
    [SerializeField] private bool _smoothMovement = true;

    [Header("Зоны перемещения")]
    [SerializeField] private float _maxRadius = 3f; // Общий радиус движения
    [SerializeField] private float _deadzoneRadius = 1f; // Радиус мёртвой зоны

    private Vector3 _targetPos;
    private Vector3 _initialPos;

    private void Start()
    {
        _initialPos = transform.position;
        _targetPos = _initialPos;
    }

    private void Update()
    {
        Vector3 mousePos = GameData<Main>.Boot.myCam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = _initialPos.z;

        Vector3 offset = mousePos - _initialPos;
        float distance = offset.magnitude;


        if (distance > _deadzoneRadius)
        {
            float normalizedDistance = Mathf.Clamp01((distance - _deadzoneRadius) / (_maxRadius - _deadzoneRadius));


            Vector3 direction = offset.normalized;
            Vector3 clampedOffset = direction * (_deadzoneRadius + normalizedDistance * (_maxRadius - _deadzoneRadius));

            _targetPos = _initialPos + clampedOffset;
        }
        else
        {
            _targetPos = _initialPos;
        }


        if (_smoothMovement)
            transform.position = Vector3.Lerp(transform.position, _targetPos, _followSpeed * Time.deltaTime);
        else
            transform.position = _targetPos;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawWireSphere(_initialPos, _deadzoneRadius);

        Gizmos.color = new Color(0, 0, 1, 0.2f);
        Gizmos.DrawWireSphere(_initialPos, _maxRadius);
    }
}
