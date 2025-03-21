using BepInEx;
using UnityEngine;
using SlugBase.Features;
using static SlugBase.Features.FeatureTypes;
using System.Collections.Generic;
using System;

namespace The_Infectious
{
    [BepInPlugin(MOD_ID, "The Infectious", "0.1.0")]
    class Plugin : BaseUnityPlugin
    {
        private const string MOD_ID = "the-infectious";

        // Parameters
        private readonly static Dictionary<Player, float> quarterFoodTracker = new();

        // Duping items
        //private readonly float requiredHoldTime = 1.5f;
        //private float grabHoldTime = 0f;

        public static readonly PlayerFeature<float> SuperJump = PlayerFloat("theinfectious/super_jump");
        private bool test = true;
        private bool test2 = false;

        // Add hooks
        public void OnEnable()
        {
            On.RainWorld.OnModsInit += Extras.WrapInit(LoadResources);

            // Put your custom hooks here!
            On.Player.Jump += Player_Jump;
            //On.RainWorldGame.
            On.Player.Update += Player_Update;
            On.HUD.FoodMeter.UpdateShowCount += FoodMeter_UpdateShowCount;
            On.HUD.FoodMeter.QuarterPipShower.Update += QuarterPipShower_Update;
            On.HUD.FoodMeter.MeterCircle.Update += MeterCircle_Update;
            //On.HUD.FoodMeter.GameUpdate += FoodMeter_GameUpdate;
            //On.HUD.FoodMeter.MeterCircle.EatFade += MeterCircle_EatFade;
            //On.HUD.FoodMeter.MeterCircle.FoodPlop += MeterCircle_FoodPlop;
        }

        //private void FoodMeter_GameUpdate(On.HUD.FoodMeter.orig_GameUpdate orig, FoodMeter self)
        //{
        //    self.quarterPipShower.quarterPipDelay = 45;
        //    orig(self);
        //}

        //private void MeterCircle_FoodPlop(On.HUD.FoodMeter.MeterCircle.orig_FoodPlop orig, HUD.FoodMeter.MeterCircle self)
        //{
        //    self.foodPlopped = true;
        //    self.rads[1, 1] += 2f;
        //    self.foodPlopDelay = 16;
        //    self.meter.hud.PlaySound(SoundID.HUD_Food_Meter_Fill_Plop_A);
        //}

        //private void MeterCircle_EatFade(On.HUD.FoodMeter.MeterCircle.orig_EatFade orig, HUD.FoodMeter.MeterCircle self)
        //{
        //    self.eatCounter = 50;
        //    self.eaten = true;
        //    self.rads[0, 0] = self.circles[0].snapRad + 1.5f;
        //    self.rads[0, 1] += 0.6f;
        //    self.meter.hud.PlaySound(SoundID.HUD_Food_Meter_Deplete_Plop_A);
        //}

        private void MeterCircle_Update(On.HUD.FoodMeter.MeterCircle.orig_Update orig, HUD.FoodMeter.MeterCircle self)
        {
            orig(self);
            //self.lastSlowXAdd = self.slowXAdd;
            //self.slowXAdd = Custom.LerpAndTick(self.slowXAdd, self.XAdd(1f), 0.06f, 2f);
            //for (int i = 0; i < self.rads.GetLength(0); i++)
            //{
            //    if (!self.plopped)
            //    {
            //        self.rads[i, 0] = self.circles[i].snapRad / 2f;
            //        self.rads[i, 1] = 0f;
            //    }
            //    else
            //    {
            //        self.rads[i, 0] += self.rads[i, 1];
            //        self.rads[i, 1] *= ((self.rads[i, 0] < self.circles[i].snapRad) ? 0.8f : 0.95f);
            //        self.rads[i, 1] += (self.circles[i].snapRad - self.rads[i, 0]) * 0.2f;
            //        self.rads[i, 0] = Custom.LerpAndTick(self.rads[i, 0], self.circles[i].snapRad, 0.0001f, 0.2f);
            //    }
            //}
            //float num = 1f;
            //self.circles[0].color = 0;
            //if (self.meter.IsPupFoodMeter)
            //{
            //    self.circles[1].color = 0;
            //}
            //if (self.meter.IsPupFoodMeter && self.meter.abstractPup != null)
            //{
            //    if (self.meter.timeCounter % 40 > 20)
            //    {
            //        self.rads[0, 0] *= 0.96f;
            //        self.rads[0, 1] *= 0.96f;
            //        self.rads[1, 0] *= 0.96f;
            //        self.rads[1, 1] *= 0.96f;
            //    }
            //    num = 0.65f + 0.35f * Mathf.Sin((float)self.meter.timeCounter / 20f * 3.1415927f);
            //    self.circles[1].fade = self.meter.fade * num;
            //}
            //else if (self.meter.IsPupFoodMeter && self.meter.hud.owner.GetOwnerType() == HUD.HUD.OwnerType.Player && self.meter.PupInDanger)
            //{
            //    if (self.meter.timeCounter % 20 > 10)
            //    {
            //        self.rads[0, 0] *= 0.96f;
            //        self.circles[0].color = 1;
            //        self.rads[0, 1] *= 0.96f;
            //        self.circles[1].color = 1;
            //    }
            //    num = 0.65f + 0.35f * Mathf.Sin((float)self.meter.timeCounter / 20f * 3.1415927f * 2f);
            //}
            //else if ((!self.meter.IsPupFoodMeter || !self.meter.PupInDanger) && (!self.meter.IsPupFoodMeter || !self.meter.PupHasDied) && self.number < self.meter.ShowSurvivalLimit && self.number >= self.meter.showCount && !self.eaten && ((self.meter.hud.owner.GetOwnerType() == HUD.HUD.OwnerType.Player && (self.meter.hud.owner as Player).room != null && (self.meter.hud.owner as Player).room.abstractRoom.shelter && !(self.meter.hud.owner as Player).room.world.brokenShelters[(self.meter.hud.owner as Player).room.abstractRoom.shelterIndex] && !(self.meter.hud.owner as Player).stillInStartShelter && !(self.meter.hud.owner as Player).readyForWin) || (self.meter.hud.owner.GetOwnerType() == HUD.HUD.OwnerType.SleepScreen && (self.meter.hud.owner as Menu.SleepAndDeathScreen).goalMalnourished && self.meter.sleepScreenPhase < 3)))
            //{
            //    if (self.meter.timeCounter % 20 > 10)
            //    {
            //        self.rads[0, 0] *= 0.96f;
            //        self.circles[0].color = 1;
            //    }
            //    num = 0.65f + 0.35f * Mathf.Sin((float)self.meter.timeCounter / 20f * 3.1415927f * 2f);
            //}
            //for (int j = 0; j < self.circles.Length; j++)
            //{
            //    self.circles[j].Update();
            //    self.circles[j].pos = self.DrawPos(1f);
            //    self.circles[j].rad = self.rads[j, 0];
            //}
            //self.circles[0].fade = (self.plopped ? (self.meter.fade * num) : 0f);
            //if (self.eaten)
            //{
            //    self.eatCounter--;
            //    if (self.eatCounter < 1)
            //    {
            //        self.foodPlopped = false;
            //        self.eaten = false;
            //    }
            //    self.circles[1].fade = Mathf.Pow(Mathf.InverseLerp(0f, 35f, (float)self.eatCounter), 1.2f) * self.meter.fade;
            //    if (self.eatCounter > 30)
            //    {
            //        self.rads[0, 0] = Custom.LerpAndTick(self.rads[0, 0], 13f + 0.5f * Mathf.Sin(Mathf.InverseLerp(30f, 50f, (float)self.eatCounter) * 3.1415927f), 0.02f, 1.5f + 0.5f * Mathf.InverseLerp(30f, 50f, (float)self.eatCounter));
            //        self.rads[0, 1] *= 0f;
            //    }
            //    else if (self.eatCounter == 29)
            //    {
            //        self.rads[0, 1] = -1.5f;
            //        self.meter.hud.PlaySound(SoundID.HUD_Food_Meter_Deplete_Plop_B);
            //    }
            //    self.rads[1, 0] = Custom.LerpMap((float)self.eatCounter, 40f, 0f, self.circles[1].snapRad, self.circles[1].snapRad / 2f);
            //}
            //else
            //{
            //    if (self.meter.refuseCounter > 0)
            //    {
            //        self.rads[0, 0] += Mathf.Lerp(-1f, 1f, UnityEngine.Random.value) * 0.5f;
            //        self.rads[1, 0] += Mathf.Lerp(-0.25f, 1f, UnityEngine.Random.value);
            //        self.rads[1, 1] += UnityEngine.Random.value * 0.4f;
            //        if (self.rads[1, 0] + 1f > self.rads[0, 0])
            //        {
            //            self.rads[0, 0] = self.rads[1, 0] + 1f;
            //            self.rads[0, 1] += 0.2f + UnityEngine.Random.value * 0.4f;
            //        }
            //    }
            //    self.circles[1].fade = ((self.plopped && self.foodPlopped) ? self.meter.fade : 0f);
            //}
            //if (self.foodPlopDelay > 0)
            //{
            //    self.foodPlopDelay--;
            //    if (self.foodPlopDelay == 12)
            //    {
            //        self.rads[0, 0] = self.circles[0].snapRad + 2f;
            //        self.rads[0, 1] += 1f;
            //        self.meter.hud.PlaySound(SoundID.HUD_Food_Meter_Fill_Plop_B);
            //    }
            //    else if (self.foodPlopDelay == 0)
            //    {
            //        self.meter.hud.fadeCircles.Add(new FadeCircle(self.meter.hud, 10f, 10f, 0.82f, 30f, 4f, self.DrawPos(1f), self.meter.fContainer));
            //        self.meter.hud.PlaySound(SoundID.HUD_Food_Meter_Fill_Fade_Circle);
            //    }
            //}
            //if (self.meter.lastFade == 0f)
            //{
            //    self.plopped = false;
            //}
            //self.circles[0].visible = self.plopped;
            //self.circles[1].visible = (self.plopped && self.foodPlopped);
        }

        // Load any resources, such as sprites or sounds
        private void LoadResources(RainWorld rainWorld)
        {
            test = true;
            Logger.LogDebug($"Loading resources... | {DateTime.Now:HH:mm:ss:ff}");
        }

        private void Player_Jump(On.Player.orig_Jump orig, Player self)
        {
            orig(self);
            Logger.LogDebug($"{self.slugcatStats.name.value}");
            if (self.slugcatStats.name.value != "The Infectious" && SuperJump.TryGet(self, out var power))
            {
                self.jumpBoost *= 1f + power;
            }
        }

        // Implement Duping
        private void Player_Update(On.Player.orig_Update orig, Player self, bool eu)
        {
            orig(self, eu); // Call original Update function

            // if (self.slugcatStats.name.value != "Duplicator") return;

            bool isHoldingGrab = self.input[0].pckp;
            _ = self.input[0].thrw;
            bool hasItem = self.grasps[0]?.grabbed != null;
            bool hasFreeHand = self.grasps[1] == null;

            // Log inputs and state
            // Debug.Log($"[Duplicator] Grab: {isHoldingGrab}, Throw: {isThrowing}, Holding Item: {hasItem}, Free Hand: {hasFreeHand}, SwallowCounter: {self.swallowAndRegurgitateCounter}");

            // Ensure player is holding an item and has a free hand
            if (hasItem && hasFreeHand)
            {
                if (isHoldingGrab)
                {
                    // grabHoldTime += Time.deltaTime;
                    // Debug.Log($"[Duplicator] Holding grab for {grabHoldTime:F2}s");

                    //if (grabHoldTime >= requiredHoldTime)
                    //{
                    Debug.Log("[Duplicator] Duplication triggered!");
                    test2 = true;
                    TryDuplicateItem(self);
                        // grabHoldTime = 0f; // Reset
                        // self.swallowAndRegurgitateCounter = 0; // Prevent swallowing
                    //}
                }
                else
                {
                    //grabHoldTime = 0f; // Reset if button is released
                }
            }
        }

        private void FoodMeter_UpdateShowCount(On.HUD.FoodMeter.orig_UpdateShowCount orig, HUD.FoodMeter self)
        {
            orig(self);
            //bool flag = true;
            //if (ModManager.MMF && self.showCount < self.circles.Count && self.circles[self.showCount].eaten)
            //{
            //    flag = false;
            //}
            //int num = self.hud.owner.CurrentFood;
            //_ = (self.hud.owner as Player).playerState.quarterFoodPoints;

            //if (self.showCount < num && flag)
            //{
            //    if (self.showCountDelay != 0)
            //    {
            //        self.showCountDelay--;
            //        return;
            //    }
            //    self.showCountDelay = 0;
            //    // self.eatCircleDelay = 1;
            //    if (self.showCount >= 0 && self.showCount < self.circles.Count && !self.circles[self.showCount].foodPlopped)
            //    {
            //        self.circles[self.showCount].FoodPlop();
            //    }
            //    self.showCount++;
            //    if (self.quarterPipShower != null)
            //    {
            //        self.quarterPipShower.Reset();
            //        return;
            //    }
            //}
            //else if (self.showCount > num)
            //{
            //    if (self.eatCircleDelay == 0)
            //    {
            //        self.eatCircleDelay = 0;
            //    }
            //    self.eatCircleDelay--;
            //    if (self.eatCircleDelay < 1)
            //    {
            //        self.circles[self.showCount - 1].EatFade();
            //        self.showCount--;
            //        self.eatCircleDelay = 0;
            //    }
            //}

            //// Update the quarter pips
            ////if (self.quarterPipShower != null)
            ////{
            ////    self.quarterPipShower.Update();
            ////}

            //Logger.LogDebug("FoodMeter_UpdateShowCount called.");
        }

        private void QuarterPipShower_Update(On.HUD.FoodMeter.QuarterPipShower.orig_Update orig, HUD.FoodMeter.QuarterPipShower self)
        {
            {
                if (test)
                {
                    self.quarterPipDelay = 45;
                    test = false;
                }
                self.lastQuarterPipSin = self.quarterPipSin;
                self.lastLightUp = self.lightUp;
                if (self.displayQuarterFood > 0)
                {
                    self.quarterPipSin += 1f;
                }
                self.lightUp *= 0.95f;
                int currentQuarterFoodPoints = self.owner.IsPupFoodMeter ? (self.owner.abstractPup.state as MoreSlugcats.PlayerNPCState).quarterFoodPoints : (self.owner.hud.owner as Player).playerState.quarterFoodPoints;
                Logger.LogDebug($"currentQuarterFoodPoints: {currentQuarterFoodPoints} | displayQuarterFood: {self.displayQuarterFood}");
                if (currentQuarterFoodPoints > self.displayQuarterFood)
                {
                    Logger.LogDebug("ran 1");
                    self.owner.visibleCounter = 80;
                    if (self.owner.fade < 0.5f)
                    {
                        Logger.LogDebug("ran fade check");
                        self.quarterPipDelay = 20;
                        if (currentQuarterFoodPoints == 3 && self.displayQuarterFood == 0) self.quarterPipDelay = 45;
                        return;
                    }
                    if (self.quarterPipDelay > 0)
                    {
                        self.quarterPipDelay--;

                        return;
                    }

                    self.quarterPipDelay = 20;
                    self.displayQuarterFood = currentQuarterFoodPoints;
                    self.lightUp = 1f;
                    if (self.owner.showCount < self.owner.circles.Count)
                    {
                        if (test2 == false)
                        {
                            self.owner.circles[self.owner.showCount].QuarterCirclePlop();
                            return;
                        }
                        test2 = false;
                    }
                }
                else if (currentQuarterFoodPoints < self.displayQuarterFood)
                {
                    Logger.LogDebug("ran 2");
                    self.owner.visibleCounter = 80;
                    if (self.owner.fade < 0.5f)
                    {
                        self.quarterPipDelay = 20;
                        if (currentQuarterFoodPoints == 0) self.quarterPipDelay = 40;
                        return;
                    }
                    if (self.quarterPipDelay > 0)
                    {
                        self.quarterPipDelay--;
                        return;
                    }
                    self.quarterPipDelay = 20;
                    if (currentQuarterFoodPoints == 0) self.quarterPipDelay = 40;
                    self.displayQuarterFood--;
                    self.lightUp = 1f;
                    if (self.owner.showCount > 0)
                    {
                        self.owner.circles[self.owner.showCount].EatFade();
                    }
                    else if (self.owner.showCount == 0)
                    {
                        self.owner.circles[0].EatFade();
                    }
                }
                //Logger.LogDebug("QuarterPipShower_Update called.");
            }
        }
        private void TryDuplicateItem(Player self)
        {
            // Ensure the player has enough food to duplicate the item (e.g., 1/4th of a food pip)
            if (self.playerState.foodInStomach < 1 && self.playerState.quarterFoodPoints < 1)
            {
                Logger.LogDebug("Not enough food to duplicate.");
                return;
            }

            // Consume 1/4th of a food pip
            Logger.LogDebug($"Before consumption: quarterFoodPoints = {self.playerState.quarterFoodPoints}, foodInStomach = {self.playerState.foodInStomach}");

            if (self.playerState.quarterFoodPoints > 0)
            {
                self.playerState.quarterFoodPoints -= 1;
            }
            else
            {
                self.playerState.foodInStomach -= 1;
                self.playerState.quarterFoodPoints = 3;
            }

            Logger.LogDebug($"After consumption: quarterFoodPoints = {self.playerState.quarterFoodPoints}, foodInStomach = {self.playerState.foodInStomach}");
            Logger.LogDebug("Food consumed to duplicate item.");

            // Force HUD to update
            // UpdateFoodMeter(self);

            // Check that the player has an item in their grasp
            AbstractPhysicalObject originalItem = self.grasps[0]?.grabbed.abstractPhysicalObject;
            if (originalItem == null)
            {
                Logger.LogDebug("No item to duplicate.");
                return;
            }

            Logger.LogDebug("Item to duplicate found.");

            AbstractPhysicalObject newItem;

            // turn this if and else into a case and switch statement
            if (originalItem != null)
            {
                Logger.LogDebug(originalItem.type);

                switch (originalItem.type.ToString())
                {
                    case "Spear":
                        newItem = new AbstractSpear(
                            self.room.world,
                            null,
                            self.room.GetWorldCoordinate(self.mainBodyChunk.pos),
                            self.room.game.GetNewID(),
                            false
                        );
                        Logger.LogDebug("New spear created.");
                        break;
                    default:
                        newItem = new AbstractPhysicalObject(
                            self.room.world,
                            originalItem.type,
                            null,
                            self.room.GetWorldCoordinate(self.mainBodyChunk.pos),
                            self.room.game.GetNewID()
                        );
                        Logger.LogDebug("New object created.");
                        break;
                }



                if (newItem != null)
                {
                    // Add the new item to the world and the room
                    self.room.abstractRoom.AddEntity(newItem);

                    Logger.LogDebug("New item added to world.");

                    // Realize the object in the room to make it visible and active
                    newItem.RealizeInRoom();

                    Logger.LogDebug("New item created and added to room.");

                    // Check if the new item is a spear
                    if (false)
                    {
                    }
                    else
                    {
                        // If it's not a spear, check for a free hand to grab it
                        int freeHand = self.grasps[0] == null ? 0 : self.grasps[1] == null ? 1 : -1;
                        if (freeHand != -1)
                        {
                            self.SlugcatGrab(newItem.realizedObject, freeHand);
                            Logger.LogDebug("New item grabbed with free hand.");
                        }
                        else
                        {
                            // Otherwise, drop the new item in front of the player
                            newItem.realizedObject.firstChunk.HardSetPosition(self.mainBodyChunk.pos + new Vector2(10f, 0f)); // Slight offset
                            Logger.LogDebug("New item dropped in front of the player.");
                        }
                    }
                }
                else
                {
                    Logger.LogDebug("Failed to create a new item.");
                }
            }
        }

        private void UpdateFoodMeter(Player player)
        {
            if (player.room != null && player.room.game != null && player.room.game.cameras != null)
            {
                foreach (var camera in player.room.game.cameras)
                {
                    if (camera.hud != null && camera.hud.foodMeter != null)
                    {
                        camera.hud.foodMeter.quarterPipShower?.Update();
                    }
                }
            }
            }
    }
}