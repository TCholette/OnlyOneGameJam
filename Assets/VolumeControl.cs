using UnityEngine;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] private GameObject _volOffContainer;
    [SerializeField] private GameObject _volOnContainer;

    public void Awake() {
        ProxyFmodPlayer.EnableSound();
        _volOffContainer.SetActive(true);
        _volOnContainer.SetActive(false);
    }
    public void ToggleVolume() {
        if (_volOffContainer.activeSelf) {
            _volOffContainer.SetActive(false);
            _volOnContainer.SetActive(true);
            ProxyFmodPlayer.DisableSound();
        } else if (_volOnContainer.activeSelf) {
            _volOnContainer.SetActive(false);
            _volOffContainer.SetActive(true);
            ProxyFmodPlayer.EnableSound();
        }
    }

}
