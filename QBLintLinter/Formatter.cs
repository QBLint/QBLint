using System.IO;
using System.Threading;
using System.Diagnostics;

namespace QBLintLinter {
    public static class Formatter {
        public static string Format(string FilePath) {
            string FormattedCode = "";
            ProcessStartInfo QB64Info = new ProcessStartInfo { FileName = "qb64", Arguments = "\"" + FilePath + "\"" };
            Process QB64 = Process.Start(QB64Info);
            Thread.Sleep(3000);

			// Save linted file
			if (System.Console.NumberLock) { // Make sure Num Lock is off or down arrows won't work
                InputSender.PressKey((ushort)Codes.NumLock);
            }
            Thread.Sleep(100);
            InputSender.PressKey((ushort)Codes.Tab);
            Thread.Sleep(100);
            InputSender.PressKey((ushort)Codes.Enter);
            Thread.Sleep(100);
            InputSender.PressKey((ushort)Codes.Keypad2_Down);
            Thread.Sleep(100);
            InputSender.PressKey((ushort)Codes.Keypad2_Down);
            Thread.Sleep(100);
            InputSender.PressKey((ushort)Codes.LAlt);
            Thread.Sleep(100);
            InputSender.PressKey((ushort)Codes.F);
			Thread.Sleep(100);
			InputSender.PressKey((ushort)Codes.Keypad2_Down);
            Thread.Sleep(100);
            InputSender.PressKey((ushort)Codes.Keypad2_Down);
            Thread.Sleep(100);
            InputSender.PressKey((ushort)Codes.Enter);
			Thread.Sleep(100);

            // Get linted file contents
            FormattedCode = File.ReadAllText(FilePath);
            QB64.Kill();
            return FormattedCode;
        }
    }
}