using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// Class that manages the population of bots in the simulation
public class PopulationManager_sc : MonoBehaviour
{
    // Prefab of the bot that will be instantiated
    public GameObject botPrefab;

    // Total number of bots in the population
    public int populationSize = 50;

    // List to hold all bots in the population
    List<GameObject> population = new List<GameObject>();

    // Static variable to track the elapsed time
    public static float elapsed = 0;

    // Time duration for each trial before breeding new bots
    public float trialTime = 5;

    // Current generation number
    int generation = 1;

    // GUIStyle for displaying statistics on the screen
    GUIStyle guiStyle = new GUIStyle();

    // Method to display GUI elements
    void OnGUI()
    {
        guiStyle.fontSize = 25; // Set font size for the GUI
        guiStyle.normal.textColor = Color.white; // Set font color to white

        // Create a group for the stats box
        GUI.BeginGroup(new Rect(10, 10, 250, 150));
        GUI.Box(new Rect(0, 0, 140, 140), "Stats", guiStyle); // Box for stats

        // Display current generation, elapsed time, and population size
        GUI.Label(new Rect(10, 25, 200, 30), "Gen: " + generation, guiStyle);
        GUI.Label(new Rect(10, 50, 200, 30), string.Format("Time: {0:0.00}", elapsed), guiStyle);
        GUI.Label(new Rect(10, 75, 200, 30), "Population: " + population.Count, guiStyle);
        GUI.EndGroup(); // End the stats group
    }

    // Use this for initialization  
    void Start()
    {
        // Create initial population of bots
        for (int i = 0; i < populationSize; i++)
        {
            // Generate a random starting position for the bot
            Vector3 startingPos = new Vector3(
                this.transform.position.x + Random.Range(-2, 2),
                this.transform.position.y,
                this.transform.position.z + Random.Range(-2, 2)
            );

            // Instantiate the bot and initialize its brain
            GameObject bot = Instantiate(botPrefab, startingPos, this.transform.rotation);
            bot.GetComponent<Brain_sc>().Init(); // Initialize bot's brain
            population.Add(bot); // Add bot to the population list
        }
    }

    // Method to breed two parent bots and create an offspring
    GameObject Breed(GameObject parent1, GameObject parent2)
    {
        // Generate a random starting position for the offspring
        Vector3 startingPos = new Vector3(
            this.transform.position.x + Random.Range(-2, 2),
            this.transform.position.y,
            this.transform.position.z + Random.Range(-2, 2)
        );

        // Instantiate the offspring bot
        GameObject offspring = Instantiate(botPrefab, startingPos, this.transform.rotation);
        Brain_sc b = offspring.GetComponent<Brain_sc>();
        b.Init(); // Initialize the brain of the offspring

        // Mutate or combine parents' DNA
        if (Random.Range(0, 100) == 1) // 1 in 100 chance to mutate
        {
            b.dna_sc.Mutate(); // Mutate the DNA of the offspring
        }
        else
        {
            // Combine DNA from both parents
            b.dna_sc.Combine(
                parent1.GetComponent<Brain_sc>().dna_sc,
                parent2.GetComponent<Brain_sc>().dna_sc
            );
        }
        return offspring; // Return the created offspring
    }

    // Method to breed a new population based on the performance of the current population
    void BreedNewPopulation()
    {
        // Sort the population by time alive (from shortest to longest)
        List<GameObject> sortedList = population.OrderBy(o => o.GetComponent<Brain_sc>().timeAlive).ToList();
        population.Clear(); // Clear the current population list

        // Create new bots from the top performers in the sorted list
        for (int i = (int)(sortedList.Count / 2.0f) - 1; i < sortedList.Count - 1; i++)
        {
            population.Add(Breed(sortedList[i], sortedList[i + 1])); // Breed parents
            population.Add(Breed(sortedList[i + 1], sortedList[i])); // Breed in reverse order
        }

        // Destroy all previous bots in the sorted list
        for (int i = 0; i < sortedList.Count; i++)
        {
            Destroy(sortedList[i]); // Clean up the previous population
        }

        generation++; // Increment the generation counter
    }

    // Update is called once per frame  
    void Update()
    {
        elapsed += Time.deltaTime; // Update elapsed time
                                   // If the trial time has elapsed, breed a new population
        if (elapsed >= trialTime)
        {
            BreedNewPopulation(); // Breed new bots based on performance
            elapsed = 0; // Reset the elapsed time
        }
    }
}
