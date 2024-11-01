using iakademi41CORE_Proje.Models.MVVM;
using Microsoft.EntityFrameworkCore;


namespace iakademi41CORE_Proje.Models
{
    public class Cls_Status
    {
        iakademi41Context context = new iakademi41Context();

        public List<Status> StatusSelect()
        {
            List<Status> statuses = context.Statuses.ToList();
            return statuses; 
        }

		public static bool StatusInsert(Status status)
		{
			try
			{
				using (iakademi41Context context = new iakademi41Context())
				{
					context.Add(status);
					context.SaveChanges();
					return true;
				}
			}
			catch (Exception)
			{
				return false;
			}
		}
		public async Task<Status> StatusDetails(int? id)
		{
			Status? status=await context.Statuses.FirstOrDefaultAsync(s=> s.StatusID ==id);
			return status;
		}
		public static bool StatusDelete(int? id)
		{
			try
			{
				using (iakademi41Context context = new iakademi41Context())
				{
					Status? status = context.Statuses.FirstOrDefault(c => c.StatusID == id);
					status.Active = false;
					context.SaveChanges();
					return true;
				}
			}
			catch (Exception)
			{
				return false;
			}
		}
        public static bool StatusUpdate(Status status)
        {
            try
            {
                //metod static olduğu için contexti burada tanımlamak zorundayım
                using (iakademi41Context context = new iakademi41Context())
                {

                    context.Update(status);
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
