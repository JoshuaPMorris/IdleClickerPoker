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
	
	private int heroNumber = 0;
	
	//List<Button> buttons = new List<Button>();

	public override void _Ready()
	{
		foreach(HBoxContainer node in buttonContainer.GetChildren())
		{
			HBoxContainer container = node;
			upgradeContainers.Add(container);
		}
	}
	
	private void _on_demo_unit_buy_pressed()
	{
		// Deleting buy unit button and revealing all unit upgrades
		//for (int i = 0; i <= upgradeContainers.Count; i++)
		//{
		//	
		//}

		//List<Button> buttons = new List<Button>();
		
		//upgradeContainers[heroNumber].GetChildren()[1].QueueFree();
		//upgradeContainers[heroNumber].GetChildren().OfType<Button>();
		//button = upgradeContainers[heroNumber].GetChildren()[2];

		//upgradeContianers[0].GetChildren()[1]

		//buyDemo.QueueFree();
		//upgradeDemo.Visible = true;

		//foreach(Node node1 in upgradeContainers[heroNumber].GetChildren())
		//{
		//	Button node = (Button) node1;
		//	Button button = node;
		//	buttons.Add(button);
		//}

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
