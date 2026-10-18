using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class WeaponMechs : MonoBehaviour
{
    private InputAction shootAction;
    private PlayerInput playerInput;
    public int ammoCount;
    public int maxAmmo;
    public GameObject audioSource;
    public TextMeshProUGUI ammoCountText;

    [Header("Shoot Settings")]
    public Transform[] shootPoints;       // Multiple shoot points
    public GameObject projectilePrefab;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        shootAction = playerInput.actions["Shoot"];
    }

    private void OnEnable()
    {
        // Subscribe to input events
        shootAction.performed += OnShootPerformed;

        // Automatically find the UI Text component if not assigned
        if (ammoCountText == null)
        {
            ammoCountText = GameObject.Find("Ammo")?.GetComponent<TextMeshProUGUI>();
            // Replace "Ammo" with the name of your UI Text object in the Canvas
        }

        UpdateAmmoUI();
    }

    private void OnDisable()
    {
        // Unsubscribe from input events
        shootAction.performed -= OnShootPerformed;

        // When the gun is disabled, display 0 ammo
        if (ammoCountText != null)
        {
            ammoCountText.text = "Ammo Left: 0";
        }
    }

    void Update()
    {
        if (PlayerMovement.instance != null && PlayerMovement.instance.isAddinAmmo == true && ammoCount < maxAmmo)
        {
            AddAmmo(maxAmmo);
        }
        else if (PlayerMovement.instance != null)
        {
            PlayerMovement.instance.isAddinAmmo = false;
        }
    }

    public void Shoot()
    {
        if (ammoCount > 0)
        {
            if (projectilePrefab == null || shootPoints.Length == 0) return;

            foreach (Transform point in shootPoints)
            {
                Instantiate(projectilePrefab, point.position, point.rotation);
                GameObject audioInstance = Instantiate(audioSource, point.position, point.rotation);
                Destroy(audioInstance, 0.5f);
                ammoCount--;
            }

            UpdateAmmoUI();
            Debug.Log("Ammo left: " + ammoCount);
        }
        if(ammoCount <= 0)
        {
            ammoCount = 0;
            UpdateAmmoUI();
            Debug.Log("Out of ammo!");
        }
    }

    void OnShootPerformed(InputAction.CallbackContext context)
    {
        Shoot();
        Debug.Log("Player Shoot!");
    }

    public void AddAmmo(int amount)
    {
        ammoCount = amount;
        UpdateAmmoUI();
        Debug.Log("Ammo added. Current ammo: " + ammoCount);
    }

    private void UpdateAmmoUI()
    {
        if (ammoCountText != null)
        {
            ammoCountText.text = "Ammo Left: " + ammoCount.ToString();
        }
    }
}
