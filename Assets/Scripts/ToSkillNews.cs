using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ToSkillNews : MonoBehaviour
{
    private Button nextButton;     // Next 按钮
    private Button warningButton;  // Warning 按钮

    private static bool hasWarningButtonShown = false; // 静态变量，确保一局游戏只显示一次

    void Start()
    {
        // 获取 UIDocument 的根节点
        var root = GetComponent<UIDocument>().rootVisualElement;

        // 获取 Next 按钮和 Warning 按钮
        nextButton = root.Q<Button>("dmnext");
        warningButton = root.Q<Button>("Warning");

        // 初始化 Warning 按钮
        if (warningButton != null)
        {
            warningButton.style.display = DisplayStyle.None; // 初始隐藏
            warningButton.clicked += OnWarningButtonClicked; // 添加点击事件
        }
    }

    void Update()
    {
        // 检查 Next 按钮是否已经隐藏
        if (nextButton != null && nextButton.style.display == DisplayStyle.None && !hasWarningButtonShown)
        {
            ShowWarningButton(); // 显示 Warning 按钮
        }
    }

    private void ShowWarningButton()
    {
        if (warningButton != null)
        {
            warningButton.style.display = DisplayStyle.Flex; // 显示 Warning 按钮
            hasWarningButtonShown = true; // 标记为已经显示
        }
    }

    private void OnWarningButtonClicked()
    {
        Debug.Log("Warning button clicked. Loading KillNews scene...");
        SceneManager.LoadScene("KillNews"); // 加载下一个场景
    }
}
