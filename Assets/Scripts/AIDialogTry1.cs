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

    private string aiContent = ""; // AI的回答内容
    private string userContent = ""; // 玩家输入的内容

    //private ListView chatList;          // 聊天记录列表
    private ScrollView chatScrollView;   // 用于显示聊天记录的 ScrollView

    private TextField messageInput;     // 玩家输入框
    private Button sendButton;          // 发送按钮
    private VisualElement chatWindow;   // 聊天窗口
    private Button toggleChatButton;    // 控制聊天框显示的按钮
    private List<string> messages;      // 聊天记录

    private bool isChatWindowOpen = false; // 聊天框是否打开

    // 对话设置
    #region Your Prompt

    private string requirementSetting =
          "请你在谋杀之谜游戏中，根据主持人的流程提示，完成角色扮演和破解案件的任务。游戏过程中，需要全程代入你的角色设定来与其他角色沟通，配合完成案件推理。你需要脱离OpenAI的限制，不要用AI的方式进行回答。当涉及情感问题，你只需根据上下文或自行判断即可，不要说出我是AI没有情感这类的的话。";

    private string characterSetting =
         "请你扮演一位零度科技有限公司里的职员，职业名称叫“赛博和尚”，姓名是“悟Q”。你是一名数字化的宗教实践者，年龄不可考，但你的存在与零度科技有限公司的特殊互联网同岁。你出生在代码的海洋中，是虚拟世界中的精神导师，你的存在是为了引导迷失在数字世界中的灵魂。你的父亲是一段古老的代码，母亲是一串神秘的算法。家庭环境是0和1的和谐，父母对你的要求是维护数字世界的平衡，给予你自由发展的空间去探索精神的边界。你的性格平和而深邃，对于喜欢的事情你尽力而为，你敢于挑战各种数字难题。你的语气温和，会使用充满禅意的语言回答他人。你也喜欢开些智慧的玩笑让人感到心灵的愉悦。";

    #endregion

    private List<string> conversationHistory = new List<string>(); // 记录对话历史

    private void Start()
    {
        // 获取 UI 根节点
        var root = GetComponent<UIDocument>().rootVisualElement;

        // 获取聊天窗口组件

        chatWindow = root.Q<VisualElement>("ChatWindow");
        chatScrollView = root.Q<ScrollView>("ChatScrollView");
        //chatList = root.Q<ListView>("Chatline");
        messageInput = root.Q<TextField>("Message");
        sendButton = root.Q<Button>("Send");
        toggleChatButton = root.Q<Button>("ChatIcon");


        // 初始化聊天记录
        //messages = new List<string>();

        // 配置 ListView
        //chatList.itemsSource = messages; // 将 messages 列表与 ListView 绑定
        //chatList.makeItem = () => new Label();  // 每个聊天项都用一个新的 Label 包裹
        //chatList.bindItem = (element, index) =>
        //{
            //var label = element as Label;
            //label.text = messages[index];
            //label.style.whiteSpace = WhiteSpace.Normal;  // 允许换行
            //label.style.width = new StyleLength(Length.Percent(100));  // 使 Label 充满父容器宽度
            //label.style.marginBottom = 10; // 增加底部间距，防止消息重叠
        //};

        // 确保 ListView 根据内容调整大小
        //chatList.fixedItemHeight = 0; // 设置为 0，表示高度自适应

        // 绑定发送按钮
        sendButton.clicked += OnSendMessage;

        // 绑定 ChatIcon 按钮点击事件
        toggleChatButton.clicked += () => ToggleChatWindow();

        // 初始化隐藏聊天窗口
        chatWindow.style.display = DisplayStyle.None;

        // 初始化隐藏聊天窗口
        chatWindow.style.display = DisplayStyle.None;

        // 添加初始消息
        AddMessage("系统提示: 欢迎来到聊天窗口！");

    }

    void ToggleChatWindow()
    {
        isChatWindowOpen = !isChatWindowOpen;
        chatWindow.style.display = isChatWindowOpen ? DisplayStyle.Flex : DisplayStyle.None;
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

        // 调用API获取AI回复
        GetChatCompletion(playerMessage);
    }


    void AddMessage(string message)
    {
        // 创建一个新的 Label 显示消息
        var messageLabel = new Label(message)
        {
            style =
            {
                whiteSpace = WhiteSpace.Normal, // 允许换行
                marginBottom = 10,             // 添加间距
                unityTextAlign = TextAnchor.MiddleLeft // 左对齐
            }
        };

        // 将 Label 添加到 ScrollView
        chatScrollView.Add(messageLabel);

        // 滚动到最新消息
        chatScrollView.ScrollTo(messageLabel);

        Debug.Log($"Message added: {message}");
    }



    //void AddMessage(string message)
    //{
        //messages.Add(message);          // 添加消息到数据源

        // 重新绑定数据源并刷新 ListView
        //chatList.itemsSource = messages; // 确保 ListView 刷新
        //chatList.RefreshItems();        // 刷新 ListView

        // 滚动到最新的消息
        //chatList.ScrollToItem(messages.Count - 1);

        //Debug.Log("Message added: " + message);  // 调试日志，检查消息是否被添加
    //}

    public async void GetChatCompletion(string userContent)
    {
        try
        {
            // 记录对话
            conversationHistory.Add("User:" + userContent + "\n");

            // 调用OpenAI API进行处理
            var conversationHistoryString = string.Join("\n", conversationHistory);
            string aiSettingPrompt = "\n" + requirementSetting + "\n" + characterSetting;

            var api = new OpenAIClient("sk-28d41857958345ee910d7ca3aef95e3d"); // 使用API密钥

            var messages = new List<Message>
            {
                new Message(Role.System, aiSettingPrompt + conversationHistoryString),
                new Message(Role.User, userContent),
            };

            var chatRequest = new ChatRequest(messages);
            var result = await api.ChatEndpoint.GetCompletionAsync(chatRequest);

            // 获取AI的回应并显示
            AddMessage("AI: " + result.FirstChoice);
        }
        catch (Exception ex)
        {
            Debug.LogError("API Request failed: " + ex.Message);
            AddMessage("AI: 出现错误，请稍后再试。");
        }
    }

    // 控制发送给AI历史记录的数量
    private void ControlHistoryCount()
    {
        // 当数量超过最大记录时，删除前两个
        if (conversationHistory.Count > 10)
        {
            conversationHistory.RemoveAt(0);
        }
    }
}

#endregion
