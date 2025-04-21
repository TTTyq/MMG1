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
            DontDestroyOnLoad(gameObject); // 在场景切换时保留该对象
        }
        else
        {
            Destroy(gameObject); // 确保只有一个实例
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