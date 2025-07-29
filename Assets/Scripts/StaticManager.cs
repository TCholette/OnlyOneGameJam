using FMOD.Studio;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

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
    public static EventInstance titleMusic;
    public static EventInstance gameMusic;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventInstance? bgInstance = ProxyFmodPlayer.CreateSound<string>("TitleMusic", gameObject);
        EventInstance? musicInstance = ProxyFmodPlayer.CreateSound<string>("InGameMusic", gameObject);
        if (bgInstance != null) titleMusic = (EventInstance)bgInstance;
        if (musicInstance != null) gameMusic = (EventInstance)musicInstance;
        ProxyFmodPlayer.SetParam<string>(gameMusic, new("Music", "Game"));
        titleMusic.start();
        flesh = fleshObj;
        fleshTemplate = fleshTemplateObj;
        player = PlayerObj;
        projectile = projectileObj;
        enemyTemplate = enemyTemplateObj;
        int i = 0;
        foreach (var template in enemyTemplateObjs) {
            if (!enemyTemplates.ContainsValue(template)) {  
            enemyTemplates.Add((EnemyType)i, template);
            }
            Debug.Log((EnemyType)i);
            i++;
        }
    }
}
