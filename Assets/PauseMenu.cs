using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] gameElements;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject menu;
    public void LeaveGame() {
        foreach (GameObject element in gameElements) {
            element.SetActive(false);
        }
        menu.SetActive(true);
        canvas.SetActive(false);
        gameObject.SetActive(false);
        StaticManager.titleMusic.start();
        StaticManager.gameMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
