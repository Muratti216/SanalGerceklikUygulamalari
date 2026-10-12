using System.Collections.Generic;
using UnityEngine;

public class CornholeScore : MonoBehaviour
{
    readonly HashSet<Rigidbody> onBoard = new();
    readonly HashSet<Rigidbody> inHole = new();
    int pushed;

    public void Enter(Rigidbody rb, bool hole)
    {
        if (hole) inHole.Add(rb); else onBoard.Add(rb);
    }

    public void Exit(Rigidbody rb, bool hole)
    {
        if (!hole) onBoard.Remove(rb); // deliğe girenler sayılmaya devam eder
    }

    void Update()
    {
        int current = inHole.Count * 3;
        foreach (var rb in onBoard)
            if (!inHole.Contains(rb) && rb.linearVelocity.magnitude < 0.1f)
                current += 1;

        if (current != pushed && ScoreManager.Instance != null)
        {
            ScoreManager.Instance.Add(current - pushed);
            pushed = current;
        }
    }
}