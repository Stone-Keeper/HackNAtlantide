using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UIElements;
public class ElectricArcTrail : MonoBehaviour
{
    #region Properties
    [SerializeField] private Transform _p1;
    [SerializeField] private Transform _p2;
    [SerializeField] private float _radius;
    [SerializeField] private float _speed;
    private List<Vector2> randomPositions;
    private LineRenderer _lineRenderer;
    [SerializeField] private float _emitDuration = 1f;
    [SerializeField] private float _timeBetweenEmissions = 0.1f;
    [SerializeField] private float _particuleLifeTime;
    [SerializeField] private List<Vector3> positionsLineRenderer;
    #endregion
    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        positionsLineRenderer = new List<Vector3>();
        randomPositions = new List<Vector2>();
        _lineRenderer.positionCount = 0;
    }
    #region Arc

    void AnimateArc()
    {
        _p1.forward = _p1.position - _p2.position;
        float distance = (_p1.position - _p2.position).magnitude;
        int detail = _lineRenderer.positionCount;
        for (int i = 0; i < detail; i++)
        {
            float angle = (Mathf.PerlinNoise(randomPositions[i].x * Time.time * _speed, randomPositions[i].y * Time.time * _speed) - 0.5f) * 2 * 2 * Mathf.PI;
            Vector3 offSet = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * _radius;
            Vector3 position = positionsLineRenderer[i] + offSet;
            _lineRenderer.SetPosition(i, position);

        }
    }
    public void PlayTrail()
    {
        StartCoroutine(SpawnTrailArc());
    }
    IEnumerator SpawnTrailArc()
    {
        float timer = _emitDuration;
        float timeBetweenSpawns = 0;
        while( timer >= 0)
        {
            Debug.Log("spawn");
            timeBetweenSpawns -= Time.deltaTime;
            timer -= Time.deltaTime;
            _lineRenderer.SetPosition(_lineRenderer.positionCount-1, _p1.position);
            AnimateArc();
            if(timeBetweenSpawns < 0)
            {
                timeBetweenSpawns = _timeBetweenEmissions;
                Spawn();
            }
            yield return null;
        }
        StartCoroutine(DespawnTrailArc());
    }
    void Spawn()
    {
        Vector3 offSet = Random.insideUnitSphere * _radius;
        Vector3 position = _p1.position + offSet;
        positionsLineRenderer.Add(position);
        _lineRenderer.positionCount = positionsLineRenderer.Count;
        _lineRenderer.SetPositions(positionsLineRenderer.ToArray());
        randomPositions.Add(new Vector2(offSet.x, offSet.y));
    }
    IEnumerator DespawnTrailArc()
    {
        float timeBetweenDespawns = _particuleLifeTime;
        while( _lineRenderer.positionCount > 0)
        {
            AnimateArc();
            timeBetweenDespawns -= Time.deltaTime;
            if(timeBetweenDespawns < 0)
            {
                Despawn();
                timeBetweenDespawns = _particuleLifeTime;
            }
            yield return null;
        }
    }
    void Despawn()
    {
        if(positionsLineRenderer.Count > 0)
        {
            positionsLineRenderer.RemoveAt(0);
            _lineRenderer.positionCount = positionsLineRenderer.Count;
            _lineRenderer.SetPositions(positionsLineRenderer.ToArray());
            randomPositions.RemoveAt(0);
        }
        else
        {
            StopCoroutine(DespawnTrailArc());
        }
    }
    #endregion
}
