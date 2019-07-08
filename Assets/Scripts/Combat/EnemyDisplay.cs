using G4AW2.Data.Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using G4AW2.Data.DropSystem;
using G4AW2.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace G4AW2.Combat {
	[RequireComponent(typeof(Animator))]
    public class EnemyDisplay : MonoBehaviour {

		public enum State {
			Idle, BeforeAttack, ExecuteAttack, AfterAttack, Stun, Disabled, Dead
		}

        [Header("Misc References")]
	    public PlayerAnimations PlayerAnimations;
	    public ItemDropBubbleManager ItemBubbleManager;
	    public PlayerFightingLogic FightingLogic;
	    public DamageNumberSpawner RegularDamageNumberSpawner;
	    public DamageNumberSpawner ElementalDamageNumberSpawner;
	    public Player Player;
	    public RuntimeSetFollowerData CurrentFollowers;
	    public LerpToPosition WalkingToPosition;
	    public DeadEnemyController DeadEnemyController;

        [Header("Settings")]
        public FloatReference StunDuration;
	    public Color BaseDamageColor;
	    public BoolReference ShowParryAndBlockColors;
	    public Color ParryColor;
	    public Color BlockColor;

        [Header("Data")]
		public State EnemyState;
		public EnemyData Enemy;

        [Header("Readable Data")]
		public IntReference MaxHealth;
	    public IntReference CurrentHealth;

        // Events
        [Header("Events")]
        public UnityEvent OnStartWalking;
        public UnityEvent OnWalkingDone;
        public UnityEventInt OnHit;
		public UnityEventInt OnElementalHit;
        public UnityEventEnemyData OnDeath;
	    public UnityEventEnemyData OnKilled;
        public UnityEventIEnumerableLoot OnDropLoot;
	    public UnityEvent CleanUp;

        private Animator MyAnimator;

	    private Image im;
	    private Image Image {
	        get {
	            if(im == null)
	                im = GetComponent<Image>();
	            return im;
	        }
	    }

	    private RectTransform rt;
	    private RectTransform RectTransform {
	        get {
	            if (rt == null) rt = GetComponent<RectTransform>();
	            return rt;
	        }
	    }

	    void Awake() {
            MyAnimator = GetComponent<Animator>();
	        rt = GetComponent<RectTransform>();
			EnemyState = State.Disabled;
        }

		public void SetEnemy( EnemyData data) {
			Enemy = data;
            
			MaxHealth.Value = data.MaxHealth;
			CurrentHealth.Value = MaxHealth;

			AnimatorOverrideController aoc = (AnimatorOverrideController)GetComponent<Animator>().runtimeAnimatorController;
			aoc["Death"] = Enemy.Death;
			aoc["Dead"] = Enemy.Dead;
			aoc["Flinch"] = Enemy.Flinch;
			aoc["BeforeAttack"] = Enemy.BeforeAttack;
			aoc["AttackExecute"] = Enemy.AttackExecute;
			aoc["AfterAttack"] = Enemy.AfterAttack;
			aoc["Idle"] = Enemy.Idle;
            aoc["Walking"] = Enemy.Walking;


            // TODO: Find a better way to do this
            Vector3 pos = transform.localPosition;
            pos.x = -70;
            transform.localPosition = pos;

		    Vector2 r = RectTransform.sizeDelta;
		    r.x = data.SizeOfSprite.x;
		    r.y = data.SizeOfSprite.y;
		    RectTransform.sizeDelta = r;

		    Image.color = Color.white;
		}

	    public void StartWalkingAnimation() {
	        StopAllCoroutines();
            MyAnimator.SetTrigger("Walking");
        }

	    public void StartWalking(Action donewalking) {
	        StopAllCoroutines();
	        OnStartWalking.Invoke();
	        MyAnimator.SetTrigger("Walking");
	        WalkingToPosition.StartLerping(() => {
	            StartAttacking();
	            MyAnimator.SetTrigger("DoneWalking");
	            OnWalkingDone.Invoke();
	            donewalking?.Invoke();
	        });
        }

        public void StartWalking() {
            StartWalking(null);
        }

		public void StartAttacking() {
			StopAllCoroutines();
			StartCoroutine(DoAttack(true));
		}

		public void Stun() {
			StopAllCoroutines();
		    MyAnimator.SetTrigger("Stun");
		    Image.color = Color.white;
			EnemyState = State.Stun;
		    Timer.StartTimer(StunDuration, UnStun, this);
        }

        public void UnStun() {
			StartCoroutine(DoAttack());
            MyAnimator.SetTrigger("StunOver");
        }

        #region Attack

		public IEnumerator DoAttack(bool first = false) {
			for (; ; ) {
				EnemyState = State.Idle;
                yield return new WaitForSeconds(first ? Enemy.TimeBetweenAttacks / 4 : Enemy.TimeBetweenAttacks);
                first = false;
			    if (EnemyState == State.Dead) break;

				EnemyState = State.BeforeAttack;
			    MyAnimator.SetTrigger("AttackStart");
			    if(ShowParryAndBlockColors) Image.color = BlockColor;

				// Wind up
				yield return new WaitForSeconds(Enemy.AttackPrepTime);
			    if(EnemyState == State.Dead)
			        break;


                EnemyState = State.ExecuteAttack;
			    MyAnimator.SetTrigger("AttackExecute");
			    if(ShowParryAndBlockColors)
			        Image.color = ParryColor;

                // Perform the attack
                yield return new WaitForSeconds(Enemy.AttackExecuteTime);
			    if(ShowParryAndBlockColors)
			        Image.color = Color.white;
			    if(EnemyState == State.Dead)
			        break;

                EnemyState = State.AfterAttack;
			    FightingLogic.OnEnemyHitPlayer(Enemy.Damage);
			    if (Enemy.HasElementalDamage) {
			        FightingLogic.OnEnemyHitPlayerElemental(Enemy.ElementalDamage, Enemy.ElementalDamageType);
			    }

                if (Enemy.OneAndDoneAttacker) {
			        EnemyState = State.Dead;

			        OnDeath.Invoke(Enemy);

			        if (Player.Health.Value > 0) {
			            yield return new WaitForSeconds(1);
                        PlayerAnimations.ResetAttack();
                        PlayerAnimations.Spin(() => {
			                CleanUp.Invoke();
			            });
			        }
			        break;
			    }

			    MyAnimator.SetTrigger("AttackEnd");
            }
        }

		public void ApplyDamage( int amount) {
		    if(EnemyState == State.Dead)
		        return;

            RegularDamageNumberSpawner.SpawnNumber(amount, BaseDamageColor);
		    OnHit.Invoke(amount);

            CurrentHealth.Value -= amount;
			if (CurrentHealth.Value <= 0) {
			    Die();
			} else {
                MyAnimator.SetTrigger("Flinch");
            }
        }

	    public void ApplyElementalDamage(int amount, ElementalType type) {
	        if(EnemyState == State.Dead)
	            return;

            amount = Mathf.RoundToInt(amount * Enemy.GetElementalWeakness(type));
	        ElementalDamageNumberSpawner.SpawnNumber(amount, type.DamageColor);
	        OnElementalHit.Invoke(amount);

            CurrentHealth.Value -= amount;
	        if(CurrentHealth.Value <= 0) {
	            Die();
	        } else {
	            MyAnimator.SetTrigger("Flinch");
	        }
	    }

        private void Die() {
	        EnemyState = State.Dead;
            Image.color = Color.white;

	        StopAllCoroutines();
	        OnDeath.Invoke(Enemy);
	        OnKilled.Invoke(Enemy);

            bool celebrateDone = false;
	        bool bubblesDone = false;

	        PlayerAnimations.ResetAttack();
	        PlayerAnimations.Celebrate(() => {
	            celebrateDone = true;
	            if(bubblesDone && celebrateDone) {
	                AllDone();
	            }
	        });

	        MyAnimator.SetTrigger("Death");
	        List<Item> items = Enemy.Drops.GetItems(true);
	        foreach(Item item in items) {
	            if(item is Weapon) {
	                Weapon weapon = item as Weapon;
	                weapon.Level = Enemy.Level;
	            }
	            if(item is Armor) {
	                Armor armor = item as Armor;
	                armor.Level = Enemy.Level;
	            }
                if(item is Headgear) {
                    Headgear hg = item as Headgear;
                    hg.Level = Enemy.Level;
                }
            }

            int count = 0;
            if (items.Count == 0) {
                bubblesDone = true;
                if(bubblesDone && celebrateDone) {
                    AllDone();
                }
            }

	        ItemBubbleManager.AddItems(items, () => {
	            count++;
	            if (count == items.Count) {
	                bubblesDone = true;
	                if(bubblesDone && celebrateDone) {
	                    AllDone();
	                }
                }
	        });

	        OnDropLoot.Invoke(items);
        }

	    private void AllDone() {
            PlayerAnimations.Spin(() => {
                CleanUp.Invoke();
                DeadEnemyController.AddDeadEnemy(RectTransform.anchoredPosition.x, RectTransform.anchoredPosition.y, Enemy);
            });
        }

		#endregion


#if UNITY_EDITOR
		[ContextMenu("Reload Enemy")]
		public void ReloadLevel() {
			SetEnemy(Enemy);
		}
#endif
	}
}

