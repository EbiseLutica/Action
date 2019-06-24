using System;
using DotFeather;
using DotFeather.Router;

namespace Action
{
    class Game : GameBase
    {
		public Game() : base(640, 480, "アクション", 60)
		{
			router = new Router(this);
		}

		protected override void OnLoad(object sender, EventArgs e)
		{
			router.ChangeScene<GameScene>();
		}

		protected override void OnUpdate(object sender, DFEventArgs e)
		{
			router.Update(e);
		}

		static int Main(string[] args) => new Game().Run();

		private readonly Router router;
	}
}
