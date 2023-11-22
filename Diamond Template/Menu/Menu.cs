using System;
using System.Linq;
using easyInputs;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.PUN;
using UnityEngine;
using UnityEngine.UI;

namespace Maximility.Menu
{
	//Credits to Lars & ColossusYTTV for the GUI/HUD Template, this is just my modified version meant for fan-games & easier use.

    #region Setup
    public class MenuOption
    {
		//DON'T MODIFY THIS!

		//The display name of the mod. (EX: "DisplayNameExample [ON]/[OFF]")
        public string DisplayName;

        //The type of mod. Either AssociatedBool, which is a toggle ("[ON]/[OFF]"). And AssociatedString, a button.
        public string _type;

		//Toggle Mod ("[ON]/[OFF]")
        public bool AssociatedBool;

		//Button Mod
        public string AssociatedString;
    }

    public class MenuSettings
    {
		//The name that will display on the top of the menu (EX: "DIAMOND : Main")
        public static string MenuName = "Name Here";

        //Your menu color. This will be your name color. You can use hex codes or color names.
        public static string MenuColor = "cyan";

        //Your pointer color. This will be your pointer color (">" color). If you dont want a pointer color just set it as "white".
        public static string PointerColor = "purple";

        //Your off color. This will be the "[OFF]" color. You can use hex codes or color names.
        public static string OffColor = "blue";

        //Your on color. This will be the "[ON]" color. You can use hex codes or color names.
        public static string OnColor = "cyan";
    }

    #endregion

    public class Menu
	{
        #region References
        public static bool GUIToggled = true;

        private static GameObject HUDObj;

        private static GameObject HUDObj2;

        private static GameObject MainCamera;

        private static Text Testtext;

        private static Material AlertText = new Material(Shader.Find("GUI/Text Shader"));

        private static Text NotifiText;

        public static string MenuState = "Main";

        public static int SelectedOptionIndex = 0;

        public static MenuOption[] CurrentViewingMenu = null;

        public static bool inputcooldown = false;

        public static bool menutogglecooldown = false;

        #endregion

        public static MenuOption[] MainMenu;

        public static MenuOption[] Movement;

        public static MenuOption[] Player;


        public static void LoadOnce()
		{
            #region Object Settings
            Menu.MainCamera = GameObject.Find("Main Camera");
			Menu.HUDObj = new GameObject();
			Menu.HUDObj2 = new GameObject();
			Menu.HUDObj2.name = "CLIENT_HUB";
			Menu.HUDObj.name = "CLIENT_HUB";
			Menu.HUDObj.AddComponent<Canvas>();
			Menu.HUDObj.AddComponent<CanvasScaler>();
			Menu.HUDObj.AddComponent<GraphicRaycaster>();
			Menu.HUDObj.GetComponent<Canvas>().enabled = true;
            Menu.HUDObj.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
            Menu.HUDObj.GetComponent<Canvas>().worldCamera = Menu.MainCamera.GetComponent<Camera>();
			Menu.HUDObj.GetComponent<RectTransform>().sizeDelta = new Vector2(5f, 5f);
			Menu.HUDObj.GetComponent<RectTransform>().position = new Vector3(Menu.MainCamera.transform.position.x, Menu.MainCamera.transform.position.y, Menu.MainCamera.transform.position.z);
			Menu.HUDObj2.transform.position = new Vector3(Menu.MainCamera.transform.position.x, Menu.MainCamera.transform.position.y, Menu.MainCamera.transform.position.z - 4.6f);
			Menu.HUDObj.transform.parent = Menu.HUDObj2.transform;
			Menu.HUDObj.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 1.6f);
			Vector3 eulerAngles = Menu.HUDObj.GetComponent<RectTransform>().rotation.eulerAngles;
			eulerAngles.y = -270f;
			Menu.HUDObj.transform.localScale = new Vector3(1f, 1f, 1f);
			Menu.HUDObj.GetComponent<RectTransform>().rotation = Quaternion.Euler(eulerAngles);
			Menu.Testtext = new GameObject
			{
				transform = 
				{
					parent = Menu.HUDObj.transform
				}
			}.AddComponent<Text>();
			Menu.Testtext.text = "";
			Menu.Testtext.fontSize = 11;
			Menu.Testtext.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
			Menu.Testtext.rectTransform.sizeDelta = new Vector2(260f, 160f);
			Menu.Testtext.rectTransform.localScale = new Vector3(0.01f, 0.01f, 1f);
			Menu.Testtext.rectTransform.localPosition = new Vector3(-1.5f, 1f, 2f);
			Menu.Testtext.material = Menu.AlertText;
			Menu.NotifiText = Menu.Testtext;
			Menu.Testtext.alignment = 0;
			Menu.HUDObj2.transform.transform.position = new Vector3(Menu.MainCamera.transform.position.x, Menu.MainCamera.transform.position.y, Menu.MainCamera.transform.position.z);
			Menu.HUDObj2.transform.rotation = Menu.MainCamera.transform.rotation;

            #endregion

			//Main Menu List
            Menu.MainMenu = new MenuOption[5];
            Menu.MainMenu[0] = new MenuOption { DisplayName = "Movement", _type = "submenu", AssociatedString = "Movement" };
            Menu.MainMenu[1] = new MenuOption { DisplayName = "Player", _type = "submenu", AssociatedString = "Player" };

            //Movement List
            Menu.Movement = new MenuOption[4];
            Menu.Movement[0] = new MenuOption { DisplayName = "ExcelFly", _type = "toggle", AssociatedBool = false };
            Menu.Movement[1] = new MenuOption { DisplayName = "TFly", _type = "toggle", AssociatedBool = false };
            Menu.Movement[3] = new MenuOption { DisplayName = "Back", _type = "submenu", AssociatedString = "Main" };

            //Player List
            Menu.Player = new MenuOption[11];
            Menu.Player[0] = new MenuOption { DisplayName = "Long Arms", _type = "toggle", AssociatedBool = false };


            Menu.MenuState = "Main";
			Menu.CurrentViewingMenu = Menu.MainMenu;
			Menu.UpdateMenuState(new MenuOption(), null, null);
		}

		public static void Load()
		{
            Plugin.ExcelFly = Menu.Movement[0].AssociatedBool;
            Plugin.TFly = Menu.Movement[1].AssociatedBool;

            #region Load Code

            if (EasyInputs.GetThumbStickButtonDown(EasyHand.LeftHand) && EasyInputs.GetThumbStickButtonDown(EasyHand.RightHand) && !Menu.menutogglecooldown)
            {
                Menu.menutogglecooldown = true;
                Menu.HUDObj2.SetActive(!Menu.HUDObj2.activeSelf);
                Menu.GUIToggled = !Menu.GUIToggled;
            }
            if (!EasyInputs.GetThumbStickButtonDown(EasyHand.LeftHand) && !EasyInputs.GetThumbStickButtonDown(EasyHand.RightHand) && Menu.menutogglecooldown)
            {
                Menu.menutogglecooldown = false;
            }
            if (Menu.GUIToggled)
            {
                Menu.HUDObj2.transform.position = new Vector3(Menu.MainCamera.transform.position.x, Menu.MainCamera.transform.position.y, Menu.MainCamera.transform.position.z);
                Menu.HUDObj2.transform.rotation = Menu.MainCamera.transform.rotation;
                if (EasyInputs.GetThumbStickButtonDown(EasyHand.LeftHand))
                {
                    if (EasyInputs.GetTriggerButtonDown(EasyHand.RightHand) && !Menu.inputcooldown)
                    {
                        Menu.inputcooldown = true;
                        if (Menu.SelectedOptionIndex + 1 == Menu.CurrentViewingMenu.Count<MenuOption>())
                        {
                            Menu.SelectedOptionIndex = 0;
                        }
                        else
                        {
                            Menu.SelectedOptionIndex++;
                        }
                        Menu.UpdateMenuState(new MenuOption(), null, null);
                    }
                    if (EasyInputs.GetGripButtonDown(EasyHand.RightHand) && !Menu.inputcooldown)
                    {
                        Menu.inputcooldown = true;
                        Menu.UpdateMenuState(Menu.CurrentViewingMenu[Menu.SelectedOptionIndex], null, "optionhit");
                    }
                    if (!EasyInputs.GetTriggerButtonDown(EasyHand.RightHand) && !EasyInputs.GetGripButtonDown(EasyHand.RightHand) && Menu.inputcooldown)
                    {
                        Menu.inputcooldown = false;
                    }
                }
            }

            string text = string.Concat(new string[]
			{
				"<color=",
				MenuSettings.MenuColor + ">",
				MenuSettings.MenuName + " : ",
				Menu.MenuState,
				"</color>\n"
			});
			int num = 0;
			if (Menu.CurrentViewingMenu != null)
			{
				foreach (MenuOption menuOption in Menu.CurrentViewingMenu)
				{
					if (Menu.SelectedOptionIndex == num)
					{
						text += "<color=" + MenuSettings.PointerColor + ">" + ">" + "</color>";
					}
					text += menuOption.DisplayName;
					if (menuOption._type == "toggle")
					{
						if (menuOption.AssociatedBool)
						{
							text = text + " <color=" + MenuSettings.OnColor + ">" + ">[ON]" + "</color>";
						}
						else
						{
							text += " <color=" + MenuSettings.OffColor + ">" + "[OFF]" + "</color>";
						}
					}
					text += "\n";
					num++;
				}
				Menu.Testtext.text = text;
				return;
			}

            #endregion
        }

        private static void UpdateMenuState(MenuOption option, string _MenuState, string OperationType)
		{
			try
			{
				if (OperationType == "optionhit")
				{
					if (option._type == "submenu")
					{
						if (option.AssociatedString == "Movement")
						{
							Menu.CurrentViewingMenu = Menu.Movement;
						}
                        if (option.AssociatedString == "Player")
                        {
                            Menu.CurrentViewingMenu = Menu.Player;
                        }
                        if (option.AssociatedString == "Main")
						{
							Menu.CurrentViewingMenu = Menu.MainMenu;
						}
						Menu.MenuState = option.AssociatedString;
						Menu.SelectedOptionIndex = 0;
					}
					if (option._type == "toggle")
					{
						if (!option.AssociatedBool)
						{
							option.AssociatedBool = true;
						}
						else
						{
							option.AssociatedBool = false;
						}
					}

                    #region Buttons

                    //Button code will only run once

                    if (option._type == "button" && option.AssociatedString == "Disconnect")
                    {
                        PhotonNetwork.Disconnect();
                    }
                    if (option._type == "button" && option.AssociatedString == "SetName")
                    {
                        PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
                        ((RoomInfo)PhotonNetwork.CurrentRoom).name = "<color=red>DIAMOND ON TOP!!!!!!</color>";
                        PhotonNetwork.CurrentRoom.maxPlayers = 255;
                        PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
                        PhotonNetwork.CurrentRoom.MaxPlayers = 255;
                        PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
                        PhotonNetwork.CurrentLobby.Name = "<color=red>DIAMOND ON TOP!!!!!!</color>";
                        PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
                    }
                    if (option._type == "button" && option.AssociatedString == "DestoryAll")
                    {
                        PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
                        PhotonNetwork.DestroyAll();
                        PhotonNetwork.DestroyAll();
                        PhotonNetwork.DestroyAll();
                        PhotonNetwork.DestroyAll();
                        PhotonNetwork.DestroyAll();
                    }
                    if (option._type == "button" && option.AssociatedString == "upspeed")
                    {
                        GorillaLocomotion.Player.Instance.maxJumpSpeed += 0.5f;
                        GorillaLocomotion.Player.Instance.jumpMultiplier += 0.5f;
                    }
                    if (option._type == "button" && option.AssociatedString == "downspeed")
                    {
                        GorillaLocomotion.Player.Instance.maxJumpSpeed -= 0.5f;
                        GorillaLocomotion.Player.Instance.jumpMultiplier -= 0.5f;
                    }
                    if (option._type == "button" && option.AssociatedString == "SetNameP")
                    {

                        PhotonNetwork.LocalPlayer.NickName = "<color=cyan><size=3>USER OF DIAMOND CLIENT</color></size>\n<color=black><size=2>Paid Version</color></size>";

                    }
                    if (option._type == "button" && option.AssociatedString == "255lobby")
                    {
                        PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
                        PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
                        ((RoomInfo)PhotonNetwork.CurrentRoom).maxPlayers = byte.MaxValue;
                        PhotonNetwork.CurrentRoom.MaxPlayers = 255;
                        PhotonNetwork.CurrentRoom.maxPlayers = 255;
                    }
                    if (option._type == "button" && option.AssociatedString == "destroyvoice")
                    {
                        foreach (PhotonVoiceView photonVoiceView in UnityEngine.Object.FindObjectsOfType<PhotonVoiceView>())
                        {
                            PhotonNetwork.TransferOwnership(photonVoiceView.photonView.Controller.actorNumber, PhotonNetwork.LocalPlayer.actorNumber);
                            PhotonNetwork.RequestOwnership(photonVoiceView.photonView.Controller.actorNumber, PhotonNetwork.LocalPlayer.actorNumber);
                            photonVoiceView.photonView.Controller.actorNumber = PhotonNetwork.LocalPlayer.actorNumber;
                            PhotonNetwork.TransferOwnership(photonVoiceView.photonView.Controller.actorNumber, PhotonNetwork.LocalPlayer.actorNumber);
                            PhotonNetwork.RequestOwnership(photonVoiceView.photonView.Controller.actorNumber, PhotonNetwork.LocalPlayer.actorNumber);
                            photonVoiceView.photonView.Controller.actorNumber = PhotonNetwork.LocalPlayer.actorNumber;
                            PhotonNetwork.TransferOwnership(photonVoiceView.photonView.Controller.actorNumber, PhotonNetwork.LocalPlayer.actorNumber);
                            PhotonNetwork.RequestOwnership(photonVoiceView.photonView.Controller.actorNumber, PhotonNetwork.LocalPlayer.actorNumber);
                            photonVoiceView.photonView.Controller.actorNumber = PhotonNetwork.LocalPlayer.actorNumber;
                            PhotonNetwork.TransferOwnership(photonVoiceView.photonView.Controller.actorNumber, PhotonNetwork.LocalPlayer.actorNumber);
                            PhotonNetwork.RequestOwnership(photonVoiceView.photonView.Controller.actorNumber, PhotonNetwork.LocalPlayer.actorNumber);
                            photonVoiceView.photonView.Controller.actorNumber = PhotonNetwork.LocalPlayer.actorNumber;
                            PhotonNetwork.Destroy(photonVoiceView.photonView);
                        }
                    }

                    #endregion
                }
            }
			catch
			{
			}
		}
	}
}
