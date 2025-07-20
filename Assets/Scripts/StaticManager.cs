using UnityEngine;

public class StaticManager : MonoBehaviour
{
    [SerializeField] private Sprite[] fleshObj;
    [SerializeField] private GameObject fleshTemplateObj;
    [SerializeField] private Player PlayerObj;
    [SerializeField] private GameObject projectileObj;
    public static Sprite[] flesh;
    public static GameObject fleshTemplate;
    public static Player player;
    public static GameObject projectile;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        flesh = fleshObj;
        fleshTemplate = fleshTemplateObj;
        player = PlayerObj;
        projectile = projectileObj;
    }
}
