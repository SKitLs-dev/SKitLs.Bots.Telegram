using SKitLs.Bots.Telegram.AdvancedMessages.Model.Menus;
using SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages;
using SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Model;
using SKitLs.Bots.Telegram.BotProcesses.Model.DefaultProcesses.PartialInput;
using SKitLs.Bots.Telegram.Core.Model.Building;
using SKitLs.Bots.Telegram.Core.Model.DelieverySystem.Protoype;
using SKitLs.Bots.Telegram.Core.Model.Management;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Core.Prototypes;
using SKitLs.Bots.Telegram.DataBases.Model;
using SKitLs.Bots.Telegram.DataBases.Model.Args;
using SKitLs.Bots.Telegram.DataBases.Prototype;
using SKitLs.Bots.Telegram.DataBases.Prototype.Attributes;
using SKitLs.Bots.Telegram.PageNavs;
using SKitLs.Bots.Telegram.PageNavs.Model;
using SKitLs.Bots.Telegram.PageNavs.Prototype;
using SKitLs.Bots.Telegram.Stateful.Prototype;
using System.Net.Sockets;
using System.Reflection;
using System.Text.RegularExpressions;

namespace SKitLs.Bots.Telegram.DataBases
{
    /// <summary>
    /// A service that provides regular data regularation.
    /// Contains a reflective representation of storaged data via <see cref="IBotDataSet"/>
    /// </summary>
    public interface IDataManager : IOwnerCompilable, IApplicant<IActionManager<SignedCallbackUpdate>>, IApplicant<IStatefulActionManager<SignedMessageTextUpdate>>, IApplicant<IMenuManager>
    {
        /// <summary>
        /// The id used for main dataset that storages all other datasets.
        /// </summary>
        public string SourceSetId { get; }
        /// <summary>
        /// The main dataset that storages all other datasets.
        /// </summary>
        public BotDataSet<IBotDataSet> SourceSet { get; }

        /// <summary>
        /// Adds new dataset to the manager.
        /// </summary>
        /// <param name="dataSet">Datadet to add</param>
        public Task Add(IBotDataSet dataSet);
        /// <summary>
        /// Gets storaged dataset by its id.
        /// </summary>
        /// <param name="setId">Dataset's id</param>
        /// <returns>Storaged dataset.</returns>
        public IBotDataSet GetSet(long setId);
        /// <summary>
        /// Gets storaged dataset by its id.
        /// </summary>
        /// <param name="setNameId">Dataset's id</param>
        /// <returns>Storaged dataset.</returns>
        public IBotDataSet GetSet(string setNameId);
        /// <summary>
        /// Gets storaged dataset by a type of the collected data.
        /// </summary>
        /// <param name="setType">Dataset's collected data type</param>
        /// <returns>Storaged dataset.</returns>
        public IBotDataSet GetSet(Type setType);
        /// <summary>
        /// Gets casted storaged dataset by a type of the collected data.
        /// </summary>
        /// <typeparam name="T">Dataset's collected data type</typeparam>
        /// <returns>Storaged dataset.</returns>
        public IBotDataSet<T> GetSet<T>() where T : class, IBotDisplayable;
        /// <summary>
        /// Gets the casted list of all collected data that is assinable from the type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Datasets' types should be assinable from this type</typeparam>
        /// <returns>Merged list of all casted data.</returns>
        public List<T> GetMergedData<T>();

        /// <summary>
        /// The main page of the dataset manager, that should be mounted to one of the branches of <see cref="IMenuManager"/>
        /// used in your project. See summary's example.
        /// </summary>
        /// <returns>The main page of the data manager.</returns>
        /// <example>
        /// var mainMenu = new PageNavMenu();
        /// var mainPage = new StaticPage(...)
        /// {
        ///     Menu = mainMenu,
        /// };
        /// var dataControlPage = _dm.GetRootPage();
        /// mainMenu.PathTo(dataControlPage);
        /// </example>
        public IBotPage GetRootPage();

        /// <summary>
        /// Default callback that opens datasets pages, including <see cref="GetRootPage"/>.
        /// </summary>
        public BotArgedCallback<PaginationInfo> OpenCallback { get; }
        /// <summary>
        /// Default callback that provides 
        /// </summary>
        public BotArgedCallback<PaginationInfo> AddCallback { get; }
        /// <summary>
        /// Default callback that
        /// </summary>
        public BotArgedCallback<ObjInfoArg> OpenObjCallback { get; }
        /// <summary>
        /// Default callback that
        /// </summary>
        public BotArgedCallback<ObjInfoArg> EditCallback { get; }
        /// <summary>
        /// Default callback that
        /// </summary>
        public BotArgedCallback<ObjInfoArg> RemoveCallback { get; }

        public static List<InputSubProcess<T>> GetSubInputs<T>(T @object, string terminationalKey, int detalizationId = 0) where T : IBotPartialInput
        {
            var result = new List<InputSubProcess<T>>();

            var props = typeof(T)
                .GetProperties()
                .Where(x => x.GetCustomAttribute<BotInputPropAttribute>() is not null)
                .Where(x => x.GetCustomAttribute<BotInputPropAttribute>()!.DetalizationId == detalizationId)
                .OrderBy(x => x.GetCustomAttribute<BotInputPropAttribute>()!.Order)
                .ToList();

            foreach (var prop in props)
            {
                var attr = prop.GetCustomAttribute<BotInputPropAttribute>()!;

                if (attr is PartialInputAttribute partial)
                {
                    var parserMethod = typeof(T).GetMethod(partial.ParserMethod ?? string.Empty);
                    if (parserMethod is null && prop.PropertyType != typeof(string)) throw new Exception();
                    var parserDelegate = parserMethod is null
                        ? null
                        : (Func<SignedMessageTextUpdate, object>)Delegate.CreateDelegate(typeof(Func<SignedMessageTextUpdate, object>), @object, parserMethod);

                    var textBuilderMethod = typeof(T).GetMethod(partial.TextBuilderMethod ?? string.Empty);
                    // Здесь интегрировать фичу типа получить метод+делегат по имени или проверить GetMessage();
                    var textBuilderDelegate = textBuilderMethod is null
                        ? u => @object.GetMesFor(prop, terminationalKey)
                        : (Func<ISignedUpdate, IBuildableMessage>)Delegate.CreateDelegate(typeof(Func<ISignedUpdate, IBuildableMessage>), @object, textBuilderMethod);

                    var sub = new InputSubProcess<T>(prop, textBuilderDelegate, parserDelegate);
                    if (partial.PreviewMethod is not null)
                    {
                        var previewMethod = typeof(T).GetMethod(partial.PreviewMethod)!;
                        var previewDelegate = (Func<SignedMessageTextUpdate, IBuildableMessage>?)Delegate
                            .CreateDelegate(typeof(Func<SignedMessageTextUpdate, IBuildableMessage>), @object, previewMethod);
                        if (previewDelegate is not null)
                            sub.InputPreview = previewDelegate;
                        else throw new Exception();
                    }
                    result.Add(sub);
                }
                else if (attr is SelectorInputAttribute selector)
                {
                    var t_val = typeof(T)
                        .GetMethod(selector.ValuesSelectorName)?
                        .Invoke(@object, null);
                    if (t_val is not Dictionary<string, object> values) throw new Exception();

                    var menu = new ReplyMenu();
                    values.ToList().ForEach(x => menu.Add(selector.Mask is null ? x.Key : string.Format(selector.Mask, x.Key)));
                    menu.Add(terminationalKey);

                    var mes = @object.GetMesFor(prop, terminationalKey);
                    mes.Menu = menu;

                    var sub = new InputSubProcess<T>(prop, mes, ListParse)
                    {
                        InputPreview = ListInputPreview
                    };
                    result.Add(sub);

                    object ListParse(SignedMessageTextUpdate input)
                    {
                        var match = Demask(input.Text);
                        if (!values.ContainsKey(match)) throw new Exception();
                        return values[match];
                    }
                    IBuildableMessage? ListInputPreview(SignedMessageTextUpdate input)
                    {
                        var match = Demask(input.Text);
                        if (values.ContainsKey(match)) return null;

                        return new OutputMessageText("Пожалуйста, выберете из доступных опций.")
                        {
                            Menu = menu
                        };
                    }
                    string Demask(string input)
                    {
                        if (selector.Mask is null) return input;
                        string pattern = selector.Mask.Replace("{0}", "(.*?)"); // Regex.Escape();
                        var match = Regex.Match(input, pattern);
                        if (!match.Success || match.Groups.Count < 2) throw new Exception();
                        return match.Groups[1].Value;
                    }
                }
            }
            return result;
        }
        public static void FillProcess<T>(T @object, PartialInputProcess<T> proc, int detalizationId = 0) where T : IBotPartialInput
        {
            var data = GetSubInputs(@object, proc.TerminationalKey, detalizationId);
            proc.AddSubRange(data);
        }
    }
}