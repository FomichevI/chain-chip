using UnityEngine;
using TMPro;

public class PlusScore : MonoBehaviour
{
    [SerializeField] private TextMeshPro _text;

    private float _lifeTime = 1;
    private Color _green = new Color(46/255f, 205/255f, 41/255f);
    private Color _red = new Color(1, 48/255f, 32/255);
    private Color _purple = new Color(146/255f, 66/255f, 1);
    private Color _blue = new Color(54/255f, 134/255f, 1);

    public void SetParameters(int value, eChipColors color)
    {
        _text.text = "+" + value;
        switch (color)
        {
            case eChipColors.green:
                _text.outlineColor = _green;
                break;
            case eChipColors.red:
                _text.outlineColor = _red;
                break;
            case eChipColors.blue:
                _text.outlineColor = _blue;
                break;
            case eChipColors.purple:
                _text.outlineColor = _purple;
                break;
        }
    }

    private void FixedUpdate()
    {
        _lifeTime -= 0.02f;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z);
        if (_lifeTime <= 0)
            Destroy(gameObject);
    }
}
