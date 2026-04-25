using Content.Shared._Sich.Hunter;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Components;
using Robust.Server.GameObjects;

namespace Content.Server._Sich.Hunter;

public sealed class HunterConsoleSystem : EntitySystem
{
    [Dependency] private readonly UserInterfaceSystem _ui = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;

    public override void Initialize()
    {
        base.Initialize();
        
        SubscribeLocalEvent<HunterCrewConsoleComponent, BoundUIOpenedEvent>(OnCrewOpened);
    }

    private void OnCrewOpened(EntityUid uid, HunterCrewConsoleComponent comp, BoundUIOpenedEvent args)
    {
        UpdateUI(uid, HunterCrewUiKey.Key);
    }

    private void UpdateUI(EntityUid uid, Enum key)
    {
        var hunters = new List<HunterTerminalData>();
        var query = EntityQueryEnumerator<HunterComponent, MetaDataComponent, MobStateComponent, TransformComponent>();
        
        while (query.MoveNext(out var hunterUid, out var hunterComp, out var meta, out var mobState, out var xform))
        {
            var status = mobState.CurrentState switch
            {
                MobState.Alive => "Живий",
                MobState.Critical => "Критичний",
                MobState.Dead => "Мертвий",
                _ => "Невідомо"
            };

            var position = "Deep Space";
            if (xform.GridUid is { } gridUid)
            {
                position = Name(gridUid);
            }
            else if (xform.MapUid is { } mapUid)
            {
                position = Name(mapUid);
            }
            
            hunters.Add(new HunterTerminalData(meta.EntityName, status, position));
        }

        _ui.SetUiState(uid, key, new HunterConsoleState(hunters));
    }
}
