using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UiController : MonoBehaviour
{
    [SerializeField]
    TMP_Text _widthValue, _heightValue,_totalText;
    [SerializeField]
    Slider _widthSlider, _heightSlider;
    [SerializeField]
    TMP_InputField _seedInput, _chestInput;

    [SerializeField]
    int _savedWidth, _savedHeight, _savedSeed, _savedRooms;
    private void Awake()
    {
        _widthSlider.onValueChanged.Invoke(_widthSlider.value);
        _heightSlider.onValueChanged.Invoke(_widthSlider.value);
        _seedInput.onEndEdit.Invoke(_seedInput.text);
        _chestInput.onEndEdit.Invoke(_chestInput.text);
    }
    public void UpdateWidth(Slider s)
    {
        int value = (int)s.value;
        _widthValue.SetText(value.ToString());
        _savedWidth = value;
    }
    public void UpdateHeight(Slider s)
    {
        int value = (int)s.value;
        _heightValue.SetText(value.ToString());
        _savedHeight = value;
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
    public void GenerateByParams()
    {
        DungeonController.instance.UpdateDungeonExternal(_savedWidth, _savedHeight, _savedRooms, _savedSeed);
    }
    public void UpdateLevelProgress(int current, int total)
    {
        _totalText.SetText($"{current} / {total}");
    }
 
}
