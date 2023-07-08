using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AStarAgent))]
public class CharacterMoveRandom : MonoBehaviour
{
    AStarAgent _Agent;

    private void Start()
    {
        _Agent = GetComponent<AStarAgent>();
        StartCoroutine(Coroutine_MoveRandom());
    }

    IEnumerator Coroutine_MoveRandom()
    {
        yield return new WaitForSeconds(WorldManager.Instance.startDelay);
        List<Point> freePoints = WorldManager.Instance.GetFreePoints();
        while (true)
        {
            if (_Agent.Status != AStarAgentStatus.InProgress)
            {
                Point p = freePoints[Random.Range(0, freePoints.Count)];
                _Agent.Pathfinding(p.WorldPosition);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
