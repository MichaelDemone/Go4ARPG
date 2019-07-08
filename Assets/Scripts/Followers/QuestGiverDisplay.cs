using System;
using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Data;
using G4AW2.Dialogue;
using G4AW2.Followers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class QuestGiverDisplay : MonoBehaviour, IPointerClickHandler {

    public UnityEvent StartedWalking;
    public UnityEvent FinishInteraction;

    private QuestGiver follower;

    public void SetData(FollowerData follower) {

        RectTransform rt = (RectTransform) transform;

        Vector3 scale = rt.localScale;
        scale.x = 1;
        rt.localScale = scale;

        Vector2 pivot = rt.pivot;
        pivot.x = 1;
        rt.pivot = pivot;

        this.follower = (QuestGiver) follower;
        gameObject.SetActive(true);

        Vector2 r = ((RectTransform) transform).sizeDelta;
        r.x = follower.SizeOfSprite.x;
        r.y = follower.SizeOfSprite.y;
        ((RectTransform) transform).sizeDelta = r;

        Vector3 pos = transform.localPosition;
        pos.x = -54;
        transform.localPosition = pos;

        AnimatorOverrideController aoc =
            (AnimatorOverrideController) GetComponent<Animator>().runtimeAnimatorController;

        aoc["Idle"] = this.follower.SideIdleAnimation;
        aoc["Walk Up"] = this.follower.WalkingAnimation;
        aoc["Random"] = this.follower.RandomAnimation;
        aoc["QuestGiving"] = this.follower.GivingQuest;
    }

    public void StartWalking() {
        StopAllCoroutines();
        GetComponent<Animator>().SetBool("Walking", true);
        StartedWalking.Invoke();
    }

    public void StopWalking() {
        GetComponent<Animator>().ResetTrigger("Random");
        GetComponent<Animator>().SetBool("Walking", false);
        GetComponent<Animator>().SetBool("Giving", true);
    }

    public RuntimeSetFollowerData ListOfCurrentFollowers;
    public RuntimeSetQuest ListOfOpenQuests;

    public GameObject DismissButton;

    public void OnPointerClick(PointerEventData eventData) {

        PopUp.SetPopUp("Accept quest from quest giver? Title: " + follower.QuestToGive.DisplayName, new[] { "Yes", "No" }, new Action[] {
            () => {
                ListOfOpenQuests.Add(follower.QuestToGive);
                ListOfCurrentFollowers.Remove(follower);

                // Flip Giver
                RectTransform rt = (RectTransform) transform;
                Vector3 scale = rt.localScale;
                scale.x = -1;
                rt.localScale = scale;

                Vector2 pivot = rt.pivot;
                pivot.x = 0;
                rt.pivot = pivot;

                FinishInteraction.Invoke();

                StartCoroutine(WalkOffScreen());

            }, () => {
                DismissButton.SetActive(true);
            } });

    }

    public void Dismiss() {
        FinishInteraction.Invoke();
        ListOfCurrentFollowers.Remove(follower);
        StartCoroutine(WalkOffScreen());
    }

    [Header("Walk Off Parameters")]
    public float End;
    public float WalkOffSpeed;
    public float ScrollSpeed;
    public BoolReference Scrolling;
    

    IEnumerator WalkOffScreen() {

        GetComponent<Animator>().SetBool("Giving", false);
        GetComponent<Animator>().SetBool("Walking", true);

        RectTransform rt = (RectTransform) transform;

        while(true) {

            Vector3 pos = rt.localPosition;
            pos.x -= WalkOffSpeed * Time.deltaTime;
            pos.x -= Scrolling ? ScrollSpeed * Time.deltaTime : 0;
            rt.localPosition = pos;

            if(pos.x < End)
                break;

            yield return null;
        }
    }
}
