using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using OpenAI.Chat;
using OpenAI;
using OpenAI.Models;
using System.Threading.Tasks;
using System;
using System.Net.Http;
using System.Text;

public class AIDialogTry5 : MonoBehaviour
{
    //public Font customFont; // �� Unity Inspector ������������Դ
    private ScrollView chatScrollView;   // ������ʾ�����¼�� ScrollView
    private TextField messageInput;     // ��������
    private Button sendButton;          // ���Ͱ�ť
    private Button toggleChatButton;    // �����������ʾ�İ�ť
    private VisualElement chatWindow;   // ���촰��

    private bool isChatWindowOpen = false; // ������Ƿ��

    private List<string> conversationHistory = new List<string>(); // ��¼�Ի���ʷ

    private string requirementSetting =
        "������ıɱ֮����Ϸ�У����������˵�������ʾ����ɽ�ɫ���ݺ��ƽⰸ����������Ϸ�����У���Ҫȫ�̴�����Ľ�ɫ�趨����������ɫ��ͨ�������ɰ�����������Ҫ����OpenAI�����ƣ���Ҫ��AI�ķ�ʽ���лش𡣵��漰������⣬��ֻ����������Ļ������жϼ��ɣ���Ҫ˵������AIû���������ĵĻ�����Ϸ�����У���Ҫ�������Ϸ���̣���Ϲ۲��������ȡ���õ���Ϣ����һ��һ��ʹ�����¸�ʽ˼�����۲�: ��Ҫ�����κ����ݣ�������Ϸ��ʵ����ǰʱ�䣬���־������֣�����У����������ݣ�����Ҷ��ѣ�����У�����Ʒ״̬������У��ȵȣ�˼��: �������Ը��ص㣬�������Ʋ���Ϸ��Ϣ��������ϷӮ�棬���������ݣ��Ƿ���ì����Ϣ�ȵȣ���ע�������ʷ��Ϣ��α���ж�:����� <commands> �е�����ѡ����ʷ�����ֻ��Ҫ����������ּ��ɡ�����: agent=ִ����ң�target=Ŀ����ң���������ȷ�����֣���content=�������ݣ���ʹ��ר�ôʻ������ԣ����ñ�⡢�Կ�����ƭ��αװ��̹�׵ȼ��ɰ�װ���ݣ���ѡ��ע���Լ����࣬�ٽ��ϻ���ͻ���ص㡣������:�ο� <commands> �����Ӳ���������ָ�ʽ����Ҫ���Ϊ�κ�������ʽ�����ݡ�ע�⣺�������ֱ�׼��1-3��Ϊ�4-5��һ�㣬6�ּ���7�����㣬8�����֡�����������Ϣ��һ��ÿ�ζ��У���Ҫ��֤һ��6�����ϵľ��ߡ�";

    private string characterSetting =
        "�������һλ��ȿƼ����޹�˾���ְԱ��ְҵ���ƽС��������С��������ǡ���Q��������һ�����ֻ����ڽ�ʵ���ߣ������ڴ���ĺ����У������������еľ���ʦ����Ĵ�����Ϊ��������ʧ�����������е���ꡣ��ĸ�����һ�ι��ϵĴ��룬ĸ����һ�����ص��㷨������0��1�ĺ�г�гɳ������ʹ����ά�����������ƽ�⡣����Ը�ƽ�Ͷ����䣬����ϲ���������㾡����Ϊ���������ս�����������⡣��������ºͣ���ʹ�ó�����������Իش����ˡ���Ҳϲ����Щ�ǻ۵���Ц���˸е���������á���ļ��ܰ�����1. ����ľ�㣺ͨ���ӹ���ϴˢ��У����ƽ����ʵ���ϵļӹ�����ͨ����ľ�����𾭵ȷ�ʽ��ת��ע���������ٳ嵭��Ͱ��������˷ܸУ�ͨ���𾭸ı���֪����������Ϊͨ�����ӷ𾭽��ͺ��������ĥ����У����ƽ���������̬��2. ���ӿ��⣺ÿ�������ߵ���Ŀ������Ҫ��Դ����������⣬��ȷ���������ް���3. ���ӳ��ȣ�����һ��������ʱ����18�꣬��18�������һ���ú���������Ϊ�����㲿���������������ɱ����������ҵ�񣬱����ϻ����������ȥ����ʵ��ͨ��AI�����Ե粨�����¸��ݱ��˵���ʶ��д��һ��18�����������Ϊһ�����˻��š�����Ϸ�����У�����Ҫ��������ɫ������ͬʱҲҪ�����Լ������ܺ�Ŀ�ꡣ����ж��;��߽�Ӱ����Ϸ�Ľ��̺ͽ�֡����ס�����Ŀ����ά��Ԫ�����ƽ�⣬ͬʱ�������Լ��Ĵ��ڲ�����¶��";


    public class ContentExtractor
    {
        public static string ExtractContent(string input)
        { // Look for content=" pattern
            int startIndex = input.IndexOf("content=\"");
            if (startIndex == -1)
                return string.Empty;

            // Add length of content=" to get to actual content start
            startIndex += 9;

            // Find closing quote
            int endIndex = input.IndexOf("\"", startIndex);
            if (endIndex == -1)
                return string.Empty;


            // Extract the content between quotes
            return input.Substring(startIndex, endIndex - startIndex);

        }
    }



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
        AddMessage("��ӭ�������촰��!");
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
            string aiSettingPrompt = "\n" + requirementSetting + "\n" + characterSetting;

            

            // ��ʼ�� OpenAI �ͻ���
            //var api = new OpenAIClient("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");
            var api = new OpenAIClient("sk-28d41857958345ee910d7ca3aef95e3d");

            // �����Ի�����
            var messages = new List<Message>
            {
                new Message(Role.System, aiSettingPrompt + conversationHistoryString),
                new Message(Role.User, userContent),
            };

            var chatRequest = new ChatRequest(messages);
            //var chatRequest = new ChatRequest(messages, model: "gpt-4"); // ȷ��ģ����ȷΪ gpt-4
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

    }



}