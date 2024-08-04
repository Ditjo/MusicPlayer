using MusicPlayer.Data.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace MusicPlayer.UI.Converters
{
    public class TimeSpanToDoubleConverter : IValueConverter
    {
        public enum TimeSpanFormat
        {
            TotalSeconds,
            TotalMinutes,
            TotalHours,
            TotalDays
        }

        public TimeSpanFormat Format { get; set; } = TimeSpanFormat.TotalSeconds;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimeSpan timeSpan)
            {
                switch (Format)
                {
                    case TimeSpanFormat.TotalSeconds:
                        return timeSpan.TotalSeconds;
                    case TimeSpanFormat.TotalMinutes:
                        return timeSpan.TotalMinutes;
                    case TimeSpanFormat.TotalHours:
                        return timeSpan.TotalHours;
                    case TimeSpanFormat.TotalDays:
                        return timeSpan.TotalDays;
                    default:
                        return timeSpan.TotalSeconds;
                }
            }

            return 0.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double doubleValue)
            {
                switch (Format)
                {
                    case TimeSpanFormat.TotalSeconds:
                        return TimeSpan.FromSeconds(doubleValue);
                    case TimeSpanFormat.TotalMinutes:
                        return TimeSpan.FromMinutes(doubleValue);
                    case TimeSpanFormat.TotalHours:
                        return TimeSpan.FromHours(doubleValue);
                    case TimeSpanFormat.TotalDays:
                        return TimeSpan.FromDays(doubleValue);
                    default:
                        return TimeSpan.FromSeconds(doubleValue);
                }
            }

            return TimeSpan.Zero;
        }
    }
    public class IsSongInPlaylistConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var list = (IEnumerable<Song>)value;
            var label = (Label)parameter;
            var anySongs = list.Where(x => x.SongName == label.Content.ToString());
            return !anySongs.Any();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
