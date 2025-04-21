using System.Collections.Generic;
using UnityEngine;

public class ChatHistoryManager : MonoBehaviour
{
    public static ChatHistoryManager Instance { get; private set; }

    public List<string> ConversationHistory { get; private set; } = new List<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �ڳ����л�ʱ�����ö���
        }
        else
        {
            Destroy(gameObject); // ȷ��ֻ��һ��ʵ��
        }
    }

    public void AddMessage(string message)
    {
        ConversationHistory.Add(message);
    }

    public void ClearHistory()
    {
        ConversationHistory.Clear();
    }
}