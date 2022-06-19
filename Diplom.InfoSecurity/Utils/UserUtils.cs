namespace Diplom.InfoSecurity.Utils
{
    public static class UserUtils
    {
        public static void CheckOrDefaultFIO(ref string firstName, ref string secondName, ref string thirdName)
        {
            if (firstName is null)
                firstName = "";
            if (secondName is null)
                secondName = "";
            if (thirdName is null)
                thirdName = "";
        }
    }
}
