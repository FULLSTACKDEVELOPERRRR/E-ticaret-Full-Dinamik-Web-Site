using iakademi41CORE_Proje.Models.MVVM;

namespace iakademi41CORE_Proje.Models
{
    public class Cls_Setting
    {
        iakademi41Context context = new iakademi41Context();

        public async Task<Setting> SettingDetails()
        {
            Setting? setting = await context.Settings.FindAsync(1);
            return setting;
        }

        public static bool SettingUpdate(Setting setting)
        {
            try
            {
                using (iakademi41Context context = new iakademi41Context())
                {
                    setting.SettingID = 1;
                    context.Update(setting);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
