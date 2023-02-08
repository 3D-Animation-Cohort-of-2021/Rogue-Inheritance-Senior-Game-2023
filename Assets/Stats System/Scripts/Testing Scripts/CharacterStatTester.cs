using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatTester : MonoBehaviour
{

    private CharacterStatsSO characterStats;
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
        Color statColor = new Color((float)characterStats.strength / (float)divider, (float)characterStats.agility / (float)divider, (float)characterStats.wit / (float)divider, 1f);
        testRenderer.material.color = statColor;
        Debug.Log(characterStats);
    }


}
