using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Part
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override IEnumerator LaunchPart()
    {
        CallKnockOff();
        yield return new WaitForSeconds(0.5f);
    }
}
