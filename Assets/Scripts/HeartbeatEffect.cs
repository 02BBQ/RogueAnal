using UnityEngine;

public class HeartbeatEffect : MonoBehaviour
{
    public float contractionTime = 0.15f; // ������ �����ϴµ� �ɸ��� �ð�
    public float expansionTime = 0.85f; // ������ Ȯ���ϴµ� �ɸ��� �ð�
    public float beatRate = 1.6f; // ����ڵ��� ��ü ����Ŭ �ð�

    private float totalTime; // ����ڵ��� ��ü ����Ŭ�� ���� Ÿ�̸�
    private Vector3 originalScale; // ���� ������Ʈ�� ������

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
            // ���� �ð� ���� ������ ������ ����
            float percent = beatTime / contractionTime;
            transform.localScale = Vector3.Lerp(originalScale * 1.3f, originalScale, percent);
        }
        else if (beatTime > contractionTime && beatTime <= beatRate)
        {
            // Ȯ�� �ð� ���� ������ õõ�� Ȯ��
            float percent = (beatTime - contractionTime) / expansionTime;
            transform.localScale = Vector3.Lerp(originalScale, originalScale * 1.3f, percent);
        }
    }
}
