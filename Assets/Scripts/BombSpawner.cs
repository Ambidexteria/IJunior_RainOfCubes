using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSpawner : GenericSpawner<Bomb>
{
    [SerializeField] private CubeSpawner _cubeSpawner;
    [SerializeField] private Vector2 _timeBeforeDeactivate = new Vector2(2f, 5f);

    private void Start()
    {
        if (_cubeSpawner == null)
            throw new System.ArgumentNullException();
    }

    private void OnEnable()
    {
        _cubeSpawner.CubeReleased += Spawn;
    }

    private void OnDisable()
    {
        _cubeSpawner.CubeReleased -= Spawn;
    }

    public void Spawn(SpawnableObject objectToReplace)
    {
        Bomb bomb = Spawn();
        bomb.transform.position = objectToReplace.transform.position;
        StartCoroutine(DeactivateCoroutine(bomb));
    }

    private IEnumerator DeactivateCoroutine(Bomb bomb)
    {
        float delay = Random.Range(_timeBeforeDeactivate.x, _timeBeforeDeactivate.y);

        yield return new WaitForSeconds(delay);
        bomb.Explode();
        ReturnToPool(bomb);
    }
}