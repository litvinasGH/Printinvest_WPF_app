using Printinvest_WPF_app.Models;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Printinvest_WPF_app.Converters
{
    public class OrderStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is OrderStatus status)
            {
                string resourceKey;
                switch (status)
                {
                    case OrderStatus.Pending:
                        resourceKey = "OrderStatusPending";
                        break;
                    case OrderStatus.Shipped:
                        resourceKey = "OrderStatusShipped";
                        break;
                    case OrderStatus.Completed:
                        resourceKey = "OrderStatusCompleted";
                        break;
                    case OrderStatus.Cancelled:
                        resourceKey = "OrderStatusCancelled";
                        break;
                    default:
                        resourceKey = string.Empty;
                        break;
                }
                return Application.Current.TryFindResource(resourceKey)?.ToString() ?? status.ToString();
            }
            return value?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}