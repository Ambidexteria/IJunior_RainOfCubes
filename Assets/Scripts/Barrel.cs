using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _explosionForce;
    [SerializeField] private ParticleSystem _effect;

    private void OnMouseUpAsButton()
    {
        Explode();
        Instantiate(_effect, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public void Explode()
    {
        Instantiate(_effect, transform.position, transform.rotation);

        foreach (Rigidbody explodableObject in GetExplodableObjects())
        {
            float distance = Vector3.Distance(transform.position, explodableObject.transform.position);
            float explosionForce = _explosionForce * (_explosionRadius - distance) / _explosionRadius;

            explodableObject.AddExplosionForce(explosionForce, transform.position, _explosionRadius);
        }
    }

    private List<Rigidbody> GetExplodableObjects()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _explosionRadius);

        List<Rigidbody> barrels = new();

        foreach (Collider hit in hits)
            if (hit.attachedRigidbody != null)
                barrels.Add(hit.attachedRigidbody);

        return barrels;
    }
}
