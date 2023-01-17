using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeChanger : MonoBehaviour
{
    private Camera cam;
    private Color[] colors;
    private int currentBg = 0;

    private void Start()
    {
        cam = GetComponent<Camera>();
        colors = new Color[5] { new Color(146/255f, 183/255f, 241/255f), new Color(219/255f, 113/255f, 127/255f), 
            new Color(95/255f, 203/255f, 191/255f), new Color(210/255f, 111/255f, 215/255f), new Color(127/255f, 210/255f, 110/255f) };
        SetRandomBg();
    }    

    public void SetNextBg()
    {
        if (currentBg < colors.Length-1)
        {
            currentBg += 1;
        }
        else
        {
            currentBg = 0;
        }
        cam.backgroundColor = colors[currentBg];
    }
    private void SetRandomBg()
    {
        currentBg = Random.Range(0, colors.Length);
        cam.backgroundColor = colors[currentBg];
    }
}
