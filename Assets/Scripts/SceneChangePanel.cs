using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SceneChangePanel : MonoBehaviour
{
    private VisualElement functionPanelContainer; // �������
    private Button arrowButton;                  // ��ͷ��ť
    private bool isPanelUp = false;              // ����Ƿ�������

    void Start()
    {
        // ��ȡ UI ���ڵ�
        var root = GetComponent<UIDocument>().rootVisualElement;

        // ��ȡ���Ͱ�ť
        functionPanelContainer = root.Q<VisualElement>("SceneChangePanel");
        arrowButton = root.Q<Button>("Arrow");

        // ��ʼ�����λ��
        functionPanelContainer.style.translate = new StyleTranslate(new Translate(0, 0, 0)); // ��ʼ����
        arrowButton.style.rotate = new StyleRotate(new Rotate(180)); // ��ͷ��ʼ����

        // ���ù���Ч��
        functionPanelContainer.style.transitionProperty = new StyleList<StylePropertyName>(
            new List<StylePropertyName> { new StylePropertyName("translate") }
        );
        functionPanelContainer.style.transitionDuration = new StyleList<TimeValue>(
            new List<TimeValue> { new TimeValue(0.3f, TimeUnit.Second) }
        );

        // �󶨰�ť�¼�
        arrowButton.clicked += TogglePanel;
    }

    void TogglePanel()
    {
        // �л����״̬
        isPanelUp = !isPanelUp;

        // ��̬�������λ��
        functionPanelContainer.style.translate = isPanelUp
            ? new StyleTranslate(new Translate(0, -200, 0))     // ������
            : new StyleTranslate(new Translate(0, 0, 0)); // ����ȥ

        // ��ת��ͷ
        arrowButton.style.rotate = new StyleRotate(new Rotate(isPanelUp ? 0 : 180));
    }
}
