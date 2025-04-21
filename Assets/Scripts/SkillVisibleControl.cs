using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SkillVisibleControl : MonoBehaviour
{
    private Dictionary<Button, VisualElement> Character = new Dictionary<Button, VisualElement>();
    private VisualElement currentlyVisibleSkill = null; // 当前显示的技能描述


    void Start()
    {
        // 获取 UI 根节点
        var root = GetComponent<UIDocument>().rootVisualElement;

        Character[root.Q<Button>("Character1")] = root.Q<VisualElement>("SkillLabel1");
        Character[root.Q<Button>("Character2")] = root.Q<VisualElement>("SkillLabel2");
        Character[root.Q<Button>("Character3")] = root.Q<VisualElement>("SkillLabel3");
        Character[root.Q<Button>("Character4")] = root.Q<VisualElement>("SkillLabel4");
        Character[root.Q<Button>("Character5")] = root.Q<VisualElement>("SkillLabel5");
        Character[root.Q<Button>("Character6")] = root.Q<VisualElement>("SkillLabel6");

        // 初始化所有技能描述为隐藏
        foreach (var skill in Character.Values)
        {
            skill.style.display = DisplayStyle.None;
        }

        // 为每个按钮绑定点击事件
        foreach (var entry in Character)
        {
            var button = entry.Key;
            var skillLabel = entry.Value;

            button.clicked += () => ToggleSkillDisplay(skillLabel);
        }
    }

    // 显示或隐藏技能描述
    void ToggleSkillDisplay(VisualElement skillLabel)
    {
        if (currentlyVisibleSkill == skillLabel)
        {
            // 如果当前显示的技能是被点击的技能，则隐藏它
            skillLabel.style.display = DisplayStyle.None;
            currentlyVisibleSkill = null;
        }
        else
        {
            // 隐藏之前显示的技能
            if (currentlyVisibleSkill != null)
            {
                currentlyVisibleSkill.style.display = DisplayStyle.None;
            }

            // 显示新的技能描述
            skillLabel.style.display = DisplayStyle.Flex;
            currentlyVisibleSkill = skillLabel;
        }
    }
}
