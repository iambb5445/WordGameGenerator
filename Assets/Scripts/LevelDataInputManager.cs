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
    private static Dictionary<Dropdown.OptionData, GridLevel.MovementType> GridMovementOptions =
        new Dictionary<Dropdown.OptionData, GridLevel.MovementType>() {
            {new Dropdown.OptionData("Choose Movement"), GridLevel.MovementType.None},
            {new Dropdown.OptionData("8 Directional"), GridLevel.MovementType.eightDirectoinal},
            {new Dropdown.OptionData("4 Directional"), GridLevel.MovementType.fourDirectoinal},
            {new Dropdown.OptionData("8 Directional, straight"), GridLevel.MovementType.eightDirectoinalNoChange},
            {new Dropdown.OptionData("4 Directional, straight"), GridLevel.MovementType.fourDirectoinalNoChange}
        };
    private static Dictionary<Dropdown.OptionData, StructuredCircleLevel.MovementType> CircleMovementOptions =
        new Dictionary<Dropdown.OptionData, StructuredCircleLevel.MovementType> {
            {new Dropdown.OptionData("Choose Movement"), StructuredCircleLevel.MovementType.None},
            {new Dropdown.OptionData("Any Direction"), StructuredCircleLevel.MovementType.Any},
            {new Dropdown.OptionData("Clockwise"), StructuredCircleLevel.MovementType.Clockwise}
        };
    [SerializeField]
    GameObject gridSize;
    [SerializeField]
    GameObject circleSize;
    [SerializeField]
    GameObject movementSelection;
    [SerializeField]
    InputField rowCount;
    [SerializeField]
    InputField columnCount;
    [SerializeField]
    InputField nodeCount;
    [SerializeField]
    InputField themeInput;
    [SerializeField]
    Slider difficultySlider;
    [SerializeField]
    Dropdown APIDropdown;
    [SerializeField]
    Dropdown levelShapeDropdown;
    [SerializeField]
    Dropdown movementDropdown;
    [SerializeField]
    Button generateButton;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            generateButton.interactable = false;
            APIDropdown.options = new List<Dropdown.OptionData>(APIOptions.Keys);
            levelShapeDropdown.options = LevelShapeOptions;
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
        if (levelShapeDropdown.value == GridOptionIndex)
        {
            gridSize.SetActive(true);
            circleSize.SetActive(false);
            movementSelection.SetActive(true);
            movementDropdown.options = new List<Dropdown.OptionData>(GridMovementOptions.Keys);
        }
        else if (levelShapeDropdown.value == CircleOptionIndex)
        {
            gridSize.SetActive(false);
            circleSize.SetActive(true);
            movementSelection.SetActive(true);
            movementDropdown.options = new List<Dropdown.OptionData>(CircleMovementOptions.Keys);
        }
        else
        {
            gridSize.SetActive(false);
            circleSize.SetActive(false);
            movementSelection.SetActive(false);
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
        if (levelShapeDropdown.value == GridOptionIndex)
        {
            int rows, columns;
            GridLevel.MovementType movementType = GridMovementOptions[movementDropdown.options[movementDropdown.value]];
            if (int.TryParse(rowCount.text, out rows) && int.TryParse(columnCount.text, out columns) &&
                movementType != GridLevel.MovementType.None)
            {
                level = new GridLevel(rows, columns, movementType);
            }
        }
        else if (levelShapeDropdown.value == CircleOptionIndex)
        {
            int nodeCount;
            StructuredCircleLevel.MovementType movementType =
                CircleMovementOptions[movementDropdown.options[movementDropdown.value]];
            if (int.TryParse(this.nodeCount.text, out nodeCount) && movementType != StructuredCircleLevel.MovementType.None)
            {
                level = new StructuredCircleLevel(nodeCount, movementType);
            }
        }
        if (theme.Length == 0 || api == null || level == null)
        {
            return null;
        }
        return new LevelData(theme, level, api);
    }
}
