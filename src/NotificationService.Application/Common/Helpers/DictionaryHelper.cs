namespace NotificationService.Application.Common.Helpers
{
    public class DictionaryHelper
    {
        public static IEnumerable<string> GetKeys(Dictionary<string, string> dict)
        {
            foreach (var k in dict.Keys)
                yield return k;
        }
    }
}