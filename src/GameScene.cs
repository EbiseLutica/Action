using System;
using System.Collections.Generic;
using System.Drawing;
using DotFeather;
using DotFeather.Router;

namespace Action
{
	/// <summary>
	/// メインゲームのシーンです。
	/// </summary>
	public class GameScene : Scene
	{
		public override void OnStart(Router router, Dictionary<string, object> args)
		{
			var map = new Map("./maps/1-1.map");
			Console.WriteLine(map.Mode);

			chips = Texture2D.LoadAndSplitFrom("./textures/chip.png", 6, 1, new Size(16, 16));
		}

		public override void OnUpdate(Router router, DFEventArgs e)
		{ 
			
		}

		private Texture2D[] chips;

		const int ChipGround = 0,
				  ChipBox = 1,
				  ChipEmptyBox = 2,
				  ChipBrick = 3,
				  ChipCoin = 4,
				  ChipGoal = 5;
	}
}