using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Settings")]
    public Transform doorModel;         // Assign your DoorModel here
    public float openAngle = 90f;       // How far it opens
    public float openSpeed = 2f;        // How fast it rotates
    public bool isOpen = false;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        // Save the initial (closed) rotation
        closedRotation = doorModel.localRotation;
        // Calculate the open rotation (rotate around Y)
        openRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);
    }

    void Update()
    {
        // Smoothly rotate between open/closed
        if (isOpen)
            doorModel.localRotation = Quaternion.Slerp(doorModel.localRotation, openRotation, Time.deltaTime * openSpeed);
        else
            doorModel.localRotation = Quaternion.Slerp(doorModel.localRotation, closedRotation, Time.deltaTime * openSpeed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOpen = true;
        }
    }

  
}