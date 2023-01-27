using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    public static ScoreController S;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private GameObject _plusScorePrefab;
    private int _scoreBitwinStages = 25;
    private int _score = 0;
    private int _stage = 0;

    private void Awake()
    {
        if (S == null)
            S = this;
    }

    public void RaiseScore(int value, Vector3 pos, eChipColors color)
    {
        _score += value;
        _scoreText.text = _score.ToString();
        if (_score/ _scoreBitwinStages > _stage)
        {
            _stage++;
            if (_stage%4 == 1)
            {
                GameManager.S.RiseColumn(1);
            }
            else if (_stage % 4 == 2)
            {
                GameManager.S.RiseColumn(2);
            }
            else if (_stage % 4 == 3)
            {
                GameManager.S.RiseColumn(3);
            }
            else if (_stage % 4 == 0)
            {
                GameManager.S.RiseColumn(0);
            }
        }
        GameObject plusGo = Instantiate<GameObject>(_plusScorePrefab);
        plusGo.GetComponent<PlusScore>().SetParameters(value, color);
        plusGo.transform.position = new Vector3(pos.x, 2f, pos.z);
    }
    public int GetScore()
    {
        return _score;
    }
    public void SetScoreAndStage(int score)
    {
        _score = score;
        _stage = score/_scoreBitwinStages;
        _scoreText.text = score.ToString();
    }
}
