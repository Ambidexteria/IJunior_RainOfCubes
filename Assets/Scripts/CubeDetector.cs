using System;
using UnityEngine;

public class CubeDetector : MonoBehaviour
{
    public event Action<Cube> CubeFell;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Cube cube))
        {
            if (cube.IsFallen == false)
            {
                cube.SetColor(ColorChanger.GetRandomColor());
                CubeFell?.Invoke(cube);
            }
        }
    }
}
