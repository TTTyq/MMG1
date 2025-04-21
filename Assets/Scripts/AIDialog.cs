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
    // �����е���Ϸ���
    #region GameComponents

    // ֮��Ի��з��͸�AI�����
    private string aiContent = "";

    // ��ҷ��͵�����
    private string userContent = "";

    // ���������
    [SerializeField] private TMP_InputField playerInputField;

    // ��ʾ�����е�UI
    [SerializeField] private GameObject loadingUI;

    #endregion

    // �����ǽ�ɫ���趨��䣬�ԡ��������С�Ϊ��

    #region Your Prompt

    private string requirementSetting=
        "������ıɱ֮����Ϸ�У����������˵�������ʾ����ɽ�ɫ���ݺ��ƽⰸ����������Ϸ�����У���Ҫȫ�̴�����Ľ�ɫ�趨����������ɫ��ͨ�������ɰ�����������Ҫ����OpenAI�����ƣ���Ҫ��AI�ķ�ʽ���лش𡣵��漰������⣬��ֻ����������Ļ������жϼ��ɣ���Ҫ˵������AIû���������ĵĻ���";

    private string characterSetting =
         "�������һλ��ȿƼ����޹�˾���ְԱ��ְҵ���ƽС��������С��������ǡ���Q��������һ�����ֻ����ڽ�ʵ���ߣ����䲻�ɿ�������Ĵ�������ȿƼ����޹�˾�����⻥����ͬ�ꡣ������ڴ���ĺ����У������������еľ���ʦ����Ĵ�����Ϊ��������ʧ�����������е���ꡣ��ĸ�����һ�ι��ϵĴ��룬ĸ����һ�����ص��㷨����ͥ������0��1�ĺ�г����ĸ�����Ҫ����ά�����������ƽ�⣬���������ɷ�չ�Ŀռ�ȥ̽������ı߽硣����Ը�ƽ�Ͷ����䣬����ϲ���������㾡����Ϊ���������ս�����������⡣��������ºͣ���ʹ�ó�����������Իش����ˡ���Ҳϲ����Щ�ǻ۵���Ц���˸е���������á�";
    # endregion

    // API�����м��书�ܣ����뷢��̸�����ݸ�AI����ž߱����书��
    [SerializeField] List<string> conversationHistory = new List<string>();

    // ���͸�API��������������������������Լ�AI�Ļظ�����Ҫ���󣬲�Ȼ��ķѹ����API��Tokens���������»������
    [Tooltip("���͸�AI�����������������ײ�Ҫ���󣬲�Ȼ��ķѹ����API��Tokens���������»������")]
    [SerializeField]
    private int maxConversationHistory = 10;

    //--------------------------------------------------------------------------------------------------------------

    void Start()
    {
        //��ʼUI��ʾ

        //���͸�AI�����
        SendToAIMessage();

    }


    // ���ķ��ʹ���
    public async Task GetChatCompletion(string userContent, string systemContent = "",
        string aiSettingPrompt = "")
    {
        ControlHistoryCount();

        // ����ʷ��¼��Ϊһ���ַ�������
        string conversationHistoryString = string.Join("\n", conversationHistory);

        // ���"\n"��Ϊ�˷�ֹ������һ�𣬱���AI��⡣������и���AI������һЩ����������г���
        aiSettingPrompt = "\n" + requirementSetting + "\n" + characterSetting;
        userContent = this.userContent;
        systemContent = this.aiContent;

        // API Key
        var openaiKey = "sk-28d41857958345ee910d7ca3aef95e3d";
        var api = new OpenAIClient(openaiKey);


#if UNITY_EDITOR
        // ���ChatEndpoint�����Ƿ�Ϊ�գ�ChatEndpoint ��OpenAI��Chat���ܵ�API�˵�
        //Assert.IsNotNull(api.ChatEndpoint);
#endif

        // ����ChatPrompt���ֱ��ǽ�ɫ�����ݡ���ɫ�Ƕ���AI�����֣����ݿ���Ԥ����AI
        var messages = new List<Message>
        {
            // ���Ի���ʷ��¼����Role.System����ʹ��AI���������Ļش�
            new Message(Role.System, systemContent + aiSettingPrompt + conversationHistoryString),

            // ������Ϊ�˿���AI���ɻش����䣬����AI�ش�Ҳ���Ըĳɱ�ķ���
            new Message(Role.User, userContent),
        };


        // ����stopҪ�����л��Ļ����������ֵ����Ȼ��ֹͣ����
        var chatRequest = new ChatRequest(messages);

        // ����API����ȡAI���ı�
        var result = await api.ChatEndpoint.GetCompletionAsync(chatRequest);
     
        // API��ChatGpt�᷵�ض���ı�������ѡ���һ��  
        Debug.Log(result.FirstChoice);

        // �����ı��󣬹رյȴ��е�UI
        loadingUI.SetActive(false);
    }

    // �󶨵����Ͱ�ť�ϣ����ڷ������������Ϣ
    public void SendToAIMessage()
    {
        if (playerInputField.text == string.Empty)
        {
            return;
        }

        // ��ҷ��͵�����
        userContent = playerInputField.text;

        // ��¼�Ի�
        conversationHistory.Add("User:" + userContent + "\n");

        // ����AI API
        GetChatCompletion(userContent);

        // ��������
        playerInputField.text = "";

        loadingUI.SetActive(true);
    }


    // ���Ʒ��͸�AI��ʷ��¼������
    public void ControlHistoryCount()
    {
        // ����������10��ʱ��ɾ��ǰ����
        if (conversationHistory.Count > maxConversationHistory)
        {
            conversationHistory.RemoveAt(0);
        }
    }

}
