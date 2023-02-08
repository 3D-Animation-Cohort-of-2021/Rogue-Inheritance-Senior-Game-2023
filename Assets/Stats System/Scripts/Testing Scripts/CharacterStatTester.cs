using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatTester : MonoBehaviour
{

    public CharacterStatsSO characterStats;
    private Renderer testRenderer;
    // Start is called before the first frame update
    private void Awake() 
    {
        testRenderer = GetComponent<Renderer>();
    }

    public void SetupStatVisualizer(CharacterStatsSO prevCharacter, CharacterStatsSO spouse)
    {
        characterStats = CharacterStatsSO.CreateInstance<CharacterStatsSO>();
        characterStats.Initialize(prevCharacter, spouse);

        int divider = characterStats.GetInheritancePoints() / 3;
        Vector4 statColorVector = new Vector4((float)characterStats.strength / (float)divider, (float)characterStats.agility / (float)divider, (float)characterStats.wit / (float)divider, 1f);
        statColorVector.Normalize();
        Color statColor = statColorVector;
        testRenderer.material.color = statColor; 
    }

    public int GetHighestStat()
    {
        if(characterStats.strength > characterStats.agility && characterStats.strength > characterStats.wit)
        {
            return 0;
        }
        else if(characterStats.agility > characterStats.strength && characterStats.agility > characterStats.wit)
        {
            return 1;
        }
        else if(characterStats.wit > characterStats.agility && characterStats.wit > characterStats.strength)
        {
            return 2;
        }
        else
        {
            return 3;
        }
    }




}
