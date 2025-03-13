using UnityEngine;

public class RotateToTarget : MonoBehaviour
{
    public Transform targetTransform;
    public Transform rotatingObject; // The object that will actually rotate
    public float rotationSpeed = 5f;
    public float activationDistance = 10f; // Distance to activate the rotation
    public float deactivationDistance = 12f; // Distance to deactivate (can be larger for hysteresis)

    private void Start()
    {

        targetTransform = GameObject.FindGameObjectWithTag("Player").transform;

        if (rotatingObject == null)
        {
            Debug.LogError("Rotating Object Transform not assigned!");
            enabled = false; // Disable the script if the rotating object isn't set.
            return;
        }

        if (targetTransform == null)
        {
            Debug.LogError("Target Transform not assigned!");
            enabled = false;
            return;
        }

    }


    private void Update()
    {
        float distanceToTarget = Vector3.Distance(transform.position, targetTransform.position);

        // Enable/Disable based on distance
        rotatingObject.gameObject.SetActive(distanceToTarget <= activationDistance);

        if (distanceToTarget <= activationDistance)
        {
            // Calculate the direction vector towards the target.
            Vector3 targetDirection = targetTransform.position - transform.position;

            // Ensure we only rotate on the Y-axis.
            targetDirection.y = 0f;

            if (targetDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                rotatingObject.rotation = Quaternion.Slerp(rotatingObject.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                //OR RotateTowards
                //rotatingObject.rotation = Quaternion.RotateTowards(rotatingObject.rotation, targetRotation, rotationSpeed * Time.deltaTime * 360);
            }
        }
        else if (distanceToTarget > deactivationDistance)
        {
            rotatingObject.gameObject.SetActive(false); // Ensure it's off if beyond deactivation distance.
        }
    }
}