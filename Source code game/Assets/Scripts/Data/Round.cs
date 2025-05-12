[System.Serializable]
public class Round
{
    public int id;
    public string participantEmail;
    public int logId;
    public int seed;
    public int round;
    public bool didCoinSpawn;
    public bool pickedUpCoin;
    public bool finished;
    public string phase;
    public string date;
    public decimal totalDistance;
    public decimal distanceCoinSpawn;
    public decimal remainingTime;
    public int totalRoundsFinished;
    public int day;
    public decimal coinPickupTime;
    public decimal coinSpawnTime;
}