﻿using System;
using easyInputs;
using GorillaLocomotion;
using Maximility.Menu.Mods.Movement;
using Maximility.Menu.Mods.Player;
using MelonLoader;
using Oculus.Interaction.HandGrab.Recorder;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Maximility
{
	public class ModManager : MelonMod
	{
		public void Mods()
		{
			//Movement Mod Toggles
			if ( Plugin.ExcelFly ) { FlyMods.ExcelFly(); }
            if ( Plugin.TFly ) { FlyMods.TFly(); }
            if (Plugin.LongArms) { longarms.LongArms(); }
        }
	}
}
