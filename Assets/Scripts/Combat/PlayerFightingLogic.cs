using G4AW2.Combat;
using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Data.DropSystem;
using UnityEngine;
using UnityEngine.Events;

public class PlayerFightingLogic : MonoBehaviour {

	public Player Player;
	public EnemyDisplay EnemyDisplay;
	public FloatReference MinSwipeDistance;
    public float FailedParryStunTime = 2f;

    public UnityEvent OnBlockStart;
    public UnityEvent OnPerfectBlock;
    public UnityEvent OnBlockEnd;
    public UnityEvent OnFailedParry;
    public UnityEvent OnFailedParryDone;
    public UnityEvent OnSuccessfulParry;
    public UnityEvent Attacked;

    public DamageNumberSpawner PlayerDamageNumberSpawner;
    public Color DamageColor;

    private bool AbleToAttack => BlockState == Armor.BlockState.None;
    private Armor.BlockState BlockState = Armor.BlockState.None;

	public void OnEnemyHitPlayer(int damage) {

        float fdamage = Player.Armor.Value.GetDamage(damage, BlockState);

        if(BlockState == Armor.BlockState.Blocking || BlockState == Armor.BlockState.PerfectlyBlocking)
        {
            if(BlockState == Armor.BlockState.PerfectlyBlocking)
                OnPerfectBlock.Invoke();

            blockTimer.Stop();
            OnBlockEnd.Invoke();
            BlockState = Armor.BlockState.None;
        }

        damage = Mathf.RoundToInt(fdamage);
        Player.DamagePlayer(damage);
	    PlayerDamageNumberSpawner.SpawnNumber(damage, DamageColor);
    }

    public void OnEnemyHitPlayerElemental(int damage, ElementalType damageType) {

        float mod = 1;
        if (Player.Armor.Value.ElementalWeakness.Value != null)
            mod = Player.Armor.Value.ElementalWeakness.Value[damageType];
        float fdamage = damage * mod;

        damage = Mathf.RoundToInt(fdamage);
        Player.DamagePlayer(damage);
        PlayerDamageNumberSpawner.SpawnNumber(damage, damageType.DamageColor);
    }

    public float StunnedDamageMultiplier = 1.15f;

	public void PlayerAttemptToHitEnemy() {
        if(AbleToAttack)
        {
            Attacked.Invoke();

            float damageMultipler = EnemyDisplay.EnemyState == EnemyDisplay.State.Stun ? StunnedDamageMultiplier : 1;

            EnemyDisplay.ApplyDamage(Mathf.RoundToInt(Player.GetLightDamage() * damageMultipler));
            if(Player.Weapon.Value.IsEnchanted)
                EnemyDisplay.ApplyElementalDamage(Player.GetElementalDamage(), Player.Weapon.Value.Enchantment.Type);
            Player.Weapon.Value.TapsWithWeapon.Value++;
        }
    }

	public void PlayerScreenSwipe( Vector3[] swipe ) {
        if (!AbleToAttack) return;
        if (swipe.Length < 2) return;

		Vector3 start = swipe[0];
		Vector3 end = swipe[swipe.Length - 1];
		if ((start - end).magnitude > MinSwipeDistance) {
			if(start.x > end.x) {
				PlayerParried();
			} else {
				PlayerBlocked();
			}
		}
	}

    void Update() {
        blockTimer.Update(Time.deltaTime);
    }

    public float MaxBlockDuration = 4f;

    private UpdateTimer blockTimer = new UpdateTimer();

	public void PlayerBlocked() {
        BlockState = Armor.BlockState.Blocking;

	    blockTimer.Start(MaxBlockDuration, () => {
            BlockState = Armor.BlockState.None;
	        OnBlockEnd.Invoke();
	    }, null);


        if (EnemyDisplay.EnemyState == EnemyDisplay.State.BeforeAttack) {
            BlockState = Armor.BlockState.PerfectlyBlocking;
        }
        OnBlockStart.Invoke();
	}

	public void PlayerParried() {
		bool success = EnemyDisplay.EnemyState == EnemyDisplay.State.ExecuteAttack;

        if (!success)
        {
            BlockState = Armor.BlockState.BadParry;
            OnFailedParry.Invoke();
            Timer.StartTimer(FailedParryStunTime, () =>
            {
                OnFailedParryDone.Invoke();
                BlockState = Armor.BlockState.None;
            }, this);
        } else
        {
            EnemyDisplay.Stun();
            OnSuccessfulParry.Invoke();
        }
    }
}
