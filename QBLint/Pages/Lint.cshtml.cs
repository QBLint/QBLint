using System;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IOFile = System.IO.File;

namespace QBLint.Pages
{
    public class LintModel : PageModel
    {
        public string Lint(string UnlintedCode, string Capitalize) {
            string LintedCode = "";

            ProcessStartInfo WhereInfo = new ProcessStartInfo { RedirectStandardOutput = true, Arguments = "qb64", FileName = "where.exe" };
            Process Where = Process.Start(WhereInfo);
            Where.WaitForExit();
            string Config = Where.StandardOutput.ReadToEnd();
            Config = Regex.Replace(Config, $"qb64.exe", "", RegexOptions.IgnoreCase);
            Config = Config.Replace("\n", "");
            Config = Config.Replace("\r", "");
            Config += "internal\\config.ini";
            // IDE_KeywordCapital
            string ConfigContents = IOFile.ReadAllText(Config);
            ConfigContents = ConfigContents.Replace("IDE_KeywordCapital=True", "IDE_KeywordCapital=" + Capitalize);
            ConfigContents = ConfigContents.Replace("IDE_KeywordCapital=False", "IDE_KeywordCapital=" + Capitalize);
            IOFile.WriteAllText(Config, ConfigContents);

            string QBLintLinterPath = Environment.GetEnvironmentVariable("QBLINTER", EnvironmentVariableTarget.Machine);
            string QBLintFolder = Path.GetTempPath() + "QBLint\\";
            if (!Directory.Exists(QBLintFolder)) {
                Directory.CreateDirectory(QBLintFolder);
                Directory.CreateDirectory(QBLintFolder + "TempBAS\\");
            }
            UnlintedCode += "QBLintLineSeparator'TEMP";
            IOFile.WriteAllText(QBLintFolder + "input.txt", UnlintedCode);
            string _ = QBLintLinterPath + "QBLintLinter.exe";
            Process.Start(_).WaitForExit();
            LintedCode = IOFile.ReadAllText(QBLintFolder + "output.txt");
            return LintedCode;
		}
    }
}
