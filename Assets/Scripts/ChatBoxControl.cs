using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ChatBoxControl : MonoBehaviour
{
    private ListView chatList;          // 聊天记录列表
    private TextField messageInput;     // 玩家输入框
    private Button sendButton;          // 发送按钮
    private VisualElement chatWindow;   // 聊天窗口
    private Button toggleChatButton;    // 控制聊天框显示的按钮
    private List<string> messages;      // 聊天记录

    private bool isChatWindowOpen = false; // 聊天框是否打开

    void Start()
    {
        // 获取 UI 根节点
        var root = GetComponent<UIDocument>().rootVisualElement;

        // 获取聊天窗口组件
        chatWindow = root.Q<VisualElement>("ChatWindow");
        chatList = root.Q<ListView>("Chatline");
        messageInput = root.Q<TextField>("Message");
        sendButton = root.Q<Button>("Send");
        toggleChatButton = root.Q<Button>("ChatIcon");

        // 初始化聊天记录
        messages = new List<string>();

        // 配置 ListView
        chatList.itemsSource = messages; // 数据源
        chatList.makeItem = () => new Label(); // 每条消息显示为 Label
        chatList.bindItem = (element, index) =>
        {
            var label = element as Label;
            label.text = messages[index];
        };
        chatList.fixedItemHeight = 30; // 每条消息的高度

        // 绑定发送按钮
        sendButton.clicked += OnSendMessage;

        // 绑定 ChatIcon 按钮点击事件
        //toggleChatButton.clicked += ToggleChatWindow;


        // 初始化隐藏聊天窗口
        chatWindow.style.display = DisplayStyle.None;

        // 添加初始消息
        AddMessage("系统提示: 欢迎来到聊天窗口！");
        AddMessage("AI 1: 大家好，我是 AI 1！");
        AddMessage("AI 2: 很高兴见到你们！");

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
        // 获取玩家输入的消息
        string playerMessage = messageInput.value;
        if (string.IsNullOrWhiteSpace(playerMessage)) return;

        // 添加玩家消息到聊天记录
        AddMessage($"玩家: {playerMessage}");

        // 清空输入框
        messageInput.value = "";

        // 模拟 AI 回复
        StartCoroutine(SimulateAIResponse());
    }

    void AddMessage(string message)
    {
        messages.Add(message);          // 添加消息到数据源
        chatList.RefreshItems();        // 刷新 ListView
        chatList.ScrollToItem(messages.Count - 1); // 滚动到最新的消息
    }

    System.Collections.IEnumerator SimulateAIResponse()
    {
        // 模拟 AI 回复延迟
        yield return new WaitForSeconds(1);

        // 预设 AI 回复
        string[] aiReplies = {
            "AI 1: 这是我的回复。",
            "AI 2: 你好呀，玩家！",
            "AI 3: 有什么需要帮助的吗？",
            "AI 4: 今天天气不错！",
            "AI 5: 别忘了完成你的任务！"
        };

        // 随机选择一个 AI 回复
        string aiMessage = aiReplies[Random.Range(0, aiReplies.Length)];
        AddMessage(aiMessage);
    }
}