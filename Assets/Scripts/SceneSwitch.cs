using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneSwitch : MonoBehaviour
{
    private VisualElement panelContainer;       // �������
    private Dictionary<string, Button> sceneButtons; // ������ť�ֵ�
    private Dictionary<string, string> buttonDisplayTexts; // ��ť��ʾ�ı��ֵ�
    private string currentScene;               // ��ǰ����������

    void Start()
    {
        // ��ȡ UI ���ڵ�
        var root = GetComponent<UIDocument>().rootVisualElement;

        // ��ȡ�������
        panelContainer = root.Q<VisualElement>("SceneSwitch");
        if (panelContainer == null)
        {
            Debug.LogError("Panel container 'SceneSwitch' not found.");
            return;
        }

        // ��ʼ����ť�ֵ�
        sceneButtons = new Dictionary<string, Button>();

        // ��̬���ؿ��ܵİ�ť
        AddButtonIfExists(root, "Office");
        AddButtonIfExists(root, "End");
        AddButtonIfExists(root, "MingDynasty");
        AddButtonIfExists(root, "Stage");

        // ��ʼ����ť��ʾ�ı��ֵ�
        buttonDisplayTexts = new Dictionary<string, string>
        {
            { "Office", "�칫��" },         // ��Ӧ�칫��
            { "End", "����һ������ܰ���ѣ���ĩ�ռ�����ҲҪ���롷" },         // ��Ӧĩ������
            { "MingDynasty", "��������������֮���ڴ������ʵۡ�" },  // ��Ӧ�������
            { "Stage", "�������� �����˸����ţ������ǡ�" } // ��Ӧ Produce God 1024
        };

        // ��ȡ��ǰ��������
        currentScene = SceneManager.GetActiveScene().name;
        Debug.Log($"Current scene: {currentScene}");

        // ��̬���ð�ť
        SetUpButtons();
    }

    // ��̬��Ӱ�ť���ֵ���
    void AddButtonIfExists(VisualElement root, string buttonName)
    {
        var button = root.Q<Button>(buttonName);
        if (button != null)
        {
            sceneButtons[buttonName] = button;
            Debug.Log($"Button '{buttonName}' loaded successfully.");
        }
        else
        {
            Debug.LogWarning($"Button '{buttonName}' not found in UI Builder.");
        }
    }

    void SetUpButtons()
    {
        // ��̬����ÿ����ť��Ŀ�곡������ʾ״̬
        foreach (var kvp in sceneButtons)
        {
            var buttonName = kvp.Key;
            var button = kvp.Value;

            // ���ݵ�ǰ���������޹ذ�ť������Ŀ�곡��
            switch (currentScene)
            {
                case "GameScene1": // ��ǰ������ Office
                    if (buttonName == "Office") button.style.display = DisplayStyle.None;
                    else if (buttonName == "End") SetButtonTarget(button, "GameSceneEnd");
                    else if (buttonName == "MingDynasty") SetButtonTarget(button, "GameSceneMingDynasty");
                    else if (buttonName == "Stage") SetButtonTarget(button, "GameSceneStage");
                    break;

                case "GameSceneEnd":
                    if (buttonName == "End") button.style.display = DisplayStyle.None;
                    else if (buttonName == "Office") SetButtonTarget(button, "GameScene1");
                    else if (buttonName == "MingDynasty") SetButtonTarget(button, "GameSceneMingDynasty");
                    else if (buttonName == "Stage") SetButtonTarget(button, "GameSceneStage");
                    break;

                case "GameSceneMingDynasty":
                    if (buttonName == "MingDynasty") button.style.display = DisplayStyle.None;
                    else if (buttonName == "Office") SetButtonTarget(button, "GameScene1");
                    else if (buttonName == "End") SetButtonTarget(button, "GameSceneEnd");
                    else if (buttonName == "Stage") SetButtonTarget(button, "GameSceneStage");
                    break;

                case "GameSceneStage":
                    if (buttonName == "Stage") button.style.display = DisplayStyle.None;
                    else if (buttonName == "Office") SetButtonTarget(button, "GameScene1");
                    else if (buttonName == "End") SetButtonTarget(button, "GameSceneEnd");
                    else if (buttonName == "MingDynasty") SetButtonTarget(button, "GameSceneMingDynasty");
                    break;

                default:
                    Debug.LogError($"Unrecognized current scene: {currentScene}");
                    button.style.display = DisplayStyle.None;
                    break;
            }
        }
    }

    void SetButtonTarget(Button button, string targetScene)
    {
        // ���ð�ť��ʾ�ı�
        if (buttonDisplayTexts.TryGetValue(button.name, out string displayText))
        {
            button.text = displayText;
        }
        else
        {
            button.text = targetScene; // ���û���Զ����ı���Ĭ����ʾĿ�곡������
        }

        // ���õ���¼�
        button.style.display = DisplayStyle.Flex;
        button.clicked += () => SwitchScene(targetScene);
    }

    void SwitchScene(string sceneName)
    {
        Debug.Log($"Switching to {sceneName}");
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError($"Scene {sceneName} cannot be loaded. Check if it is added to Build Settings.");
        }
    }
}
