using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponSelect : MonoBehaviour, IPointerEnterHandler/*, IPointerExitHandler, IPointerClickHandler*/ {

    [SerializeField] private Weapon type;
    [SerializeField] private Player player;
    [SerializeField] private GameObject display;
    public void OnPointerEnter(PointerEventData eventData) {
        if (player.LastWheel) {
            player.LastWheel.Deselect();
        }
        player.LastWheel = this;
        GetComponent<Image>().color = new Color(GetComponent<Image>().color.r, GetComponent<Image>().color.g, GetComponent<Image>().color.b, 1f);
        switch (type) {
            case Weapon.None:
                break;
            case Weapon.Sword:
                player.Attack = new Slash(player);
                ProxyFmodPlayer.PlaySound<string>("Select", gameObject, new("UI", "Soft"));
                break;
            case Weapon.Spear:
                player.Attack = new Dash(player, 15f, 0.1f, 1f, 0.1f);
                ProxyFmodPlayer.PlaySound<string>("Select", gameObject, new("UI", "Soft"));
                break;
            case Weapon.Shield:
                player.Attack = new Guard(player, 0.5f, 2f, 0.2f, 3f, 15f);
                ProxyFmodPlayer.PlaySound<string>("Select", gameObject, new("UI", "Soft"));
                break;
            default:
                break;
        }
        display.SetActive(true);
    }

    /*public void OnPointerExit(PointerEventData eventData) {
        GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.1f);
        Debug.Log("b");
    }*/

    public void Deselect() {
        GetComponent<Image>().color = new Color(GetComponent<Image>().color.r, GetComponent<Image>().color.g, GetComponent<Image>().color.b, 0.1f);
        display.SetActive(false);
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
