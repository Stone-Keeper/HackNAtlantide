using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public enum CurveType
{
    Linear,
    CubicBezier,
}
public class ElectricArc : MonoBehaviour
{
    #region Properties

    [SerializeField] private CurveType _curveType;
    [SerializeField] private float _bezierArcRadius = 10;
    [SerializeField] private float _bezierRotationSpeed;
    [SerializeField] private Transform _p1;
    [SerializeField] private Transform _p2;
    [SerializeField] private int _detail;
    [SerializeField] private float _radius;
    [SerializeField] private float _speed;
    [SerializeField] private float _desactivationTime;
    private Vector2[] randomPositions;
    private LineRenderer _lineRenderer;
    private float bezierArcAngle;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private ParticleSystem _particleP1Sparke;
    [SerializeField] private ParticleSystem _ParticleP2Sparke;
    #endregion

    #region MonoBehaviourMethods
    private void OnEnable()
    {
        _p1.gameObject.SetActive(true);
        _p2.gameObject.SetActive(true);
    }
    private void OnDisable()
    {
        _p1.gameObject.SetActive(false);
        _p2.gameObject.SetActive(false);
    }
    void OnValidate()
    {
        InitializeArc();
    }
    private void Awake()
    {
        InitializeArc();
    }
    private void Update()
    {
        AnimateArc();
    }
    private void OnDrawGizmos()
    {
        AnimateArc();
    }

    #endregion

    #region GenericMethods
    public void InitializeArc()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        switch (_curveType)
        {
            case CurveType.Linear:
                CreateLinearArc();
                break;
            case CurveType.CubicBezier:
                bezierArcAngle = 0;
                CreateCubicBezierArc();
                break;
        }
    }

    void AnimateArc()
    {
        float distance = (_p1.position - _p2.position).magnitude;
        var shape = _particleSystem.shape;
        shape.scale = new Vector3(distance/2f - 0.5f, 0.1f,0.1f);
        _particleSystem.transform.position = (_p1.position + _p2.position) / 2f;
        Vector3 direction = _p1.position - _p2.position;
        _particleSystem.transform.forward = direction;
        _particleP1Sparke.transform.forward = -direction;
        _ParticleP2Sparke.transform.forward = direction;

        switch (_curveType)
        {
            case CurveType.Linear:
                AnimateLinearArc();
                break;
            case CurveType.CubicBezier:
                AnimateCubicBezierArc();
                break;
        }
    }

    public void SetLastPosParent(Transform parent)
    {
        _p2.gameObject.transform.position = parent.transform.position;
        _p2.transform.parent = parent;
    }
    public void SetBothParent(Transform parent, Transform parent2)
    {
        _p2.gameObject.transform.position = parent2.transform.position;
        _p2.transform.parent = parent2;
        _p1.gameObject.transform.position = parent.transform.position;
        _p1.transform.parent = parent;
    }
    public void UnSetLastPosParent()
    {
        _p2.transform.parent = transform;
    }
    public void UnSetBothParent()
    {
        _p2.transform.parent = transform;
        _p1.transform.parent = transform;

    }
    #endregion

    #region LinearArc
    void CreateLinearArc()
    {
        //lineRenderer
        _lineRenderer.positionCount = _detail+1;
        randomPositions = new Vector2[_detail-1];
        _lineRenderer.SetPosition(0, _p1.position);
        for (int i = 1; i < _detail; i++)
        {
            Vector3 offSet = Random.insideUnitSphere * _radius;
            randomPositions[i-1] = new Vector2(offSet.x, offSet.y);
            Vector3 position = Vector3.Lerp(_p1.position, _p2.position, i / (float)_detail)+offSet;
            _lineRenderer.SetPosition(i, position);
        }
        _lineRenderer.SetPosition(_detail, _p2.position);
    }

    void AnimateLinearArc()
    {
        _lineRenderer.SetPosition(0, _p1.position);
        _p1.forward = _p1.position - _p2.position;
        float distance = (_p1.position - _p2.position).magnitude;
        for (int i = 1; i < _detail; i++)
        {
            float angle = (Mathf.PerlinNoise(randomPositions[i - 1].x * Time.time * _speed, randomPositions[i - 1].y * Time.time * _speed) - 0.5f) * 2 * 2 * Mathf.PI;
            Vector3 offSet = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * _radius;

            Vector3 position = Vector3.Lerp(_p1.position, _p2.position, i / (float)_detail) + _p1.TransformDirection(offSet);
            _lineRenderer.SetPosition(i, position);

        }
        _lineRenderer.SetPosition(_detail, _p2.position);
    }
    #endregion

    #region CubicBezierArc
    void CreateCubicBezierArc()
    {
        //lineRenderer
        _lineRenderer.positionCount = _detail + 1;
        randomPositions = new Vector2[_detail - 1];
        _lineRenderer.SetPosition(0, _p1.position);
        Vector3 Curve1Pos = Vector3.Lerp(_p1.position, _p2.position,0.333f)+Vector3.down * _bezierArcRadius;
        Vector3 Curve2Pos = Vector3.Lerp(_p1.position, _p2.position,0.666f)+Vector3.up * _bezierArcRadius;
        for (int i = 1; i < _detail; i++)
        {
            Vector3 offSet = Random.insideUnitSphere * _radius;
            randomPositions[i - 1] = new Vector2(offSet.x, offSet.y);
            Vector3 position = MathExtension.CalculateCubicBezierPoint(i / (float)_detail, _p1.position, Curve1Pos, Curve2Pos, _p2.position) + _p1.TransformDirection(offSet);
            _lineRenderer.SetPosition(i, position);
        }
        _lineRenderer.SetPosition(_detail, _p2.position);
    }
    void AnimateCubicBezierArc()
    {
        _lineRenderer.SetPosition(0, _p1.position);
        _p1.forward = _p1.position - _p2.position;
        float distance = (_p1.position - _p2.position).magnitude;
        bezierArcAngle += Time.deltaTime * _bezierRotationSpeed; 
        Vector3 Curve1Pos = Vector3.Lerp(_p1.position, _p2.position, 0.333f) + _p1.TransformDirection(new Vector3(Mathf.Cos(bezierArcAngle), Mathf.Sin(bezierArcAngle),0) * _bezierArcRadius);
        Vector3 Curve2Pos = Vector3.Lerp(_p1.position, _p2.position, 0.666f) + _p1.TransformDirection(new Vector3(Mathf.Cos(bezierArcAngle-180), Mathf.Sin(bezierArcAngle-180), 0) * _bezierArcRadius);
        for (int i = 1; i < _detail; i++)
        {
            float angle = (Mathf.PerlinNoise(randomPositions[i-1].x * Time.time * _speed, randomPositions[i-1].y * Time.time * _speed)-0.5f) * 2 * 2 * Mathf.PI;
            Vector3 offSet = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * _radius;
            Vector3 position = MathExtension.CalculateCubicBezierPoint(i / (float)_detail, _p1.position, Curve1Pos, Curve2Pos, _p2.position) + _p1.TransformDirection(offSet);
            _lineRenderer.SetPosition(i, position);

        }
        _lineRenderer.SetPosition(_detail, _p2.position);
    }
    public void Deactivate()
    {
        if(isActiveAndEnabled)
        {
            StartCoroutine(Desactivation());
        }
    }

    IEnumerator Desactivation()
    {
        float time = _desactivationTime;
        while(time>0)
        {
            time -= Time.deltaTime;
            _p2.position = Vector3.Lerp(_p2.position, _p1.position,1 - time / _desactivationTime);
            yield return null;
        }
        gameObject.SetActive(false);
    }
    #endregion
}
