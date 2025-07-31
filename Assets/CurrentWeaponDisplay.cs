using UnityEngine;

public class CurrentWeaponDisplay : MonoBehaviour
{
    [SerializeField] private GameObject _sword;
    [SerializeField] private GameObject _spear;
    [SerializeField] private GameObject _shield;

    public void SelectWeapon(Weapon weapon) {
        switch (weapon) {
            case Weapon.Sword: 
                _sword.SetActive(true);
                _spear.SetActive(false);
                _shield.SetActive(false);
                break;
            case Weapon.Spear:
                _sword.SetActive(false);
                _spear.SetActive(true);
                _shield.SetActive(false);
                break;
            case Weapon.Shield:
                _sword.SetActive(false);
                _spear.SetActive(false);
                _shield.SetActive(true);
                break;
        }
    }
}
