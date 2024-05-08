using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SpawnZone : MonoBehaviour
{
    private BoxCollider _collider;
    private Vector3 _position;
    private Vector3 _scale;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _collider.isTrigger = true;

        _position = _collider.transform.position;
        _scale = _collider.transform.localScale;
    }

    public Vector3 GetRandomSpawnPosition()
    {
        float x = GetRandomCoordinate(_scale.x, _position.x);
        float y = GetRandomCoordinate(_scale.y, _position.y);
        float z = GetRandomCoordinate(_scale.z, _position.z);

        return new Vector3(x, y, z);
    }

    private float GetRandomCoordinate(float scale, float position)
    {
        scale = Mathf.Abs(scale);

        float coordinate1 = position - (scale / 2);
        float coordinate2 = position + (scale / 2);

        return Random.Range(coordinate1, coordinate2);
    }
}
