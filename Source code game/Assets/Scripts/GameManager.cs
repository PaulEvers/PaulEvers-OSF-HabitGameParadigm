using System;
using System.Collections;
using TMPro;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    private enum GameState
    {
        Introduction,
        Practice, // No reward yet, familiarilizing with controls
        Training, // Training phase, with reward.
        DevalueIntroduction,
        Test,
        Debrief
    }

    private GameState gameState = GameState.Practice;

    [SerializeField]
    GenerateMaze generateMazeController;
    [SerializeField]
    CountdownTimer countdownTimer;
    [SerializeField]
    public PlayerMovement player;
    [SerializeField]
    public CoinController coinController;
    [SerializeField]
    public DataController dataController;
    [SerializeField]
    public AIAgent finishAi;

    public static GameManager Instance { get; private set; }

    public int practiceRounds = 5;
    public int trainingRounds = 5;
    public int testRounds = 5;

    public int round { get; private set; } = 1;
    public int totalRound { get; private set; } = 0;
    public int score { get; set; } = 0;

    public bool isCoinDevalued = false;

    private int[] practiceLevels;
    private int[] trainingLevels;
    private int[] testLevels;

    public int currentSeed;

    public float currentTotalDistance = 0;

    // UI
    public UIManager uiManager;

    // Database data
    public Participant participantData = new Participant();

    private string currentDate;
    public bool pickedUpCoin = false;
    private decimal coinPickupTime;

    public int day = 1;

    [SerializeField] private TMP_Text dayText;

    [SerializeField]
    public CharacterAnimationController playerAnimationController;

    [SerializeField] private GameObject sceneControllerDay1;
    [SerializeField] private GameObject sceneControllerDay2;
    [SerializeField] private GameObject sceneControllerDay3;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayerPrefs.SetInt("Day", day);
        dayText.text = "Day " + day;
        participantData.email = PlayerPrefs.GetString("ParticipantEmail");
        currentDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        if (day == 1)
        {
            sceneControllerDay1.SetActive(true);
            GeneratePracticeLevels();
        }

        if (day == 2 || day == 3)
        {
            gameState = GameState.Training;
            StartCoroutine(WaitAndLoadParticipant());
        }

        if (day == 2)
        {
            sceneControllerDay2.SetActive(true);
        }

        if (day == 3)
        {
            sceneControllerDay3.SetActive(true);
        }

        GenerateLevelOrder();
    }


    IEnumerator WaitAndLoadParticipant()
    {
        yield return new WaitForSeconds(1);
        dataController.LoadParticipant(participantData.email);
    }

    private void GeneratePracticeLevels()
    {
        int startValue = 900; // Starting value for seeds
        practiceLevels = new int[practiceRounds]; // Create an array with the size of testRounds

        for (int i = 0; i < practiceRounds; i++)
        {
            practiceLevels[i] = startValue + i; // Increment the start value by 1 for each element creating consistent seeds e.g. 900, 901, 902...
        }

        Debug.Log("Pratice Levels: " + string.Join(", ", practiceLevels));
    }

    private void GenerateLevelOrder()
    {
        System.DateTime now = System.DateTime.Now;
        int minutes = now.Minute;
        int seconds = now.Second;
        int milliseconds = now.Millisecond;
        int seed = minutes * 100000 + seconds * 1000 + milliseconds;

        UnityEngine.Random.InitState(seed);
        Debug.Log(seed);

        int totalRounds = trainingRounds + testRounds;

        // Create an array with numbers from 1 to 100
        int[] allLevels = new int[totalRounds];
        for (int i = 0; i < totalRounds; i++)
        {
            allLevels[i] = i + 1;
        }

        // Shuffle the array
        for (int i = allLevels.Length - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1); // Get a random index between 0 and i
            // Swap numbers[i] and numbers[j]
            int temp = allLevels[i];
            allLevels[i] = allLevels[j];
            allLevels[j] = temp;
        }

        Debug.Log("Training Levels: " + string.Join(", ", allLevels));

        trainingLevels = new int[trainingRounds];
        testLevels = new int[testRounds];

        Array.Copy(allLevels, 0, trainingLevels, 0, trainingRounds);
        Array.Copy(allLevels, trainingRounds, testLevels, 0, testRounds);

        Debug.Log("Training Levels: " + string.Join(", ", trainingLevels));
        Debug.Log("Test Levels: " + string.Join(", ", testLevels));

        participantData.trainingSeeds = string.Join(", ", trainingLevels);
        participantData.testSeeds = string.Join(", ", testLevels);
    }

    public void FinishRound(bool reachedFinish)
    {
        dataController.Stop();
        player.enabled = false;

        Rigidbody2D rbPlayer = player.gameObject.GetComponent<Rigidbody2D>();
        rbPlayer.linearVelocity = Vector2.zero;
        player.moveVelocity = Vector2.zero;

        countdownTimer.StopTimer();


        // Prepare DB data
        Round roundInfo = new Round();

        roundInfo.participantEmail = participantData.email;
        roundInfo.seed = currentSeed;
        roundInfo.round = round;
        roundInfo.didCoinSpawn = coinController.hasSpawned;
        roundInfo.pickedUpCoin = pickedUpCoin;
        roundInfo.finished = reachedFinish;
        roundInfo.phase = gameState.ToString();
        roundInfo.date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        roundInfo.totalDistance = Math.Round((decimal)coinController.totalDistance, 2); ;
        roundInfo.distanceCoinSpawn = Math.Round((decimal)coinController.spawnDistance, 2); ;
        roundInfo.remainingTime = Math.Round((decimal)countdownTimer.timeRemaining, 2);
        roundInfo.totalRoundsFinished = totalRound;
        roundInfo.day = day;
        roundInfo.coinPickupTime = coinPickupTime; // TODO: Add to db
        roundInfo.coinSpawnTime = coinController.spawnTime; // TODO: Add to db

        RoundData roundData = new RoundData();
        roundData.roundLogs = dataController.roundLogs;
        roundData.round = roundInfo;


        dataController.InsertRoundDB(roundData);


        if (reachedFinish && gameState != GameState.Practice)
        {
            score += 2;
            uiManager.SetScore(score);
        }

        // Check if finished practice
        if (gameState == GameState.Practice && round == practiceRounds)
        {
            gameState = GameState.Training;
            uiManager.ShowFinishedPractice();
            round = 1;
            score = 0;
            return;
        }

        // Check if finished training and is day 2 so should not continue to testing
        if (gameState == GameState.Training && round == trainingRounds && day == 2)
        {
            gameState = GameState.Debrief;
            uiManager.ShowFinishedTest();
            round = 1;
            return;
        }


        // Check if finished training
        if (gameState == GameState.Training && round == trainingRounds)
        {
            isCoinDevalued = true;
            gameState = GameState.Test;
            uiManager.ShowFinishedTraining();
            round = 1;
            return;
        }

        // Check if finished test
        if (gameState == GameState.Test && round == testRounds)
        {
            gameState = GameState.Debrief;
            uiManager.ShowFinishedTest();
            round = 1;
            return;
        }

        uiManager.ShowRoundFinished(reachedFinish);
    }

    public void PickUpCoin()
    {
        pickedUpCoin = true;
        coinPickupTime = Math.Round((decimal)countdownTimer.timeRemaining, 2);
        if (isCoinDevalued) { return; }
        if (gameState == GameState.Practice) { return; }
        score++;
        uiManager.SetScore(score);
    }

    public void NextRound(bool isNewPhase, bool isEngaging, bool isFirst)
    {
        uiManager.HideUI();
        if (!isNewPhase) { round++; }
        totalRound++;
        GenerateSeed();

        coinPickupTime = 0;
        uiManager.SetRound(round);
        generateMazeController.CreateMaze();
        player.transform.position = new Vector2(9, 0);
        finishAi.StartPathFinding();

        //StartCoroutine(CheckTotalDistanceCoroutine());

        coinController.Reset();

        countdownTimer.Reset();
        countdownTimer.StartTimer();
        dataController.Reset();
        
        if (!isFirst)
        {
            dataController.Begin();
        }

        if (!isEngaging)
        {
            player.enabled = true;
        }
    }

    private int maxTries = 50;
    IEnumerator CheckTotalDistanceCoroutine()
    {
        int attempts = 0;
        float distance = 0;

        while (attempts < maxTries)
        {
            distance = finishAi.path.remainingDistance;
            // Check the condition
            if (distance > 30f && !float.IsInfinity(distance))
            {
                currentTotalDistance = finishAi.path.remainingDistance;
                break;
            }

            // If not met, wait for the interval and try again
            Debug.Log($"Attempt {attempts + 1}: Condition not met yet.");
            attempts++;
            yield return new WaitForSecondsRealtime(0.05f);
        }

        if (attempts >= maxTries)
        {
            Debug.Log("Condition not met after max attempts.");
        }
        // Continue with the next part of your logic
        Debug.Log("Total distance: " + distance);
    }

    public string GetCurrentGameState()
    {
        switch (gameState)
        {
            case GameState.Introduction:
                return "Introduction";
            case GameState.Practice:
                return "Practice";
            case GameState.Training:
                return "Training";
            case GameState.DevalueIntroduction:
                return "DevalueIntroduction";
            case GameState.Test:
                return "Test";
            case GameState.Debrief:
                return "Debrief";
            default:
                return "UNKOWN STATE";

        }
    }

    public void StartEngagingGame()
    {
        player.enabled = true;
        countdownTimer.gameObject.SetActive(true);
        dataController.Begin();
    }

    public void GenerateSeed()
    {
        if (gameState == GameState.Practice)
        {
            currentSeed = practiceLevels[round - 1];
            UnityEngine.Random.InitState(currentSeed);
        }

        if (gameState == GameState.Training)
        {
            currentSeed = trainingLevels[round - 1];
            UnityEngine.Random.InitState(currentSeed);
        }

        if (gameState == GameState.Test)
        {
            currentSeed = testLevels[round - 1];
            UnityEngine.Random.InitState(currentSeed);
        }
    }

    public string GetCurrentTotalRounds()
    {
        switch (gameState)
        {
            case GameState.Practice:
                return practiceRounds.ToString();
            case GameState.Training:
                return trainingRounds.ToString();
            case GameState.Test:
                return testRounds.ToString();
            default:
                return "";

        }
    }
}
