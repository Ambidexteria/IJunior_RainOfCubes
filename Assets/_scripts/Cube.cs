using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Renderer))]
public class Cube : MonoBehaviour
{
    private Material _material;

    public Rigidbody Rigidbody { get; private set; }
    public bool IsFallen { get; private set; } = false;

    private void Awake()
    {
        _material = GetComponent<Renderer>().material;
        Rigidbody = GetComponent<Rigidbody>();
    }

    public void ActivateFallenState()
    {
        IsFallen = true;
    }
    
    public void DeactivateFallenState()
    {
        IsFallen = false;
    }

    public void SetColor(Color color)
    {
        _material.color = color;
    }
}
