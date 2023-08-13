using SKitLs.Bots.Telegram.AdvancedMessages.Model.Buttons.Reply;
using SKitLs.Bots.Telegram.AdvancedMessages.Model.Menus;
using SKitLs.Bots.Telegram.AdvancedMessages.Model.Menus.Reply;
using SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages;
using SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages.Text;
using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.BotProcesses.Model;
using SKitLs.Bots.Telegram.BotProcesses.Model.Defaults.Processes.Partial;
using SKitLs.Bots.Telegram.Core.Model.DeliverySystem.Prototype;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.DataBases.Extensions.Attributes;
using SKitLs.Bots.Telegram.DataBases.Prototype;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace SKitLs.Bots.Telegram.DataBases.Extensions
{
    public static class FillHelper
    {
        public static long FirstId<T>(this List<T> data) where T : IBotDisplayable
        {
            int id = 0;
            for (int i = 0; i < data.Count; i++)
            {
                if (id < data[i].BotArgId)
                    return id;
                id++;
            }
            return id;
        }

        public static string GetShotLabels(Type type)
        {
            var result = string.Empty;
            type.GetProperties()
                .Where(x => x.GetCustomAttribute<BotInputPropAttribute>() is not null)
                .OrderBy(x => x.GetCustomAttribute<BotInputPropAttribute>()!.Order)
                .ToList()
                .ForEach(x => result += $"{Label(x)}\n");
            return result;

            static string Label(PropertyInfo prop)
            {
                var labelProp = prop.GetCustomAttribute<ShotLabeledAttribute>();
                return labelProp is null ? prop.Name : labelProp.Label;
            }
        }

        //public static async Task<List<PartialSubProcess<T>>> GetSubInputs<T>(T @object, string terminationalKey, int detalizationId = 0) where T : IBotPartialInput
        //{
        //    var result = new List<PartialSubProcess<T>>();

        //    var props = typeof(T)
        //        .GetProperties()
        //        .Where(x => x.GetCustomAttribute<BotInputPropAttribute>() is not null)
        //        .Where(x => x.GetCustomAttribute<BotInputPropAttribute>()!.DetalizationId == detalizationId)
        //        .OrderBy(x => x.GetCustomAttribute<BotInputPropAttribute>()!.Order)
        //        .ToList();

        //    foreach (var prop in props)
        //    {
        //        var attr = prop.GetCustomAttribute<BotInputPropAttribute>()!;

        //        if (attr is PartialInputAttribute partial)
        //        {
        //            var parserMethod = typeof(T).GetMethod(partial.ParserMethod ?? string.Empty);
        //            // TODO
        //            if (parserMethod is null && prop.PropertyType != typeof(string)) throw new Exception();
        //            var parser = parserMethod is not null
        //                ? (ParseInputDelegate)Delegate.CreateDelegate(typeof(ParseInputDelegate), @object, parserMethod)
        //                : null;

        //            var textBuilderMethod = typeof(T).GetMethod(partial.TextBuilderMethod ?? string.Empty);
        //            // Здесь интегрировать фичу типа получить метод + делегат по имени или проверить GetMessage();
        //            var textBuilderDelegate = textBuilderMethod is null
        //                ? async (prop, key) => await @object.GetMesFor(prop, key)
        //                : (TextBuilderDelegate)Delegate.CreateDelegate(typeof(TextBuilderDelegate), @object, textBuilderMethod);

        //            //var sub = new PartialSubProcess<T>(prop, new OutputMessageText() { ContentBuilder = textBuilderDelegate! }, parserDelegate is not null ? new ParseInputDelegate(parserDelegate) : null);
        //            var sub = new PartialSubProcess<T>(prop, new OutputMessageText() { ContentBuilder = textBuilderDelegate! }, parser);
        //            if (partial.PreviewMethod is not null)
        //            {
        //                var previewMethod = typeof(T).GetMethod(partial.PreviewMethod)!;
        //                var previewDelegate = (Func<SignedMessageTextUpdate, IBuildableMessage>?)Delegate
        //                    .CreateDelegate(typeof(Func<SignedMessageTextUpdate, IBuildableMessage>), @object, previewMethod);
        //                if (previewDelegate is not null)
        //                    sub.InputPreview = new InputPreviewDelegate<IBuildableMessage>(previewDelegate);
        //                else throw new Exception();
        //            }
        //            result.Add(sub);
        //        }
        //        else if (attr is SelectorInputAttribute selector)
        //        {
        //            var t_val = typeof(T)
        //                .GetMethod(selector.ValuesSelectorName)?
        //                .Invoke(@object, null);
        //            if (t_val is not Dictionary<string, object> values) throw new Exception();

        //            var menu = new ReplyMenu();
        //            values.ToList().ForEach(x => menu.AddRange(selector.Mask is null ? x.Key : string.Format(selector.Mask, x.Key)));
        //            menu.AddRange(terminationalKey);

        //            var mes = await @object.GetMesFor(prop, terminationalKey);
        //            mes.Menu = menu;

        //            var sub = new PartialSubProcess<T>(prop, mes, ListParse)
        //            {
        //                InputPreview = ListInputPreview
        //            };
        //            result.Add(sub);

        //            object ListParse(SignedMessageTextUpdate input)
        //            {
        //                var match = Demask(input.Text);
        //                if (!values.ContainsKey(match)) throw new Exception();
        //                return values[match];
        //            }
        //            IBuildableMessage? ListInputPreview(SignedMessageTextUpdate input)
        //            {
        //                var match = Demask(input.Text);
        //                if (values.ContainsKey(match)) return null;

        //                return new OutputMessageText("Пожалуйста, выберете из доступных опций.")
        //                {
        //                    Menu = menu
        //                };
        //            }
        //            string Demask(string input)
        //            {
        //                if (selector.Mask is null) return input;
        //                string pattern = selector.Mask.Replace("{0}", "(.*?)"); // Regex.Escape();
        //                var match = Regex.Match(input, pattern);
        //                if (!match.Success || match.Groups.Count < 2) throw new Exception();
        //                return match.Groups[1].Value;
        //            }
        //        }
        //    }
        //    return result;
        //}
        //public static void FillProcess<T>(T @object, PartialInputProcess<T> proc, int detalizationId = 0) where T : IBotPartialInput
        //{
        //    var data = GetSubInputs(@object, proc.TerminationalKey, detalizationId);
        //    proc.AddSubRange(data);
        //}
    }
}