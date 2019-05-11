using DragonBones;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControl : MonoBehaviour {

    //float timer;

    //bool isMoveControl = false;
    int running = 0;

    public bool IsFinished()
    {
        if (running == 0)
            return true;
        else
            return false;
    }

    public void Move(string charaName, float oriX, float finX, float duration)
    {
        running++;
        StartCoroutine(MoveTo(GameObject.Find(charaName), oriX, finX, duration));
    }
    IEnumerator MoveTo(GameObject chara, float ori, float fin, float duration)
    {
        chara.transform.position = new Vector3(ori, chara.transform.position.y);
        var armature = chara.GetComponent<UnityArmatureComponent>();
        float time = 0, speed = (fin - ori) / duration;
        armature.animation.FadeIn("walk", 0.15f, -1);
        armature.animationName = "walk";
        while (time < duration)
        {
            time += Time.fixedDeltaTime;
            if (speed < 0)
                armature.armature.flipX = false;
            else
                armature.armature.flipX = true;
            chara.transform.Translate(Vector2.right * speed * Time.fixedDeltaTime);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        armature.animation.FadeIn("stand", 0.15f, -1);
        armature.animationName = "stand";
        chara.transform.position = new Vector3(fin, chara.transform.position.y);
        running--;
    }
}
