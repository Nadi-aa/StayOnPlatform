using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class that defines the behavior of a bot in the game
public class Brain_sc : MonoBehaviour
{
    // Length of the DNA for the bot (number of genes)
    int DNALength = 2;

    // Time the bot has been alive
    public float timeAlive;

    // Reference to the bot's DNA script
    public DNA_sc dna_sc;

    // Reference to the GameObject representing the bot's eyes
    public GameObject eyes;

    // Indicates whether the bot is alive
    bool isAlive = true;

    // Indicates whether the bot can see the ground
    bool canSeeGround = true;

    // Variables to control movement and turning
    float turn = 0;
    float move = 0;

    // Called when the bot collides with another object
    void OnCollisionEnter(Collision other)
    {
        // If the object has the "dead" tag, the bot is considered dead
        if (other.gameObject.tag == "dead")
        {
            isAlive = false; // Mark the bot as not alive
        }
    }

    // Initialize the bot's DNA and reset its state
    public void Init()
    {
        // Initialize DNA with DNALength and 3 possible values for each gene
        dna_sc = new DNA_sc(DNALength, 3);
        timeAlive = 0; // Reset time alive
        isAlive = true; // Set the bot as alive
    }

    // Update is called once per frame
    private void Update()
    {
        // If the bot is not alive, exit the update function
        if (!isAlive) return;

        // Draw a ray in the forward direction from the bot's eyes for visualization
        Debug.DrawRay(eyes.transform.position, eyes.transform.forward * 10, Color.red, 10);
        canSeeGround = false; // Reset ground visibility

        RaycastHit hit; // Variable to store raycast hit information

        // Perform a raycast to check if there is ground ahead
        if (Physics.Raycast(eyes.transform.position, eyes.transform.forward * 10, out hit))
        {
            // If the raycast hits an object with the "platform" tag, set canSeeGround to true
            if (hit.collider.gameObject.tag == "platform")
            {
                canSeeGround = true;
            }
        }

        // Update the time the bot has been alive based on the population manager's elapsed time
        timeAlive = PopulationManager_sc.elapsed;

        // If the bot can see the ground, read the first gene to determine movement
        if (canSeeGround)
        {
            // Move forward, turn left, or turn right based on the gene value
            if (dna_sc.GetGene(0) == 0) move = 1; // Move forward
            else if (dna_sc.GetGene(0) == 1) turn = -90; // Turn left
            else if (dna_sc.GetGene(0) == 2) turn = 90; // Turn right
        }
        // If the bot cannot see the ground, read the second gene for movement
        else
        {
            if (dna_sc.GetGene(1) == 0) move = 1; // Move forward
            else if (dna_sc.GetGene(1) == 1) turn = -90; // Turn left
            else if (dna_sc.GetGene(1) == 2) turn = 90; // Turn right
        }

        // Move the bot forward by translating it along the z-axis
        this.transform.Translate(0, 0, move * 0.1f);
        // Rotate the bot around the y-axis based on the turn value
        this.transform.Rotate(0, turn, 0);
    }
}
