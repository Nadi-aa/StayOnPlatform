using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA_sc
{
    // List to hold the genes (integer values representing traits)
    List<int> genes = new List<int>();

    // Length of the DNA, determining how many genes it will have
    int dnaLength = 0;

    // Maximum possible value for each gene
    int maxValues = 0;

    // Constructor to initialize DNA length and maximum gene values
    public DNA_sc(int l, int v)
    {
        dnaLength = l;       // Set the length of the DNA
        maxValues = v;       // Set the maximum gene value
        SetRandom();         // Initialize genes randomly
    }

    // Sets random values for each gene in the DNA sequence
    public void SetRandom()
    {
        genes.Clear();       // Clear any existing genes
        for (int i = 0; i < dnaLength; i++)
        {
            // Add a random integer between 0 and maxValues for each gene
            genes.Add(Random.Range(0, maxValues));
        }
    }

    // Sets a specific gene's value at the given position
    public void SetInt(int pos, int value)
    {
        genes[pos] = value;  // Set the gene at position 'pos' to 'value'
    }

    // Gets the value of a gene at a specified position
    public int GetGene(int pos)
    {
        return genes[pos];   // Return the gene at position 'pos'
    }

    // Combines genes from two parent DNA objects (d1 and d2) to create this DNA
    public void Combine(DNA_sc d1, DNA_sc d2)
    {
        for (int i = 0; i < dnaLength; i++)
        {
            // If within the first half of the DNA length, take gene from d1
            if (i < dnaLength / 2.0)
            {
                int c = d1.genes[i];
                genes[i] = c;
            }
            // Otherwise, take gene from d2 for the second half
            else
            {
                int c = d2.genes[i];
                genes[i] = c;
            }
        }
    }

    // Mutates a random gene by assigning it a new random value
    public void Mutate()
    {
        // Select a random gene and set it to a new random value
        genes[Random.Range(0, dnaLength)] = Random.Range(0, maxValues);
    }
}
