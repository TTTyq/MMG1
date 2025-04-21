using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SkillVisibleControl : MonoBehaviour
{
    private Dictionary<Button, VisualElement> Character = new Dictionary<Button, VisualElement>();
    private VisualElement currentlyVisibleSkill = null; // ��ǰ��ʾ�ļ�������


    void Start()
    {
        // ��ȡ UI ���ڵ�
        var root = GetComponent<UIDocument>().rootVisualElement;

        Character[root.Q<Button>("Character1")] = root.Q<VisualElement>("SkillLabel1");
        Character[root.Q<Button>("Character2")] = root.Q<VisualElement>("SkillLabel2");
        Character[root.Q<Button>("Character3")] = root.Q<VisualElement>("SkillLabel3");
        Character[root.Q<Button>("Character4")] = root.Q<VisualElement>("SkillLabel4");
        Character[root.Q<Button>("Character5")] = root.Q<VisualElement>("SkillLabel5");
        Character[root.Q<Button>("Character6")] = root.Q<VisualElement>("SkillLabel6");

        // ��ʼ�����м�������Ϊ����
        foreach (var skill in Character.Values)
        {
            skill.style.display = DisplayStyle.None;
        }

        // Ϊÿ����ť�󶨵���¼�
        foreach (var entry in Character)
        {
            var button = entry.Key;
            var skillLabel = entry.Value;

            button.clicked += () => ToggleSkillDisplay(skillLabel);
        }
    }

    // ��ʾ�����ؼ�������
    void ToggleSkillDisplay(VisualElement skillLabel)
    {
        if (currentlyVisibleSkill == skillLabel)
        {
            // �����ǰ��ʾ�ļ����Ǳ�����ļ��ܣ���������
            skillLabel.style.display = DisplayStyle.None;
            currentlyVisibleSkill = null;
        }
        else
        {
            // ����֮ǰ��ʾ�ļ���
            if (currentlyVisibleSkill != null)
            {
                currentlyVisibleSkill.style.display = DisplayStyle.None;
            }

            // ��ʾ�µļ�������
            skillLabel.style.display = DisplayStyle.Flex;
            currentlyVisibleSkill = skillLabel;
        }
    }
}
