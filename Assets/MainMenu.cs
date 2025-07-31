using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] gameElements;

    public void Awake() {
        foreach (GameObject element in gameElements) {
            element.SetActive(false);
        }
    }

    public void StartGame() {
        foreach (GameObject element in gameElements) {
            element.SetActive(true);
        }
        gameObject.SetActive(false);
        ProxyFmodPlayer.PlaySound<string>("Select", gameObject, new("UI", "Start"));
        StaticManager.titleMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        StaticManager.gameMusic.start();
    }

    public void QuitGame() {
        ProxyFmodPlayer.PlaySound<string>("Select", gameObject, new("UI", "Quit"));
        Application.Quit();
    }
}
