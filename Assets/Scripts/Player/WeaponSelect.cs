using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponSelect : MonoBehaviour, IPointerEnterHandler/*, IPointerExitHandler, IPointerClickHandler*/ {

    [SerializeField] private GameObject wheel;
    [SerializeField] private Weapon type;
    [SerializeField] private Player player;
    [SerializeField] private CurrentWeaponDisplay display;
    public void OnPointerEnter(PointerEventData eventData) {
        if (player.LastWheel) {
            player.LastWheel.Deselect();
        }
        player.LastWheel = this;
        GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        switch (type) {
            case Weapon.None:
                break;
            case Weapon.Sword:
                player.Attack = new Slash(player);
                display.SelectWeapon(Weapon.Sword);
                break;
            case Weapon.Spear:
                player.Attack = new Dash(player, 15f, 0.1f, 1f, 0.1f);
                display.SelectWeapon(Weapon.Spear);
                break;
            case Weapon.Shield:
                player.Attack = new Guard(player, 0.5f, 2f, 0.2f, 3f, 15f);
                display.SelectWeapon(Weapon.Shield);
                break;
            default:
                break;
        }
        wheel.transform.eulerAngles = new Vector3(0f, 0f, 120f * (int)type);
    }

    /*public void OnPointerExit(PointerEventData eventData) {
        GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.1f);
        Debug.Log("b");
    }*/

    public void Deselect() {
        GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.1f);
    }

    /*public void OnPointerClick(PointerEventData eventData) {
        switch (type) {
            case Weapon.None:
                break;
            case Weapon.Sword:
                player.Attack = new Slash(player);
                break;
            case Weapon.Spear:
                player.Attack = new Dash(player, 15f, 0.1f, 1f, 0.1f);
                break;
            case Weapon.Shield:
                player.Attack = new Guard(player, 0.5f, 2f, 0.2f, 3f, 15f);
                break;
            case Weapon.Axe:
                break;
            default:
                break;
        }
        wheel.transform.eulerAngles = new Vector3(0f, 0f, 120f*(int)type);
    }*/

}
