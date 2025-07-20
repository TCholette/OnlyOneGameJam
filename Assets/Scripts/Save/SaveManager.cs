using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class SaveManager: MonoBehaviour
{
    private SaveManager instance;
    public static SaveManager Instance { get; private set; }

    public SaveStruct saves = new();

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
}
