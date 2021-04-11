using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Security.Cryptography;

namespace QBLintLinter
{
	public class Linter {
		// Start variable declarations
		private string UnlintedCode;
		private string LintedCode;
		// End variable declarations

		public Linter(string Code) {
			Code = Code.Replace("QBLintLineSeparator", "\n");
			UnlintedCode = Code;
			LintedCode = Code;
		}

		public string Lint() { // Main linting function
			string ReturnValue = "";
			string s = Environment.NewLine + "QBLintFieldSeparator" + Environment.NewLine;
			string TempBAS = Path.GetTempPath() + @"QBLint\TempBAS\";
			Directory.CreateDirectory(TempBAS);
			string FileToWrite = TempBAS + GetHash(UnlintedCode) + ".bas";

			// Make temporary bas file for compilation
			File.Create(FileToWrite).Close();
			File.WriteAllText(FileToWrite, UnlintedCode);
			LintedCode = Formatter.Format(FileToWrite);
			ProcessStartInfo QB64 = new ProcessStartInfo("qb64", "-z -w " + '"' + FileToWrite + "\"");
			QB64.RedirectStandardOutput = true;
			Process QB64Process = Process.Start(QB64);
			QB64Process.WaitForExit();

			string output = "";
			StreamReader Output = QB64Process.StandardOutput;
            if(QB64Process.HasExited) {
                output = Output.ReadToEnd();
            }

			string FileName = FileToWrite.Split("\\")[FileToWrite.Split("\\").Length - 1];
			output = output.Replace(FileName + ":", "");
			if(!output.Contains("Syntax") && !output.Contains("warning")) {
				output = "0 Warnings and 0 Errors";
			}

			ReturnValue = LintedCode + s + output;

			// If there are 10 files or more in the TempBAS directory, then delete them all
			if (Directory.GetFiles(TempBAS).Length >= 10) {
				foreach (var file in Directory.GetFiles(TempBAS)) {
						File.Delete(file);
				}
			}

			return ReturnValue;
		}

		public static string GetHash(String value) {
			StringBuilder Sb = new StringBuilder();

			using (SHA256 hash = SHA256Managed.Create())
			{
				Encoding enc = Encoding.UTF8;
				Byte[] result = hash.ComputeHash(enc.GetBytes(value));

				foreach (Byte b in result)
					Sb.Append(b.ToString("x2"));
			}

			return Sb.ToString();
		}
	}
}