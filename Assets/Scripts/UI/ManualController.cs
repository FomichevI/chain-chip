using System.Collections.Generic;
using UnityEngine;

public class ManualController : MonoBehaviour
{
    [SerializeField] private List<GameObject> pages;
    [SerializeField] private GameObject backBut;
    private int currentPage = 0;

    private Vector2 swipeStartPos;
    private float swipeStartTime;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            swipeStartPos = Input.mousePosition;
            swipeStartTime = Time.time;
        }
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 swipePos = Input.mousePosition;
            if (Mathf.Abs(swipePos.y - swipeStartPos.y)<100 && Time.time - swipeStartTime < 0.5f)
            {
                if ((swipePos.x - swipeStartPos.x) < -100)
                {
                    SetNextPage();
                }
                else if ((swipePos.x - swipeStartPos.x) > 100)
                {
                    SetBackPage();
                }
            }
        }
    }

    public void SetNextPage()
    {
        pages[currentPage].SetActive(false);
        currentPage++;
        if (currentPage == pages.Count)
        {
            gameObject.SetActive(false);
        }
        else
        {
            pages[currentPage].SetActive(true);
        }
        backBut.SetActive(true);
    }
    public void SetBackPage()
    {
        if (currentPage > 0)
        {
            pages[currentPage].SetActive(false);
            currentPage--;
            if (currentPage == 0)
            {
                backBut.SetActive(false);
            }
            pages[currentPage].SetActive(true);
        }
    }
    public void SetStartPage()
    {
        foreach (GameObject go in pages)
            go.SetActive(false);
        currentPage = 0;
        pages[currentPage].SetActive(true);
        backBut.SetActive(false);
    }
}
