using UnityEngine;

[CreateAssetMenu(fileName = "EnemyName", menuName = "ScriptableObjects/EnemyData", order = 2)]
public class EnemyDataScriptableObject : ScriptableObject
{
    [Header("Speed info")]
    public float patrolSpeed;
    public float chaseSpeed;
    public float attackSpeed;

    [Header("Other basic info")]
    public float playerDetectionRange;
    public float patrolWalkRange;

    [Header("Weaponry related")]
    public GameObject weaponPrefab;
    public float damage;

    [Header("Drops")]
    public GameObject[] dropsOnDeath;
}
