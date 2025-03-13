using System;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public AIType aiType;
    public NavMeshAgent agent;
    public Animator anim;
    public Transform LeavTransform;
    public Transform assignedTable;
    public Transform assignedDeliveryStation;
    public event Action<AIController> OnAIDestroyed;

    public float actionDistance;

    private Transform FoodServingSlot = null;

    // Local Variable
    private bool isTeleported = false;
    private Transform GO_T;
    private bool Leaving = false;

    private void Start()
    {

        agent = GetComponent<NavMeshAgent>();

        if (assignedDeliveryStation != null) {
            assignedDeliveryStation.GetComponent<ActionManager>().AI_instance = this;
            GO_T = assignedDeliveryStation.GetComponent<ActionManager>().GO_Position;
        }
        if (assignedTable != null) {
            assignedTable.GetComponent<ActionManager>().AI_instance = this;
            GO_T = assignedTable.GetComponent<ActionManager>().GO_Position;
        }

    }

    private void Update()
    {
        if (agent.hasPath)
        {
            if (agent.remainingDistance > actionDistance)
            {
                anim.SetBool("Walking", true);
            }
            else
            {
                if (Leaving) {
                    Destroy(this.gameObject, 1.0f);
                }
                anim.SetBool("Walking", false);
            }
        }
        else {
            anim.SetBool("Walking", false);
        }

        CheckArrivalAndTeleport();
    }

    private void CheckArrivalAndTeleport()
    {
        if (isTeleported || assignedTable == null) return;

        if (agent.hasPath)
        {
            float distance = agent.remainingDistance;
            if (distance < actionDistance)  // Adjust this distance threshold as needed
            {
                ActionManager actionManager = assignedTable.GetComponent<ActionManager>();
                if (actionManager != null)
                {
                    Transform waitingPlace = actionManager.ReturnWaitingPlace();
                    
                    FoodServingSlot = actionManager.GetFoodSlotTransform(waitingPlace);

                    if (waitingPlace != null)
                    {
                        agent.enabled = false;
                        TeleportToPosition(waitingPlace);
                        isTeleported = true;
                        anim.SetBool("Sitting", true);
                    }
                }
                this.GetComponent<AIFoodManager>().StartFoodWait();
            }
        }
    }

    private void TeleportToPosition(Transform T)
    {
        transform.parent = T;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    public void LeaveRestuarent() {
        transform.position = GO_T.position;
        transform.rotation = GO_T.rotation;
        anim.SetBool("Sitting", false);
        agent.enabled = true; 
        agent.SetDestination(LeavTransform.position);
        FoodServingSlot = null;
        Leaving = true;
    }

    private void OnDestroy()
    {
        if (OnAIDestroyed != null)
        {
            OnAIDestroyed(this);
        }
    }

    public Vector3 GetFoodServingSlot() 
    {
        if(FoodServingSlot == null) return Vector3.zero;
        return FoodServingSlot.position;
    }
}