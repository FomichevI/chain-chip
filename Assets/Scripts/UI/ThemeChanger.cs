using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeChanger : MonoBehaviour
{
    [SerializeField] Color[] _colors;
    private Camera _camera;
    private int _currentColorIndex;

    private void Start()
    {
        _camera = Camera.main;
        _currentColorIndex = Random.Range(0, _colors.Length);
        _camera.backgroundColor = _colors[_currentColorIndex];
    }
    public void ChangeColor()
    {
        if (_currentColorIndex < _colors.Length - 1)
            _currentColorIndex++;
        else
            _currentColorIndex = 0;
        _camera.backgroundColor = _colors[_currentColorIndex];
    }
}
