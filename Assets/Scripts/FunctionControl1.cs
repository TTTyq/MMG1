using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FunctionControl1 : MonoBehaviour
{
    private Dictionary<Button, VisualElement> Function = new Dictionary<Button, VisualElement>();
    private VisualElement currentlyVisibleFunction = null; // 当前显示的技能描述


    void Start()
    {
        // 获取 UI 根节点
        var root = GetComponent<UIDocument>().rootVisualElement;

        Function[root.Q<Button>("FunctionClue")] = root.Q<VisualElement>("FunctionFace1");
        Function[root.Q<Button>("FunctionSkill")] = root.Q<VisualElement>("FunctionFace2");
        Function[root.Q<Button>("FunctionTask")] = root.Q<VisualElement>("FunctionFace3");

        // 初始化所有技能描述为隐藏
        foreach (var function in Function.Values)
        {
            function.style.display = DisplayStyle.None;
        }

        // 为每个按钮绑定点击事件
        foreach (var entry in Function)
        {
            var button = entry.Key;
            var functionLabel = entry.Value;

            button.clicked += () => ToggleSkillDisplay (functionLabel);
        }
    }

    // 显示或隐藏技能描述
    void ToggleSkillDisplay(VisualElement functionLabel)
    {
        if (currentlyVisibleFunction == functionLabel)
        {
            // 如果当前显示的技能是被点击的技能，则隐藏它
            functionLabel.style.display = DisplayStyle.None;
            currentlyVisibleFunction = null;
        }
        else
        {
            // 隐藏之前显示的技能
            if (currentlyVisibleFunction != null)
            {
                currentlyVisibleFunction.style.display = DisplayStyle.None;
            }

            // 显示新的技能描述
            functionLabel.style.display = DisplayStyle.Flex;
            currentlyVisibleFunction = functionLabel;
        }
    }
}
