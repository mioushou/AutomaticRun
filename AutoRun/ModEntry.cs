using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace AutoRun
{
    /// <summary>The mod entry point.</summary>
    internal sealed class ModEntry : Mod
    {
        private ModConfig Config = null!;
        private bool IsAutoRunning;
        
        public override void Entry(IModHelper helper)
        {
            Config = helper.ReadConfig<ModConfig>();
            helper.Events.GameLoop.UpdateTicking += OnUpdateTicking;
        }

        private void OnUpdateTicking(object? sender, UpdateTickingEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            // Handle toggle
            if (Config.ToggleKey.JustPressed())
            {
                IsAutoRunning = !IsAutoRunning;
            }

            if (!IsAutoRunning)
                return;
            
            bool isChangingDirection =
                Game1.options.moveUpButton.Any(button => Helper.Input.IsDown((SButton)button.key)) ||
                Game1.options.moveDownButton.Any(button => Helper.Input.IsDown((SButton)button.key)) ||
                Game1.options.moveLeftButton.Any(button => Helper.Input.IsDown((SButton)button.key)) ||
                Game1.options.moveRightButton.Any(button => Helper.Input.IsDown((SButton)button.key));

            if (isChangingDirection)
                return;

            int direction = Game1.player.facingDirection.Value;
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