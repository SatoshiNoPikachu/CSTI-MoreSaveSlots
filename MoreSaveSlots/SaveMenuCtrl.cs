using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace MoreSaveSlots;

public class SaveMenuCtrl : MonoBehaviour
{
    private static SaveMenuCtrl _instance;

    public static SaveMenuCtrl Instance
    {
        get => _instance.SafeAccess();
        private set => _instance = value;
    }

    public static void Create(SaveMenu menu)
    {
        if (Instance is not null) return;
        if (!menu) return;

        menu.gameObject.AddComponent<SaveMenuCtrl>();
    }

    private SaveMenu _menu;

    private void Awake()
    {
        Instance = this;
        _menu = GetComponent<SaveMenu>();

        MakeLocalization();

        if (CheatsManager.Instance?.CheatsActive is true) return;
        foreach (var comp in GetComponents<ActiveIfCheats>())
        {
            comp.InactiveIfCheats = !comp.InactiveIfCheats;
        }
    }

    private void MakeLocalization()
    {
        var buttonPrefab = _menu.ButtonPrefab;

        var deleteText = buttonPrefab.DeleteButton.transform.Find("ButtonText");
        SetText(deleteText?.gameObject, Localization.KeyDelete);

        var deleteConfirmText = buttonPrefab.DeleteConfirm.transform.Find("Message");
        SetText(deleteConfirmText?.gameObject, Localization.KeyDeleteConfirm);

        var deleteConfirmYesText = buttonPrefab.DeleteConfirm.transform.Find("Confirm/ButtonText");
        SetText(deleteConfirmYesText?.gameObject, Localization.KeyYes);

        var deleteConfirmNoText = buttonPrefab.DeleteConfirm.transform.Find("Cancel/ButtonText");
        SetText(deleteConfirmNoText?.gameObject, Localization.KeyNo);

        var content = transform.Find("ShadowAndPopup/Content");

        var saveGameText = content.Find("Main/SaveGame/ButtonText");
        SetText(saveGameText?.gameObject, Localization.KeySaveGame);

        var loadGameText = content.Find("Main/LoadGame/ButtonText");
        SetText(loadGameText?.gameObject, Localization.KeyLoadGame);

        var savesBackText = content.Find("Saves/Back/ButtonText");
        SetText(savesBackText?.gameObject, Localization.KeyBack);

        var loadsBackText = content.Find("Loads/Back/ButtonText");
        SetText(loadsBackText?.gameObject, Localization.KeyBack);

        var cheatsConfirmQuitText = content.Find("ConfirmQuit/CheatsConfirmQuit");
        SetText(cheatsConfirmQuitText?.gameObject, Localization.KeyDoYouWantToSave);
    }

    private static void SetText(GameObject go, string key)
    {
        var textMeshProUGUI = go?.GetComponent<TextMeshProUGUI>();
        if (textMeshProUGUI is null) return;

        var localizedStaticText = go.GetComponent<LocalizedStaticText>() ?? go.AddComponent<LocalizedStaticText>();

        localizedStaticText.LocalizedStringKey = key;
        localizedStaticText.LocalizedText = new LocalizedString
        {
            DefaultText = textMeshProUGUI.text,
            LocalizationKey = key
        };
    }

    public static void OnRefresh(SaveMenu menu)
    {
        if (!menu) return;

        var lastIndex = SaveSlotCtrl.GetLastSaveIndex();

        foreach (var text in menu.CurrentGameText)
        {
            if (!text) continue;

            var index = menu.GL.CurrentGameDataIndex;
            if (index >= 0)
            {
                text.text = string.Format(new LocalizedString
                {
                    DefaultText = text.text,
                    LocalizationKey = Localization.KeyCurrentSlot
                }, index + 1);
            }
            else
            {
                text.text = string.Format(new LocalizedString
                {
                    DefaultText = text.text,
                    LocalizationKey = Localization.KeyCurrentSlotAutoSave
                }, lastIndex + 1);
            }
        }

        menu.SavingButtons[menu.SavingButtons.Count - 1].GameIndex = lastIndex;
    }
}