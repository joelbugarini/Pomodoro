using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pomodoro
{
    public class PomodoroArray
    {
        private int[] Pomodoro { get; set; }

        private int StreaksNumber { get; set; }
        private int StreakLength { get; set; }
        private int ShortBreakLength { get; set; }
        private int LongStreakLength { get; set; }
        private int CurrentStreak { get; set; }

        public PomodoroArray(int Streaks, int Length, int Break, int LongBreak) {
            this.StreaksNumber = Streaks;
            this.StreakLength = Length;
            this.ShortBreakLength = Break;
            this.LongStreakLength = LongBreak;
            this.CurrentStreak = 0;
            this.Pomodoro = this.CreateArray();
        }

        private int[] CreateArray()
        {
            int length = StreaksNumber * 2;
            int[] result = new int[length];
            for (int c = 0; c < length; c+=2) {
                result[c] = StreakLength;
                result[c + 1] = ShortBreakLength;
            }
            result[length - 1] = LongStreakLength;
            return result;
        }

        public void Next() {
            if (CurrentStreak == Pomodoro.Length - 1)
            {
                CurrentStreak = 0;
            } else {
                CurrentStreak += 1;
            }
        }

        public void Previous()
        {
            if (CurrentStreak == 0)
            {
                CurrentStreak = Pomodoro.Length - 1;
            }
            else
            {
                CurrentStreak -= 1;
            }
        }

        public int GetCurrentStreakLength()
        {
            return Pomodoro[CurrentStreak];
        }
    }
}
