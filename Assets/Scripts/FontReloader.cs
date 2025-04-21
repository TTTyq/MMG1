using UnityEngine;
using UnityEngine.TextCore.Text;

public class FontPreloader : MonoBehaviour
{
    public FontAsset fontAsset;

    void Start()
    {
        string charactersToAdd = "需要预加载的字符"; // 替换为需要显示的字符
        if (fontAsset.TryAddCharacters(charactersToAdd, out string missingCharacters))
        {
            Debug.Log($"Successfully added characters: {charactersToAdd}");
        }
        else
        {
            Debug.LogError($"Missing characters: {missingCharacters}");
        }
    }
}