using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBinMovement : MonoBehaviour
{
    [Tooltip("Skor bu değerlere ulaştığında kova bir sonraki konuma geçer.")]
    public List<int> scoreThresholds = new() { 3, 6, 9 };
    [Tooltip("Dünya koordinatındaki hareket yönü. Varsayılan: ileri-geri (Z ekseni).")]
    public Vector3 movementAxis = Vector3.forward;
    [Min(0f)] public float movementDistance = 0.5f;
    [Min(0f)] public float movementDuration = 0.5f;
    [Header("Salınım fazı")]
    public bool enableOscillation = true;
    [Min(0)] public int oscillationStartScore = 12;
    [Min(0)] public int oscillationEndScore = 30;
    [Min(0f)] public float oscillationDistance = 0.5f;
    [Min(0f)] public float oscillationDuration = 0.5f;
    public bool returnToStartAtEnd = true;

    Transform binTransform;
    Vector3 startLocalPosition;
    Vector3 localMovementAxis;
    int nextThresholdIndex;
    bool returnedToStart;
    Coroutine movementCoroutine;
    Coroutine oscillationCoroutine;

    void Awake()
    {
        binTransform = transform;
        startLocalPosition = binTransform.localPosition;
        localMovementAxis = GetLocalMovementAxis();
        SortAndValidateThresholds();
    }

    void Start()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ScoreChanged += OnScoreChanged;
            OnScoreChanged(ScoreManager.Instance.CurrentScore);
        }
    }

    void OnDisable()
    {
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.ScoreChanged -= OnScoreChanged;
        StopOscillation();
    }

    void OnScoreChanged(int score)
    {
        while (nextThresholdIndex < scoreThresholds.Count &&
               scoreThresholds[nextThresholdIndex] < oscillationStartScore &&
               score >= scoreThresholds[nextThresholdIndex])
        {
            MoveByDistance(nextThresholdIndex % 2 == 0 ? movementDistance : -movementDistance,
                movementDuration);
            nextThresholdIndex++;
        }

        if (enableOscillation && score >= oscillationStartScore)
        {
            if (score >= oscillationEndScore)
            {
                StopOscillation();
                if (!returnedToStart && returnToStartAtEnd)
                {
                    StartMovement(startLocalPosition, oscillationDuration);
                    returnedToStart = true;
                }
            }
            else if (oscillationCoroutine == null)
            {
                oscillationCoroutine = StartCoroutine(Oscillate());
            }
        }
    }

    IEnumerator Oscillate()
    {
        bool movePositive = true;
        while (ScoreManager.Instance != null &&
               ScoreManager.Instance.CurrentScore < oscillationEndScore)
        {
            Vector3 target = startLocalPosition +
                localMovementAxis * (movePositive ? oscillationDistance : -oscillationDistance);
            yield return MoveTo(target, oscillationDuration);
            movePositive = !movePositive;
        }

        oscillationCoroutine = null;
    }

    void StopOscillation()
    {
        if (oscillationCoroutine != null)
        {
            StopCoroutine(oscillationCoroutine);
            oscillationCoroutine = null;
        }
    }

    void MoveByDistance(float distance, float duration)
    {
        StartMovement(startLocalPosition + localMovementAxis * distance, duration);
    }

    void StartMovement(Vector3 target, float duration)
    {
        if (movementCoroutine != null)
            StopCoroutine(movementCoroutine);

        movementCoroutine = StartCoroutine(MoveTo(target, duration));
    }

    IEnumerator MoveTo(Vector3 target, float duration)
    {
        Vector3 initial = binTransform.localPosition;
        if (duration <= 0f)
        {
            binTransform.localPosition = target;
            movementCoroutine = null;
            yield break;
        }

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            binTransform.localPosition = Vector3.Lerp(initial, target, elapsed / duration);
            yield return null;
        }

        binTransform.localPosition = target;
        movementCoroutine = null;
    }

    void SortAndValidateThresholds()
    {
        scoreThresholds.RemoveAll(threshold => threshold < 0);
        scoreThresholds.Sort();
    }

    Vector3 GetLocalMovementAxis()
    {
        Vector3 axis = movementAxis.sqrMagnitude > 0f ? movementAxis.normalized : Vector3.forward;
        return binTransform.parent != null
            ? binTransform.parent.InverseTransformDirection(axis)
            : axis;
    }
}
