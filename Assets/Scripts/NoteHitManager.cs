using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class NoteHitManager : MonoBehaviour
{
    public int score = 0; // Player's score
    public int miss = 0;
    public LayerMask noteLayer; // Layer assigned to notes
    public Transform blueLaneTrigger; // Trigger area for the left lane
    public Transform redLaneTrigger; // Trigger area for the right lane
    public float detectionRadius = 0.5f; // Radius for note detection in each lane

    public TMP_Text scoreText;
    public TMP_Text missText;
    public TMP_Text hitText;


    public GameObject LeftRim;
    public GameObject RightRim;
    public GameObject LeftCenter;
    public GameObject RightCenter;

    public AudioClip hitSound;

    private PlayerInputActions inputActions;

    void Awake()
    {
        inputActions = new PlayerInputActions();

        inputActions.Game.LeftRim.performed += _ => CheckLaneHit(blueLaneTrigger);
        inputActions.Game.LeftRim.performed += _ => HighlightColour(LeftRim,Color.blue);
        inputActions.Game.LeftRim.canceled += _ => StartCoroutine(FadeColour(LeftRim, Color.white));


        inputActions.Game.RightRim.performed += _ => CheckLaneHit(blueLaneTrigger);
        inputActions.Game.RightRim.performed += _ => HighlightColour(RightRim, Color.blue);
        inputActions.Game.RightRim.canceled += _ => StartCoroutine(FadeColour(RightRim, Color.white));

        inputActions.Game.LeftCenter.performed += _ => CheckLaneHit(redLaneTrigger);
        inputActions.Game.LeftCenter.performed += _ => HighlightColour(LeftCenter, Color.red);
        inputActions.Game.LeftCenter.canceled += _ => StartCoroutine(FadeColour(LeftCenter, Color.white));


        inputActions.Game.RightCenter.performed += _ => CheckLaneHit(redLaneTrigger);
        inputActions.Game.RightCenter.performed += _ => HighlightColour(RightCenter, Color.red);
        inputActions.Game.RightCenter.canceled += _ => StartCoroutine(FadeColour(RightCenter, Color.white));
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
            miss++;
            missText.text = "Misses: " + miss;
            hitText.text = "Miss";
        }
        StartCoroutine(HideText());
    }

    void HighlightColour(GameObject gameObject, Color colour)
    {
        gameObject.GetComponent<SpriteRenderer>().color = colour;
    }
    
    IEnumerator FadeColour(GameObject gameObject, Color colour)
    {

        yield return new WaitForSeconds(0.12f);
        gameObject.GetComponent<SpriteRenderer>().color = colour;
    }

    IEnumerator HideText()
    {
        yield return new WaitForSeconds(0.15f);
        hitText.text = "";
    }
}
