using Godot;
using System;

public partial class PopupMenu : MarginContainer
{
	[Export] private HBoxContainer upgradeScreen;
	[Export] private HBoxContainer openUpgradeScreen;

	[Export] private MarginContainer upgradeMenu1;
	[Export] private MarginContainer upgradeMenu2;
	[Export] private MarginContainer upgradeMenu3;

	private static void ToggleVisibility(HBoxContainer _container)
	{
		if(_container.Visible)
			_container.Visible = !_container.Visible;
		else
			_container.Visible = true;
	}
	
	
	private void _on_toggle_upgrade_button_pressed()
	{
		ToggleVisibility(upgradeScreen);
		ToggleVisibility(openUpgradeScreen);
	}
	
	private void _on_upgrade_menu_changer_1_pressed()
	{
		if(upgradeMenu1.Visible != true)
		{
			upgradeMenu1.Visible = true;
			upgradeMenu2.Visible = false;
			upgradeMenu3.Visible = false;
		}
	}
	
	private void _on_upgrade_menu_changer_2_pressed()
	{
		if(upgradeMenu2.Visible != true)
		{
			upgradeMenu2.Visible = true;
			upgradeMenu1.Visible = false;
			upgradeMenu3.Visible = false;
		}
	}
	
	private void _on_upgrade_menu_changer_3_pressed()
	{
		if(upgradeMenu3.Visible != true)
		{
			upgradeMenu3.Visible = true;
			upgradeMenu1.Visible = false;
			upgradeMenu2.Visible = false;
		}
	}
}
