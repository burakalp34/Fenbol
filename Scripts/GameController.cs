using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] int P1CountryIndex, P2CountryIndex;
    [SerializeField] GameObject[] P1Flags, P2Flags;
    [SerializeField] string[] CountryNames;
    [SerializeField] GameObject Ball, PlayerRed, PlayerBlue, ScoreboardGO, TimerGO, PointsGO, ScorerGO, GameCreator;
    [SerializeField] Camera mainCam;
    [SerializeField] Vector2[] redGoals, blueGoals;
    [SerializeField] Vector3 BallStartPosRed, BallStartPosBlue, RedStartPos, BlueStartPos, CameraStartPos;
    [SerializeField] int redScore, blueScore, TimeLimit, ScoreLimit;
    [SerializeField] UnityEngine.UI.Text Scoreboard, TimerText, Scorer, BounceIndicator, ScoreIndicator, TimeIndicator, RadiusIndicator, SpeedIndicator, P1Country, P2Country;
    [SerializeField] float Timer, TimerMinutes, TimerSeconds, TimerMultiplier, CameraSize = 20f;
    [SerializeField] PhysicsMaterial2D pm2d;
    [SerializeField] UnityEngine.UI.Slider SpeedSlider, BounceSlider, ScoreSlider, TimeSlider, RadiusSlider;
    [SerializeField] UnityEngine.UI.Toggle ScoreToggle, TimeToggle, KickToggle;
    [SerializeField] bool LimitType, ZoomingIn = false, ZoomingOut = false; //False: Timer | True: Score
    [SerializeField] bool Paused, Celebration = false;
    [SerializeField] string SpeedKey, BounceKey, PushKey, EndKey, ScoreKey, MinuteKey, KickKey;
    public AudioSource GoalAudio, MissedShotAudio, WhistleAudio;
    public bool Kickable, GameOver = false;
    public int PushRadius;
    public float GameSpeed;
    // Start is called before the first frame update
    void Start()
    {
        P1CountryIndex = PlayerPrefs.GetInt("Country1", 0);
        P2CountryIndex = PlayerPrefs.GetInt("Country2", 0);
        P1Country.text = CountryNames[P1CountryIndex];
        P2Country.text = CountryNames[P2CountryIndex];
        P1Flags[P1CountryIndex].SetActive(true);
        P2Flags[P2CountryIndex].SetActive(true);
        Ball = GameObject.FindGameObjectWithTag("Ball");
        PlayerRed = GameObject.FindGameObjectWithTag("Red");
        PlayerBlue = GameObject.FindGameObjectWithTag("Blue");
        RedStartPos = PlayerRed.transform.position;
        BlueStartPos = PlayerBlue.transform.position;
        ScorerGO.SetActive(false);
        Time.timeScale = 0;

        SpeedSlider.value = PlayerPrefs.GetFloat(SpeedKey, 2);
        BounceSlider.value = PlayerPrefs.GetFloat(BounceKey, 3);
        int i = PlayerPrefs.GetInt(PushKey, 1);
        if (i == 1) {
            KickToggle.isOn = true;
        }
        if (i == 0)
        {
            KickToggle.isOn = false;
        }
        i = PlayerPrefs.GetInt(EndKey, 0);
        if (i == 1)
        {
            ScoreToggle.isOn = false;
        }
        if (i == 0)
        {
            ScoreToggle.isOn = true;
        }
        ScoreSlider.value = PlayerPrefs.GetInt(ScoreKey, 3);
        TimeSlider.value = PlayerPrefs.GetInt(MinuteKey, 1);
        RadiusSlider.value = PlayerPrefs.GetInt(KickKey, 1);
    }

    // Update is called once per frame
    void Update()
    {
        TimerMinutes = (int)Timer / 60;
        TimerSeconds = (int)Timer - (TimerMinutes * 60);
        BounceIndicator.text = BounceSlider.value.ToString();
        ScoreIndicator.text = ScoreSlider.value.ToString();
        TimeIndicator.text = TimeSlider.value.ToString();
        RadiusIndicator.text = RadiusSlider.value.ToString();
        SpeedIndicator.text = (SpeedSlider.value / 2).ToString() + "x";
        mainCam.orthographicSize = CameraSize;
        if (TimerSeconds >= 10) {
            TimerText.text = " " + TimerMinutes + ":" + TimerSeconds + " ";
        }
        if (TimerSeconds < 10)
        {
            TimerText.text = " " + TimerMinutes + ":0" + TimerSeconds + " ";
        }
        Scoreboard.text = redScore + " - " + blueScore;
        if (Ball.transform.position.x > redGoals[0].x && Ball.transform.position.x < redGoals[0].y && Ball.transform.position.y > redGoals[1].x && Ball.transform.position.y < redGoals[1].y && !Celebration && !GameOver) {
            StartCoroutine(BlueScoreWaiter());
            ScorerGO.SetActive(true);
            GoalAudio.Play();
            Celebration = true;
        }
        if (Ball.transform.position.x > blueGoals[0].x && Ball.transform.position.x < blueGoals[0].y && Ball.transform.position.y > blueGoals[1].x && Ball.transform.position.y < blueGoals[1].y && !Celebration && !GameOver)
        {
            StartCoroutine(RedScoreWaiter());
            ScorerGO.SetActive(true);
            GoalAudio.Play();
            Celebration = true;
        }
        if (LimitType && redScore >= ScoreLimit) {
            ScorerGO.SetActive(true);
            GameOver = true;
            Scorer.text = "Red Wins!";
            Scorer.color = Color.red;
        }
        if (LimitType && blueScore >= ScoreLimit)
        {
            ScorerGO.SetActive(true);
            GameOver = true;
            Scorer.text = "Blue Wins!";
            Scorer.color = Color.blue;
        }
        if (!LimitType && Timer <= 0f && redScore >= blueScore)
        {
            ScorerGO.SetActive(true);
            GameOver = true;
            Scorer.text = "Red Wins!";
            Scorer.color = Color.red;
        }
        if (!LimitType && Timer <= 0f && blueScore >= redScore)
        {
            ScorerGO.SetActive(true);
            GameOver = true;
            Scorer.text = "Blue Wins!";
            Scorer.color = Color.blue;
        }
        if (!LimitType && Timer <= 0f && blueScore == redScore) {
            ScorerGO.SetActive(true);
            GameOver = true;
            Scorer.text = "Draw";
            Scorer.color = Color.black;
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !Paused) {
            Time.timeScale = 0;
            Paused = true;
            ScorerGO.SetActive(true);
            Scorer.text = "Game Paused";
            Scorer.color = Color.black;
            //ZoomingOut = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && Paused)
        {
            Time.timeScale = 1;
            Paused = false;
            ScorerGO.SetActive(false);
            //ZoomingIn = true;
        }
    }
    private void FixedUpdate()
    {
        if (!Celebration && !GameOver) {
            Timer += Time.fixedDeltaTime * TimerMultiplier;
        }
        if (ZoomingIn) {
            CameraSize -= Time.fixedDeltaTime * 6f;
        }
        if (ZoomingOut) {
            CameraSize += Time.fixedDeltaTime * 6f;
        }
        if (CameraSize <= 12f) {
            ZoomingIn = false;
        }
        if (CameraSize >= 20f)
        {
            ZoomingOut = false;
        }
    }
    IEnumerator BlueScoreWaiter() {
        blueScore++;
        Scorer.text = CountryNames[P2CountryIndex] + " Scores!";
        Scorer.color = Color.blue;
        Scoreboard.text = redScore + " - " + blueScore;
        yield return new WaitForSeconds(5);
        if (!GameOver) {
            RestartGame("Blue");
        }
    }
    IEnumerator RedScoreWaiter()
    {
        redScore++;
        Scorer.text = CountryNames[P1CountryIndex] + " Scores!";
        Scorer.color = Color.red;
        Scoreboard.text = redScore + " - " + blueScore;
        yield return new WaitForSeconds(5);
        if (!GameOver)
        {
            RestartGame("Red");
        }
    }
    public void StartButton() {
        //mainCam.transform.position = CameraStartPos;
        ScoreboardGO.SetActive(true);
        ScorerGO.SetActive(false);
        Time.timeScale = 1;
        pm2d.bounciness = BounceSlider.value / 4;
        Ball.GetComponent<CircleCollider2D>().sharedMaterial = pm2d;
        TimeLimit = (int)TimeSlider.value * 60;
        ScoreLimit = (int)ScoreSlider.value;
        PushRadius = (int)RadiusSlider.value;
        GameSpeed = SpeedSlider.value / 2;
        LimitType = ScoreToggle.isOn;
        Kickable = KickToggle.isOn;
        PlayerPrefs.SetFloat(SpeedKey, SpeedSlider.value);
        PlayerPrefs.SetFloat(BounceKey, BounceSlider.value);
        if (KickToggle.isOn) {
            PlayerPrefs.SetInt(PushKey, 1);
        }
        if (!KickToggle.isOn)
        {
            PlayerPrefs.SetInt(PushKey, 0);
        }
        if (ScoreToggle.isOn)
        {
            PlayerPrefs.SetInt(EndKey, 0);
            Timer = 0f;
            TimerMultiplier = 1f;
        }
        if (!ScoreToggle.isOn)
        {
            PlayerPrefs.SetInt(EndKey, 1);
            Timer = TimeLimit;
            TimerMultiplier = -1f;
        }
        PlayerPrefs.SetInt(ScoreKey, (int)ScoreSlider.value);
        PlayerPrefs.SetInt(MinuteKey, (int)TimeSlider.value);
        PlayerPrefs.SetInt(KickKey, (int)RadiusSlider.value);
        GameCreator.SetActive(false);
        ZoomingIn = true;
        WhistleAudio.Play();
    }
    public void ScoreToggleChange() {
        if (ScoreToggle.isOn) {
            TimeToggle.isOn = false;
        }
        if (!ScoreToggle.isOn)
        {
            TimeToggle.isOn = true;
        }
    }
    public void TimeToggleChange()
    {
        if (TimeToggle.isOn)
        {
            ScoreToggle.isOn = false;
        }
        if (!TimeToggle.isOn)
        {
            ScoreToggle.isOn = true;
        }
    }
    public void MainMenu() {
        SceneManager.LoadScene("MainMenu");
    }
    public void Restart() {
        SceneManager.LoadScene("PvP");
    }
    public void ResetSettings() {
        SpeedSlider.value = 2;
        BounceSlider.value = 3;
        KickToggle.isOn = true;
        ScoreToggle.isOn = true;
        ScoreSlider.value = 3;
        TimeSlider.value = 1;
        RadiusSlider.value = 1;
        PlayerPrefs.SetFloat(SpeedKey, SpeedSlider.value);
        PlayerPrefs.SetFloat(BounceKey, BounceSlider.value);
        if (KickToggle.isOn)
        {
            PlayerPrefs.SetInt(PushKey, 1);
        }
        if (!KickToggle.isOn)
        {
            PlayerPrefs.SetInt(PushKey, 0);
        }
        if (ScoreToggle.isOn)
        {
            PlayerPrefs.SetInt(EndKey, 0);
        }
        if (!ScoreToggle.isOn)
        {
            PlayerPrefs.SetInt(EndKey, 1);
        }
        PlayerPrefs.SetInt(ScoreKey, (int)ScoreSlider.value);
        PlayerPrefs.SetInt(MinuteKey, (int)TimeSlider.value);
        PlayerPrefs.SetInt(KickKey, (int)RadiusSlider.value);
    }
    public void RestartGame(string Scorer) {
        Rigidbody2D rb2d;
        rb2d = Ball.GetComponent<Rigidbody2D>();
        rb2d.velocity = Vector2.zero;
        rb2d.angularVelocity = 0f;
        rb2d.constraints = RigidbodyConstraints2D.None;
        if (Scorer == "Red") {
            Ball.transform.position = BallStartPosBlue;
        }
        if (Scorer == "Blue")
        {
            Ball.transform.position = BallStartPosRed;
        }

        PlayerRed.transform.position = RedStartPos;
        rb2d = PlayerRed.GetComponent<Rigidbody2D>();
        rb2d.velocity = Vector2.zero;
        rb2d.angularVelocity = 0f;

        PlayerBlue.transform.position = BlueStartPos;
        rb2d = PlayerBlue.GetComponent<Rigidbody2D>();
        rb2d.velocity = Vector2.zero;
        rb2d.angularVelocity = 0f;

        ScorerGO.SetActive(false);
        Celebration = false;
        WhistleAudio.Play();
    }
}