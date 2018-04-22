using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour {
    public SpriteRenderer FieldSection;
    public int YardsPerSection;

    public PlanExecutor Plan;

    public float YardLength
    {
        get
        {
            return FieldSection.bounds.size.y / YardsPerSection;
        }
    }

    private int _numberOfSections
    {
        get
        {
            return Mathf.CeilToInt(Plan.YardageGoal / (float)YardsPerSection);
        }
    }

	// Use this for initialization
	void Start () {
		for (var s = 0; s < _numberOfSections; s++)
        {
            Instantiate(FieldSection, new Vector3(0, FieldSection.bounds.size.y * s + FieldSection.bounds.size.y / 2, 1), Quaternion.identity);
        }
	}

    // Update is called once per frame
    void Update() { }
}
