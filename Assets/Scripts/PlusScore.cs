using UnityEngine;
using TMPro;

public class PlusScore : MonoBehaviour
{
    private float lifeTime = 1;

    private Color green = new Color(46/255f, 205/255f, 41/255f);
    private Color red = new Color(1, 48/255f, 32/255);
    private Color purple = new Color(146/255f, 66/255f, 1);
    private Color blue = new Color(54/255f, 134/255f, 1);

    [SerializeField] private TextMeshPro text;

    public void SetParameters(int value, eChipColors color)
    {
        text.text = "+" + value;
        switch (color)
        {
            case eChipColors.green:
                text.outlineColor = green;
                break;
            case eChipColors.red:
                text.outlineColor = red;
                break;
            case eChipColors.blue:
                text.outlineColor = blue;
                break;
            case eChipColors.purple:
                text.outlineColor = purple;
                break;
        }
    }

    private void FixedUpdate()
    {
        lifeTime -= 0.02f;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z);
        if (lifeTime <= 0)
            Destroy(gameObject);
    }
}
