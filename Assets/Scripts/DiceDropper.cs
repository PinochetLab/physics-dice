using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Class <c>DiceDropper</c> drops dices.
/// </summary>
public class DiceDropper : MonoBehaviour
{
    [SerializeField] private List<Dice> dices;

    private List<DropPreset> dropPresets;
    private bool isRealDropStarted;

    private void Start()
    {
        Drop();
    }

    private void PrepareFirstPhase()
    {
        dices.ForEach(x => x.SetVisible(false));
        Time.timeScale = 10;
    }

    private void PrepareSecondPhase()
    {
        dices.ForEach(x => x.SetVisible(true));
        Time.timeScale = 1;
    }

    /// <summary>
    /// <c>Drop</c> drops dices with random drop presets.
    /// </summary>
    public void Drop()
    {
        // At first we drop "fast" and "invisible" dices to look on what side dices are landed
        PrepareFirstPhase();
        isRealDropStarted = false;
        dropPresets = dices.Select(_ => DropPreset.GenerateRandom()).ToList();

        for (var i = 0; i < dices.Count; i++)
        {
            dices[i].Drop(dropPresets[i]);
            dices[i].StartRecordPath();
        }
    }

    private void FixedUpdate()
    {
        if (!isRealDropStarted && dices.All(dice => dice.IsSleeping()))
        {
            // Turn over dices and drop again
            dices.ForEach(dice => dice.TurnOver());
            isRealDropStarted = true;
            PrepareSecondPhase();
            dices.ForEach(dice => dice.StopRecordPathAndPlay());
        }
    }
}
