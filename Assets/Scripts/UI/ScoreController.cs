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
    public void Init()
    {
        SetScoreAndStage(XmlReader.S.GetScore());
    }
    private void OnEnable()
    {
        EventAggregator.ChipUnification.AddListener(RaiseScore);
    }
    private void OnDisable()
    {
        EventAggregator.ChipUnification.RemoveListener(RaiseScore);
    }
    public void RaiseScore(int value, Vector3 pos, eChipColors color)
    {
        _score += value;
        _scoreText.text = _score.ToString();
        if (_score/ _scoreBitwinStages > _stage)
        {
            _stage++;
            GameManager.S.StartNewStage(_stage);
        }
        GameObject plusGo = Instantiate<GameObject>(_plusScorePrefab);
        plusGo.GetComponent<PlusScore>().SetParameters(value, color);
        plusGo.transform.position = new Vector3(pos.x, 2f, pos.z);
        XMLSaver.S.SaveScore(_score);
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
