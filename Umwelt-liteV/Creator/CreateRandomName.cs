namespace Umwelt_liteV.Creator
{
    public class CreateRandomName
    {
        public static string CreateName()
        {
            var rnd = new Random();
            string ConfirmCode = rnd.Next(123, 99999).ToString();
            for (int i = 0; i < 8; ++i)
            {
                ConfirmCode += char.ConvertFromUtf32(rnd.Next(66, 80));
            }

            return ConfirmCode;
        }
    }
}
