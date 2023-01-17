using DarkTreeFPS;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Ammo IDs
    private const int
        ak47AmmoID = 1930692970,
        ar15AmmoID = 409415247,
        barretAmmoID = 2130079816,
        scarAmmoID = 865834413,
        glockAmmoID = 1870401757,
        makarovAmmoID = 907915441,
        rpgAmmoID = 1946995732,
        shotgunAmmoID = 1671565074;
    #endregion

    [Header("Player Weapons:")]
    public Weapon primaryWeapon;
    public Weapon secondaryWeapon;
    public Weapon pistol;

    public KeyCode PrimaryWeaponCode = KeyCode.Alpha1;
    public KeyCode SecondaryWeaponCode = KeyCode.Alpha2;
    public KeyCode PistolWeaponCode = KeyCode.Alpha3;

    [Header("Maximum Ammo Storage:")]
    [Tooltip("Starts at 4x the mag capacity")]
    public AmmoData maximumAmmos = new AmmoData
    {
        ak47Ammo = 30 * 4,
        ar15Ammo = 30 * 4,
        barretAmmo = 10 * 4,
        scarAmmo = 30 * 4,
        glockAmmo = 12 * 4,
        makarovAmmo = 8 * 4,
        rpgAmmo = 1 * 4,
        shotgunAmmo = 8 * 4,
        grenades = 2 // only 2 grenades of 
    };

    [Header("Ammo in Inventory:")]
    public AmmoData ammo;

    [Header("Weapon Drop Stuff")]
    public Transform player;
    public int distanceAwayFromPlayerToDrop = 1;

    public static Inventory inventorySingleton;
    private WeaponManager weaponManager;

    private DTInventory.Item primaryWeaponItem;
    private DTInventory.Item secondaryWeaponItem;
    private DTInventory.Item pistolItem;

    private System.Collections.Generic.List<string> weaponsPickedUp;

    private void Awake()
    {
        if (inventorySingleton == null) inventorySingleton = this;
        else Destroy(this);

        // load this, along with current weapons
        ammo = new AmmoData();

       // ammo.ak47Ammo = 30;
       // ammo.ar15Ammo = 30;
       // ammo.barretAmmo = 10;
       // ammo.scarAmmo = 30;
       // ammo.glockAmmo = 12;
       // ammo.makarovAmmo = 8;
       // ammo.rpgAmmo = 1;
       // ammo.shotgunAmmo = 8;
       // ammo.grenades = 0;

        weaponManager = FindObjectOfType<WeaponManager>();

        primaryWeaponItem = null;
        secondaryWeaponItem = null;

        if (!player) player = GameObject.FindGameObjectWithTag("Player").transform;

        weaponsPickedUp = new System.Collections.Generic.List<string>();
    }


    public static int GetAmmo(int ammoID)
    {
        switch (ammoID) {
            case ak47AmmoID:
                return inventorySingleton.ammo.ak47Ammo;
            case ar15AmmoID:
                return inventorySingleton.ammo.ar15Ammo;
            case barretAmmoID:
                return inventorySingleton.ammo.barretAmmo;
            case scarAmmoID:
                return inventorySingleton.ammo.scarAmmo;
            case glockAmmoID:
                return inventorySingleton.ammo.glockAmmo;
            case makarovAmmoID:
                return inventorySingleton.ammo.makarovAmmo;
            case rpgAmmoID:
                return inventorySingleton.ammo.rpgAmmo;
            case shotgunAmmoID:
                return inventorySingleton.ammo.shotgunAmmo;
            default:
                return 0;
        }
    }

    public static void UseBullets(int ammoID, int amount)
    {
        switch (ammoID)
        {
            case ak47AmmoID:
                inventorySingleton.ammo.ak47Ammo -= amount;
                break;
            case ar15AmmoID:
                inventorySingleton.ammo.ar15Ammo -= amount;
                break;
            case barretAmmoID:
                inventorySingleton.ammo.barretAmmo -= amount;
                break;
            case scarAmmoID:
                inventorySingleton.ammo.scarAmmo -= amount;
                break;
            case glockAmmoID:
                inventorySingleton.ammo.glockAmmo -= amount;
                break;
            case makarovAmmoID:
                inventorySingleton.ammo.makarovAmmo -= amount;
                break;
            case rpgAmmoID:
                inventorySingleton.ammo.rpgAmmo -= amount;
                break;
            case shotgunAmmoID:
                inventorySingleton.ammo.shotgunAmmo -= amount;
                break;
            default:
                break;
        }
    }

    public static void PickupItem(DTInventory.Item item)
    {
        if (item.type.Equals("Ammo"))
        {            
            GainAmmo(item);

            // Ammo cannot be dropped, so just destroy the pickup
            //Destroy(item.gameObject);
        }
        else if (item.type.Equals("Rifle"))
        {
            // If the weapon is already equipped, just take its ammo
            Weapon newWeapon = inventorySingleton.weaponManager.GetWeapon(item);
           
            /*if ((inventorySingleton.primaryWeapon?.weaponName.Equals(newWeapon.weaponName) ?? false) || (inventorySingleton.secondaryWeapon?.weaponName.Equals(newWeapon.weaponName) ?? false))
            {
                //   GainAmmo(newWeapon.ammoItemID, newWeapon.maxAmmo);
                item.stackSize -= GainAmmo(newWeapon.ammoItemID, item.stackSize);
                
                if (item.stackSize == 0) Destroy(item.gameObject);

                return;
            }

            // if it the first time picking up a weapon, get a full mag-worth of ammo
            if (!inventorySingleton.weaponsPickedUp.Contains(newWeapon.weaponName))
            {
                // increase that weapon's ammo
                inventorySingleton.weaponsPickedUp.Add(newWeapon.weaponName);

            } */
            
            // always gain ammo if available
            item.stackSize -= GainAmmo(newWeapon.ammoItemID, item.stackSize);
            
            if ((inventorySingleton.primaryWeapon?.weaponName.Equals(newWeapon.weaponName) ?? false) || (inventorySingleton.secondaryWeapon?.weaponName.Equals(newWeapon.weaponName) ?? false))
            {
                print("that weapon is already held!");

                // only destroy a weapon item if it is emptied and not needed from dropping later
                if (item.stackSize == 0) Destroy(item.gameObject);

                return;
            }

            if (!inventorySingleton.primaryWeapon)
            {
                inventorySingleton.primaryWeapon = inventorySingleton.weaponManager.GetWeapon(item); // pick up first weapon
                inventorySingleton.weaponManager.SwapToWeapon(1, true);
                inventorySingleton.primaryWeaponItem = item;                                
            }
            else if (!inventorySingleton.secondaryWeapon)
            {
                inventorySingleton.secondaryWeapon = inventorySingleton.weaponManager.GetWeapon(item); // pick up second weapon
                inventorySingleton.weaponManager.SwapToWeapon(2, true);
                inventorySingleton.secondaryWeaponItem = item;
            }
            else
            {                
                // swap current weapon for new weapon!
                if (inventorySingleton.primaryWeapon.gameObject.activeInHierarchy)
                {
                    // gain what ammo you can, and clear out the weapon
                    GainAmmo(inventorySingleton.primaryWeapon.ammoItemID, inventorySingleton.primaryWeapon.GetAmmoInMag());
                    DropWeapon(inventorySingleton.primaryWeaponItem);
                    inventorySingleton.primaryWeapon.ClearAmmoInMag();

                    inventorySingleton.primaryWeapon = inventorySingleton.weaponManager.GetWeapon(item);
                    inventorySingleton.weaponManager.SwapToWeapon(1, true);

                    inventorySingleton.primaryWeaponItem = item;
                }
                else if (inventorySingleton.secondaryWeapon.gameObject.activeInHierarchy)
                {
                    // gain what ammo you can, and clear out the weapon
                    GainAmmo(inventorySingleton.secondaryWeapon.ammoItemID, inventorySingleton.secondaryWeapon.GetAmmoInMag());
                    DropWeapon(inventorySingleton.secondaryWeaponItem);
                    inventorySingleton.secondaryWeapon.ClearAmmoInMag();

                    inventorySingleton.secondaryWeapon = inventorySingleton.weaponManager.GetWeapon(item);
                    inventorySingleton.weaponManager.SwapToWeapon(2, true);

                    inventorySingleton.secondaryWeaponItem = item;
                }
            }

            // parent pickup under this object and disable so it can be dropped later
            item.transform.parent = inventorySingleton.transform;
            item.gameObject.SetActive(false);
        }
        else if (item.type.Equals("Pistol"))
        {
            if (inventorySingleton.pistol)
            {
                // gain the ammo you can, and clear out the weapon
                GainAmmo(inventorySingleton.pistol.ammoItemID, inventorySingleton.pistol.GetAmmoInMag());
                DropWeapon(inventorySingleton.pistolItem);
                inventorySingleton.pistol.ClearAmmoInMag();
            }

            
            inventorySingleton.pistol = inventorySingleton.weaponManager.GetWeapon(item);
            inventorySingleton.pistolItem = item;

            inventorySingleton.weaponManager.SwapToWeapon(3, true);

            // parent pickup under this object and disable so it can be dropped later
            item.transform.parent = inventorySingleton.transform;
            item.gameObject.SetActive(false);
        }
    }

    private static void DropWeapon(DTInventory.Item weaponItem)
    {
        weaponItem.gameObject.SetActive(true);
        weaponItem.transform.position = inventorySingleton.player.position + inventorySingleton.player.forward * inventorySingleton.distanceAwayFromPlayerToDrop;
        weaponItem.transform.parent = null;

        // drop with 0 stack size, but get all ammo back from it
        weaponItem.stackSize = 0;
    }

    // TODO - Show HUD on right with weapon icon in each equipped slot e.g.
    // 1 - <primary weapon icon> <ammo in mag> | <ammo for weapon>
    // 2 - <secondary weapon icon> <ammo in mag> | <ammo for weapon>
    // 3 - <pistol weapon icon> <ammo in mag> | <ammo for weapon>
    
    // TODO - Add health kits (already exist, but add to this inventory)


    /// <summary>
    /// Adds ammo to the specified weapon
    /// </summary>
    /// <param name="ammoItemID"></param>
    /// <param name="amount"></param>
    /// <returns>Was the ammo actually gained?</returns>
    private static int GainAmmo(int ammoItemID, int amount)
    {
        int ammoInMagBefore = GetAmmo(ammoItemID);

        switch (ammoItemID)
        {
            case ak47AmmoID:
                inventorySingleton.ammo.ak47Ammo = Mathf.Clamp(inventorySingleton.ammo.ak47Ammo + amount, 0, inventorySingleton.maximumAmmos.ak47Ammo);
                break;
            case ar15AmmoID:
                inventorySingleton.ammo.ar15Ammo = Mathf.Clamp(inventorySingleton.ammo.ar15Ammo + amount, 0, inventorySingleton.maximumAmmos.ar15Ammo);
                break;
            case barretAmmoID:
                inventorySingleton.ammo.barretAmmo = Mathf.Clamp(inventorySingleton.ammo.barretAmmo + amount, 0, inventorySingleton.maximumAmmos.barretAmmo);
                break;
            case glockAmmoID:
                inventorySingleton.ammo.glockAmmo = Mathf.Clamp(inventorySingleton.ammo.glockAmmo + amount, 0, inventorySingleton.maximumAmmos.glockAmmo);
                break;
            case makarovAmmoID:
                inventorySingleton.ammo.makarovAmmo = Mathf.Clamp(inventorySingleton.ammo.makarovAmmo + amount, 0, inventorySingleton.maximumAmmos.makarovAmmo);
                break;
            case rpgAmmoID:
                inventorySingleton.ammo.rpgAmmo = Mathf.Clamp(inventorySingleton.ammo.rpgAmmo + amount, 0, inventorySingleton.maximumAmmos.rpgAmmo);
                break;
            case scarAmmoID:
                inventorySingleton.ammo.scarAmmo = Mathf.Clamp(inventorySingleton.ammo.scarAmmo + amount, 0, inventorySingleton.maximumAmmos.scarAmmo);
                break;
            case shotgunAmmoID:
                inventorySingleton.ammo.shotgunAmmo = Mathf.Clamp(inventorySingleton.ammo.shotgunAmmo + amount, 0, inventorySingleton.maximumAmmos.shotgunAmmo);
                break;
        }

        int ammoInMagAfter = GetAmmo(ammoItemID);

        // return the amount of ammo taken
        return ammoInMagAfter - ammoInMagBefore;
    }

    private static void GainAmmo(DTInventory.Item ammoItem)
    {
        int amountToTake;
        
        switch (ammoItem.id)
        {
            case ak47AmmoID:
                // difference
                amountToTake= inventorySingleton.maximumAmmos.ak47Ammo - inventorySingleton.ammo.ak47Ammo;

                // cant take more than the stack
                amountToTake = Mathf.Clamp(amountToTake, 0, ammoItem.stackSize);

                inventorySingleton.ammo.ak47Ammo += amountToTake;
                ammoItem.stackSize -= amountToTake;

                break;
            case ar15AmmoID:
                amountToTake = inventorySingleton.maximumAmmos.ar15Ammo - inventorySingleton.ammo.ar15Ammo;

                amountToTake = Mathf.Clamp(amountToTake, 0, ammoItem.stackSize);

                inventorySingleton.ammo.ar15Ammo += amountToTake;
                ammoItem.stackSize -= amountToTake;
                
                break;
            case barretAmmoID:
                amountToTake = inventorySingleton.maximumAmmos.barretAmmo - inventorySingleton.ammo.barretAmmo;

                amountToTake = Mathf.Clamp(amountToTake, 0, ammoItem.stackSize);

                inventorySingleton.ammo.barretAmmo += amountToTake;
                ammoItem.stackSize -= amountToTake;

                break;
            case glockAmmoID:
                amountToTake = inventorySingleton.maximumAmmos.glockAmmo - inventorySingleton.ammo.glockAmmo;

                amountToTake = Mathf.Clamp(amountToTake, 0, ammoItem.stackSize);

                inventorySingleton.ammo.glockAmmo += amountToTake;
                ammoItem.stackSize -= amountToTake;

                break;
            case makarovAmmoID:
                amountToTake = inventorySingleton.maximumAmmos.makarovAmmo - inventorySingleton.ammo.makarovAmmo;

                amountToTake = Mathf.Clamp(amountToTake, 0, ammoItem.stackSize);

                inventorySingleton.ammo.makarovAmmo += amountToTake;
                ammoItem.stackSize -= amountToTake;

                break;
            case rpgAmmoID:
                amountToTake = inventorySingleton.maximumAmmos.rpgAmmo - inventorySingleton.ammo.rpgAmmo;

                amountToTake = Mathf.Clamp(amountToTake, 0, ammoItem.stackSize);

                inventorySingleton.ammo.rpgAmmo += amountToTake;
                ammoItem.stackSize -= amountToTake;

                break;
            case scarAmmoID:
                amountToTake = inventorySingleton.maximumAmmos.scarAmmo - inventorySingleton.ammo.scarAmmo;

                amountToTake = Mathf.Clamp(amountToTake, 0, ammoItem.stackSize);

                print("amount to take: " + amountToTake);

                inventorySingleton.ammo.scarAmmo += amountToTake;
                ammoItem.stackSize -= amountToTake;

                break;
            case shotgunAmmoID:
                amountToTake = inventorySingleton.maximumAmmos.shotgunAmmo - inventorySingleton.ammo.shotgunAmmo;

                amountToTake = Mathf.Clamp(amountToTake, 0, ammoItem.stackSize);

                inventorySingleton.ammo.shotgunAmmo += amountToTake;
                ammoItem.stackSize -= amountToTake;

                break;
        }

        if (ammoItem.stackSize == 0) Destroy(ammoItem.gameObject);
    }
}
