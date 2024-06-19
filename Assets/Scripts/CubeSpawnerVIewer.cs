using System;
using TMPro;
using UnityEngine;

public class CubeSpawnerViewer : MonoBehaviour
{
    [SerializeField] private GenericSpawner<Cube> _spawner;
    [SerializeField] private TextMeshProUGUI _activeObjectsText;
    [SerializeField] private TextMeshProUGUI _allSpawnedObjectsText;

    private void Awake()
    {
        if (_spawner == null || _activeObjectsText == null || _allSpawnedObjectsText == null)
            throw new ArgumentNullException();
    }

    private void OnEnable()
    {
        _spawner.ActiveCountChanged += ChangeActiveObjects;
        _spawner.AllCountChanged += ChangeAllSpawnedObjects;
    }

    private void OnDisable()
    {
        _spawner.ActiveCountChanged -= ChangeActiveObjects;
        _spawner.AllCountChanged -= ChangeAllSpawnedObjects;
    }

    private void ChangeActiveObjects(int value)
    {
        string text = $"Active: {value}";
        _activeObjectsText.text = text;
    }

    private void ChangeAllSpawnedObjects(int value)
    {
        string text = $"All spawned: {value}";
        _allSpawnedObjectsText.text = text;
    }

}
