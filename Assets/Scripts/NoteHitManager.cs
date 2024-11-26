using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class NoteHitManager : MonoBehaviour
{
    public int score = 0; // Player's score
    public LayerMask noteLayer; // Layer assigned to notes
    public Transform leftLaneTrigger; // Trigger area for the left lane
    public Transform rightLaneTrigger; // Trigger area for the right lane
    public float detectionRadius = 0.5f; // Radius for note detection in each lane

    public TMP_Text scoreText;

    private PlayerInputActions inputActions;

    void Awake()
    {
        inputActions = new PlayerInputActions();

        inputActions.Game.Left.performed += _ => CheckLaneHit(leftLaneTrigger);
        inputActions.Game.Right.performed += _ => CheckLaneHit(rightLaneTrigger);
    }

    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    void CheckLaneHit(Transform laneTrigger)
    {
        // Check for notes in the lane's detection area
        Collider[] hitColliders = Physics.OverlapSphere(laneTrigger.position, detectionRadius, noteLayer);

        if (hitColliders.Length > 0)
        {
            // Award points and destroy the note
            score += 1;
            Destroy(hitColliders[0].gameObject);
            Debug.Log($"Hit! Current score: {score}");
            scoreText.text = "Notes Hit: " + score;
        }
        else
        {
            Debug.Log("Miss!");
        }
    }
}
