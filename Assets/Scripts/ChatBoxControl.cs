using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ChatBoxControl : MonoBehaviour
{
    private ListView chatList;          // �����¼�б�
    private TextField messageInput;     // ��������
    private Button sendButton;          // ���Ͱ�ť
    private VisualElement chatWindow;   // ���촰��
    private Button toggleChatButton;    // �����������ʾ�İ�ť
    private List<string> messages;      // �����¼

    private bool isChatWindowOpen = false; // ������Ƿ��

    void Start()
    {
        // ��ȡ UI ���ڵ�
        var root = GetComponent<UIDocument>().rootVisualElement;

        // ��ȡ���촰�����
        chatWindow = root.Q<VisualElement>("ChatWindow");
        chatList = root.Q<ListView>("Chatline");
        messageInput = root.Q<TextField>("Message");
        sendButton = root.Q<Button>("Send");
        toggleChatButton = root.Q<Button>("ChatIcon");

        // ��ʼ�������¼
        messages = new List<string>();

        // ���� ListView
        chatList.itemsSource = messages; // ����Դ
        chatList.makeItem = () => new Label(); // ÿ����Ϣ��ʾΪ Label
        chatList.bindItem = (element, index) =>
        {
            var label = element as Label;
            label.text = messages[index];
        };
        chatList.fixedItemHeight = 30; // ÿ����Ϣ�ĸ߶�

        // �󶨷��Ͱ�ť
        sendButton.clicked += OnSendMessage;

        // �� ChatIcon ��ť����¼�
        //toggleChatButton.clicked += ToggleChatWindow;


        // ��ʼ���������촰��
        chatWindow.style.display = DisplayStyle.None;

        // ��ӳ�ʼ��Ϣ
        AddMessage("ϵͳ��ʾ: ��ӭ�������촰�ڣ�");
        AddMessage("AI 1: ��Һã����� AI 1��");
        AddMessage("AI 2: �ܸ��˼������ǣ�");

        toggleChatButton.clicked += () =>
        {
            Debug.Log("ChatIcon button clicked!");
            ToggleChatWindow();
        };

    }

    void ToggleChatWindow()
    {
        isChatWindowOpen = !isChatWindowOpen;
        chatWindow.style.display = isChatWindowOpen ? DisplayStyle.Flex : DisplayStyle.None;

        Debug.Log($"ChatWindow state changed. isChatWindowOpen: {isChatWindowOpen}");
        Debug.Log($"Current display style: {chatWindow.style.display}");
    }


    void OnSendMessage()
    {
        // ��ȡ����������Ϣ
        string playerMessage = messageInput.value;
        if (string.IsNullOrWhiteSpace(playerMessage)) return;

        // ��������Ϣ�������¼
        AddMessage($"���: {playerMessage}");

        // ��������
        messageInput.value = "";

        // ģ�� AI �ظ�
        StartCoroutine(SimulateAIResponse());
    }

    void AddMessage(string message)
    {
        messages.Add(message);          // �����Ϣ������Դ
        chatList.RefreshItems();        // ˢ�� ListView
        chatList.ScrollToItem(messages.Count - 1); // ���������µ���Ϣ
    }

    System.Collections.IEnumerator SimulateAIResponse()
    {
        // ģ�� AI �ظ��ӳ�
        yield return new WaitForSeconds(1);

        // Ԥ�� AI �ظ�
        string[] aiReplies = {
            "AI 1: �����ҵĻظ���",
            "AI 2: ���ѽ����ң�",
            "AI 3: ��ʲô��Ҫ��������",
            "AI 4: ������������",
            "AI 5: ����������������"
        };

        // ���ѡ��һ�� AI �ظ�
        string aiMessage = aiReplies[Random.Range(0, aiReplies.Length)];
        AddMessage(aiMessage);
    }
}