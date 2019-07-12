using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DotFeather;

namespace Action
{
	/// <summary>
	/// シーンを読み込みます。
	/// </summary>
	public class Map
	{
		/// <summary>
		/// BGM のパスを取得します。
		/// </summary>
		/// <value></value>
		public string BgmPath { get; set; }
		public LevelMode Mode { get; set; }
		public int GoalPositionX { get; set; }
		public Vector StartPosition { get; set; }
		public Chip[,] Data { get; set; }

		public Map(string path)
		{
			Parse(File.ReadAllLines(path));
		}
		
		public Map(string[] lines)
		{
			Parse(lines);
		}

		public void Parse(string[] lines)
		{
			// マップ読み取りが始まっていれば true
			var isMapMode = false;

			// マップデータ用のバッファ
			Queue<string> buf = new Queue<string>();

			// デバッグ用に行番号を記録する
			int number = 0;

			foreach (var line in lines)
			{
				var trimmedLine = line.Trim();
				// コメントの読み飛ばし
				if (trimmedLine.StartsWith("#"))
					continue;

				if (isMapMode)
				{
					if (trimmedLine.ToLowerInvariant().StartsWith("mapend:"))
					{
						// マップ定義終わり
						isMapMode = false;
					}
					else
					{
						// エラー処理
						if (!line.All(c => " 0OSM#?!G".Contains(c)))
							throw new ParserException("Invalid char", number);
						// マップは後で纏めて解析する為、とりあえずキューに入れる
						buf.Enqueue(line);
					}
				}
				else
				{
					// BGM 指定
					if (trimmedLine.ToLowerInvariant().StartsWith("bgm:"))
					{
						BgmPath = trimmedLine.Substring(4).Trim();
					}
					// モード指定
					if (trimmedLine.ToLowerInvariant().StartsWith("mode:"))
					{
						var mode = trimmedLine.ToLowerInvariant().Substring(5).Trim();
						switch (mode)
						{
							case "0":
							case "ground":
								Mode = LevelMode.Ground;
								break;
							case "1":
							case "underground":
								Mode = LevelMode.Underground;
								break;
							case "2":
							case "underwater":
								Mode = LevelMode.UnderWater;
								break;
							case "3":
							case "castle":
								Mode = LevelMode.Castle;
								break;
							default:
								throw new ParserException($"Invalid Mode '{mode}'", number);
						}
					}
					// マップ解析を始める
					if (trimmedLine.ToLowerInvariant().StartsWith("mapstart:"))
					{
						// マップの定義がもう終わっていればエラー
						if (buf.Count > 0)
							throw new ParserException("Duplicate MAPSTART", number);
						isMapMode = true;
					}
				}
				number++;
			}
			if (buf.Count == 0)
				throw new ParserException("Empty map data is not allowed", number);
			
			Data = new Chip[buf.Max(s => s.Length), buf.Count];
			var yMax = buf.Count;
			for (var y = 0; y < yMax; y++)
			{
				var line = buf.Dequeue();
				for (var x = 0; x < line.Length; x++)
				{
					Console.WriteLine(line);
					switch (line[x])
					{
						case Ground:
							Data[x, y] = Chip.Ground;
							break;
						case Coin:
							Data[x, y] = Chip.Coin;
							break;
						case Start:
							StartPosition = new Vector(x, y);
							break;
						case Enemy:
							// TODO: Implement Enemy
							break;
						case Brick:
							Data[x, y] = Chip.Brick;
							break;
						case BoxCoin:
							Data[x, y] = Chip.BoxCoin;
							break;
						case BoxMushroom:
							Data[x, y] = Chip.BoxMushroom;
							break;
						case Goal:
							Data[x, y] = Chip.Goal;
							GoalPositionX = x;
							break;
					}
				}
			}
		}

		const char Air = ' ';
		const char Ground = '0';
		const char Coin = 'O';
		const char Start = 'S';
		const char Enemy = 'M';
		const char Brick = '#';
		const char BoxCoin = '?';
		const char BoxMushroom = '!';
		const char Goal = 'G';
	}
}