using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour {
    public SpriteRenderer FieldSection;
    public SpriteRenderer Endzone;
    public int YardsPerSection;

    public PlanExecutor Plan;

    private bool _initialized;

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
		
	}

    // Update is called once per frame
    void Update()
    {
        if (_numberOfSections > 0 && !_initialized)
        {
            for (var s = 0; s < _numberOfSections; s++)
            {
                Instantiate(FieldSection, new Vector3(0, FieldSection.bounds.size.y * s + FieldSection.bounds.size.y / 2, 1), Quaternion.identity, transform);
            }

            Instantiate(Endzone, new Vector3(0, FieldSection.bounds.size.y * _numberOfSections + Endzone.bounds.size.y / 2, 1), Quaternion.identity, transform);

            _initialized = true;
        }
    }
}
