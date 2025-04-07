using Newtonsoft.Json;

namespace MultiTargetLibrary
{
    public class PersonSerializer
    {
        public static string Serialize(Person person)
        {
            return JsonConvert.SerializeObject(person);
        }
    }
}
