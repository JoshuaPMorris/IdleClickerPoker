using Godot;
using System;

public partial class PopupMenu : MarginContainer
{
	[Export] private HBoxContainer upgradeScreen;
	[Export] private HBoxContainer openUpgradeScreen;

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
}
