using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SceneChangePanel : MonoBehaviour
{
    private VisualElement functionPanelContainer; // 面板容器
    private Button arrowButton;                  // 箭头按钮
    private bool isPanelUp = false;              // 面板是否升上来

    void Start()
    {
        // 获取 UI 根节点
        var root = GetComponent<UIDocument>().rootVisualElement;

        // 获取面板和按钮
        functionPanelContainer = root.Q<VisualElement>("SceneChangePanel");
        arrowButton = root.Q<Button>("Arrow");

        // 初始化面板位置
        functionPanelContainer.style.translate = new StyleTranslate(new Translate(0, 0, 0)); // 初始隐藏
        arrowButton.style.rotate = new StyleRotate(new Rotate(180)); // 箭头初始向下

        // 设置过渡效果
        functionPanelContainer.style.transitionProperty = new StyleList<StylePropertyName>(
            new List<StylePropertyName> { new StylePropertyName("translate") }
        );
        functionPanelContainer.style.transitionDuration = new StyleList<TimeValue>(
            new List<TimeValue> { new TimeValue(0.3f, TimeUnit.Second) }
        );

        // 绑定按钮事件
        arrowButton.clicked += TogglePanel;
    }

    void TogglePanel()
    {
        // 切换面板状态
        isPanelUp = !isPanelUp;

        // 动态调整面板位置
        functionPanelContainer.style.translate = isPanelUp
            ? new StyleTranslate(new Translate(0, -200, 0))     // 升上来
            : new StyleTranslate(new Translate(0, 0, 0)); // 降下去

        // 翻转箭头
        arrowButton.style.rotate = new StyleRotate(new Rotate(isPanelUp ? 0 : 180));
    }
}
