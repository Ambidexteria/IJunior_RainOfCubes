using System.Collections;
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

    public void Spawn(Cube cubeToReplace)
    {
        Bomb bomb = Spawn();
        bomb.transform.position = cubeToReplace.transform.position;
        bomb.Rigidbody.velocity = cubeToReplace.Rigidbody.velocity;
        StartCoroutine(DeactivateCoroutine(bomb));
    }

    private IEnumerator DeactivateCoroutine(Bomb bomb)
    {
        float delay = Random.Range(_timeBeforeDeactivate.x, _timeBeforeDeactivate.y);
        StartCoroutine(bomb.FadeCoroutine(delay));

        yield return new WaitForSeconds(delay);

        bomb.Explode();
        ReturnToPool(bomb);
    }
}