using SKitLs.Bots.Telegram.Core.Exceptions.Inexternal;
using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Model.Building;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Stateful.Exceptions.Inexternal;
using SKitLs.Bots.Telegram.Stateful.Prototype;
using System.Collections.ObjectModel;

namespace SKitLs.Bots.Telegram.Stateful.Model
{
    /// <summary>
    /// Default implementation of <see cref="IStatefulActionManager{TUpdate}"/>. 
    /// Provides a sectioned architecture with deep iteration search capabilities and a one-of-many
    /// <see cref="IBotAction{TUpdate}.ShouldBeExecutedOn(TUpdate)"/> selector.
    /// </summary>
    /// <typeparam name="TUpdate">The specific casted update that this manager should work with.</typeparam>
    public class StatefulActionManager<TUpdate> : IStatefulActionManager<TUpdate>, IOwnerCompilable where TUpdate : ICastedUpdate, ISignedUpdate
    {
        /// <inheritdoc/>
        public string? DebugName { get; set; }

        private BotManager? _owner;
        /// <inheritdoc/>
        public BotManager Owner
        {
            get => _owner ?? throw new NullOwnerException(this);
            set => _owner = value;
        }
        /// <inheritdoc/>
        public Action<object, BotManager>? OnCompilation => null;

        /// <summary>
        /// Determines whether <see cref="StatefulActionManager{TUpdate}"/> should break iterator after first matched action was executed.
        /// </summary>
        public bool OnlyOneAction { get; set; }

        /// <summary>
        /// An internal collection used for storing action sections. 
        /// </summary>
        public List<IStateSection<TUpdate>> ActionSections { get; private set; }

        /// <inheritdoc/>
        public IEnumerable<IStateSection<TUpdate>> GetActionSections() => ActionSections;

        private IStateSection<TUpdate>? _defaultSection;
        /// <inheritdoc/>
        public IStateSection<TUpdate> DefaultStateSection
        {
            get => _defaultSection ?? ActionSections
                .ToList()
                .Find(x => x.EnabledAny)
                ?? throw new NoDefaultSectionException(this);
            set => _defaultSection = value;
        }

        /// <inheritdoc/>
        public List<IBotAction> GetActionsContent()
        {
            var res = new List<IBotAction>();
            ActionSections.ToList().ForEach(x => res.AddRange(x.GetActionsContent()));
            return res;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatefulActionManager{TUpdate}"/> class with the specified parameters.
        /// </summary>
        /// <param name="debugName">Optional. Debug name.</param>
        public StatefulActionManager(string? debugName = null)
        {
            DebugName = debugName;
            ActionSections = new List<IStateSection<TUpdate>>
            {
                new DefaultStateSection<TUpdate>()
            };
        }

        /// <inheritdoc/>
        public void AddSafely(IBotAction<TUpdate> action) => DefaultStateSection.AddSafely(action);

        /// <inheritdoc/>
        public void AddRangeSafely(ICollection<IBotAction<TUpdate>> actions) => actions
            .ToList()
            .ForEach(sec => AddSafely(sec));

        /// <inheritdoc/>
        public void AddSectionSafely(IStateSection<TUpdate> section)
        {
            // Найти секции с тем же набором доступным состояний
            var existing = ActionSections.ToList().Find(x => x.Equals(section));

            // Если такие существует - безопасно добавить
            if (existing is not null) existing.MergeSafely(section);

            // Если таких нет, а данная секция доступна из любых состояний - сохранить её
            else if (section.EnabledAny) ActionSections.Add(section);

            // Иначе найти все секции, вызываемые в любых из доступных состояний и проверить,
            // не существуют ли в них уже акторы с той же базой
            else
            {
                var intersectedStates = ActionSections
                    .Where(x => !x.EnabledAny)
                    .Where(x => x.GetEnabledStates().Intersect(section.GetEnabledStates()).Any())
                    .SelectMany(x => x.GetActionsContent());
                if (intersectedStates.Intersect(section.GetActionsContent()).Any())
                    throw new Exception();

                ActionSections.Add(section);
            }

            // Данная проверка необходима для следующего кейса:
            // Если существующая секция доступна для состояний (0, 1, 2) и содержит условный /start
            // То /start из новой секции (0, 1) будет просто напросто игнорироваться
        }

        /// <inheritdoc/>
        public void AddSectionsRangeSafely(ICollection<IStateSection<TUpdate>> sections) => sections
            .ToList()
            .ForEach(s => AddSectionSafely(s));

        /// <inheritdoc/>
        public async Task ManageUpdateAsync(TUpdate update)
        {
            if (update.Sender is not IStatefulUser stateful)
                throw new NotStatefulException(this);

            var enabled = ActionSections
                .ToList()
                .FindAll(x => x.IsEnabledWith(stateful.State))
                .SelectMany(x => x);

            foreach (var action in enabled)
                if (action.ShouldBeExecutedOn(update))
                {
                    await action.Action(update);
                    if (OnlyOneAction)
                        break;
                }
        }

        /// <inheritdoc/>
        public override string? ToString() => DebugName ?? base.ToString();
    }
}