using System.Collections.Generic;
using System.IO;
using System.Linq;
using DotFeather;

namespace Action
{
	public class SceneLoader
	{
		public string BgmPath { get; set; }
		public LevelMode Mode { get; set; }
		public int GoalPositionX { get; set; }
		public Vector StartPosition { get; set; }

		public SceneLoader(string path)
		{
			Parse(File.ReadAllLines(path));
		}
		
		public SceneLoader(string[] lines)
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

			foreach (var line in lines.Select(l => l.Trim()))
			{
				// コメントの読み飛ばし
				if (line.StartsWith("#"))
					continue;

				if (isMapMode)
				{
					if (line.ToLowerInvariant().StartsWith("mapend:"))
					{
						// マップ定義終わり
						isMapMode = false;
					}
					else
					{
						// マップは後で纏めて解析する為、とりあえずキューに入れる
						buf.Enqueue(line);
					}
				}
				else
				{
					// BGM 指定
					if (line.ToLowerInvariant().StartsWith("bgm:"))
					{
						BgmPath = line.Trim().Substring(4);
					}
					// モードしてい
					if (line.ToLowerInvariant().StartsWith("mode:"))
					{
						var mode = line.Trim().ToLowerInvariant().Substring(4);
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
					if (line.ToLowerInvariant().StartsWith("mapstart:"))
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