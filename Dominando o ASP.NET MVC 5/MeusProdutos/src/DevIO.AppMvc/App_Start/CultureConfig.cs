using System.Globalization;

namespace DevIO.AppMvc.App_Start
{
    public class CultureConfig
    {
        public static void RegisterCultures()
        {
            var cultureInfo = new CultureInfo("pt-BR");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;
        }
    }
}