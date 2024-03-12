using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;


public class Lobby : MonoBehaviour
{
    private UIDocument _uIDocument;
    private VisualElement _rootElement;

    private Button _refreshButton;
    private Button _backButton;

    [SerializeField] GameObject mainMenuGO;

    private VisualElement _listOfLobbies;
    private ScrollView _listLobbiesScrollView;


    private void Awake()
    {
        _uIDocument = GetComponent<UIDocument>();
    }


    private void OnEnable()
    {
        if (_uIDocument == null) return;
        _rootElement = _uIDocument.rootVisualElement;
        _listOfLobbies = _rootElement.Q<VisualElement>("ListOfLobbies");
        _listLobbiesScrollView = _listOfLobbies?.Q<ScrollView>();
        _backButton = _rootElement.Q<Button>("BackButton");
        _refreshButton = _rootElement.Q<Button>("RefreshButton");
        _backButton.clicked += BackToMainMenu;
        _refreshButton.clicked += RefreshLobbies;
        //_listLobbiesScrollView.Add(CreateLobbyContainer(i));

    }

    private void OnDisable()
    {
        _backButton.clicked -= BackToMainMenu;
        _refreshButton.clicked -= RefreshLobbies;
    }


    private void RefreshLobbies()
    {
    }

    private void BackToMainMenu()
    {
        mainMenuGO.SetActive(true);
        gameObject.SetActive(false);
    }


    private VisualElement CreateLobbyContainer(int lobbyIndex)
    {
        VisualElement lobbyContainer = new VisualElement();

        // Add a Label to the LobbyContainer for Lobby Name
        Label lobbyNameLabel = new Label("Lobby Name " + lobbyIndex);
        lobbyNameLabel.AddToClassList("leftLabelLobby");
        lobbyContainer.Add(lobbyNameLabel);

        // Add a Button to the LobbyContainer for joining the lobby
        Button joinButton = new Button();
        joinButton.text = "Join Lobby";
        joinButton.AddToClassList("defaultButtonCLass");
        lobbyContainer.Add(joinButton);

        // Add a Label to the LobbyContainer for Lobby Status
        Label lobbyStatusLabel = new Label("0/20");
        lobbyStatusLabel.AddToClassList("leftLabelLobby");
        lobbyStatusLabel.AddToClassList("rightLabelLobby");
        lobbyContainer.Add(lobbyStatusLabel);

        return lobbyContainer;
    }
}