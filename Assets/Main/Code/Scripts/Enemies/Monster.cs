using UnityEngine;

[CreateAssetMenu(fileName = "New Monster", menuName = "Mobs/Monster")]
public class Monster : ScriptableObject
{
    [Header("Monster")]
    [Space]
    public string MonsterName = "New Monster";
    public Sprite Icon = null;
    public float MobDifficulty;
    public GameObject MobPrefab;
    public float spawnChance;

}
