using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour {
    public SpriteRenderer Endzone;
    public int YardsPerSection;

    public PlanExecutor Plan;

    private bool _initialized;
    private SpriteRenderer _field;
    private SpriteRenderer _instanciatedEndzone;

    public float YardLength
    {
        get
        {
            return _field.bounds.size.y / YardsPerSection / _numberOfSections;
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
        _field = GetComponent<SpriteRenderer>();
        _instanciatedEndzone = Instantiate(Endzone, Vector3.zero, Quaternion.identity);
	}

    // Update is called once per frame
    void Update()
    {
        _field.size = new Vector2(_field.size.x, 1.2f * _numberOfSections);
        transform.position = new Vector3(0, _field.bounds.size.y / 2, 1);

        _instanciatedEndzone.transform.position = new Vector3(0, _field.bounds.size.y + Endzone.bounds.size.y / 2, 1);
    }
}
