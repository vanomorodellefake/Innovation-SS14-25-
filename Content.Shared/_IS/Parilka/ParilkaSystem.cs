using Content.Shared.Stacks;
using Content.Shared.Interaction;
using Content.Shared.Tag;
using Robust.Shared.Prototypes;
using Content.Shared.Item.ItemToggle.Components;
using Content.Shared.IgnitionSource;
using Content.Shared.Temperature;
using System;





// буду записывать че делаю чтоб не забывать после перерывов сука

namespace Content.Shared._IS.Parilka;

public sealed class ParilkaSystem : EntitySystem
{
            [Dependency] private readonly TagSystem _tag = default!; // тут зависимости систем тэгов и стаков предметов
            [Dependency] private readonly SharedStackSystem _stack = default!;



    public override void Initialize()
    {
        SubscribeLocalEvent<ParilkaComponent, InteractUsingEvent>(OnInteractUsing); // тут я подписался на ивент клика по печке
    }

    private bool IsHot(EntityUid used) //проверка на предмет который может поджечь печку
    {
        var ev = new IsHotEvent();
        RaiseLocalEvent(used, ev);
        return ev.IsHot;
    }

    public override void Update(float frameTime) // симуляция времени
    {
        var query = EntityQueryEnumerator<ParilkaComponent>(); // смотрим на все парилки на карте ебать
        while (query.MoveNext(out var uid, out var comp))
        {
            if (!comp.Active)
                continue; // если не активна (символ ! в начале означает "не") ничего не делать

            comp.BurnTime += frameTime;
            if (comp.BurnTime < 1)
                continue;

            comp.BurnTime -= 1;

            if (comp.Fuel <= 0) // топливо закончилось печка гаснет
            {
                comp.Active = false;
                continue;
            }

            comp.Fuel = Math.Max(0, comp.Fuel - (int)comp.UsePerSecond); //сжигание топлива
            comp.Temperature += Math.Min(comp.Temperature + comp.HeatPerSecond, 500); // нагрев

            if (comp.Temperature >= 100f && comp.Water > 0) //удаление воды и спавн пара
            {
                comp.SteamTimer += frameTime;

                if (comp.SteamTimer < 2f)
                    continue;

                comp.SteamTimer = 0f;

                comp.Water = Math.Max(0, comp.Water - 1);
                RaiseLocalEvent(uid, new ParilkaSteamEvent(5f));
            }




        }
    }


    private void OnInteractUsing(EntityUid uid, ParilkaComponent component, InteractUsingEvent args)  // проверка на интеракт игрока
    {
        var used = args.Used;
        if (_tag.HasTag(used, "FuelForFire")) //добавил тек доскам для того чтобы их можно было вставить. Возможно есть более простой способ
        {
            if (component.Fuel >= component.MaxFuel)
                return;

            if (!EntityManager.TryGetComponent<StackComponent>(used, out var stack))
                return;
            if (!_stack.Use(used, 1, stack!))
                return;
            component.Fuel = Math.Min(component.Fuel + 1, component.MaxFuel);
            args.Handled = true;
            return;
        }






        if (!IsHot(used)) // если предмет не горячий её нельзя поджечь
            return;

        if (component.Fuel <= 0) // нет топлива нет горения
            return;
        component.Active = true; // печка зажглась
        args.Handled = true;
    }
}
