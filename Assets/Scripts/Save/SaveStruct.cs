using System.Collections.Generic;
using UnityEngine;
public struct SaveStruct
{
    public int level;
    public List<Weapon> weapons;
    public Weapon currentWeapon;
    public Vector2 lastCheckpoint;
}
