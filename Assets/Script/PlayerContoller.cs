using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;
    [Header("UI Settings")]
    public TextMeshProUGUI grenadeCountText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI ammoCountText;
    public TextMeshProUGUI weaponText;
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 9f;
    public float jumpForce = 7f;
    public float gravity = -9.81f;

    [Header("health Settings")]
    public int maxHealth = 3;
    public int currentHealth;

    [Header("Audio Settings")]
     public GameObject hitSFX;
     public GameObject GrenadeThrowSFX;
     public GameObject treasureCollectSFX;
     public GameObject weaponPickupSFX;


    [Header("Interaction Settings")]
    public float interactionRange = 3f;
    private int interact;
    private CharacterController cc;
    private Vector3 velocity;
    private bool isGrounded;
    public int grenadeCount;
    public int treasureBoxCount;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction runAction;
    private InputAction interactAction;
    private InputAction switchAction;
    private InputAction grenadeAction;
    private Vector2 moveInput;
    private bool jumpInput;
    private bool runInput;


    // Interaction variables
    public bool isAddinAmmo;
    private GameObject currentInteractable;

    public WeaponManager weaponManager;

    public GameObject projctilePrefab;

    public Transform shootPoint;
    private GameObject weapon;

    private GameObject Ammo;
    private GameObject HealthPack;
    private GameObject GrenadePack;

    private bool canInteract = false;

    public GameObject granadePrefab;

   
    private void Awake()
    {
        
        grenadeCount = 3;
        healthText.text = "Health: " + maxHealth.ToString();
        grenadeCountText.text = "Grenades: " + grenadeCount.ToString();
        
        instance = this;
        isAddinAmmo = false;
        cc = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        currentHealth = maxHealth;
        
        // Get input actions
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        runAction = playerInput.actions["Sprint"];
        interactAction = playerInput.actions["Interact"];
        switchAction = playerInput.actions["Switch"];
        grenadeAction = playerInput.actions["Grenade"];
        

    }

    private void OnEnable()
    {
        // Subscribe to input events
        jumpAction.performed += OnJumpPerformed;
        interactAction.performed += OnInteractPerformed;
        //shootAction.performed += OnShootPerformed;
        switchAction.performed += OnSwitchPerformed;
        grenadeAction.performed += OnGrenadePerformed;
    }

    private void OnDisable()
    {
        // Unsubscribe from input events
        jumpAction.performed -= OnJumpPerformed;
        interactAction.performed -= OnInteractPerformed;
        // shootAction.performed -= OnShootPerformed;
        switchAction.performed -= OnSwitchPerformed;
        grenadeAction.performed -= OnGrenadePerformed;
    }

    private void Update()
    {
        
        MovePlayer();
    }

    private void MovePlayer()
    {
        // Read input values
        moveInput = moveAction.ReadValue<Vector2>();
        runInput = runAction.ReadValue<float>() > 0.1f;

        isGrounded = cc.isGrounded;

        // Reset vertical velocity when grounded
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // Calculate movement speed
        float speed = runInput ? runSpeed : walkSpeed;

        // Calculate movement direction
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        cc.Move(move * speed * Time.deltaTime);

        // Apply jump
        if (jumpInput && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            jumpInput = false;
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);
    }
    
    private void OnGrenadePerformed(InputAction.CallbackContext context)
    {
        if(grenadeCount>0){
        
        GameObject grenadeAudioInstance = Instantiate(GrenadeThrowSFX, shootPoint.position, shootPoint.rotation);
        Destroy(grenadeAudioInstance, 1f);
        Instantiate(granadePrefab, shootPoint.position, shootPoint.rotation);
        
        Debug.Log("Grenade thrown!");
        grenadeCount--;

        }
        else{
            Debug.Log("No grenades left!");}
        grenadeCountText.text = "Grenades: " + grenadeCount.ToString();
    }
    private void OnShootPerformed(InputAction.CallbackContext context)
    {
        
        Instantiate(projctilePrefab, shootPoint.position, shootPoint.rotation);
        Debug.Log("Shoot action performed!");
        // For example, instantiate a projectile at the weapon's shoot point
    }
    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        jumpInput = true;
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        if (canInteract == true && currentInteractable != null)
        {
            Debug.Log("Interacted with Treasure Box! Destroying in 2 seconds...");


            // Destroy after 2 seconds (allows for animations/effects)
            Destroy(currentInteractable, 2f);

            
            GameObject treasureAudioInstance = Instantiate(treasureCollectSFX, currentInteractable.transform.position, Quaternion.identity);
            Destroy(treasureAudioInstance, 2f);
            
            currentInteractable = null;
            canInteract = false;
            treasureBoxCount++;
        }
        if (canInteract == true && weapon != null)
        {

            interact++;
            Debug.Log(interact);
            GameObject weaponAudioInstance = Instantiate(weaponPickupSFX, weapon.transform.position, Quaternion.identity);
            Destroy(weaponAudioInstance, 2f);
            Debug.Log("Picked up Weapon!");
            // Add logic to add weapon to inventory here
            weaponManager.SpawnWeapon(weaponManager.currentWeaponIndex + 1);

            weapon.SetActive(false); // Hide the weapon in the world
            weapon = null;
            UpdateWeaponUI();

        }
       
    }
    
    private void OnSwitchPerformed(InputAction.CallbackContext context)
    {
       
        if (interact < 2)
        {

            Debug.Log("No weapon to switch to!");

        }
        else
        {
            weaponManager.SpawnWeapon((weaponManager.currentWeaponIndex + 1) % weaponManager.currentWeapon.Length);
            UpdateWeaponUI();
            Debug.Log("Switched Weapon!");
        }
        
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TreasureBox"))
        {
            Debug.Log("Can interact with Treasure Box! Press Interact key.");
            currentInteractable = other.gameObject;
            canInteract = true;
        }
        if(other.CompareTag("Weapon"))
        {
            Debug.Log("Can pick up Weapon! Press Interact key.");
            weapon = other.gameObject;
            canInteract = true;
        }
        if (other.CompareTag("Ammo"))
    {
        Debug.Log("Picked up Ammo!");

        // Use currentWeaponIndex to get the active weapon
        WeaponMechs activeWeapon = weaponManager.currentWeapon[weaponManager.currentWeaponIndex]
                                   .GetComponent<WeaponMechs>();

        if (activeWeapon != null)
        {
            // Only add ammo if it’s not already full
            if (activeWeapon.ammoCount < activeWeapon.maxAmmo)
            {
                activeWeapon.AddAmmo(activeWeapon.maxAmmo);
                Debug.Log("Ammo added to: " + activeWeapon.name);
                Destroy(other.gameObject); // Destroy the ammo pickup
            }
            else
            {
                Debug.Log("Ammo is already full for: " + activeWeapon.name);
            }
        }

        
    }
        if(other.CompareTag("HP") && currentHealth < maxHealth)
        {
            Debug.Log("Picked up Health Pack!");
            HealthPack = other.gameObject;
            currentHealth++; // Increase health but not exceed max
            healthText.text = "Health: " + currentHealth.ToString();
            Destroy(HealthPack);
        }
        if(other.CompareTag("Grenade_BOX"))
        {
            Debug.Log("Picked up Grenade Pack!");
            GrenadePack = other.gameObject;
            grenadeCount += 3; // Increase grenade count
            grenadeCountText.text = "Grenades: " + grenadeCount.ToString();
            Destroy(GrenadePack);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TreasureBox"))
        {
            Debug.Log("Left Treasure Box interaction range.");
            currentInteractable = null;
            canInteract = false;
        }
        if (other.CompareTag("Weapon"))
        {
            Debug.Log("Left Weapon pickup range.");
            weapon = null;
            canInteract = false;
        }
         if(other.CompareTag("Ammo"))
        {
            Debug.Log("Picked up Ammo!");
            isAddinAmmo = false;
          
        }
        
       
       
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EnnemyProjectile"))
        {
            GameObject hitInstance = Instantiate(hitSFX, collision.contacts[0].point, Quaternion.identity);
            Destroy(hitInstance, 1f);
            currentHealth -= 1;
            Debug.Log("Player hit! Current Health: " + currentHealth);
            Destroy(collision.gameObject); // Destroy the projectile on hit
            healthText.text = "Health: " + currentHealth.ToString();
            if (currentHealth <= 0)
            {
                Debug.Log("Player Dead!");
                // Add death handling logic here (e.g., respawn, game over screen)
            }
            

        }
         if(collision.gameObject.CompareTag("Grenade"))
        {
            Debug.Log("Grenade explosion range.");
            currentHealth = 0;
            GameObject hitInstance = Instantiate(hitSFX, collision.contacts[0].point, Quaternion.identity);
            Destroy(hitInstance, 1f);
        }
         
    }

  public void UpdateWeaponUI()
{
    if (weaponManager.currentWeapon.Length == 0)
    {
        weaponText.text = "Weapon: None";
        return;
    }

    int activeIndex = weaponManager.currentWeaponIndex;
    string weaponName = weaponManager.currentWeapon[activeIndex].name.Replace("(Clone)", "");
    weaponText.text = "Weapon: " + weaponName;
}


}