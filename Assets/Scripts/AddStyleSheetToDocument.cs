using UnityEngine;
using UnityEngine.UIElements;

public class AddStyleSheetToDocument : MonoBehaviour
{
    public UIDocument uiDocument;
    public StyleSheet styleSheet; // 在 Inspector 中绑定您的 USS 文件

    void Start()
    {
        if (uiDocument == null || styleSheet == null)
        {
            Debug.LogError("UIDocument 或 StyleSheet 未设置！");
            return;
        }

        // 获取 UIDocument 的根 VisualElement
        VisualElement root = uiDocument.rootVisualElement;

        // 添加样式表到根节点
        root.styleSheets.Add(styleSheet);
        Debug.Log("样式表已添加！");
    }
}
