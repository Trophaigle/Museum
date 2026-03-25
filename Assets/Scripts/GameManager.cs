using UnityEngine;

public class GameManager : MonoBehaviour
{
       // Singleton
    public static GameManager Instance { get; private set; }

    public MIDIPianoPlayer pianoPlayer;
    public bool isPianoPlaying = false;

    // Enum des états du jeu
    public enum GameState
    {
        Intro,
        Game
    }

    private GameState currentState;

    void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // optionnel

        currentState = GameState.Intro; // état par défaut

        if(pianoPlayer == null)
        {
            Debug.LogError("MIDIPianoPlayer not assigned !");
        }
        StartPlayPiano();
    }

    // Getter
    public GameState GetState()
    {
        return currentState;
    }

    // Setter
    public void SetState(GameState newState)
    {
        if (currentState == newState) return;

        currentState = newState;

        // Tu peux déclencher des actions ici selon l'état
        switch (currentState)
        {
            case GameState.Intro:
                Debug.Log("Etat : Intro");
                break;

            case GameState.Game:
                Debug.Log("Etat : Game");
                break;
        }
    }

    public bool IsState(GameState state)
    {
        return currentState == state;
    }

    public void StartPlayPiano()
    {
        pianoPlayer.PlayMusic();
        isPianoPlaying = true;
    }

    public void StopPlayPiano()
    {
        pianoPlayer.StopMusic();
        isPianoPlaying = false;
    }
}
