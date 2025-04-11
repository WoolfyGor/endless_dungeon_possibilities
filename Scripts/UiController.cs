using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UiController : MonoBehaviour
{
    [SerializeField]
    TMP_Text _widthValue, _heightValue,_totalText,_roomsValue;
    [SerializeField]
    Slider _widthSlider, _heightSlider,_roomsSlider;
    [SerializeField]
    TMP_InputField _seedInput;

    [SerializeField]
    int _savedWidth, _savedHeight, _savedSeed, _savedRooms;
    private void Awake()
    {
        _widthSlider.onValueChanged.Invoke(_widthSlider.value);
        _heightSlider.onValueChanged.Invoke(_widthSlider.value);
        _roomsSlider.onValueChanged.Invoke(_roomsSlider.value);
        _seedInput.onEndEdit.Invoke(_seedInput.text);
    }
    public void UpdateWidth(Slider s)
    {
        int value = (int)s.value;
        _widthValue.SetText(value.ToString());
        _savedWidth = value;
        _roomsSlider.maxValue = RecalculateMaxRooms();
    }
    public void UpdateHeight(Slider s)
    {
        int value = (int)s.value;
        _heightValue.SetText(value.ToString());
        _savedHeight = value;
        _roomsSlider.maxValue = RecalculateMaxRooms();
    }
    public void UpdateSeed(TMP_InputField field)
    {
        string newSeed = field.text;
        int.TryParse(newSeed, out int result);
        _savedSeed = result;
    }
    public void UpdateRooms(TMP_InputField field)
    {
        string newRooms = field.text;
        int.TryParse(newRooms, out int result);
        _savedRooms = result;
    }
    public void UpdateRooms(Slider s)
    {
        int value = (int)s.value;
        _roomsValue.SetText(value.ToString());
        _savedRooms = value;
    }
    public void GenerateByParams()
    {
        DungeonController.instance.UpdateDungeonExternal(_savedWidth, _savedHeight, _savedRooms, _savedSeed);
    }
    public void UpdateLevelProgress(int current, int total)
    {
        _totalText.SetText($"{current} / {total}");
    }
 
    private int RecalculateMaxRooms()
    {
        return _savedHeight * _savedWidth / 12;
    }
}
