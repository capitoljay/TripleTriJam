using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyBoundary : MonoBehaviour
{
    public BoundaryType boundaryType = BoundaryType.Floor;
    public EnemyBoundary otherBoundary;
    public float spawnOffset = 1f;
    // Start is called before the first frame update
    void Start()
    {
        if (otherBoundary == null)
        {
            if (boundaryType == BoundaryType.Floor)
                otherBoundary = FindObjectsOfType<EnemyBoundary>().Where(b => b.boundaryType == BoundaryType.Ceiling).FirstOrDefault();
            else
                otherBoundary = FindObjectsOfType<EnemyBoundary>().Where(b => b.boundaryType == BoundaryType.Floor).FirstOrDefault();
        }
    }
}
public enum BoundaryType
{
    Floor,
    Ceiling
}