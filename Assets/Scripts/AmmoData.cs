using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class AmmoData
{
    [Header("Gun Ammo:")]
    public int ak47Ammo;
    public int ar15Ammo;
    public int barretAmmo;
    public int scarAmmo;
    public int glockAmmo;
    public int makarovAmmo;
    public int rpgAmmo;
    public int shotgunAmmo;

    [Header("Throwable Ammo:")]
    public int grenades;
}
