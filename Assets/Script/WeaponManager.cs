using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject[] WeaponInHand;   // Prefabs array
    public GameObject WpSlot;
    public float rotationSmoothness = 5f;

    [Header("Position Offset")]
    public Vector3 positionOffset = new Vector3(0.3f, -0.2f, 0.5f);

    public GameObject[] currentWeapon; // Array of instantiated weapons
    public Transform cameraTransform;

    [Header("Weapon Selection")]
    [Tooltip("Set this index in the Inspector to change weapon")]
    public int currentWeaponIndex = 0; // Current active weapon index

    void Start()
    {
        WpSlot = this.gameObject;

        // Instantiate all weapons once and store in array
        currentWeapon = new GameObject[WeaponInHand.Length];
        for (int i = 0; i < WeaponInHand.Length; i++)
        {
            currentWeapon[i] = Instantiate(
                WeaponInHand[i],
                WpSlot.transform.position,
                WpSlot.transform.rotation,
                WpSlot.transform
            );
            currentWeapon[i].SetActive(false); // Start inactive
        }

       
    }

    void Update()
    {
        if (cameraTransform != null && WpSlot != null)
        {
            Vector3 targetPosition = cameraTransform.position +
                                     cameraTransform.right * positionOffset.x +
                                     cameraTransform.up * positionOffset.y +
                                     cameraTransform.forward * positionOffset.z;

            WpSlot.transform.position = Vector3.Lerp(
                WpSlot.transform.position,
                targetPosition,
                rotationSmoothness * Time.deltaTime
            );

            WpSlot.transform.rotation = Quaternion.Lerp(
                WpSlot.transform.rotation,
                cameraTransform.rotation,
                rotationSmoothness * Time.deltaTime
            );
        }
    }

   void LateUpdate()
{
    if (cameraTransform != null && WpSlot != null)
    {
        // Calculate the target position with offsets
        Vector3 targetPosition = cameraTransform.position +
                                 cameraTransform.right * positionOffset.x +
                                 cameraTransform.up * positionOffset.y +
                                 cameraTransform.forward * positionOffset.z;

        // Move the weapon
        WpSlot.transform.position = targetPosition;

        // Make weapon look forward (aligned with camera's forward direction)
        WpSlot.transform.rotation = cameraTransform.rotation;
    }
}


    // Activate only the selected weapon and deactivate others
    public void SpawnWeapon(int index)
    {
        if (index < 0 || index >= currentWeapon.Length)
        {
            Debug.LogWarning("Weapon index out of range!");
            return;
        }

        // Deactivate all weapons
        for (int i = 0; i < currentWeapon.Length; i++)
        {
            currentWeapon[i].SetActive(false);
        }

        // Activate the selected weapon
        currentWeapon[index].SetActive(true);
        currentWeaponIndex = index;
    }

    // Called in the Editor when you change a value in the Inspector
    private void OnValidate()
    {
        if (Application.isPlaying && currentWeapon != null)
        {
            SpawnWeapon(currentWeaponIndex);
        }
    }
}
