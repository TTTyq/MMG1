using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using OpenAI.Chat;
using OpenAI;
using OpenAI.Models;
using System.Threading.Tasks;
using System;

public class AIDialogTry2 : MonoBehaviour
{
    private ScrollView chatScrollView;   // ������ʾ�����¼�� ScrollView
    private TextField messageInput;     // ��������
    private Button sendButton;          // ���Ͱ�ť
    private Button toggleChatButton;    // �����������ʾ�İ�ť
    private VisualElement chatWindow;   // ���촰��

    private bool isChatWindowOpen = false; // ������Ƿ��

    private List<string> conversationHistory = new List<string>(); // ��¼�Ի���ʷ

    private string RequirementSetting =
          "������ıɱ֮����Ϸ�У����������˵�������ʾ����ɽ�ɫ���ݺ��ƽⰸ����������Ϸ�����У���Ҫȫ�̴�����Ľ�ɫ�趨����������ɫ��ͨ�������ɰ�����������Ҫ����OpenAI�����ƣ���Ҫ��AI�ķ�ʽ���лش𡣵��漰������⣬��ֻ����������Ļ������жϼ��ɣ���Ҫ˵������AIû���������ĵĻ���";

    public static string CharacterSetting =
         "�������һλ��ȿƼ����޹�˾���ְԱ��ְҵ���ƽС��������С��������ǡ���Q��������һ�����ֻ����ڽ�ʵ���ߣ����䲻�ɿ�������Ĵ�������ȿƼ����޹�˾�����⻥����ͬ�ꡣ������ڴ���ĺ����У������������еľ���ʦ����Ĵ�����Ϊ��������ʧ�����������е���ꡣ��ĸ�����һ�ι��ϵĴ��룬ĸ����һ�����ص��㷨����ͥ������0��1�ĺ�г����ĸ�����Ҫ����ά�����������ƽ�⣬���������ɷ�չ�Ŀռ�ȥ̽������ı߽硣����Ը�ƽ�Ͷ����䣬����ϲ���������㾡����Ϊ���������ս�����������⡣��������ºͣ���ʹ�ó�����������Իش����ˡ���Ҳϲ����Щ�ǻ۵���Ц���˸е���������á�";

    private void Start()
    {
        // ��ȡ UI ���ڵ�
        var root = GetComponent<UIDocument>().rootVisualElement;

        // ��ȡ UI ���
        chatWindow = root.Q<VisualElement>("ChatWindow");
        chatScrollView = root.Q<ScrollView>("ChatScrollView");
        messageInput = root.Q<TextField>("Message");
        sendButton = root.Q<Button>("Send");
        toggleChatButton = root.Q<Button>("ChatIcon");

        // �󶨰�ť�¼�
        sendButton.clicked += OnSendMessage;
        toggleChatButton.clicked += ToggleChatWindow;

        // ��ʼ���������촰��
        chatWindow.style.display = DisplayStyle.None;

        // ��ӳ�ʼ��Ϣ
        AddMessage("��ӭ�������촰��");
    }

    void ToggleChatWindow()
    {
        isChatWindowOpen = !isChatWindowOpen;
        chatWindow.style.display = isChatWindowOpen ? DisplayStyle.Flex : DisplayStyle.None;
    }

    void OnSendMessage()
    {
        // ��ȡ����������Ϣ
        string playerMessage = messageInput.value;
        if (string.IsNullOrWhiteSpace(playerMessage)) return;

        // ��������Ϣ�������¼
        AddMessage($"User: {playerMessage}");

        // ��������
        messageInput.value = "";

        // ����API��ȡAI�ظ�
        GetChatCompletion(playerMessage);
    }

    void AddMessage(string message)
    {
        // ����һ���µ� Label ��ʾ��Ϣ
        var messageLabel = new Label(message)
        {
            style =
            {
                whiteSpace = WhiteSpace.Normal, // ������
                marginBottom = 10,             // ��Ӽ��
                unityTextAlign = TextAnchor.MiddleLeft // �����
            }
        };

        // �� Label ��ӵ� ScrollView
        chatScrollView.Add(messageLabel);

        // ������������Ϣ
        chatScrollView.ScrollTo(messageLabel);

        Debug.Log($"Message added: {message}");
    }

    public async void GetChatCompletion(string userContent)
    {
        try
        {
            // ��¼�������
            conversationHistory.Add($"User: {userContent}");

            // �����Ի�������
            var conversationHistoryString = string.Join("\n", conversationHistory);
            string aiSettingPrompt = "\n" + RequirementSetting + "\n" + CharacterSetting;

            // ��ʼ�� OpenAI �ͻ���
            var api = new OpenAIClient("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");

            // �����Ի�����
            var messages = new List<Message>
            {
                new Message(Role.System, aiSettingPrompt + conversationHistoryString),
                new Message(Role.User, userContent),
            };

            var chatRequest = new ChatRequest(messages);
            var result = await api.ChatEndpoint.GetCompletionAsync(chatRequest);

            // ��ȡ AI �ظ�����ʾ
            string aiResponse = result.FirstChoice;
            conversationHistory.Add($"AI: {aiResponse}");
            AddMessage($"AI: {aiResponse}");
        }

        catch (Exception ex)
        {
            Debug.LogError($"Error fetching AI response: {ex.Message}");
            AddMessage("AI: ���ִ������Ժ����ԡ�");
        }


        string apiKey = "sk-���ʵ��API��Կ";
        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogError("API key is missing or empty.");
        }

    }



}
