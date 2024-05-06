using UnityEngine;

public class Cube : MonoBehaviour
{
    private Material _material;

    public bool IsFallen { get; private set; } = false;

    private void Awake()
    {
        _material = GetComponent<Renderer>().material;
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
