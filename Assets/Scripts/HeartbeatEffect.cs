using UnityEngine;

public class HeartbeatEffect : MonoBehaviour
{
    public float contractionTime = 0.15f; // 심장이 수축하는데 걸리는 시간
    public float expansionTime = 0.85f; // 심장이 확장하는데 걸리는 시간
    public float beatRate = 1.6f; // 심장박동의 전체 사이클 시간

    private float totalTime; // 심장박동의 전체 사이클을 위한 타이머
    private Vector3 originalScale; // 원래 오브젝트의 스케일

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        totalTime += Time.deltaTime;
        float beatTime = totalTime % beatRate;

        if (beatTime <= contractionTime)
        {
            // 수축 시간 동안 심장이 빠르게 수축
            float percent = beatTime / contractionTime;
            transform.localScale = Vector3.Lerp(originalScale * 1.3f, originalScale, percent);
        }
        else if (beatTime > contractionTime && beatTime <= beatRate)
        {
            // 확장 시간 동안 심장이 천천히 확장
            float percent = (beatTime - contractionTime) / expansionTime;
            transform.localScale = Vector3.Lerp(originalScale, originalScale * 1.3f, percent);
        }
    }
}
