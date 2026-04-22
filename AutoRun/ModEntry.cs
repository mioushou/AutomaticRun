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
        private bool IsAutoRunning = false;
        
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

            IsAutoRunning = !IsAutoRunning;
            string message = IsAutoRunning ? "[AutoRun]: activated" : "[AutoRun]: deactivated";
            Monitor.Log(message, LogLevel.Debug);
        }

        private void OnUpdateTicked(object? sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady || !IsAutoRunning)
                return;
 
            Helper.Input.Press(SButton.Q);
        }
    }
}