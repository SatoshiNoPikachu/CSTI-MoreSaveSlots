using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MoreSaveSlots;

public class PageButton : MonoBehaviour
{
    private static PageButton CreateButton(MainMenu menu)
    {
        var tmp = menu?.EditCharacterButton?.transform.parent?.gameObject;
        if (tmp is null) return null;
        if (menu.GameSlots?.Length is null or 0) return null;

        var parent = menu.GameSlots[0]?.transform.parent?.parent?.parent;
        if (parent is null) return null;

        var obj = Instantiate(tmp, parent, false);
        return obj.AddComponent<PageButton>();
    }

    public static PageButton CreatePreButton(MainMenu menu)
    {
        var btn = CreateButton(menu);
        if (btn is null) return null;
        btn.name = "PreviousButton";
        btn.Setup(-758, "上一页");
        btn.SetInteractable(false);

        return btn;
    }

    public static PageButton CreateNextButton(MainMenu menu)
    {
        var btn = CreateButton(menu);
        if (btn is null) return null;
        btn.name = "NextButton";
        btn.Setup(775, "下一页");

        return btn;
    }

    private Button _button;

    private void Setup(float x, string text)
    {
        transform.localPosition = new Vector3(x, 275, 0);

        _button = transform.Find("ButtonObject")?.GetComponent<Button>();
        if (_button is not null) _button.onClick = new Button.ButtonClickedEvent();

        SetText(text);
    }

    private void SetText(string text)
    {
        var textObj = transform.Find("ButtonText");

        var textMeshProUGUI = textObj?.GetComponent<TextMeshProUGUI>();
        if (textMeshProUGUI is null) return;

        var localizedStaticText = textObj.GetComponent<LocalizedStaticText>();
        if (localizedStaticText is null) return;

        textMeshProUGUI.SetText(text);
        localizedStaticText.LocalizedStringKey = Plugin.PluginName;
    }

    public void SetInteractable(bool interactable)
    {
        _button.interactable = interactable;
    }

    public void AddListener(UnityAction action)
    {
        _button?.onClick.AddListener(action);
    }
}