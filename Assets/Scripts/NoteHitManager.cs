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
    public TMP_Text hitText;

    public AudioClip hitSound;

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
            // Adds one score and destroy the note (didnt have time to scale score with accuracy)
            score ++;
            Destroy(hitColliders[0].gameObject);
            scoreText.text = "Notes Hit: " + score;
            hitText.text = "Hit";
            
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
        }
        else
        {
            hitText.text = "Miss";
        }
    }
}
