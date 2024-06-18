using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Renderer))]
public class Cube : SpawnableObject
{
    private Material _material;
    private Color _defaultColor;
    private Rigidbody _rigidbody;

    public bool IsFallen { get; private set; } = false;

    private void Awake()
    {
        _material = GetComponent<Renderer>().material;
        _defaultColor = _material.color;
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void ActivateFallenState()
    {
        IsFallen = true;
    }

    public override void PrepareForSpawn()
    {
        SetDefaultColor();
        DeactivateFallenState();
        _rigidbody.velocity = Vector3.zero;
    }

    public void SetColor(Color color)
    {
        _material.color = color;
    }

    private void SetDefaultColor()
    {
        _material.color = _defaultColor;
    }

    private void DeactivateFallenState()
    {
        IsFallen = false;
    }
}