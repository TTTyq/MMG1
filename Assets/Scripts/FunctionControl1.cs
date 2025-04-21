using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FunctionControl1 : MonoBehaviour
{
    private Dictionary<Button, VisualElement> Function = new Dictionary<Button, VisualElement>();
    private VisualElement currentlyVisibleFunction = null; // ��ǰ��ʾ�ļ�������


    void Start()
    {
        // ��ȡ UI ���ڵ�
        var root = GetComponent<UIDocument>().rootVisualElement;

        Function[root.Q<Button>("FunctionClue")] = root.Q<VisualElement>("FunctionFace1");
        Function[root.Q<Button>("FunctionSkill")] = root.Q<VisualElement>("FunctionFace2");
        Function[root.Q<Button>("FunctionTask")] = root.Q<VisualElement>("FunctionFace3");

        // ��ʼ�����м�������Ϊ����
        foreach (var function in Function.Values)
        {
            function.style.display = DisplayStyle.None;
        }

        // Ϊÿ����ť�󶨵���¼�
        foreach (var entry in Function)
        {
            var button = entry.Key;
            var functionLabel = entry.Value;

            button.clicked += () => ToggleSkillDisplay (functionLabel);
        }
    }

    // ��ʾ�����ؼ�������
    void ToggleSkillDisplay(VisualElement functionLabel)
    {
        if (currentlyVisibleFunction == functionLabel)
        {
            // �����ǰ��ʾ�ļ����Ǳ�����ļ��ܣ���������
            functionLabel.style.display = DisplayStyle.None;
            currentlyVisibleFunction = null;
        }
        else
        {
            // ����֮ǰ��ʾ�ļ���
            if (currentlyVisibleFunction != null)
            {
                currentlyVisibleFunction.style.display = DisplayStyle.None;
            }

            // ��ʾ�µļ�������
            functionLabel.style.display = DisplayStyle.Flex;
            currentlyVisibleFunction = functionLabel;
        }
    }
}
