﻿using AmongUs.GameOptions;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using TOHE.Roles._Ghosts_.Impostor;
using TOHE.Roles.Crewmate;

namespace TOHE.Roles.Core.AssignManager;

public static class GhostRoleAssign
{
    public static Dictionary<byte, CustomRoles> GhostGetPreviousRole = [];
    private static Dictionary<CustomRoles, int> getCount;
    public static void GhostAssignPatch(PlayerControl player)
    {
        var getplrRole = player.GetCustomRole();
        if (GameStates.IsHideNSeek || getplrRole.IsGhostRole() || player.GetCustomSubRoles().Any(x => x.IsGhostRole()) || Options.CustomGhostRoleCounts.Count <= 0) return;
        if (getplrRole is CustomRoles.GM) return;
        GhostGetPreviousRole.Add(player.PlayerId, getplrRole);
        Logger.Warn("I WAS RUN", "GhostAssignPatch");

        List<CustomRoles> HauntedList = [];
        List<CustomRoles> RateHauntedList = [];

        List<CustomRoles> ImpHauntedList = [];
        List<CustomRoles> ImpRateHauntedList = [];

        CustomRoles ChosenRole = CustomRoles.NotAssigned;

        bool IsSetRole = false;

        var IsCrewmate = CustomRolesHelper.IsCrewmate(getplrRole);
        var IsImpostor = CustomRolesHelper.IsImpostor(getplrRole);
        var IsNeutral = CustomRolesHelper.IsNeutral(getplrRole);

        foreach (var ghostRole in Options.CustomGhostRoleCounts.Keys) if (ghostRole.GetMode() == 2)
            {
                if (CustomRolesHelper.IsCrewmate(ghostRole))
                {
                    if (HauntedList.Contains(ghostRole) && getCount[ghostRole] <= 0)
                        HauntedList.Remove(ghostRole);

                    if (HauntedList.Contains(ghostRole) || getCount[ghostRole] <= 0)
                        continue;

                    
                    HauntedList.Add(ghostRole); continue;
                }
                if (CustomRolesHelper.IsImpostor(ghostRole))
                {
                    if (ImpHauntedList.Contains(ghostRole) && getCount[ghostRole] <= 0)
                        ImpHauntedList.Remove(ghostRole);

                    if (ImpHauntedList.Contains(ghostRole) || getCount[ghostRole] <= 0)
                        continue;

                    
                    ImpHauntedList.Add(ghostRole);
                }
            }
        foreach (var ghostRole in Options.CustomGhostRoleCounts.Keys) if (ghostRole.GetMode() == 1)
            {
                if (CustomRolesHelper.IsCrewmate(ghostRole))
                {

                    if (RateHauntedList.Contains(ghostRole) && getCount[ghostRole] <= 0)
                        RateHauntedList.Remove(ghostRole);

                    if (RateHauntedList.Contains(ghostRole) || getCount[ghostRole] <= 0)
                        continue;

                    
                    RateHauntedList.Add(ghostRole); continue;
                }
                if (CustomRolesHelper.IsImpostor(ghostRole))
                {
                    if (ImpRateHauntedList.Contains(ghostRole) && getCount[ghostRole] <= 0)
                        ImpRateHauntedList.Remove(ghostRole);

                    if (ImpRateHauntedList.Contains(ghostRole) || getCount[ghostRole] <= 0)
                        continue;

                    
                    ImpRateHauntedList.Add(ghostRole);
                }
            }

        if (IsCrewmate)
        {
            if (HauntedList.Count > 0)
            {
                System.Random rnd = new System.Random();
                int randindx = rnd.Next(HauntedList.Count);
                ChosenRole = HauntedList[randindx];
                IsSetRole = true;

            }
            else if (RateHauntedList.Count > 0 && !IsSetRole)
            {
                System.Random rnd = new System.Random();
                int randindx = rnd.Next(RateHauntedList.Count);
                ChosenRole = RateHauntedList[randindx];

            }
            if (ChosenRole.IsGhostRole())
            {
                getCount[ChosenRole]--; // Only deduct count after it's been assigned.
                player.RpcSetRole(RoleTypes.GuardianAngel);
                player.RpcSetCustomRole(ChosenRole);
                player.AddPlayerId(ChosenRole);
            }
            return;
        }

        if (IsImpostor)
        {
            if (ImpHauntedList.Count > 0)
            {
                System.Random rnd = new System.Random();
                int randindx = rnd.Next(ImpHauntedList.Count);
                ChosenRole = ImpHauntedList[randindx];
                IsSetRole = true;

            }
            else if (ImpRateHauntedList.Count > 0 && !IsSetRole)
            {
                System.Random rnd = new System.Random();
                int randindx = rnd.Next(ImpRateHauntedList.Count);
                ChosenRole = ImpRateHauntedList[randindx];

            }
            if (ChosenRole.IsGhostRole())
            {
                getCount[ChosenRole]--; 
                player.RpcSetRole(RoleTypes.GuardianAngel);
                player.RpcSetCustomRole(ChosenRole);
                player.AddPlayerId(ChosenRole);
            }
            return;
        }

        if (IsNeutral)
        {
            return;
        }

    }
    public static void Init() 
    {
        getCount = []; // Remove oldcount
    }
    public static void Add()
    {
        Options.CustomGhostRoleCounts.Keys.Do(x 
            => getCount.Add(x, Options.CustomGhostRoleCounts[x].GetInt())); // Add new count Instance (Optionitem gets constantly refreshed)
    }
    public static void AddPlayerId(this PlayerControl target, CustomRoles GhostRole)
    {
        switch (GhostRole)
        {
            case CustomRoles.Retributionist:
                Retributionist.Add(target.PlayerId);
                break;
             case CustomRoles.Nemesis:
                Nemesis.Add(target.PlayerId);
                break;
        }
    }
}
