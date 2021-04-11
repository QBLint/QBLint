using System;
using System.IO;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IOFile = System.IO.File;

namespace QBLint.Pages
{
    public class LintModel : PageModel
    {
        public string Lint(string UnlintedCode) {
            string LintedCode = "";
            string QBLintLinterPath = Environment.GetEnvironmentVariable("QBLINTER", EnvironmentVariableTarget.Machine);
            string QBLintFolder = Path.GetTempPath() + "QBLint\\";
            UnlintedCode += "QBLintLineSeparator'TEMP";
            IOFile.WriteAllText(QBLintFolder + "input.txt", UnlintedCode);
            string _ = QBLintLinterPath + "QBLintLinter.exe";
            Process.Start(_).WaitForExit();
            LintedCode = IOFile.ReadAllText(QBLintFolder + "output.txt");
            return LintedCode;
		}
    }
}
