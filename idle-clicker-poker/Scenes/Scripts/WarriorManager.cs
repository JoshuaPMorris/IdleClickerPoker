using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class WarriorManager : MarginContainer
{	
	[Export] private Button buyDemo;
	[Export] private Button upgradeDemo;
	
	[Export] private VBoxContainer buttonContainer;
	
	List<HBoxContainer> upgradeContainers = new List<HBoxContainer>();
	List<NinePatchRect> backgroundContainers = new List<NinePatchRect>();
	
	private int heroNumber = 0;

	public override void _Ready()
	{
		//foreach(NinePatchRect background in buttonContainer.GetChildren().OfType<NinePatchRect>())
		//{
		//	backgroundContainers.Add(background);
		//}
		
		foreach(HBoxContainer node in buttonContainer.GetChildren().OfType<HBoxContainer>())
		{
			HBoxContainer container = node;
			upgradeContainers.Add(container);
		}
	}
	
	private void _on_demo_unit_buy_pressed()
	{
		for(int i = 1; i < upgradeContainers[heroNumber].GetChildren().Count; i++)
		{
			Button button;
			button = (Button)upgradeContainers[heroNumber].GetChildren()[i];
			if(i == 1)
			{
				button.QueueFree();
			}
			else
			{
				button.Visible = true;
			}
		}


		upgradeContainers[heroNumber + 1].Visible = true;
		
		heroNumber++;
	}
}
