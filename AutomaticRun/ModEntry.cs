using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace AutomaticRun
{
	/// <summary>The mod entry point.</summary>
	internal sealed class ModEntry : Mod
	{
		private ModConfig Config = null!;
		private bool IsAutoRunning;
		private const int OFFSET = 4;
		public override void Entry(IModHelper helper)
		{
			Config = helper.ReadConfig<ModConfig>();
			helper.WriteConfig(Config);
			helper.Events.GameLoop.UpdateTicking += OnUpdateTicking;
			helper.Events.GameLoop.GameLaunched += OnGameLaunched;
		}

		private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
		{
			var configMenu = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
			if (configMenu is null)
				return;

			configMenu.Register(
				mod: ModManifest,
				reset: () => Config = new ModConfig(),
				save: () => Helper.WriteConfig(Config)
			);

			configMenu.AddKeybindList(
				mod: ModManifest,
				name: () => Helper.Translation.Get("config.toggleKey.name"),
				tooltip: () => Helper.Translation.Get("config.toggleKey.tooltip"),
				getValue: () => Config.ToggleKey,
				setValue: value => Config.ToggleKey = value
			);

			configMenu.AddBoolOption(
				mod: ModManifest,
				name: () => Helper.Translation.Get("config.stopOnCollision.name"),
				tooltip: () => Helper.Translation.Get("config.stopOnCollision.tooltip"),
				getValue: () => Config.StopOnCollision,
				setValue: value => Config.StopOnCollision = value
			);
		}

		private bool IsColliding(int direction)
		{
			Rectangle box = Game1.player.GetBoundingBox();
			Rectangle shifted = direction switch
			{
				0 => new Rectangle(box.X, box.Y - OFFSET, box.Width, box.Height),
				1 => new Rectangle(box.X + OFFSET, box.Y, box.Width, box.Height),
				2 => new Rectangle(box.X, box.Y + OFFSET, box.Width, box.Height),
				3 => new Rectangle(box.X - OFFSET, box.Y, box.Width, box.Height),
				_ => box
			};

			bool isBlocked = Game1.currentLocation.isCollidingPosition(shifted, Game1.viewport, Game1.player);

			if (isBlocked)
			{
				IsAutoRunning = false;
				Game1.addHUDMessage(new HUDMessage(Helper.Translation.Get("hud.autorun.off"), HUDMessage.newQuest_type));
			}

			return isBlocked;
		}

		private void OnUpdateTicking(object? sender, UpdateTickingEventArgs e)
		{
			if (!Context.IsWorldReady)
				return;

			// Handle toggle
			if (Config.ToggleKey.JustPressed())
			{
				IsAutoRunning = !IsAutoRunning;
				Game1.addHUDMessage(new HUDMessage(Helper.Translation.Get(IsAutoRunning ? "hud.autorun.on" : "hud.autorun.off"), HUDMessage.newQuest_type));
			}

			if (!IsAutoRunning)
				return;

			bool isChangingDirection =
				// Left joystick
				Helper.Input.IsDown(SButton.LeftThumbstickLeft) ||
				Helper.Input.IsDown(SButton.LeftThumbstickRight) ||
				Helper.Input.IsDown(SButton.LeftThumbstickUp) ||
				Helper.Input.IsDown(SButton.LeftThumbstickDown) ||
				// DPad
				Helper.Input.IsDown(SButton.DPadLeft) ||
				Helper.Input.IsDown(SButton.DPadRight) ||
				Helper.Input.IsDown(SButton.DPadUp) ||
				Helper.Input.IsDown(SButton.DPadDown) ||
				// Keyboard
				Game1.options.moveUpButton.Any(button => Helper.Input.IsDown((SButton)button.key)) ||
				Game1.options.moveDownButton.Any(button => Helper.Input.IsDown((SButton)button.key)) ||
				Game1.options.moveLeftButton.Any(button => Helper.Input.IsDown((SButton)button.key)) ||
				Game1.options.moveRightButton.Any(button => Helper.Input.IsDown((SButton)button.key));

			if (isChangingDirection)
				return;

			int direction = Game1.player.facingDirection.Value;

			if (Config.StopOnCollision && IsColliding(direction))
				return;

			var buttons = direction switch
			{
				0 => Game1.options.moveUpButton,
				1 => Game1.options.moveRightButton,
				2 => Game1.options.moveDownButton,
				3 => Game1.options.moveLeftButton,
				_ => null
			};

			if (buttons != null && buttons.Length > 0)
			{
				SButton key = (SButton)buttons[0].key;
				Helper.Input.Press(key);
			}
		}
	}
}