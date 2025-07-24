using UnityEngine;
using System.Collections.Generic;

public class StaticManager : MonoBehaviour
{
    [SerializeField] private Sprite[] fleshObj;
    [SerializeField] private GameObject fleshTemplateObj;
    [SerializeField] private Player PlayerObj;
    [SerializeField] private GameObject projectileObj;
    [SerializeField] private GameObject enemyTemplateObj;
    [SerializeField] private GameObject[] enemyTemplateObjs;
    public static Sprite[] flesh;
    public static GameObject fleshTemplate;
    public static Player player;
    public static GameObject projectile;
    public static GameObject enemyTemplate;
    public static Dictionary<EnemyType, GameObject> enemyTemplates = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        flesh = fleshObj;
        fleshTemplate = fleshTemplateObj;
        player = PlayerObj;
        projectile = projectileObj;
        enemyTemplate = enemyTemplateObj;
        int i = 0;
        foreach (var template in enemyTemplateObjs) {
            enemyTemplates.Add((EnemyType)i, template);
            Debug.Log((EnemyType)i);
            i++;
        }
    }
}
