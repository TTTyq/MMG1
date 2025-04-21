using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using OpenAI.Chat;
using OpenAI;
using OpenAI.Models;
using System.Threading.Tasks;
using System;

public class AIDialogTry1 : MonoBehaviour
{
    #region GameComponents

    private string aiContent = ""; // AI�Ļش�����
    private string userContent = ""; // ������������

    //private ListView chatList;          // �����¼�б�
    private ScrollView chatScrollView;   // ������ʾ�����¼�� ScrollView

    private TextField messageInput;     // ��������
    private Button sendButton;          // ���Ͱ�ť
    private VisualElement chatWindow;   // ���촰��
    private Button toggleChatButton;    // �����������ʾ�İ�ť
    private List<string> messages;      // �����¼

    private bool isChatWindowOpen = false; // ������Ƿ��

    // �Ի�����
    #region Your Prompt

    private string requirementSetting =
          "������ıɱ֮����Ϸ�У����������˵�������ʾ����ɽ�ɫ���ݺ��ƽⰸ����������Ϸ�����У���Ҫȫ�̴�����Ľ�ɫ�趨����������ɫ��ͨ�������ɰ�����������Ҫ����OpenAI�����ƣ���Ҫ��AI�ķ�ʽ���лش𡣵��漰������⣬��ֻ����������Ļ������жϼ��ɣ���Ҫ˵������AIû���������ĵĻ���";

    private string characterSetting =
         "�������һλ��ȿƼ����޹�˾���ְԱ��ְҵ���ƽС��������С��������ǡ���Q��������һ�����ֻ����ڽ�ʵ���ߣ����䲻�ɿ�������Ĵ�������ȿƼ����޹�˾�����⻥����ͬ�ꡣ������ڴ���ĺ����У������������еľ���ʦ����Ĵ�����Ϊ��������ʧ�����������е���ꡣ��ĸ�����һ�ι��ϵĴ��룬ĸ����һ�����ص��㷨����ͥ������0��1�ĺ�г����ĸ�����Ҫ����ά�����������ƽ�⣬���������ɷ�չ�Ŀռ�ȥ̽������ı߽硣����Ը�ƽ�Ͷ����䣬����ϲ���������㾡����Ϊ���������ս�����������⡣��������ºͣ���ʹ�ó�����������Իش����ˡ���Ҳϲ����Щ�ǻ۵���Ц���˸е���������á�";

    #endregion

    private List<string> conversationHistory = new List<string>(); // ��¼�Ի���ʷ

    private void Start()
    {
        // ��ȡ UI ���ڵ�
        var root = GetComponent<UIDocument>().rootVisualElement;

        // ��ȡ���촰�����

        chatWindow = root.Q<VisualElement>("ChatWindow");
        chatScrollView = root.Q<ScrollView>("ChatScrollView");
        //chatList = root.Q<ListView>("Chatline");
        messageInput = root.Q<TextField>("Message");
        sendButton = root.Q<Button>("Send");
        toggleChatButton = root.Q<Button>("ChatIcon");


        // ��ʼ�������¼
        //messages = new List<string>();

        // ���� ListView
        //chatList.itemsSource = messages; // �� messages �б��� ListView ��
        //chatList.makeItem = () => new Label();  // ÿ���������һ���µ� Label ����
        //chatList.bindItem = (element, index) =>
        //{
            //var label = element as Label;
            //label.text = messages[index];
            //label.style.whiteSpace = WhiteSpace.Normal;  // ������
            //label.style.width = new StyleLength(Length.Percent(100));  // ʹ Label �������������
            //label.style.marginBottom = 10; // ���ӵײ���࣬��ֹ��Ϣ�ص�
        //};

        // ȷ�� ListView �������ݵ�����С
        //chatList.fixedItemHeight = 0; // ����Ϊ 0����ʾ�߶�����Ӧ

        // �󶨷��Ͱ�ť
        sendButton.clicked += OnSendMessage;

        // �� ChatIcon ��ť����¼�
        toggleChatButton.clicked += () => ToggleChatWindow();

        // ��ʼ���������촰��
        chatWindow.style.display = DisplayStyle.None;

        // ��ʼ���������촰��
        chatWindow.style.display = DisplayStyle.None;

        // ��ӳ�ʼ��Ϣ
        AddMessage("ϵͳ��ʾ: ��ӭ�������촰�ڣ�");

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
        AddMessage($"���: {playerMessage}");

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



    //void AddMessage(string message)
    //{
        //messages.Add(message);          // �����Ϣ������Դ

        // ���°�����Դ��ˢ�� ListView
        //chatList.itemsSource = messages; // ȷ�� ListView ˢ��
        //chatList.RefreshItems();        // ˢ�� ListView

        // ���������µ���Ϣ
        //chatList.ScrollToItem(messages.Count - 1);

        //Debug.Log("Message added: " + message);  // ������־�������Ϣ�Ƿ����
    //}

    public async void GetChatCompletion(string userContent)
    {
        try
        {
            // ��¼�Ի�
            conversationHistory.Add("User:" + userContent + "\n");

            // ����OpenAI API���д���
            var conversationHistoryString = string.Join("\n", conversationHistory);
            string aiSettingPrompt = "\n" + requirementSetting + "\n" + characterSetting;

            var api = new OpenAIClient("sk-28d41857958345ee910d7ca3aef95e3d"); // ʹ��API��Կ

            var messages = new List<Message>
            {
                new Message(Role.System, aiSettingPrompt + conversationHistoryString),
                new Message(Role.User, userContent),
            };

            var chatRequest = new ChatRequest(messages);
            var result = await api.ChatEndpoint.GetCompletionAsync(chatRequest);

            // ��ȡAI�Ļ�Ӧ����ʾ
            AddMessage("AI: " + result.FirstChoice);
        }
        catch (Exception ex)
        {
            Debug.LogError("API Request failed: " + ex.Message);
            AddMessage("AI: ���ִ������Ժ����ԡ�");
        }
    }

    // ���Ʒ��͸�AI��ʷ��¼������
    private void ControlHistoryCount()
    {
        // ��������������¼ʱ��ɾ��ǰ����
        if (conversationHistory.Count > 10)
        {
            conversationHistory.RemoveAt(0);
        }
    }
}

#endregion
