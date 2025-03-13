using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AISpawner : MonoBehaviour
{
    [Header("Settings")]
    public GameObject aiPrefab;              // Prefab of the AI agent
    public Transform spawnPoint;
    public Transform LeavePoint;
    public float spawnInterval = 5.0f;       // Time interval between spawns
    public float dineInProbability = 0.5f;   // Probability of spawning a DineIn AI (0 to 1)
    public float aiLifetime = 30.0f;         // Lifetime of each AI agent

    [Header("AI Instance References")]
    public List<GameObject> aiAgents = new List<GameObject>();  // List to manage AI agents

    [Header("Table Management")]
    public Transform[] availableTables;      // Array of available tables
    private List<Transform> occupiedTables = new List<Transform>();  // List of occupied tables

    [Header("Delivery Station Management")]
    public Transform[] availableDeliveryStations;  // Array of available delivery stations
    private List<Transform> occupiedDeliveryStations = new List<Transform>();  // List of occupied delivery stations

    void Start()
    {
        // Start the spawning coroutine
        StartCoroutine(SpawnAICoroutine());
    }

    void Update()
    {
        // Update the AI agents (e.g., manage their behavior, remove expired ones)
        ManageAIAgents();
    }

    private void TrySpawnAI()
    {
        if (HasAvailability())
        {
            GameObject aiAgent = Instantiate(aiPrefab, spawnPoint.position, Quaternion.identity);
            aiAgents.Add(aiAgent);
            AssignAIDestination(aiAgent);
        }
    }

    private bool HasAvailability()
    {
        return availableTables.Length > 0 || availableDeliveryStations.Length > 0;
    }

    private void AssignAIDestination(GameObject aiAgent)
    {
        AIController aiController = aiAgent.GetComponent<AIController>();
        if (aiController != null)
        {
            aiController.LeavTransform = LeavePoint;
            aiController.OnAIDestroyed += HandleAIDestroyed;  // Subscribe to the event
            aiController.aiType = GetRandomAIType();
            Transform destination = null;

            if (aiController.aiType == AIType.DineIn)
            {
                if (availableTables.Length > 0)
                {
                    destination = GetRandomAvailableTable();
                    aiController.assignedTable = destination;
                }
                else if (availableDeliveryStations.Length > 0)
                {
                    aiController.aiType = AIType.TakeAway;
                    destination = GetRandomAvailableDeliveryStation();
                    aiController.assignedDeliveryStation = destination;
                }
            }
            else if (aiController.aiType == AIType.TakeAway)
            {
                if (availableDeliveryStations.Length > 0)
                {
                    destination = GetRandomAvailableDeliveryStation();
                    aiController.assignedDeliveryStation = destination;
                }
                else if (availableTables.Length > 0)
                {
                    aiController.aiType = AIType.DineIn;
                    destination = GetRandomAvailableTable();
                    aiController.assignedTable = destination;
                }
            }

            if (destination != null)
            {
                Transform target = destination.GetComponent<ActionManager>().GO_Position;
                if (target != null)
                {
                    aiController.GetComponent<NavMeshAgent>().SetDestination(target.position);
                }
                else
                {
                    aiController.GetComponent<NavMeshAgent>().SetDestination(destination.position);
                }
            }
            else
            {
                aiAgents.Remove(aiAgent);
                Destroy(aiAgent);
            }
        }
    }

    private void HandleAIDestroyed(AIController aiController)
    {
        if (aiController.aiType == AIType.DineIn && aiController.assignedTable != null)
        {
            occupiedTables.Remove(aiController.assignedTable);
            availableTables = AddToArray(availableTables, aiController.assignedTable);
        }
        else if (aiController.aiType == AIType.TakeAway && aiController.assignedDeliveryStation != null)
        {
            occupiedDeliveryStations.Remove(aiController.assignedDeliveryStation);
            availableDeliveryStations = AddToArray(availableDeliveryStations, aiController.assignedDeliveryStation);
        }
    }

    private AIType GetRandomAIType()
    {
        return (Random.value < dineInProbability) ? AIType.DineIn : AIType.TakeAway;
    }

    private IEnumerator SpawnAICoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            TrySpawnAI();
        }
    }

    private Transform GetRandomAvailableTable()
    {
        if (availableTables.Length == 0) return null;

        int index = Random.Range(0, availableTables.Length);
        Transform table = availableTables[index];
        occupiedTables.Add(table);
        availableTables = RemoveFromArray(availableTables, table);

        return table;
    }

    private Transform GetRandomAvailableDeliveryStation()
    {
        if (availableDeliveryStations.Length == 0) return null;

        int index = Random.Range(0, availableDeliveryStations.Length);
        Transform deliveryStation = availableDeliveryStations[index];
        occupiedDeliveryStations.Add(deliveryStation);
        availableDeliveryStations = RemoveFromArray(availableDeliveryStations, deliveryStation);

        return deliveryStation;
    }

    private T[] RemoveFromArray<T>(T[] array, T item)
    {
        List<T> list = new List<T>(array);
        list.Remove(item);
        return list.ToArray();
    }

    private T[] AddToArray<T>(T[] array, T item)
    {
        List<T> list = new List<T>(array);
        list.Add(item);
        return list.ToArray();
    }

    private void ManageAIAgents()
    {
        // Example: Remove any null AI agents from the list
        aiAgents.RemoveAll(agent => agent == null);
    }
}
