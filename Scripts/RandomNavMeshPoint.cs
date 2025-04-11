using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class RandomNavMeshPoint : MonoBehaviour
{
    public NavMeshAgent agent;
    public float maxDistance = 50f; // Максимальный радиус поиска
    private void Start()
    {
        StartCoroutine(RandomWalker());
    }

    Vector3 GetRandomPoint(Vector3 center, float range)
    {
        Vector3 randomDirection = Random.insideUnitSphere * range;
        randomDirection += center;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, range, NavMesh.AllAreas))
        {
            return hit.position; 
        }

        Debug.LogWarning("Не удалось найти точку на NavMesh!");
        return center;
    }

    IEnumerator RandomWalker()
    {
        Vector3 randomPoint;
        while (gameObject.activeSelf)
        {
            randomPoint = GetRandomPoint(transform.position, maxDistance);
            agent.SetDestination(randomPoint);
            while ((transform.position - randomPoint).sqrMagnitude > 0.5)
                yield return new WaitForSeconds(0.3f);
        }
        yield return null;
    }
}