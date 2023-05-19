using SKitLs.Bots.Telegram.Core.Exceptions;
using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Model.Builders;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Management.Integration;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Stateful.Prototype;
using System.Collections.ObjectModel;

namespace SKitLs.Bots.Telegram.Stateful.Model
{
    public class DefaultStatefulManager<TUpdate> : IStatefulActionManager<TUpdate>, IOwnerCompilable where TUpdate : ICastedUpdate, ISignedUpdate
    {
        public string? DebugName { get; set; }

        private BotManager? _owner;
        public BotManager Owner
        {
            get => _owner ?? throw new NullOwnerException();
            set => _owner = value;
        }
        public Action<object, BotManager>? OnCompilation => null;

        public IList<IUserState> DeterminedStates => ((IStatefulActionManager<TUpdate>)this).DeterminedStates;
        public ICollection<IStateSection<TUpdate>> ActionSections { get; set; }
        public IStateSection<TUpdate> DefaultStateSection => ActionSections
            .ToList()
            .Find(x => x.EnabledAny) ?? throw new Exception();

        public DefaultStatefulManager(string? debugName)
        {
            DebugName = debugName;
            ActionSections = new Collection<IStateSection<TUpdate>>();
        }

        public void AddSafely(IBotAction<TUpdate> action) => DefaultStateSection.AddSafely(action);
        public void AddRangeSafely(ICollection<IBotAction<TUpdate>> actions) => actions
            .ToList()
            .ForEach(sec => AddSafely(sec));
        public void Apply(IIntegratable<TUpdate> actions) => ((IStatefulActionManager<TUpdate>)this).Apply(actions);

        public void AddSectionSafely(IStateSection<TUpdate> section)
        {
            // Найти секции с тем же набором доступным состояний
            var existing = ActionSections.ToList().Find(x => x.Equals(section));

            // Если такие существует - безопасно добавить
            if (existing is not null) existing.Apply(section);

            // Если таких нет, а данная секция доступна из любых состояний - сохранить её
            else if (section.EnabledAny) ActionSections.Add(section);

            // Иначе найти все секции, вызываемые в любых из доступных состояний и проверить,
            // не существуют ли в них уже акторы с той же базой
            else
            {
                var intersectedStates = ActionSections
                    .Where(x => !x.EnabledAny)
                    .Where(x => x.EnabledStates!.Intersect(section.EnabledStates!).Any())
                    .SelectMany(x => x.GetActionsList());
                if (intersectedStates.Intersect(section.GetActionsList()).Any())
                    throw new Exception();

                ActionSections.Add(section);
            }

            // Данная проверка необходима для следующего кейса:
            //  Если существующая секция доступна для состояний (0, 1, 2) и содержит условный /start
            //  То /start из новой секции (0, 1) будет просто напросто игнорироваться
        }
        public void AddSectionsRangeSafely(ICollection<IStateSection<TUpdate>> sections) => sections
            .ToList()
            .ForEach(s => AddSectionSafely(s));
        public void Apply(IStatefulIntegratable<TUpdate> integrations) => AddSectionsRangeSafely(integrations.GetSectionsList());
        
        public async Task ManageUpdateAsync(TUpdate update)
        {
            // TODO
            if (update.Sender is not IStatefulUser stateful)
                throw new Exception();

            var enabled = ActionSections
                .ToList()
                .FindAll(x => x.IsEnabledWith(stateful.State))
                .SelectMany(x => x);

            // TODO
            foreach (var action in enabled)
                if (action.ShouldBeExecutedOn(update))
                    await action.Action(action, update);
        }

        public override string? ToString() => DebugName ?? base.ToString();
    }
}