using Kosa.ECommerce.Shared.Constants;

namespace Kosa.ECommerce.Shared.Helpers;

public static class DateTimeHelper
{
    public static DateTime GetEstimatedDeliveryTime(DateTime orderDate)
    {
        return orderDate.AddHours(AppConstants.EstimatedDeliveryHours); // حساب الوقت المتوقع للوصيل 
    }
}
