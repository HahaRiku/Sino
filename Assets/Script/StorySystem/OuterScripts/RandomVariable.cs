using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomVariable : MonoBehaviour {
    [System.Serializable]
    public struct Parameter {
        public string variableName;
        public int randomMax_exclusive;
        public int randomMin_inclusive;
    }
    public Parameter[] parameterList;

    public void RandomNumber(int index) {
        SystemVariables.AddIntVariable(parameterList[index].variableName,
            Random.Range(parameterList[index].randomMin_inclusive, parameterList[index].randomMax_exclusive));
    }
}
