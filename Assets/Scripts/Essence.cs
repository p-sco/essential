using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EssenceArmourUpgrade
{
    Resistance,
    Health

}
public enum EssenceWeaponUpgrade
{
    Speed,
    Damage
}
public enum EssenceSpellUpgrade
{
    SpellPower,
    SpellCD
}

public enum EssenceType
{
    Dark,
    Light
}

[CreateAssetMenu(fileName = "Essence", menuName = "Essence")]
public class Essence : ScriptableObject
{
    

    public EssenceArmourUpgrade ArmourUpgrade = EssenceArmourUpgrade.Resistance;
    public EssenceWeaponUpgrade WeaponUpgrade = EssenceWeaponUpgrade.Damage;
    public EssenceSpellUpgrade SpellUpgrade = EssenceSpellUpgrade.SpellPower;
    public EssenceType type = EssenceType.Dark;
    public float UpgradeWeight;
    public string displayName;
    public Sprite sprite;
    public GameObject prefab;


    public void Drop(Vector2 t)
    {

        Instantiate(prefab, t, Quaternion.identity);
    }
}
