using UnityEngine;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] private GameObject _volOffContainer;
    [SerializeField] private GameObject _volOnContainer;

    public void Awake() {
        ProxyFmodPlayer.EnableSound();
        _volOffContainer.SetActive(false);
        _volOnContainer.SetActive(true);
    }
    public void ToggleVolume() {
        if (_volOnContainer.activeSelf) {
            _volOnContainer.SetActive(false);
            _volOffContainer.SetActive(true);
            ProxyFmodPlayer.DisableSound();
        } else if (_volOffContainer.activeSelf) {
            _volOffContainer.SetActive(false);
            _volOnContainer.SetActive(true);
            ProxyFmodPlayer.EnableSound();
        }
    }

}
