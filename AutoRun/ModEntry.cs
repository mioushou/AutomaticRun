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
        
        public override void Entry(IModHelper helper)
        {
            Config = helper.ReadConfig<ModConfig>();
            helper.Events.Input.ButtonPressed += OnToggle;
            helper.Events.GameLoop.UpdateTicked += OnUpdateTicked;
        }


        
        private void OnToggle(object? sender, ButtonPressedEventArgs e)
        {
            // ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
                return;
            
            if (!Config.ToggleKey.JustPressed())
                return;

            Monitor.Log("AutoRun key pressed !", LogLevel.Debug);
        }

        private void OnUpdateTicked(object? sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;
            
            // For testing, to avoid log on every tick
            if(!e.IsMultipleOf(30))
                return;

            var direction = Game1.player.facingDirection.Value switch
            {
                0 => "up",
                1 => "right",
                2 => "down",
                3 => "left",
                _ => "unknown"
            };
            
            Monitor.Log($"Player is facing {direction}", LogLevel.Debug);
        }
    }
}