using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelData
{
    public string theme;
    public Level level;
    public API api;
    public LevelData(string theme, Level level, API api)
    {
        this.theme = theme;
        this.level = level;
        this.api = api;
    }
}

public class LevelDataInputManager : MonoBehaviour {
    private static LevelDataInputManager instance;
    private static Dictionary<Dropdown.OptionData, API> APIOptions = new Dictionary<Dropdown.OptionData, API>(){
        {new Dropdown.OptionData("Choose API"), null},
        {new Dropdown.OptionData("Datamuse Association"), new Datamuse.AssociationAPI()},
        {new Dropdown.OptionData("Datamuse Similar Meaning"), new Datamuse.SimilarMeaningAPI()}
    };
    private static int GridOptionIndex = 1;
    private static int CircleOptionIndex = 2;
    private static List<Dropdown.OptionData> LevelShapeOptions = new List<Dropdown.OptionData>(){
        new Dropdown.OptionData("Choose Shape"),
        new Dropdown.OptionData("Grid"),
        new Dropdown.OptionData("Circle")
    };
    [SerializeField]
    GameObject GridSize;
    [SerializeField]
    GameObject CircleSize;
    [SerializeField]
    InputField RowCount;
    [SerializeField]
    InputField ColumnCount;
    [SerializeField]
    InputField NodeCount;
    [SerializeField]
    InputField themeInput;
    [SerializeField]
    Slider difficultySlider;
    [SerializeField]
    Dropdown APIDropdown;
    [SerializeField]
    Dropdown levelShapeDropDown;
    [SerializeField]
    Button generateButton;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            generateButton.interactable = false;
            APIDropdown.options = new List<Dropdown.OptionData>(APIOptions.Keys);
            levelShapeDropDown.options = LevelShapeOptions;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public static LevelDataInputManager getInstance()
    {
        return instance;
    }
    public void updateInput()
    {
        if (levelShapeDropDown.value == GridOptionIndex)
        {
            GridSize.SetActive(true);
            CircleSize.SetActive(false);
        }
        else if (levelShapeDropDown.value == CircleOptionIndex)
        {
            GridSize.SetActive(false);
            CircleSize.SetActive(true);
        }
        else
        {
            GridSize.SetActive(false);
            CircleSize.SetActive(false);
        }
        if (getLevelData() != null)
        {
            generateButton.interactable = true;
        }
        else
        {
            generateButton.interactable = false;
        }
    }
    public LevelData getLevelData()
    {
        string theme = themeInput.text;
        API api = APIOptions[APIDropdown.options[APIDropdown.value]];
        Level level = null;
        if (levelShapeDropDown.value == GridOptionIndex)
        {
            int rows, columns;
            if (int.TryParse(RowCount.text, out rows) && int.TryParse(ColumnCount.text, out columns))
            {
                level = new GridLevel(rows, columns, GridLevel.eightDirectionsMovement);
            }
        }
        else if (levelShapeDropDown.value == CircleOptionIndex)
        {
            int nodeCount;
            if (int.TryParse(NodeCount.text, out nodeCount))
            {
                level = new CircleLevel(nodeCount);
            }
        }
        if (theme.Length == 0 || api == null || level == null)
        {
            return null;
        }
        return new LevelData(theme, level, api);
    }
}
