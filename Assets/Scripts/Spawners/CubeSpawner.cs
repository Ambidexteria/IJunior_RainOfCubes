using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public class CubeSpawner : GenericSpawner<Cube>
{
    [SerializeField] private bool _activateSpawn = false;
    [SerializeField] private float _spawnDelay = 0.5f;
    [SerializeField] private Vector2 _timeBeforeDeactivate = new Vector2(2f, 5f);
    [SerializeField] private Color _defaultColor;
    [SerializeField] private List<CubeDetector> _platforms;

    private bool _coroutineActive = false;
    private Coroutine _spawnCoroutine;
    private WaitForSeconds _wait;

    public event UnityAction<Cube> CubeReleased;

    private void Start()
    {
        if (_platforms.Count == 0)
            throw new System.ArgumentNullException();
    }

    private void OnEnable()
    {
        if (_platforms.Count != 0)
        {
            foreach (var platform in _platforms)
                platform.CubeFell += Deactivate;
        }
    }

    private void OnDisable()
    {
        if (_platforms.Count != 0)
        {
            foreach (var platform in _platforms)
                platform.CubeFell -= Deactivate;
        }
    }

    private void Update()
    {
        if (_activateSpawn && !_coroutineActive)
        {
            _coroutineActive = true;
            _spawnCoroutine = StartCoroutine(SpawnCoroutine());
        }
        else if (!_activateSpawn && _coroutineActive)
        {
            _coroutineActive = false;
            StopCoroutine(_spawnCoroutine);
        }
    }

    public override void PrepareToDeactivate(Cube cube)
    {
        cube.ActivateFallenState();
        cube.SetColor(ColorChanger.GetRandomColor());
    }

    private IEnumerator SpawnCoroutine()
    {
        _wait = new WaitForSeconds(_spawnDelay);
        Cube cube;

        while (_coroutineActive)
        {
            cube = Spawn();
            cube.transform.position = GetRandomSpawnPosition();

            yield return _wait;
        }
    }

    private void Deactivate(Cube cube)
    {
        StartCoroutine(DeactivateCoroutine(cube));
    }

    private IEnumerator DeactivateCoroutine(Cube cube)
    {
        float delay = Random.Range(_timeBeforeDeactivate.x, _timeBeforeDeactivate.y);

        yield return new WaitForSeconds(delay);

        ReturnToPool(cube);
        CubeReleased?.Invoke(cube);
    }
}