using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro_Timer
{
    public class MP3Player
    {
        readonly static bool repeat = true;

        public MP3Player(string fileName, string alias)
        {
            Dispose(alias);
            //open filename then type must be mpegvideo
            const string format = @"open ""{0}"" type mpegvideo alias ""{1}""";
            string command = string.Format(format, fileName, alias);
            mciSendString(command, null, 0, IntPtr.Zero);
        }

        public void Play(string alias)
        {
            string format = @"play ""{0}""";
            if (repeat == true)
            {
                format += " REPEAT";
            }
            string command = string.Format(format, alias);
            mciSendString(command, null, 0, IntPtr.Zero);
        }

        public void Stop(string alias)
        {
            string format = @"stop ""{0}""";
            string command = string.Format(format, alias);
            mciSendString(command, null, 0, IntPtr.Zero);
        }

        public void Volume(string alias, int volume)
        {
            string format = @"setaudio ""{0}"" volume to {1}";
            string command = string.Format(format, alias, volume.ToString());
            mciSendString(command, null, 0, IntPtr.Zero);
        }

        [DllImport("winmm.dll")]
        private static extern long mciSendString(string command, StringBuilder buffer, int bufferSize, IntPtr hwndCallback);

        public void Dispose(string alias)
        {
            string format = @"close ""{0}""";
            string command = string.Format(format, alias);
            mciSendString(command, null, 0, IntPtr.Zero);
        }
    }
}
