using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKitLs.Bots.Telegram.ArgsConverting.Argumenting
{
    public class ConvertResultList : IEnumerable<ConvertResult>, IReadOnlyList<ConvertResult>
    {
        public ConvertResultType ResultType { get; set; }
        public string Message { get; set; }

        public List<ConvertResult> Results { get; set; }

        private ConvertResultList(string mes)
        {
            Results = new();
            Message = mes;
        }

        public static ConvertResultList IncorrectRange()
            => new("Число полученных аргументов не соответствует заданному шаблону конвертации")
            {
                ResultType = ConvertResultType.Incorrect,
            };
        public static ConvertResultList WithData(List<ConvertResult> results)
        {
            int success = results.Count(x => x.ResultType == ConvertResultType.Ok);
            return new($"Успешная конвертация: {success}/{results.Count}")
            {
                Results = results,
                ResultType = success == results.Count ? ConvertResultType.Ok : ConvertResultType.Semicorrect,
            };
        }

        public int Count => Results.Count;

        public ConvertResult this[int index] => Results[index];
        public IEnumerator<ConvertResult> GetEnumerator()
        {
            foreach (var res in Results)
                yield return res;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
