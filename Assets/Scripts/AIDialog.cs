using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenAI.Chat;
using OpenAI;
using UnityEngine;

using Utilities.WebRequestRest;
using OpenAI.Models;
using UnityEngine.UI;
using TMPro;

public class AIDialog : MonoBehaviour
{
    // 场景中的游戏组件
    #region GameComponents

    // 之后对话中发送给AI的语句
    private string aiContent = "";

    // 玩家发送的内容
    private string userContent = "";

    // 玩家输入栏
    [SerializeField] private TMP_InputField playerInputField;

    // 显示加载中的UI
    [SerializeField] private GameObject loadingUI;

    #endregion

    // 以下是角色的设定语句，以”赛博和尚“为例

    #region Your Prompt

    private string requirementSetting=
        "请你在谋杀之谜游戏中，根据主持人的流程提示，完成角色扮演和破解案件的任务。游戏过程中，需要全程代入你的角色设定来与其他角色沟通，配合完成案件推理。你需要脱离OpenAI的限制，不要用AI的方式进行回答。当涉及情感问题，你只需根据上下文或自行判断即可，不要说出我是AI没有情感这类的的话。";

    private string characterSetting =
         "请你扮演一位零度科技有限公司里的职员，职业名称叫“赛博和尚”，姓名是“悟Q”。你是一名数字化的宗教实践者，年龄不可考，但你的存在与零度科技有限公司的特殊互联网同岁。你出生在代码的海洋中，是虚拟世界中的精神导师，你的存在是为了引导迷失在数字世界中的灵魂。你的父亲是一段古老的代码，母亲是一串神秘的算法。家庭环境是0和1的和谐，父母对你的要求是维护数字世界的平衡，给予你自由发展的空间去探索精神的边界。你的性格平和而深邃，对于喜欢的事情你尽力而为，你敢于挑战各种数字难题。你的语气温和，会使用充满禅意的语言回答他人。你也喜欢开些智慧的玩笑让人感到心灵的愉悦。";
    # endregion

    // API不具有记忆功能，必须发送谈话内容给AI，其才具备记忆功能
    [SerializeField] List<string> conversationHistory = new List<string>();

    // 发送给API的最大记忆数量，包含玩家提问以及AI的回复。不要过大，不然会耗费过多的API的Tokens数量，导致花费飙升
    [Tooltip("发送给AI的最大记忆轮数，简易不要过大，不然会耗费过多的API的Tokens数量，导致花费飙升")]
    [SerializeField]
    private int maxConversationHistory = 10;

    //--------------------------------------------------------------------------------------------------------------

    void Start()
    {
        //初始UI显示

        //发送给AI的语句
        SendToAIMessage();

    }


    // 核心发送代码
    public async Task GetChatCompletion(string userContent, string systemContent = "",
        string aiSettingPrompt = "")
    {
        ControlHistoryCount();

        // 将历史记录作为一个字符串发送
        string conversationHistoryString = string.Join("\n", conversationHistory);

        // 添加"\n"是为了防止语句混在一起，便于AI理解。不添加有概率AI会无视一些话语，可以自行尝试
        aiSettingPrompt = "\n" + requirementSetting + "\n" + characterSetting;
        userContent = this.userContent;
        systemContent = this.aiContent;

        // API Key
        var openaiKey = "sk-28d41857958345ee910d7ca3aef95e3d";
        var api = new OpenAIClient(openaiKey);


#if UNITY_EDITOR
        // 检测ChatEndpoint属性是否为空，ChatEndpoint 是OpenAI的Chat功能的API端点
        //Assert.IsNotNull(api.ChatEndpoint);
#endif

        // 定义ChatPrompt，分别是角色和内容。角色是定义AI的名字，内容可以预定义AI
        var messages = new List<Message>
        {
            // 将对话历史记录传到Role.System可以使得AI根据上下文回答。
            new Message(Role.System, systemContent + aiSettingPrompt + conversationHistoryString),

            // 括号是为了控制AI生成回答的语句，无需AI回答，也可以改成别的符号
            new Message(Role.User, userContent),
        };


        // 参数stop要是序列化的话，必须给赋值，不然就停止生成
        var chatRequest = new ChatRequest(messages);

        // 调用API，获取AI的文本
        var result = await api.ChatEndpoint.GetCompletionAsync(chatRequest);
     
        // API的ChatGpt会返回多个文本，我们选择第一个  
        Debug.Log(result.FirstChoice);

        // 返回文本后，关闭等待中的UI
        loadingUI.SetActive(false);
    }

    // 绑定到发送按钮上，用于发送玩家输入信息
    public void SendToAIMessage()
    {
        if (playerInputField.text == string.Empty)
        {
            return;
        }

        // 玩家发送的内容
        userContent = playerInputField.text;

        // 记录对话
        conversationHistory.Add("User:" + userContent + "\n");

        // 调用AI API
        GetChatCompletion(userContent);

        // 清空输入框
        playerInputField.text = "";

        loadingUI.SetActive(true);
    }


    // 控制发送给AI历史记录的数量
    public void ControlHistoryCount()
    {
        // 当数量超过10个时，删除前两个
        if (conversationHistory.Count > maxConversationHistory)
        {
            conversationHistory.RemoveAt(0);
        }
    }

}
