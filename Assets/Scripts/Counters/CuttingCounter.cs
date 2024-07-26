using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    [SerializeField] private CuttingRecipeSO[] cutKitchenObjectSOArray;
    private int cuttingProgress;

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public static event EventHandler OnAnyCut;
    public event EventHandler OnCut;
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //the counter has no kitchen object
            if (player.HasKitchenObject())
            {
                //if the player has a kitchen object AND the object can be sliced
                if (HasRecipeforInputKitchenObjectSO(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    CuttingRecipeSO cuttingRecipeSO = GetRecipeForInputKitchenObjectSO(GetKitchenObject().GetKitchenObjectSO());
                    cuttingProgress = 0;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = (float)cuttingProgress / cuttingRecipeSO.maxCuttingProgress
                    });
                }
            }
            else
            {
                //Player has nothing
            }
        }
        else
        {
            //the counter has a kitchen object
            if (player.HasKitchenObject())
            {
                //player has something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    //player is holding a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
            }
            else
            {
                //Player has nothing
                this.GetKitchenObject().SetKitchenObjectParent(player);
            }
        }

    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeforInputKitchenObjectSO(this.GetKitchenObject().GetKitchenObjectSO()))
        {
            //Only allows cutting if the item is on the Cuttingcounter AND it has an output (aka if it can be sliced)

            cuttingProgress++;

            CuttingRecipeSO cuttingRecipeSO = GetRecipeForInputKitchenObjectSO(this.GetKitchenObject().GetKitchenObjectSO());

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.maxCuttingProgress
            });

            OnCut?.Invoke(this, EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);

            if (cuttingProgress >= cuttingRecipeSO.maxCuttingProgress)
            {
                KitchenObjectSO outputKitchenObjectSO = GetOutputForInputKitchenObjectSO(this.GetKitchenObject().GetKitchenObjectSO());
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }
                
        }
    }

    private KitchenObjectSO GetOutputForInputKitchenObjectSO(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetRecipeForInputKitchenObjectSO(inputKitchenObjectSO);

        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private bool HasRecipeforInputKitchenObjectSO(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetRecipeForInputKitchenObjectSO(inputKitchenObjectSO);

        return cuttingRecipeSO != null;
    }

    private CuttingRecipeSO GetRecipeForInputKitchenObjectSO(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cutKitchenObjectSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }

    new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }
}
