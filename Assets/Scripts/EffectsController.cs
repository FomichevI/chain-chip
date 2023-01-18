using UnityEngine;

public class EffectsController : MonoBehaviour
{
    public static EffectsController S;

    [SerializeField] private GameObject _hitGreenPsPrefab;
    [SerializeField] private GameObject _hitRedPsPrefab;
    [SerializeField] private GameObject _hitPurplePsPrefab;
    [SerializeField] private GameObject _hitBluePsPrefab;

    [SerializeField] private GameObject _fireExpPsPrefab;
    [SerializeField] private GameObject _frostExpPsPrefab;
    [SerializeField] private GameObject _cristalHitPsPrefab;

    private void Awake()
    {
        if (S == null)
        {
            S = this;
        }
    }

    public void ShowHitEffect(Vector3 pos, eChipColors col)
    {
        GameObject ps;
        switch (col)
        {
            case eChipColors.green:
                ps = Instantiate<GameObject>(_hitGreenPsPrefab);
                ps.transform.position = new Vector3(pos.x, 0.8f, pos.z);
                break;
            case eChipColors.red:
                ps = Instantiate<GameObject>(_hitRedPsPrefab);
                ps.transform.position = new Vector3(pos.x, 0.8f, pos.z);
                break;
            case eChipColors.purple:
                ps = Instantiate<GameObject>(_hitPurplePsPrefab);
                ps.transform.position = new Vector3(pos.x, 0.8f, pos.z);
                break;
            case eChipColors.blue:
                ps = Instantiate<GameObject>(_hitBluePsPrefab);
                ps.transform.position = new Vector3(pos.x, 0.8f, pos.z);
                break;
        }
    }

    public void ShowCristalHitEffect(Vector3 pos)
    {
        GameObject ps = Instantiate<GameObject>(_cristalHitPsPrefab);
        ps.transform.position = new Vector3(pos.x, 0.8f, pos.z);
    }

    public void ShowExplosionEffect(Vector3 pos)
    {
        GameObject ps = Instantiate<GameObject>(_fireExpPsPrefab);
        ps.transform.position = new Vector3(pos.x, 0.5f, pos.z);
    }

    public void ShowFrostEffect(Vector3 pos)
    {
        GameObject ps = Instantiate<GameObject>(_frostExpPsPrefab);
        ps.transform.position = new Vector3(pos.x, 0.5f, pos.z);
    }
}
