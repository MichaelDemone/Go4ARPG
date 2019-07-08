using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Combat;
using G4AW2.Data.Combat;
using G4AW2.Questing;
using UnityEngine;
using UnityEngine.Events;

public class BossQuestController : MonoBehaviour {

    public ActiveQuestBaseVariable CurrentQuest;

    public ScrollingImages BackgroundImages;
    public PlayerAnimations PlayerAnimations;

    public EnemyDisplay Enemy;
    public DragObject BackgroundDraggable;

    public UnityEvent OnWaitingStart;
    public GameEvent FightStarted;

    public int ScrollSpeedToBoss = 12;
    public int OriginalScrollSpeed = 5;

    public LerpToPosition WorldCameraLerper;
    public DeadEnemyController DeadEnemyController;

    public GameObject EnemyArrow;

    private RectTransformPositionSetter playerPositionSetter;
    private RectTransformPositionSetter enemyPositionSetter;

    void Awake() {
        playerPositionSetter = PlayerAnimations.GetComponent<RectTransformPositionSetter>();
        enemyPositionSetter = Enemy.GetComponent<RectTransformPositionSetter>();
    }

    public void StartBossQuest() {
        if(!(CurrentQuest.Value is BossQuest))
            return;

        BossQuest quest = (BossQuest) CurrentQuest.Value;

        // Set enemy display to boss
        var enemy = Instantiate(quest.Enemy);
        enemy.Level = quest.Level;
        Enemy.SetEnemy(enemy);

        Enemy.OnDeath.RemoveListener(OnEnemyDeath);
        Enemy.OnDeath.AddListener(OnEnemyDeath);

        // Set scale of enemy display so boss is facing you
        var scale = Enemy.transform.localScale;
        scale.x = -1 * Mathf.Abs(scale.x);
        Enemy.transform.localScale = scale;

        // Disallow scrolling
        BackgroundDraggable.Disable();

        // Turn off enemy arrow (red one that indicates there's enemies)
        EnemyArrow.SetActive(false);

        // Scroll to player
        WorldCameraLerper.StartLerping(() => {

            // 1. Start screen scroll if not started
            BackgroundImages.Play();
            BackgroundImages.ScrollSpeed = ScrollSpeedToBoss;

            // 2. Stop player from walking & drag player backwards
            PlayerAnimations.StopWalking();

            scrolling = true;

            float widthOfEnemy = ((RectTransform) Enemy.transform).sizeDelta.x;
            enemyPositionSetter.SetX(54 + widthOfEnemy / 2);
            Enemy.gameObject.SetActive(true);

        });

        
    }

    public float MinXForBoss = 33;
    private bool scrolling = false;
    private bool playerWalking = false;

    public float WalkSpeedOfPlayer = 15;
    public float PlayerFightPosition = 12;

    public RectTransform WorldTransform;

    void Update() {
        if (scrolling) {
            Vector2 pos = WorldTransform.anchoredPosition;
            pos.x -= ScrollSpeedToBoss * Time.deltaTime;

            if (enemyPositionSetter.GetX() < MinXForBoss) {
                scrolling = false;
                BackgroundImages.Pause();
                OnWaitingStart.Invoke();
                BackgroundImages.ScrollSpeed = OriginalScrollSpeed;
                DeadEnemyController.ScrollSpeed = OriginalScrollSpeed;
            }
        }

        if (playerWalking) {
            playerPositionSetter.SetX(playerPositionSetter.GetX() + Time.deltaTime * WalkSpeedOfPlayer);
            if(playerPositionSetter.GetX() >= PlayerFightPosition) {
                playerWalking = false;
                PlayerAnimations.StopWalking();
                PlayerAnimations.Spin(() => { });
            }
        }
    }


    public void FightBoss() {
        PlayerAnimations.StartWalking();
        Enemy.StartWalking(() => {
            FightStarted.Raise();
            enemyPositionSetter.SetScaleX(1);
        });

        playerWalking = true;
    }

    public void OnDeath() {
        if(CurrentQuest.Value is BossQuest) 
            ResumeBossQuest();
    }

    [ContextMenu("Resume")]
    public void ResumeBossQuest() {
        if(!(CurrentQuest.Value is BossQuest))
            return;

        BossQuest quest = (BossQuest) CurrentQuest.Value;

        // Set enemy display to boss
        var enemy = Instantiate(quest.Enemy);
        enemy.Level = quest.Level;
        Enemy.SetEnemy(enemy);

        Enemy.OnDeath.RemoveListener(OnEnemyDeath);
        Enemy.OnDeath.AddListener(OnEnemyDeath);

        // Set scale of enemy display so boss is facing you
        var scale = Enemy.transform.localScale;
        scale.x = -1 * Mathf.Abs(scale.x);
        Enemy.transform.localScale = scale;

        PlayerAnimations.StopWalking();

        Enemy.gameObject.SetActive(true);

        // Set player into boss position
        enemyPositionSetter.SetX(MinXForBoss);
        playerPositionSetter.SetX(-20);

        // Turn off enemy arrow (red one that indicates there's enemies)
        EnemyArrow.SetActive(false);

        BackgroundImages.Pause();
        OnWaitingStart.Invoke();
    }

    void OnEnemyDeath(EnemyData death) {
        FinishBossQuest();
        Enemy.OnDeath.RemoveListener(OnEnemyDeath);
    }

    private void FinishBossQuest() {
        BossQuest quest = (BossQuest) CurrentQuest.Value;
        quest.Finish();
        EnemyArrow.SetActive(true);
    }
}
