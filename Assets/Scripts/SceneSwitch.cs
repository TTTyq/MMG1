using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneSwitch : MonoBehaviour
{
    private VisualElement panelContainer;       // 面板容器
    private Dictionary<string, Button> sceneButtons; // 场景按钮字典
    private Dictionary<string, string> buttonDisplayTexts; // 按钮显示文本字典
    private string currentScene;               // 当前场景的名称

    void Start()
    {
        // 获取 UI 根节点
        var root = GetComponent<UIDocument>().rootVisualElement;

        // 获取面板容器
        panelContainer = root.Q<VisualElement>("SceneSwitch");
        if (panelContainer == null)
        {
            Debug.LogError("Panel container 'SceneSwitch' not found.");
            return;
        }

        // 初始化按钮字典
        sceneButtons = new Dictionary<string, Button>();

        // 动态加载可能的按钮
        AddButtonIfExists(root, "Office");
        AddButtonIfExists(root, "End");
        AddButtonIfExists(root, "MingDynasty");
        AddButtonIfExists(root, "Stage");

        // 初始化按钮显示文本字典
        buttonDisplayTexts = new Dictionary<string, string>
        {
            { "Office", "办公室" },         // 对应办公室
            { "End", "副本一：《温馨提醒：在末日捡垃圾也要绿码》" },         // 对应末日生存
            { "MingDynasty", "副本二：《重生之我在大明当皇帝》" },  // 对应明朝变革
            { "Stage", "副本三： 《粉了个神团，家人们》" } // 对应 Produce God 1024
        };

        // 获取当前场景名称
        currentScene = SceneManager.GetActiveScene().name;
        Debug.Log($"Current scene: {currentScene}");

        // 动态设置按钮
        SetUpButtons();
    }

    // 动态添加按钮到字典中
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
        // 动态设置每个按钮的目标场景和显示状态
        foreach (var kvp in sceneButtons)
        {
            var buttonName = kvp.Key;
            var button = kvp.Value;

            // 根据当前场景隐藏无关按钮并设置目标场景
            switch (currentScene)
            {
                case "GameScene1": // 当前场景是 Office
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
        // 设置按钮显示文本
        if (buttonDisplayTexts.TryGetValue(button.name, out string displayText))
        {
            button.text = displayText;
        }
        else
        {
            button.text = targetScene; // 如果没有自定义文本，默认显示目标场景名称
        }

        // 设置点击事件
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
