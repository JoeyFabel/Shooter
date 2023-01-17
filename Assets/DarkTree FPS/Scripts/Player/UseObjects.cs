/// DarkTreeDevelopment (2019) DarkTree FPS v1.21
/// If you have any questions feel free to write me at email --- darktreedevelopment@gmail.com ---
/// Thanks for purchasing my asset!

using UnityEngine;
using UnityEngine.UI;
using DTInventory;

namespace DarkTreeFPS
{
    public class UseObjects : MonoBehaviour
    {
        [Tooltip("The distance within which you can pick up item")]
        public float distance = 1.5f;

        private GameObject use;
        private GameObject useCursor;
        private Text useText;

        private InputManager input;
        private DTInventory.DTInventory inventory;

        private Button useButton;

        public static bool useState;

        private WeaponManager weaponManager;
        private SoundManager soundManager;
        private PickupItem pickupItem;

        private void Start()
        {
            useCursor = GameObject.Find("UseCursor");
            if (useCursor != null)
            {
                useText = useCursor.GetComponentInChildren<Text>();
                useCursor.SetActive(false);
            }

            inventory = FindObjectOfType<DTInventory.DTInventory>();
            input = FindObjectOfType<InputManager>();

            weaponManager = FindObjectOfType<WeaponManager>();

            soundManager = FindObjectOfType<SoundManager>();

            pickupItem = FindObjectOfType<PickupItem>();
        }

        void Update()
        {
            Pickup();
        }

        public void Pickup()
        {
            RaycastHit hit;

            //Hit an object within pickup distance
            if (Physics.Raycast(transform.position, transform.forward, out hit, distance))
            {
                if (hit.collider.tag == "Item")
                {
                    useState = true;
                    //Get an item which we want to pickup
                    use = hit.collider.gameObject;
                    useCursor.SetActive(true);

                    Item item = use.GetComponent<Item>();

                    if (item)
                    {                        
                        useText.text = item.title + " (" + item.stackSize + ")";

                        if (Input.GetKeyDown(input.Use))
                        {
                            soundManager.Pickup();
                            //inventory.AddItem(use.GetComponent<Item>());
                            // TODO - Remove Item from ground
                            Inventory.PickupItem(item);
                            print("Picking up item");
                            use = null;
                        }
                    }

                }
                else if (hit.collider.tag == "LootBox")
                {
                    useCursor.SetActive(true);
                    useText.text = "Inspect";
                }
                else
                {
                    useState = false;
                    //Clear use object if there is no an object with "Item" tag
                    use = null;
                    useCursor.SetActive(false);

                    useText.text = "";
                }
            }
            else
            {
                useState = false;

                if (useCursor != null)
                    useCursor.SetActive(false);

                if (useText != null)
                {
                    useText.text = "";
                }
            }
        }
    }
}