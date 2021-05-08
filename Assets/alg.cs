using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alg : MonoBehaviour
{
    public GameObject cell;
    private int[,] grid;
    private int[,] next;
    private int resolution = 1;
    private int width = 20;
    private int height = 20;
    private int cols;
    private int rows;
 

    // Start is called before the first frame update
    void Start()
    {
        cols = width / resolution;
        rows = height / resolution;

        //make 2D array
        grid = new int[cols,rows];

        //randomly fill with zeros and ones
        for (int i = 0; i < cols; i++) {
            for (int j = 0; j < rows; j++) {
                grid[i,j] = Random.Range(0, 2);
            }
        }

        //Instantiate prefabs if array at index i,j holds a 1.
        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                if (grid[i, j] == 1)
                {
                    Instantiate(cell, new Vector3(i, j, 0), Quaternion.identity);
                }
            }
        }

        InvokeRepeating("Change_grid", 0.1f, 0.1f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {


    }

    //Method to destroy the prefabs every generation
    void Destroy_cells() {
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject obj in allObjects)
        {
            Destroy(obj);
        }
    }

    //Method that creates array that holds the number of neighbors for each cell,
    //which determines whether the cell lives or dies next generation.
    int[,] Create_count_map(int[,] arr, int c, int r) {
        int[,] countArray = new int[c, r];

        for (int i = 0; i < c; i++) {
            for (int j = 0; j < r; j++) {
                int sum = 0;

                for (int k = -1; k < 2; k++)
                {
                    for (int l = -1; l < 2; l++)
                    {

                        if (!(((i + k) < 0) || ((i + k) > c - 1) || ((j + l) < 0) || ((j + l) > r - 1)))
                        {
                            sum += arr[i + k, j + l];
                        }

                    }
                }

                sum -= arr[i, j];

                countArray[i,j] = sum;

            }
        }

        return countArray;
    }

    //Method that takes the old generation, the no. of surrounding cells,
    //and applies the rules to make the new generation.
    int[,] Apply_rules(int[,] arr, int[,] countArray, int[,] arr2, int c, int r) {

        for (int i = 0; i < c; i++)
        {
            for (int j = 0; j < r; j++)
            {

                if (arr[i,j] == 0)
                {
                    if (countArray[i,j] == 3)
                    {
                        arr2[i,j] = 1;
                    }
                    else
                    {
                        arr2[i,j] = 0;
                    }
                }
                else if (arr[i,j] == 1)
                {
                    if ((countArray[i,j] < 2) || (countArray[i,j] > 3))
                    {
                        arr2[i,j] = 0;
                    }
                    else
                    {
                        arr2[i,j] = 1;
                    }
                }

            }
        }

        return arr2;
    }


    //Method that iteratively creates each generation
    void Change_grid() {
        Destroy_cells();
        

        cols = width / resolution;
        rows = height / resolution;

        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                if (grid[i, j] == 1)
                {
                    Instantiate(cell, new Vector3(i, j, 0), Quaternion.identity);
                }
            }
        }


        next = new int[cols, rows];

        int [,] countArray = Create_count_map(grid, cols, rows);

        next = Apply_rules(grid, countArray, next, cols, rows);

        grid = next;
    }
}
