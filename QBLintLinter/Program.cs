using System.IO;

namespace QBLintLinter {
	class Program {
		static void Main() {
			string TempPath = Path.GetTempPath() + "QBLint\\";
			string UnlintedCode = File.ReadAllText(TempPath + "input.txt");
			UnlintedCode = UnlintedCode.Replace("QBLintLineSeparator", System.Environment.NewLine);
			Linter linter = new Linter(UnlintedCode);
			Directory.CreateDirectory(TempPath);
			File.Create(TempPath + "output.txt").Close();
			string output = linter.Lint();
			File.WriteAllText(TempPath + "output.txt", output);
			// Kill self
			System.Diagnostics.Process.GetCurrentProcess().Kill();
		}
	}
}