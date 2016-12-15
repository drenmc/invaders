using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum GameStates
{
	START,
	PLAYING,
	GAME_OVER,
	GETTING_SCORE
};

[System.Serializable]
public class TempoBeatChangeInfo
{
	public double _tempoIncrease = 10;
	public StepSequencer[] _seqsToEnable;
	public StepSequencer[] _seqsToDisable;
}

public class SceneGamePlayManager : MonoBehaviour 
{
	public static SceneGamePlayManager _instance;

    // |
    // | ITEMS HOOKED UP IN THE UNITY EDITOR
    // |

    // References to the Game's various views
//    public GameObject StartView;
//    public GameObject PlayView;
    public GameObject GameOverView;
    public GameObject ExplosionParticles;
    public GameObject PlayerExplosionParticles;
    public AudioClip ExplosionSound;

	[SerializeField] private Metronome _metronome;
	[SerializeField] private int _ticksPerMove = 4;
	private int _ticksWrapped;
	[SerializeField] private double _baseTempo = 90.0;
	[SerializeField] private int _downMovesPerTempoIncrease = 3;
	[SerializeField] private TempoBeatChangeInfo[] _tempoBeatChangeInfo;
	private int _currentTempoBeatChangeIdx = -1;
	private int _downMovesDone = 0;


    // The container for our invaders and a container for their missiles.
    // Seperate containers because we do some tweaking to the objects in one that we dont want to
    // do to the other.
    public Transform EnemyContainer;
    public Transform EnemyMissileContainer;

//    public Transform HighScoresContainer; // Container for Highscores
//    public GameObject HighScoresRenderer; // Rendering object of high scores

    // Player Object Controller Reference
    public PlayerController Player;
    public Transform[] PlayerShields;


	public GameObject[] EnemyPrefabs; // An array of enemies to create; must be set up in the editor

    // Missile for Enemy Invaders to fire
    public GameObject EnemyMissilePrefab;

    // UI Elements to update: Score Text and Wave Text
    public Text ScoreText;
    public Text WaveText;

    // |
    // | END OF ITEMS HOOKED UP IN THE UNITY EDITOR
    // |


    // Manage game states which basically set various modes, show screens, etc
    public GameStates gameState = GameStates.START;

    
    public int currentScore = 0;
    public int currentWave = 1;

    // Maintain a list of the invaders. 
	private List<EnemyController> enemiesList;

    // Invader missile drop controls
    private float lastFireTime  = 0.0f;             // when was the last missile fired?    
    private float fireDelay     = 1.0f;             // Fire delay, we dont want a bunch of missiles flying out
    private int maxMissiles     = 10;               // Maximum allowable fired missiles
    
    void Awake()
	{
		if(_instance == null)
		{
			_instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(this.gameObject);
		}
	}

	// Use this for initialization
	void Start () 
    {
		_metronome.Ticked += OnMetronomeTick;
        
		enemiesList = new List<EnemyController>();

		gameState = GameStates.START;

		StartGame();
//		Debug.Log("START CALLED! " +  this.currentScore);
	}

	void OnMetronomeTick(double tickTime)
	{
		if (_ticksWrapped == 0)
		{
			moveEnemies();
		}
		_ticksWrapped = (_ticksWrapped + 1) % _ticksPerMove;
	}

    public bool isPlaying()
    {
        return gameState == GameStates.PLAYING;
    }

    // Start the game; update the game start to PLAYING,
    public void StartGame()
    {
		_metronome.SetTempo(_baseTempo);

        Player.gameLogicManager = this;

        // Set all Shields to active:
        int shieldCnt = PlayerShields.Length;
        for (int cnt = 0; cnt < shieldCnt; cnt ++ )
        {
            Transform playerShield = PlayerShields[cnt];
            for (int childNdx = 0; childNdx < playerShield.childCount; childNdx++)
                playerShield.GetChild(childNdx).gameObject.SetActive(true);

        }

        Player.restart();

        gameState = GameStates.PLAYING;
        currentScore = 0;
        currentWave = 1;

        updateScore();
        updateWave();
        drawEnemies();

        this.Player.autoMode = false;

    }

    private void updateScore(int amount=0)
    {
        currentScore += amount;
        ScoreText.text = "SCORE\n" + currentScore.ToString();
    }
    private void updateWave(int amount = 0)
    {
        currentWave += amount;
        WaveText.text = "WAVE\n" + currentWave.ToString();
    }

    // Draw a grid of 11x5 containing our enemy invaders
    private void drawEnemies()
    {

		enemiesList.Clear();

        for (int cnt = 0; cnt < EnemyContainer.childCount; cnt++)	Destroy(EnemyContainer.GetChild(cnt).gameObject);

        int enemyIndex = 0;
        for(int ycnt = 0; ycnt < 5; ycnt ++)
        {

			if (ycnt == 2) enemyIndex = 1;
			if (ycnt == 4) enemyIndex = 2;
            for(int xcnt = 0; xcnt < 11; xcnt ++)
            {
                Vector3 pos = new Vector3(0.75f * (float)xcnt , 0.75f * (float)ycnt, 0);

				GameObject newEnemy = Instantiate(EnemyPrefabs[enemyIndex]) as GameObject;
				newEnemy.transform.SetParent(EnemyContainer);
                newEnemy.transform.localPosition = pos;

				EnemyController enemyController = newEnemy.GetComponent<EnemyController>();
				enemyController.gameLogicManager = this;

				enemiesList.Add(enemyController);
                
				newEnemy.transform.localScale = new Vector3(0.7f, 0.7f, 1.0f);
            }

        }
        // Setup Enemy Invader Speeds..
        float cwave = (float)currentWave;
		EnemyController.MoveDirection = 1.0f;
		EnemyController.MoveVel = 0.1f + (0.01f * (cwave - 1)); // Start out each moving just a bit more

    }

	public void EnemyHit(EnemyController enemy)
    {
		GameObject go = Instantiate(ExplosionParticles) as GameObject;
		go.transform.position = enemy.transform.position;

		updateScore( enemy.ScoreValue );

		enemiesList.Remove(enemy);
		Destroy(enemy.gameObject);

        // Slightly increase move distance
		EnemyController.MoveVel += 0.005f;

		// If there are no enemy invaders left, start the next wave
		int enemyCount = enemiesList.Count;
        if (enemyCount <= 0)
        {
            updateWave(1);
            drawEnemies();
        }

    }

    public void DoGameOver()
    {
        GameObject go = Instantiate(PlayerExplosionParticles) as GameObject;
        go.transform.position = Player.transform.position;

		gameState = GameStates.GAME_OVER;
        Player.gameObject.SetActive(false);

		var audioSource = go.GetComponent<AudioSource>();
		StartCoroutine(WaitForGameOverSound(audioSource));
    }

	public IEnumerator WaitForGameOverSound(AudioSource audioSource)
	{
		Debug.Log(audioSource.name);
		while (audioSource != null && audioSource.isPlaying)
		{
			Debug.Log("wait");
			yield return null;
		}
		SceneManager.LoadScene("GameOver");
	}


    // March the invaders across the screen. if one hits an extent, move them all down and switch the movement direction
    // Update them here since they are sort of herky-jerky and all must move along X before a Y change takes place
    private void moveEnemies()
    {
		int maxCount = enemiesList.Count;
        bool moveDown = false;
        bool hitBottom = false;
        bool isPlaying = (gameState == GameStates.PLAYING);

        for (int cnt = 0; cnt < maxCount; cnt++)
        {
			EnemyController enemyController = enemiesList[cnt];
			float xloc = enemyController.UpdateSprite(isPlaying);

            if (isPlaying)
            {
                float nextFireTime = lastFireTime + this.fireDelay;
                float currTime = Time.time;

                // Randomly shoot a missile if currtime has passed the limit
                if (Random.Range(0, 100) < this.maxMissiles && currTime > nextFireTime )
                {
					enemyFireMissile(enemyController);
                    lastFireTime = currTime;
                }

				// X boundaries.
                if ((xloc <= -5.5f || xloc >= 5.5f) )
                    moveDown = true;

            }

        }

        if(moveDown)
        {
			EnemyController.MoveDirection = -EnemyController.MoveDirection;
            for (int cnt = 0; cnt < maxCount; cnt++)
            {
				EnemyController enemyController = enemiesList[cnt];
                float yloc = enemyController.dropDownRow();
                if (yloc <= -4.25f) hitBottom = true;
            }

			// increase the tempo
			int modMoves = (++_downMovesDone) % _downMovesPerTempoIncrease;

			if (modMoves == 0)
			{
				if (++_currentTempoBeatChangeIdx < _tempoBeatChangeInfo.Length)
				{
					var tbc = _tempoBeatChangeInfo[_currentTempoBeatChangeIdx];
					var tempo = _metronome.GetTempo() + tbc._tempoIncrease;
					_metronome.SetTempo(tempo);
					Debug.Log("Set tempo to " + tempo);

					foreach (var seq in tbc._seqsToDisable)
					{
						seq.Suspend = true;
					}
					foreach (var seq in tbc._seqsToEnable)
					{
						seq.Suspend = false;
					}
				}
			}
        }

        if (hitBottom)  DoGameOver();
    }


	private void enemyFireMissile(EnemyController enemy)
    {
		// Fire a missile from the invader...
        int max = 10;
        int missileCount = EnemyMissileContainer.childCount;
        GameObject missile = null;

        

        if(missile == null && missileCount < max)
        {
			missile = Instantiate(EnemyMissilePrefab) as GameObject;
            missile.transform.parent = EnemyMissileContainer;
           
        }

        if (missile != null)
        {
            MissileController mc = missile.GetComponent<MissileController>();
			mc.enemyFired(enemy.gameObject);
        }
    }

}
