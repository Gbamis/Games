using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;


public class LSystem : MonoBehaviour
{
    public string axiom;
    public Dictionary<char,string> rules = new Dictionary<char, string>();
    public string generated;
    private StringBuilder sb = new StringBuilder();
    public int iterations;


    private void Start(){
        SetRules();
        Generate();
    }

    private void SetRules(){
        rules.Add('F',"F+G");
        rules.Add('G',"F-G");
        rules.Add('+',"+");
        rules.Add('-',"-");
    }

    private void Generate(){
        for(int i =0; i < iterations; i++){
            sb.Clear();
            foreach(Char letter in axiom){
                string value = rules[letter];
                sb.Append(value);
            }
            generated = axiom = sb.ToString();
        }
    }
}
