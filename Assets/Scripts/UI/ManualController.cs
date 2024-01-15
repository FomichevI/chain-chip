using System.Collections.Generic;
using UnityEngine;

public class ManualController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _pages;
    [SerializeField] private GameObject _backBut;
    private int _currentPageIndex = 0;
    //параметры для свайпа
    private Vector2 _swipeStartPos;
    private float _swipeStartTime;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _swipeStartPos = Input.mousePosition;
            _swipeStartTime = Time.time;
        }
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 swipePos = Input.mousePosition;
            if (Mathf.Abs(swipePos.y - _swipeStartPos.y) < 100 && Time.time - _swipeStartTime < 0.5f)
            {
                if ((swipePos.x - _swipeStartPos.x) < -100)
                {
                    SetNextPage();
                }
                else if ((swipePos.x - _swipeStartPos.x) > 100)
                {
                    SetBackPage();
                }
            }
        }
    }

    public void SetNextPage()
    {
        _pages[_currentPageIndex].SetActive(false);
        _currentPageIndex++;
        if (_currentPageIndex == _pages.Count)
        {
            gameObject.SetActive(false);
        }
        else
        {
            _pages[_currentPageIndex].SetActive(true);
        }
        _backBut.SetActive(true);
    }
    public void SetBackPage()
    {
        if (_currentPageIndex > 0)
        {
            _pages[_currentPageIndex].SetActive(false);
            _currentPageIndex--;
            if (_currentPageIndex == 0)
            {
                _backBut.SetActive(false);
            }
            _pages[_currentPageIndex].SetActive(true);
        }
    }
    public void StartPage()
    {
        foreach (GameObject go in _pages)
            go.SetActive(false);
        _currentPageIndex = 0;
        _pages[_currentPageIndex].SetActive(true);
        _backBut.SetActive(false);
    }
}
