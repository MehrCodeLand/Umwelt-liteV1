namespace Umwelt_liteV.Creator
{
    public class CreateRandomId
    {
        public static int CreateId()
        {
            var rnd = new Random();
            return rnd.Next(111,999);
        }
    }
}
