using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    public static ScoreController S;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private int scoreBitwinStages = 50;
    [SerializeField] private GameObject plusScorePrefab;
    private int score = 0;
    private int stage = 0;

    private void Awake()
    {
        if (S == null)
            S = this;
    }

    public void RaiseScore(int value, Vector3 pos, eChipColors color)
    {
        score += value;
        scoreText.text = score.ToString();
        if (score/ scoreBitwinStages > stage)
        {
            stage++;
            if (stage%4 == 1)
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
        GameObject plusGo = Instantiate<GameObject>(plusScorePrefab);
        plusGo.GetComponent<PlusScore>().SetParameters(value, color);
        plusGo.transform.position = new Vector3(pos.x, 2f, pos.z);
    }
    public int GetScore()
    {
        return score;
    }
    public void SetScoreAndStage(int score)
    {
        this.score = score;
        this.stage = score/scoreBitwinStages;
        scoreText.text = score.ToString();
    }
}
