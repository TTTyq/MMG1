using UnityEngine;
using UnityEngine.UIElements;

public class AddStyleSheetToDocument : MonoBehaviour
{
    public UIDocument uiDocument;
    public StyleSheet styleSheet; // �� Inspector �а����� USS �ļ�

    void Start()
    {
        if (uiDocument == null || styleSheet == null)
        {
            Debug.LogError("UIDocument �� StyleSheet δ���ã�");
            return;
        }

        // ��ȡ UIDocument �ĸ� VisualElement
        VisualElement root = uiDocument.rootVisualElement;

        // �����ʽ�����ڵ�
        root.styleSheets.Add(styleSheet);
        Debug.Log("��ʽ������ӣ�");
    }
}
