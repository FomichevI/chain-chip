using UnityEngine;

public class EffectsController : MonoBehaviour
{
    public static EffectsController S;

    [SerializeField] private GameObject hitGreenPsPrefab;
    [SerializeField] private GameObject hitRedPsPrefab;
    [SerializeField] private GameObject hitPurplePsPrefab;
    [SerializeField] private GameObject hitBluePsPrefab;

    [SerializeField] private GameObject fireExpPsPrefab;
    [SerializeField] private GameObject frostExpPsPrefab;
    [SerializeField] private GameObject cristalHitPsPrefab;

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
                ps = Instantiate<GameObject>(hitGreenPsPrefab);
                ps.transform.position = new Vector3(pos.x, 0.8f, pos.z);
                break;
            case eChipColors.red:
                ps = Instantiate<GameObject>(hitRedPsPrefab);
                ps.transform.position = new Vector3(pos.x, 0.8f, pos.z);
                break;
            case eChipColors.purple:
                ps = Instantiate<GameObject>(hitPurplePsPrefab);
                ps.transform.position = new Vector3(pos.x, 0.8f, pos.z);
                break;
            case eChipColors.blue:
                ps = Instantiate<GameObject>(hitBluePsPrefab);
                ps.transform.position = new Vector3(pos.x, 0.8f, pos.z);
                break;
        }
    }

    public void ShowCristalHitEffect(Vector3 pos)
    {
        GameObject ps = Instantiate<GameObject>(cristalHitPsPrefab);
        ps.transform.position = new Vector3(pos.x, 0.8f, pos.z);
    }

    public void ShowExplosionEffect(Vector3 pos)
    {
        GameObject ps = Instantiate<GameObject>(fireExpPsPrefab);
        ps.transform.position = new Vector3(pos.x, 0.5f, pos.z);
    }

    public void ShowFrostEffect(Vector3 pos)
    {
        GameObject ps = Instantiate<GameObject>(frostExpPsPrefab);
        ps.transform.position = new Vector3(pos.x, 0.5f, pos.z);
    }
}