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
    }
}