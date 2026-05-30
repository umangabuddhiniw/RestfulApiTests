using System.IO;
using System.Text;

namespace RestfulApiTests.Reporting
{
    public static class HtmlReportGenerator
    {
        public static void Generate(string path)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<html><body>");
            sb.AppendLine("<h1>API Test Report</h1>");
            sb.AppendLine("<table border='1'>");
            sb.AppendLine("<tr><th>Test</th><th>Status</th><th>Message</th></tr>");

            foreach (var r in TestResultStore.Results)
            {
                sb.AppendLine($"<tr><td>{r.TestName}</td><td>{r.Status}</td><td>{r.Message}</td></tr>");
            }

            sb.AppendLine("</table></body></html>");

            File.WriteAllText(path, sb.ToString());
        }
    }
}