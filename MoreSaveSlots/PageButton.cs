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
        if (!tmp) return null;
        if (menu.GameSlots?.Length is null or 0) return null;

        var parent = menu.GameSlots[0]?.transform.parent?.parent?.parent;
        if (!parent) return null;

        var obj = Instantiate(tmp, parent, false);
        return obj.AddComponent<PageButton>();
    }

    public static PageButton CreatePreButton(MainMenu menu)
    {
        var btn = CreateButton(menu);
        if (!btn) return null;
        btn.name = "PreviousButton";
        btn.Setup(-758, "上一页", Localization.KeyPreviousPage);
        btn.SetInteractable(false);

        return btn;
    }

    public static PageButton CreateNextButton(MainMenu menu)
    {
        var btn = CreateButton(menu);
        if (!btn) return null;
        btn.name = "NextButton";
        btn.Setup(775, "下一页", Localization.KeyNextPage);

        return btn;
    }

    private Button _button;

    private void Setup(float x, string text, string key)
    {
        transform.localPosition = new Vector3(x, 275, 0);

        _button = transform.Find("ButtonObject")?.GetComponent<Button>();
        if (_button) _button.onClick = new Button.ButtonClickedEvent();

        SetText(text, key);
    }

    private void SetText(string text, string key)
    {
        var textObj = transform.Find("ButtonText");

        var textMeshProUGUI = textObj?.GetComponent<TextMeshProUGUI>();
        if (textMeshProUGUI is null) return;

        var localizedStaticText = textObj.GetComponent<LocalizedStaticText>();
        if (localizedStaticText is null) return;

        textMeshProUGUI.SetText(text);
        localizedStaticText.LocalizedStringKey = key;
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