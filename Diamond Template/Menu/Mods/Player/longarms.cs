using easyInputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Maximility.Menu.Mods.Player
{
    public class longarms
    {
        public static void LongArms()
        {
            if (EasyInputs.GetPrimaryButtonDown(EasyHand.RightHand))
            {
                GorillaLocomotion.Player.Instance.transform.localScale = new Vector3(1f, 1, 1f);
            }

            if (EasyInputs.GetTriggerButtonDown(EasyHand.RightHand))
            {
                GorillaLocomotion.Player.Instance.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            }
        }

    }
}
