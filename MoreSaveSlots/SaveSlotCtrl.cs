using System.IO;
using UnityEngine;

namespace MoreSaveSlots;

public class SaveSlotCtrl : MonoBehaviour
{
    private static SaveSlotCtrl _instance;

    public static SaveSlotCtrl Instance
    {
        get => _instance.SafeAccess();
        private set => _instance = value;
    }

    public static void Create(MainMenu menu)
    {
        if (Instance is not null) return;
        if (!menu || menu.GameSlots.Length == 0) return;

        var parent = menu.GameSlots[0]?.transform.parent?.parent?.parent.gameObject;
        if (parent is null) return;

        var ctrl = parent.gameObject.AddComponent<SaveSlotCtrl>();
        Instance = ctrl;
        ctrl.Init(menu);
    }

    public static string GenerateGameFileName(int index)
    {
        return $"Slot_{index + 1}.json";
    }

    public static void DeleteSlot(MainMenu menu, int index)
    {
        if (!menu.SaveData) return;

        menu.SaveData.DeleteGameData(index);
        Instance.ChangePage();
    }

    public static void OnMenuButtonSetup(MenuSlotButton button)
    {
        if (button is null) return;

        button.LoadedCharacter = null;
    }

    public static void OnSaveGame(ref int index)
    {
        if (index < 0)
        {
            index = GetLastSaveIndex();
        }
    }

    public static void DeleteGameData(GameLoad load, int index)
    {
        if (index < 0 || index >= load.Games.Count)
        {
            return;
        }

        var path = GameLoad.GameFilePath(load.Games[index].FileName);

        load.Games[index] = new GameSaveFile(GameLoad.GenerateGameFileName(index), index);

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        if (File.Exists(GameLoad.WritingFileBackupPath(path)))
        {
            File.Delete(GameLoad.WritingFileBackupPath(path));
        }

        for (var i = 0; i < load.Games.Count; i++)
        {
            load.Games[i].SlotIndex = i;
        }

        if (index == load.CurrentGameDataIndex)
        {
            load.CurrentGameDataIndex = load.Games.Count;
        }
    }

    public static int GetLastSaveIndex()
    {
        var saves = GameLoad.Instance.Games;

        var i = saves.Count - 1;
        for (; i >= 0; i--)
        {
            var save = saves[i];
            if (save.MainData.HasCardsData) break;
        }

        return ++i;
    }

    private MainMenu _menu;
    private PageButton _btnPre;
    private PageButton _btnNext;
    private int _page;

    private void OnDestroy()
    {
        Instance = null;
    }

    private void Init(MainMenu menu)
    {
        _menu = menu;
        InitButton(menu);
    }

    private void InitButton(MainMenu menu)
    {
        _btnPre = PageButton.CreatePreButton(menu);
        _btnNext = PageButton.CreateNextButton(menu);

        if (_btnPre is null || _btnNext is null)
        {
            Plugin.Log.LogWarning("PageButton not initialized.");
            return;
        }

        _btnPre.AddListener(PreviousPage);
        _btnNext.AddListener(NextPage);
    }

    public void PreviousPage()
    {
        if (_page == 0) return;

        _page -= 1;
        ChangePage();
    }

    public void NextPage()
    {
        _page += 1;
        ChangePage();
    }

    public void ChangePage()
    {
        var minIndex = _page * 4;
        var maxIndex = minIndex + 3;

        _btnPre.SetInteractable(_page != 0);

        var saves = GameLoad.Instance.Games;
        while (saves.Count <= maxIndex)
        {
            var index = saves.Count;
            saves.Add(new GameSaveFile(GenerateGameFileName(index), index));
        }

        var hasData = false;
        for (var i = minIndex; i < saves.Count; i++)
        {
            if (saves[i]?.MainData?.HasCardsData is null or false) continue;
            hasData = true;
            break;
        }

        _btnNext.SetInteractable(hasData);

        var slots = _menu.GameSlots;
        for (var i = 0; i < slots.Length; i++)
        {
            var index = i + minIndex;
            slots[i].Setup(saves[index].MainData, index, _menu);
        }

        if (slots.Length != 4)
        {
            Debug.LogWarning(string.Concat([
                "There's ",
                slots.Length,
                " game slots set up in the menu for ",
                4,
                " official save slots."
            ]));
        }
    }
}