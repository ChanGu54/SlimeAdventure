using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAnimCtrl
{
    public static Animator ctrl_Target;
    private static bool walking;
    private static bool die;

    public static void Idle()
    {
        walking = false;
        die = false;
        Set_Animation();
    }

    public static void Walking()
    {
        walking = true;
        Set_Animation();
    }

    public static void Die()
    {
        die = true;
        Set_Animation();
    }

    private static void Set_Animation()
    {
        ctrl_Target.SetBool("Walking", walking);
        ctrl_Target.SetBool("Die", die);
    }
}
