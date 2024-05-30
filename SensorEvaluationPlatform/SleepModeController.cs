using System;
using System.Runtime.InteropServices;

namespace SensorEvaluationPlatform
{
    class SleepModeController
    {
        // Import the SetThreadExecutionState function from the Windows API
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

        [Flags]
        private enum EXECUTION_STATE : uint
        {
            ES_CONTINUOUS = 0x80000000,
            ES_SYSTEM_REQUIRED = 0x00000001,
            //ES_DISPLAY_REQUIRED = 0x00000002,
            //ES_USER_PRESENT = 0x00000004
        }

        public static void PreventSleepMode()
        {
            // Set the ES_CONTINUOUS flag to prevent the system from sleeping.
            SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_SYSTEM_REQUIRED);
        }

        public static void AllowSleepMode()
        {
            // Remove the ES_CONTINUOUS flag to allow the system to sleep again.
            SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
        }
    }
}
