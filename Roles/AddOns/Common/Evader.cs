using static TOHE.Options;

namespace TOHE.Roles.AddOns.Common;

public static class Evader
{
    private const int Id = 28500;

    public static OptionItem ChanceToEvadeVote;
    public static OptionItem ImpCanBeEvader;
    public static OptionItem CrewCanBeEvader;
    public static OptionItem NeutralCanBeEvader;


    private static Dictionary<byte, bool> Evade;
        
    
    public static void SetupCustomOptions()
    {
        SetupAdtRoleOptions(Id, CustomRoles.Evader, canSetNum: true);
        ChanceToEvadeVote = IntegerOptionItem.Create(Id + 13, "ChanceToEvadeVote", new(0, 100, 5), 50, TabGroup.Addons, false).SetParent(CustomRoleSpawnChances[CustomRoles.Evader])
            .SetValueFormat(OptionFormat.Percent);
        ImpCanBeEvader = BooleanOptionItem.Create(Id + 10, "ImpCanBeEvader", true, TabGroup.Addons, false).SetParent(CustomRoleSpawnChances[CustomRoles.Evader]);
        CrewCanBeEvader = BooleanOptionItem.Create(Id + 11, "CrewCanBeEvader", true, TabGroup.Addons, false).SetParent(CustomRoleSpawnChances[CustomRoles.Evader]);
        NeutralCanBeEvader = BooleanOptionItem.Create(Id + 12, "NeutralCanBeEvader", true, TabGroup.Addons, false).SetParent(CustomRoleSpawnChances[CustomRoles.Evader]);
    }
    public static void Init()
    {
        Evade = [];
    }

    private static void EvadeChance()
    {
        var rd = IRandom.Instance;
        if (rd.Next(0, 101) < ChanceToEvadeVote.GetInt());
        {
        //Code suggests there should be something here
        }
    }

    public static void CheckRealVotes(PlayerControl target, ref int VoteNum)

    {
        EvadeChance();
        {
            if (target.Is(CustomRoles.Evader))
            {
                VoteNum = 0;
            }
        }
    }
}
