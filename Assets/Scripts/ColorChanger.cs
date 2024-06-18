using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public static Color GetRandomColor()
    {
        return new Color(Random.value, Random.value, Random.value, 1f);
    }
}
