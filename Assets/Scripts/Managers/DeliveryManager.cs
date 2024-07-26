using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailure;

    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeComplete;
    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeListSO recipeListSO;

    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipeMax = 4;
    private int successdulRecipes;

    private void Awake()
    {
        Instance = this;

        waitingRecipeSOList = new List<RecipeSO>();
    }
    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;

        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax; 

            if (KitchenGameManager.Instance.IsGamePlaying() && waitingRecipeSOList.Count < waitingRecipeMax)
            {
                RecipeSO waitingRecipe = recipeListSO.validRecipeSOList[UnityEngine.Random.Range(0, recipeListSO.validRecipeSOList.Count - 1)];
                waitingRecipeSOList.Add(waitingRecipe);

                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }

        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipe = waitingRecipeSOList[i];

            if (waitingRecipe.ingredientList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                bool recipesMatch = true;
                //if number of ingredients are same
                foreach (KitchenObjectSO waitingListIngredient in waitingRecipe.ingredientList)
                {
                    bool ingredientsMatch = false;
                    //cycling through all ingredients in current recipe
                    foreach (KitchenObjectSO ingredientOnPlate in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        //cycling through all ingredients on plate
                        if (ingredientOnPlate == waitingListIngredient)
                        {
                            ingredientsMatch = true;
                            break;
                        }

                    }

                    if (!ingredientsMatch)
                    {
                        //in the scenario that ingredients do not match for this specific recipe
                        recipesMatch = false;
                        break;
                    }
                }

                if (recipesMatch)
                {
                    waitingRecipeSOList.RemoveAt(i);
                    successdulRecipes++;
                    OnRecipeComplete?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    return;
                }

            }
        }

        //No match found! Incorrect Recipe!
        OnRecipeFailure?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }

    public int GetSuccessfulRecipes()
    {
        return successdulRecipes;
    }

}
