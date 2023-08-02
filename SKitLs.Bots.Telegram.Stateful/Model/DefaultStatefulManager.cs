using SKitLs.Bots.Telegram.Core.Exceptions.Inexternal;
using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Model.Building;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Stateful.Prototype;
using System.Collections.ObjectModel;

namespace SKitLs.Bots.Telegram.Stateful.Model
{
    /// <summary>
    /// Default realization of <see cref="IStatefulActionManager{TUpdate}"/>. Provides sectioned architecture
    /// with deep iteration searcher and one-of-many
    /// <see cref="IBotAction{TUpdate}.ShouldBeExecutedOn(TUpdate)"/> selector.
    /// </summary>
    /// <typeparam name="TUpdate">Specific casted update that this manager should work with.</typeparam>
    public class DefaultStatefulManager<TUpdate> : IStatefulActionManager<TUpdate>, IOwnerCompilable where TUpdate : ICastedUpdate, ISignedUpdate
    {
        /// <summary>
        /// Name, used for simplifying debugging process.
        /// </summary>
        public string? DebugName { get; set; }

        private BotManager? _owner;
        /// <summary>
        /// Instance's owner.
        /// </summary>
        public BotManager Owner
        {
            get => _owner ?? throw new NullOwnerException(this);
            set => _owner = value;
        }
        /// <summary>
        /// Specified method that raised during reflective <see cref="IOwnerCompilable.ReflectiveCompile(object, BotManager)"/> compilation.
        /// Declare it to extend preset functionality.
        /// Invoked after <see cref="Owner"/> updating, but before recursive update.
        /// </summary>
        public Action<object, BotManager>? OnCompilation => null;

        /// <summary>
        /// Internal collection used for storing action sections. 
        /// </summary>
        public ICollection<IStateSection<TUpdate>> ActionSections { get; set; }
        /// <summary>
        /// State section that is defined as a default one.
        /// </summary>
        public IStateSection<TUpdate> DefaultStateSection => ActionSections
            .ToList()
            .Find(x => x.EnabledAny) ?? throw new Exception();
        /// <summary>
        /// Collects all <see cref="IBotAction"/>s declared in the class.
        /// </summary>
        /// <returns>Collected list of declared actions.</returns>
        public List<IBotAction> GetActionsContent()
        {
            var res = new List<IBotAction>();
            ActionSections.ToList().ForEach(x => res.AddRange(x.GetActionsContent()));
            return res;
        }

        /// <summary>
        /// Creates a new instance of <see cref="DefaultStatefulManager{TUpdate}"/> with specified data.
        /// </summary>
        /// <param name="debugName">Optional. Debug name.</param>
        public DefaultStatefulManager(string? debugName = null)
        {
            DebugName = debugName;
            ActionSections = new Collection<IStateSection<TUpdate>>
            {
                new DefaultStateSection<TUpdate>()
            };
        }

        /// <summary>
        /// Safely adds new action to internal storage.
        /// Verifies it is unique via <see cref="IBotAction.ActionId"/>.
        /// </summary>
        /// <param name="action">Action to be stored.</param>
        public void AddSafely(IBotAction<TUpdate> action) => DefaultStateSection.AddSafely(action);
        /// <summary>
        /// Safely adds range of actions to internal storage.
        /// Verifies they are unique via <see cref="IBotAction.ActionId"/>.
        /// </summary>
        /// <param name="actions">Actions to be stored.</param>
        public void AddRangeSafely(ICollection<IBotAction<TUpdate>> actions) => actions
            .ToList()
            .ForEach(sec => AddSafely(sec));

        /// <summary>
        /// Safely adds new state section.
        /// </summary>
        /// <param name="section">Section to add.</param>
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
                    .Where(x => x.EnabledStates!.Intersect(section.EnabledStates!).Any())
                    .SelectMany(x => x.GetActionsContent());
                if (intersectedStates.Intersect(section.GetActionsContent()).Any())
                    throw new Exception();

                ActionSections.Add(section);
            }

            // Данная проверка необходима для следующего кейса:
            //  Если существующая секция доступна для состояний (0, 1, 2) и содержит условный /start
            //  То /start из новой секции (0, 1) будет просто напросто игнорироваться
        }
        /// <summary>
        /// Safely adds a range of state section.
        /// </summary>
        /// <param name="sections">Sections to add.</param>
        public void AddSectionsRangeSafely(ICollection<IStateSection<TUpdate>> sections) => sections
            .ToList()
            .ForEach(s => AddSectionSafely(s));

        /// <summary>
        /// Applies and integrates custom class that supports <see cref="IStatefulIntegratable{TUpdate}"/>.
        /// </summary>
        /// <param name="integration">An item to be integrated.</param>
        [Obsolete("Will be removed in future versions. Use IApplicant instead.", true)]
        public void Apply(IStatefulIntegratable<TUpdate> integration) => AddSectionsRangeSafely(integration.GetSectionsList());

        /// <summary>
        /// Manages incoming update, delegating it to one of a stored actions.
        /// </summary>
        /// <param name="update">Update to be handled.</param>
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
                {
                    await action.Action(update);
                    break;
                }
        }

        /// <summary>
        /// Returns a string that represents current object.
        /// </summary>
        /// <returns>A string that represents current object.</returns>
        public override string? ToString() => DebugName ?? base.ToString();
    }
}