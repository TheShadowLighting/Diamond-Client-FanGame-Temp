using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.XR;
using UnityEngine;
using easyInputs;

namespace Maximility.Menu.Mods.pHOTON
{
    public class Ironman
    {
        public static void IronMan()
        {
            Ironman.ironL();
            Ironman.ironR();
        }


        public static void ironL()
        {
            if (EasyInputs.GetTriggerButtonDown(EasyHand.LeftHand))
            {
                GorillaLocomotion.Player.Instance.transform.position += (GorillaLocomotion.Player.Instance.leftHandTransform.transform.forward * Time.deltaTime) * 15; GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }

        public static void ironR()
        {
            if (EasyInputs.GetTriggerButtonDown(EasyHand.RightHand))
            {
                GorillaLocomotion.Player.Instance.transform.position += (GorillaLocomotion.Player.Instance.rightHandTransform.transform.forward * Time.deltaTime) * 15; GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
    }
}
