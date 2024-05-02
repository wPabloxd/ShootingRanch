using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] GameObject[] tutorialsText;
    [SerializeField] TractorTrigger tractor;
    [Header("Inputs")]
    [SerializeField] InputActionReference swapWeapon;
    [SerializeField] InputActionReference fire;
    [SerializeField] InputActionReference meleeAttack;
    [SerializeField] InputActionReference rotateLeft;
    [SerializeField] InputActionReference rotateRight;
    [SerializeField] InputActionReference[] selectWeaponInputs;
    [SerializeField] InputActionReference move;
    int tutorialStatus;
    private void Start()
    {
        if (GameManager.Instance.tutorialCompleted == 1)
        {
            tutorialStatus = 6;
            tractor.Activate();
        }
        UpdateTurorialTexts();
    }
    private void OnEnable()
    {
        move.action.Enable();
        meleeAttack.action.Enable();
        fire.action.Enable();
        swapWeapon.action.Enable();
        rotateLeft.action.Enable();
        rotateRight.action.Enable();
        foreach (InputActionReference iar in selectWeaponInputs)
        {
            iar.action.Enable();
        }
    }
    void UpdateTurorialTexts()
    {
        int index = 0;
        foreach(GameObject tutorial in tutorialsText)
        {
            if(index == tutorialStatus)
            {
                tutorial.SetActive(true);
            }
            else
            {
                tutorial.SetActive(false);
            }
            index++;
        }
    }
    private void Update()
    {
        if(tutorialStatus == 0)
        {
            if (move.action.WasPerformedThisFrame())
            {
                tutorialStatus++;
                UpdateTurorialTexts();
            }
        }
        else if (tutorialStatus == 1)
        {
            if (rotateLeft.action.WasPerformedThisFrame() || rotateRight.action.WasPerformedThisFrame())
            {
                tutorialStatus++;
                UpdateTurorialTexts();
            }
        }
        else if (tutorialStatus == 2)
        {
            if (fire.action.WasPerformedThisFrame())
            {
                tutorialStatus++;
                UpdateTurorialTexts();
            }
        }
        else if(tutorialStatus == 3)
        {
            if (swapWeapon.action.WasPerformedThisFrame() || selectWeaponInputs[0].action.WasPerformedThisFrame() || selectWeaponInputs[1].action.WasPerformedThisFrame() || selectWeaponInputs[2].action.WasPerformedThisFrame() || selectWeaponInputs[3].action.WasPerformedThisFrame())
            {
                tutorialStatus++;
                UpdateTurorialTexts();
            }
        }
        else if (tutorialStatus == 4)
        {
            if (meleeAttack.action.WasPerformedThisFrame())
            {
                tutorialStatus++;
                tractor.Activate();
                UpdateTurorialTexts();
            }
        }
        else if(tutorialStatus == 5)
        {
            if (tractor.alreadyTriggered)
            {
                tutorialStatus++;
                UpdateTurorialTexts();
                GameManager.Instance.UpdateGameState(1);
            }
        }
    }
}