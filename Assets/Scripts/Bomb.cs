using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Renderer))]
public class Bomb : SpawnableObject
{
    [SerializeField] private float _explosionRadius = 30;
    [SerializeField] private float _explosionForce = 500;
    [SerializeField] private float _minAlphaValue = 0.1f;
    [SerializeField] private ParticleSystem _effect;

    private Material _material;

    public Rigidbody Rigidbody { get; private set; }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        _material = GetComponent<Renderer>().material;
    }

    public override void PrepareForSpawn()
    {
        Color color = _material.color;
        color.a = 1;
        _material.color = color;
    }

    public void Explode()
    {
        Instantiate(_effect, transform.position, transform.rotation);

        foreach (Rigidbody explodableObject in GetObjectsInExplodeRadius())
        {
            float distance = Vector3.Distance(transform.position, explodableObject.transform.position);
            float explosionForce = _explosionForce * (_explosionRadius - distance) / _explosionRadius;

            explodableObject.AddExplosionForce(explosionForce, transform.position, _explosionRadius);
        }
    }

    public IEnumerator FadeCoroutine(float duration)
    {
        float endTime = Time.time + duration;
        Color color = _material.color;
        float step = color.a / duration;

        while (endTime > Time.time)
        {
            color.a = Mathf.MoveTowards(color.a, _minAlphaValue, step * Time.deltaTime);
            _material.color = color;
            yield return null;
        }
    }

    private List<Rigidbody> GetObjectsInExplodeRadius()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _explosionRadius);

        List<Rigidbody> collisions = new();

        foreach (Collider hit in hits)
            if (hit.attachedRigidbody != null)
                collisions.Add(hit.attachedRigidbody);

        return collisions;
    }
}
