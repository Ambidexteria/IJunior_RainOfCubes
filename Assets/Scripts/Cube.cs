using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Renderer))]
public class Cube : SpawnableObject
{
    private Material _material;
    private Color _defaultColor;

    public bool IsFallen { get; private set; } = false;
    public Rigidbody Rigidbody { get; private set; }

    private void Awake()
    {
        _material = GetComponent<Renderer>().material;
        _defaultColor = _material.color;
        Rigidbody = GetComponent<Rigidbody>();
    }

    public void ActivateFallenState()
    {
        IsFallen = true;
    }

    public override void PrepareForSpawn()
    {
        SetDefaultColor();
        DeactivateFallenState();
        Rigidbody.velocity = Vector3.zero;
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