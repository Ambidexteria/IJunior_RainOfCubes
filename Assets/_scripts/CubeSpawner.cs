using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private bool _activateSpawn = false;
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private SpawnZone _spawnZone;
    [SerializeField] private float _spawnDelay = 0.5f;
    [SerializeField] private Color _defaultColor;
    [SerializeField] private CubeDetector _ground;
    [SerializeField] private int _poolDefaultCapacity = 20;
    [SerializeField] private int _poolMaxSize = 100;
    [SerializeField] private List<Cube> _cubes;

    private ObjectPool<Cube> _pool;

    private bool _coroutineActive = false;
    private Coroutine _spawnCoroutine;
    private WaitForSeconds _wait;

    private void Awake()
    {
        if (_cubePrefab == null)
            throw new System.ArgumentNullException(nameof(_cubePrefab) + " in " + name);

        if (_spawnZone == null)
            throw new System.ArgumentNullException(nameof(_spawnZone) + " in " + name);

        if (_ground == null)
            throw new System.ArgumentNullException(nameof(_ground) + " in " + name);

        _defaultColor = _cubePrefab.GetComponent<Renderer>().sharedMaterial.color;

        InitializePool();
    }

    private void OnEnable()
    {
        _ground.CubeFell += Deactivate;
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

    private void OnDisable()
    {
        _ground.CubeFell -= Deactivate;
    }

    private void InitializePool()
    {
        _pool = new ObjectPool<Cube>(
            createFunc: () => Create(),
            actionOnGet: (cube) => ActionOnGet(cube),
            actionOnRelease: (cube) => cube.gameObject.SetActive(false),
            actionOnDestroy: (cube) => Destroy(cube.gameObject),
            collectionCheck: true,
            defaultCapacity: _poolDefaultCapacity,
            maxSize: _poolMaxSize
            );
    }

    private void ActionOnGet(Cube cube)
    {
        cube.transform.position = _spawnZone.GetRandomSpawnPosition();
        cube.SetColor(_defaultColor);
        cube.DeactivateFallenState();
        cube.Rigidbody.velocity = Vector3.zero;

        cube.gameObject.SetActive(true);
    }

    private void Deactivate(Cube cube)
    {
        StartCoroutine(DeactivateCoroutine(cube));
    }

    private IEnumerator DeactivateCoroutine(Cube cube)
    {
        float minTime = 2f;
        float maxTime = 5f;
        float delay = Random.Range(minTime, maxTime);

        yield return new WaitForSeconds(delay);

        _pool.Release(cube);
    }

    private IEnumerator SpawnCoroutine()
    {
        _wait = new WaitForSeconds(_spawnDelay);

        while (_coroutineActive)
        {
            _pool.Get();

            yield return _wait;
        }
    }

    private Cube Create()
    {
        Cube cube = Instantiate(_cubePrefab);
        _cubes.Add(cube);

        return cube;
    }
}