//Created by: Marshall Krueger
//Last edited by: Marshall krueger 2/7/23
//Purpose: This scriptableObject is used to track an individual character's stats
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStats", menuName = "ScriptableObjects/StatsSystem/CharacterStats", order = 0)]
public class CharacterStatsSO : ScriptableObject {

    public int health = 0;
    public int strength = 1;
    public int agility = 1;
    public int wit = 1;
    int inheritancePoints = 0;

    /// <summary>
    /// Purpose: Construct the character stats with reference of parents' stats
    /// </summary>
    /// <param name="prevCharacter">previous character's CharacterStatsSO</param>
    /// <param name="spouse">prev character's spouse's CharacterStatsSO</param>
    public void Initialize(CharacterStatsSO prevCharacter, CharacterStatsSO spouse)
    {
        InheritStats(prevCharacter, spouse);
        UpdateInheritancePoints();
    }
    /// <summary>
    /// Purpose: Construct the character stats with no parents
    /// </summary>
    public void Initialize()
    {
        health = 5;
        strength = 5;
        agility = 5;
        wit = 5;
        UpdateInheritancePoints();
    }

    /// <summary>
    /// Purpose: Setup the character stats with reference of parents' stats
    /// </summary>
    /// <param name="prevCharacter">previous character's CharacterStatsSO</param>
    /// <param name="spouse">prev character's spouse's CharacterStatsSO</param>
    private void InheritStats(CharacterStatsSO prevCharacter, CharacterStatsSO spouse)
    {
        int parentSumIP = prevCharacter.GetInheritancePoints() + spouse.GetInheritancePoints();
        int parentSumStrength = prevCharacter.strength + spouse.strength;
        int parentSumAgility = prevCharacter.agility + spouse.agility;
        int parentSumWit = prevCharacter.wit + spouse.wit;

        int tempIP = Random.Range(spouse.GetInheritancePoints(), prevCharacter.GetInheritancePoints());

        health = Random.Range(spouse.health, prevCharacter.health);
        
        for(int i = 0; i < tempIP; i++)
        {
            int randStat = Random.Range(0,3);

            if(randStat == 0 && tempIP / strength > (parentSumIP / parentSumStrength) - 1)
            {
                strength++;
            }
            else if(randStat == 1 && tempIP / agility > (parentSumIP / parentSumAgility) - 1)
            {
                agility++;
            }
            else if(randStat == 2 && tempIP / wit > (parentSumIP / parentSumWit) - 1)
            {
                wit++;
            }
            else
            {
                i--;
            }

        }

        health += Random.Range(-1, 1);
        strength += Random.Range(-1, 1);
        agility += Random.Range(-1, 1);
        wit += Random.Range(-1, 1);
    }

    /// <summary>
    /// Purpose: Recalculate the number of inheritance points
    /// </summary>
    private void UpdateInheritancePoints()
    {
        inheritancePoints = strength + agility + wit;
    }

    /// <summary>
    /// Purpose: Get the inheritance points of this character
    /// </summary>
    /// <returns>an integer with the </returns>
    public int GetInheritancePoints()
    {
        return inheritancePoints;
    }

    public override string ToString()
    {
        return $"Hp: {health}, Str: {strength}, Agi: {agility}, Wit: {wit}, IP: {inheritancePoints}";
    }
    
}
