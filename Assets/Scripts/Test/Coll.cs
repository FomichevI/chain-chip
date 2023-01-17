using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coll : MonoBehaviour
{
    int stage;
    void Start()
    {
        StartCoroutine(Rise());
    }

    IEnumerator Rise()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            stage++;
            if (stage % 4 == 1)
            {
                GameManager.S.RiseColumn(1);
            }
            else if (stage % 4 == 2)
            {
                GameManager.S.RiseColumn(2);
            }
            else if (stage % 4 == 3)
            {
                GameManager.S.RiseColumn(3);
            }
            else if (stage % 4 == 0)
            {
                GameManager.S.RiseColumn(0);
            }
        }
    }
}
